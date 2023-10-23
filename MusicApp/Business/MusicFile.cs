using System;
using System.IO;
using System.Reflection;
using NAudio.Wave;

namespace Business
{
    public class MusicFile : IDisposable
    {
        private readonly TagLib.File _file;
        private string _path;
        private bool _dirty;

        public MusicFile(string path)
        {
            _file = TagLib.File.Create(path);
            _path = path;
        }

        public string Title
        {
            get
            {
                return _file.Tag.Title;
            }
            set
            {
                _file.Tag.Title = value;
                _dirty = true;
            }
        }

        public string Artist
        {
            get
            {
                return _file.Tag.Performers.Length > 0 ? string.Join("/", _file.Tag.Performers) : null;
            }
            set
            {
                if (value != null)
                    _file.Tag.Performers = new[] { value };
                else
                    _file.Tag.Performers = Array.Empty<string>();

                _dirty = true;
            }
        }

        public string Album
        {
            get
            {
                return _file.Tag.Album;
            }
            set
            {
                _file.Tag.Album = value;
                _dirty = true;
            }
        }

        public string AlbumArtist
        {
            get
            {
                return _file.Tag.AlbumArtists.Length > 0 ? string.Join("/", _file.Tag.AlbumArtists) : null;
            }
            set
            {
                if (value != null)
                    _file.Tag.AlbumArtists = new[] { value };
                else
                    _file.Tag.AlbumArtists = Array.Empty<string>();

                _dirty = true;
            }
        }

        public string Composer
        {
            get
            {
                return _file.Tag.Composers.Length > 0 ? string.Join("/", _file.Tag.Composers) : null;
            }
            set
            {
                if (value != null)
                    _file.Tag.Composers = new[] { value };
                else
                    _file.Tag.Composers = Array.Empty<string>();

                _dirty = true;
            }
        }

        public string Grouping
        {
            get
            {
                return _file.Tag.Grouping;
            }
            set
            {
                _file.Tag.Grouping = value;
                _dirty = true;
            }
        }

        public string Genre
        {
            get
            {
                return _file.Tag.Genres.Length > 0 ? string.Join("/", _file.Tag.Genres) : null;
            }
            set
            {
                if (value != null)
                    _file.Tag.Genres = new[] { value };
                else
                    _file.Tag.Genres = Array.Empty<string>();

                _dirty = true;
            }
        }

        public uint Year
        {
            get
            {
                return _file.Tag.Year;
            }
            set
            {
                _file.Tag.Year = value;
                _dirty = true;
            }
        }

        public uint TrackNumber
        {
            get
            {
                return _file.Tag.Track;
            }
            set
            {
                _file.Tag.Track = value;
                _dirty = true;
            }
        }

        public uint TrackCount
        {
            get
            {
                return _file.Tag.TrackCount;
            }
            set
            {
                _file.Tag.TrackCount = value;
                _dirty = true;
            }
        }

        public uint DiscNumber
        {
            get
            {
                return _file.Tag.Disc;
            }
            set
            {
                _file.Tag.Disc = value;
                _dirty = true;
            }
        }

        public uint DiscCount
        {
            get
            {
                return _file.Tag.DiscCount;
            }
            set
            {
                _file.Tag.DiscCount = value;
                _dirty = true;
            }
        }

        public string Comment
        {
            get
            {
                return _file.Tag.Comment;
            }
            set
            {
                _file.Tag.Comment = value;
                _dirty = true;
            }
        }

        public uint Rating
        {
            get
            {
                return _file.Tag.BeatsPerMinute;
            }
            set
            {
                _file.Tag.BeatsPerMinute = value;
                _dirty = true;
            }
        }

        public TagLib.IPicture Artwork
        {
            get
            {
                return _file.Tag.Pictures.Length > 0 ? _file.Tag.Pictures[0] : null;
            }
            set
            {
                if (value != null)
                    _file.Tag.Pictures = new[] { value };
                else
                    _file.Tag.Pictures = Array.Empty<TagLib.IPicture>();

                _dirty = true;
            }
        }

        public string ArtworkPath
        {
            set
            {
                _file.Tag.Pictures = new[] { new TagLib.Picture(value) };
                _dirty = true;
            }
        }

        public void Save()
        {
            if (_dirty)
                _file.Save();
        }

        public string SaveAsMp3(string destinationDirectory)
        {
            using (var reader = new MediaFoundationReader(_path))
            {
                string fileName = Path.GetFileNameWithoutExtension(_path);
                string filePath = Path.Combine(destinationDirectory, $"{fileName}.mp3");
                MediaFoundationEncoder.EncodeToMp3(reader, filePath, 320000);

                var newFile = new MusicFile(filePath);

                Type type = typeof(MusicFile);
                PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo property in properties)
                {
                    if (property.CanRead && property.CanWrite)
                    {
                        object value = property.GetValue(this);
                        property.SetValue(newFile, value);
                    }
                }

                newFile.Save();

                return filePath;
            }
        }

        public void Copy()
        {
            var decade = uint.Parse(Year.ToString().Substring(2, 1));
            var decadeStart = decade == 6 || decade == 8 ? decade - 1 : decade;
            var decadeEnd = decade == 5 || decade == 7 ? decade + 1 : decade;

            var language = Grouping != null && Grouping.Contains("Hun") ? "Hun" : "Int";

            var genre = Genre != null && Genre.Contains(" (") ? Genre.Substring(0, Genre.IndexOf(" (")) : Genre;

            switch (genre)
            {
                case "Dance":
                case "Dance & House":
                case "House":
                case "Dance-Pop":
                case "Hard Dance":
                case "Club":
                case "Club-House":
                case "Techno":
                case "Rave":
                case "Electronic":
                    genre = "Dance";
                    break;
                case "Trance":
                case "Eurotrance":
                    genre = "Trance";
                    break;
                case "Pop":
                case "Teen-Pop":
                case "Latin":
                case "Jazz":
                case "Soul":
                case "R&B":
                case "Hip-Hop":
                case "Rap":
                case "Soundtrack":
                case "Rockabilly":
                    genre = "Pop";
                    break;
                case "Euro Disco":
                case "Eurodisco":
                case "New Wave":
                    genre = "Euro Disco";
                    break;
            }

            var folder = $"{genre} ({language})";

            var directory = Path.GetDirectoryName(_path);
            var destinationDirectory = Path.Combine(directory, folder);
            Directory.CreateDirectory(destinationDirectory);

            var fileName = Path.GetFileName(_path);
            string filePath = Path.Combine(destinationDirectory, fileName);
            File.Copy(_path, filePath, true);

            if (genre != "Rock" && genre != "Musical")
            {
                folder = $"{Year.ToString().Substring(0, 2)}{decadeStart}0-{decadeEnd}9 {folder}";

                destinationDirectory = Path.Combine(directory, folder);
                Directory.CreateDirectory(destinationDirectory);

                filePath = Path.Combine(destinationDirectory, fileName);
                File.Copy(_path, filePath, true);
            }
        }

        public void Copy2(string folder)
        {
            var directory = Path.GetDirectoryName(_path);
            var destinationDirectory = Path.Combine(directory, folder);
            Directory.CreateDirectory(destinationDirectory);

            var fileName = Path.GetFileName(_path);
            string filePath = Path.Combine(destinationDirectory, fileName);
            File.Copy(_path, filePath, true);
        }

        public void Dispose()
        {
            _file.Dispose();
        }
    }
}
