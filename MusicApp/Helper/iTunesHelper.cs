using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Helper
{
    public class iTunesHelper
    {
        private readonly StringBuilder _message = new StringBuilder();
        private readonly string _iTunesMusicLibraryXml;
        private DateTime _lastModifiedDate;
        private Library _library { get; set; }

        public Library Library
        {
            get
            {
                if (_lastModifiedDate != File.GetLastWriteTime(_iTunesMusicLibraryXml))
                {
                    _lastModifiedDate = File.GetLastWriteTime(_iTunesMusicLibraryXml);
                    _library = LoadLibrary();
                }

                return _library;
            }
        }

        public iTunesHelper(string iTunesDirectory)
        {
            _iTunesMusicLibraryXml = Constant.ITunesMusicLibraryXml;
            _lastModifiedDate = File.GetLastWriteTime(_iTunesMusicLibraryXml);
            _library = LoadLibrary();
        }

        private Library LoadLibrary()
        {
            var library = new Library();

            XDocument xDocument = XDocument.Load(_iTunesMusicLibraryXml);

            IEnumerable<XElement> libraryKeyElements = xDocument.Elements().Elements("dict").Elements("key");

            library.MajorVersion = GetUIntFromElement(libraryKeyElements, "Major Version");
            library.MinorVersion = GetUIntFromElement(libraryKeyElements, "Minor Version");
            library.ApplicationVersion = GetStringFromElement(libraryKeyElements, "Application Version");
            library.Date = GetDateFromElement(libraryKeyElements, "Date");
            library.Features = GetUIntFromElement(libraryKeyElements, "Features");
            library.ShowContentRatings = GetBoolFromElement(libraryKeyElements, "Show Content Ratings");
            library.LibraryPersistentID = GetStringFromElement(libraryKeyElements, "Library Persistent ID");

            XElement tracksElement = libraryKeyElements.FirstOrDefault(x => x.Value == "Tracks");

            if (tracksElement != null)
            {
                IEnumerable<XElement> tracksKeyElements = ((XElement)tracksElement.NextNode).Elements("key");

                foreach (XElement tracksKeyElement in tracksKeyElements)
                {
                    string ID = tracksKeyElement.Value;

                    IEnumerable<XElement> keyElements = ((XElement)tracksKeyElement.NextNode).Elements("key");

                    Track track = LoadTrack(keyElements);
                    library.Tracks.Add(track);
                }
            }

            XElement playlistsElement = libraryKeyElements.FirstOrDefault(x => x.Value == "Playlists");

            if (playlistsElement != null)
            {
                IEnumerable<XElement> playlistDictElements = ((XElement)playlistsElement.NextNode).Elements("dict");

                foreach (XElement playlistDictElement in playlistDictElements)
                {
                    IEnumerable<XElement> keyElements = playlistDictElement.Elements("key");

                    Playlist playlist = LoadPlaylist(keyElements);
                    library.Playlists.Add(playlist);
                }
            }

            library.MusicFolder = GetStringFromElement(libraryKeyElements, "Music Folder");

            return library;
        }

        private Track LoadTrack(IEnumerable<XElement> keyElements)
        {
            var track = new Track();

            track.TrackID = GetUIntFromElement(keyElements, "Track ID");
            track.Size = GetUIntFromElement(keyElements, "Size");
            track.TotalTime = GetUIntFromElement(keyElements, "Total Time");
            track.StartTime = GetUIntFromElement(keyElements, "Start Time");
            track.DiscNumber = GetUIntFromElement(keyElements, "Disc Number");
            track.DiscCount = GetUIntFromElement(keyElements, "Disc Count");
            track.TrackNumber = GetUIntFromElement(keyElements, "Track Number");
            track.TrackCount = GetUIntFromElement(keyElements, "Track Count");
            track.Year = GetUIntFromElement(keyElements, "Year");
            track.BPM = GetUIntFromElement(keyElements, "BPM");
            track.DateModified = GetDateFromElement(keyElements, "Date Modified");
            track.DateAdded = GetDateFromElement(keyElements, "Date Added");
            track.BitRate = GetUIntFromElement(keyElements, "Bit Rate");
            track.SampleRate = GetUIntFromElement(keyElements, "Sample Rate");
            track.VolumeAdjustment = GetUIntFromElement(keyElements, "Volume Adjustment");
            track.PartOfGaplessAlbum = GetBoolFromElement(keyElements, "Part Of Gapless Album");
            track.PlayCount = GetUIntFromElement(keyElements, "Play Count");
            track.PlayDate = GetUIntFromElement(keyElements, "Play Date");
            track.PlayDateUTC = GetDateFromElement(keyElements, "Play Date UTC");
            track.SkipCount = GetUIntFromElement(keyElements, "Skip Count");
            track.SkipDate = GetDateFromElement(keyElements, "Skip Date");
            track.Rating = GetUIntFromElement(keyElements, "Rating");
            track.RatingComputed = GetBoolFromElement(keyElements, "Rating Computed");
            track.AlbumRating = GetUIntFromElement(keyElements, "Album Rating");
            track.AlbumRatingComputed = GetBoolFromElement(keyElements, "Album Rating Computed");
            track.Compilation = GetBoolFromElement(keyElements, "Compilation");
            track.ReleaseDate = GetDateFromElement(keyElements, "Release Date");
            track.ArtworkCount = GetUIntFromElement(keyElements, "Artwork Count");
            track.PersistentID = GetStringFromElement(keyElements, "Persistent ID");
            track.Clean = GetBoolFromElement(keyElements, "Clean");
            track.Explicit = GetBoolFromElement(keyElements, "Explicit");
            track.TrackType = GetStringFromElement(keyElements, "Track Type");
            track.AppleMusic = GetBoolFromElement(keyElements, "Apple Music");
            track.Podcast = GetBoolFromElement(keyElements, "Podcast");
            track.Unplayed = GetBoolFromElement(keyElements, "Unplayed");
            track.Movie = GetBoolFromElement(keyElements, "Movie");
            track.MusicVideo = GetBoolFromElement(keyElements, "Music Video");
            track.HasVideo = GetBoolFromElement(keyElements, "Has Video");
            track.Protected = GetBoolFromElement(keyElements, "Protected");
            track.Purchased = GetBoolFromElement(keyElements, "Purchased");
            track.PlaylistOnly = GetBoolFromElement(keyElements, "Playlist Only");
            track.FileFolderCount = GetUIntFromElement(keyElements, "File Folder Count");
            track.LibraryFolderCount = GetUIntFromElement(keyElements, "Library Folder Count");
            track.Name = GetStringFromElement(keyElements, "Name");
            track.Artist = GetStringFromElement(keyElements, "Artist");
            track.AlbumArtist = GetStringFromElement(keyElements, "Album Artist");
            track.Composer = GetStringFromElement(keyElements, "Composer");
            track.Album = GetStringFromElement(keyElements, "Album");
            track.Grouping = GetStringFromElement(keyElements, "Grouping");
            track.Genre = GetStringFromElement(keyElements, "Genre");
            track.Kind = GetStringFromElement(keyElements, "Kind");
            track.Comments = GetStringFromElement(keyElements, "Comments");
            track.SortName = GetStringFromElement(keyElements, "Sort Name");
            track.SortArtist = GetStringFromElement(keyElements, "Sort Artist");
            track.SortAlbumArtist = GetStringFromElement(keyElements, "Sort Album Artist");
            track.SortComposer = GetStringFromElement(keyElements, "Sort Composer");
            track.SortAlbum = GetStringFromElement(keyElements, "Sort Album");
            track.ContentRating = GetStringFromElement(keyElements, "Content Rating");
            track.Work = GetStringFromElement(keyElements, "Work");
            track.Location = GetStringFromElement(keyElements, "Location");

            return track;
        }

        private Playlist LoadPlaylist(IEnumerable<XElement> keyElements)
        {
            var playlist = new Playlist();

            playlist.Master = GetBoolFromElement(keyElements, "Master");
            playlist.PlaylistID = GetUIntFromElement(keyElements, "Playlist ID");
            playlist.ParentPersistentID = GetStringFromElement(keyElements, "Parent Persistent ID");
            playlist.PlaylistPersistentID = GetStringFromElement(keyElements, "Playlist Persistent ID");
            playlist.DistinguishedKind = GetUIntFromElement(keyElements, "Distinguished Kind");
            playlist.Music = GetBoolFromElement(keyElements, "Music");
            playlist.Movies = GetBoolFromElement(keyElements, "Movies");
            playlist.TVShows = GetBoolFromElement(keyElements, "TV Shows");
            playlist.Podcasts = GetBoolFromElement(keyElements, "Podcasts");
            playlist.iTunesU = GetBoolFromElement(keyElements, "iTunes U");
            playlist.Audiobooks = GetBoolFromElement(keyElements, "Audiobooks");
            playlist.Books = GetBoolFromElement(keyElements, "Books");
            playlist.PurchasedMusic = GetBoolFromElement(keyElements, "Purchased Music");
            playlist.AllItems = GetBoolFromElement(keyElements, "All Items");
            playlist.Visible = GetBoolFromElement(keyElements, "Visible");
            playlist.Folder = GetBoolFromElement(keyElements, "Folder");
            playlist.Name = GetStringFromElement(keyElements, "Name");
            playlist.Description = GetStringFromElement(keyElements, "Description");
            //playlist.SmartInfo = GetStringFromElement(keyElements, "Smart Info");
            //playlist.SmartCriteria = GetStringFromElement(keyElements, "Smart Criteria");

            XElement element = keyElements.FirstOrDefault(x => x.Value == "Smart Info");
            if (element != null)
            {
                playlist.SmartInfo = "¤";
            }

            element = keyElements.FirstOrDefault(x => x.Value == "Playlist Items");

            if (element != null)
            {
                IEnumerable<XElement> playlistItemElements = ((XElement)element.NextNode).Elements("dict");

                foreach (XElement pi in playlistItemElements)
                {
                    IEnumerable<XElement> keyElements2 = pi.Elements("key");

                    int? trackID = GetUIntFromElement(keyElements2, "Track ID");

                    if (trackID != null)
                    {
                        playlist.PlaylistItemIDs.Add(trackID.Value);
                        //Track Track = Library.Tracks.FirstOrDefault(t => t.TrackID == value);

                        //if (Track != null)
                        //{
                        //    playlist.PlaylistItems.Add(Track);
                        //    Track.Playlists.Add(playlist);
                        //}
                        //else
                        //{
                        //    throw new Exception("Track not found in the Library.Tracks");
                        //}
                    }
                }
            }

            return playlist;
        }

        private bool GetBoolFromElement(IEnumerable<XElement> keyElements, string value)
        {
            XElement element = keyElements.FirstOrDefault(x => x.Value == value);
            if (element != null)
            {
                string localName = ((XElement)element.NextNode).Name.LocalName;
                bool result;
                if (bool.TryParse(localName, out result))
                    return result;
                else
                    return false;
            }
            else
                return false;
        }

        private string GetStringFromElement(IEnumerable<XElement> keyElements, string value)
        {
            XElement element = keyElements.FirstOrDefault(x => x.Value == value);
            if (element != null)
                return ((XElement)element.NextNode).Value;
            else
                return null;
        }

        private int? GetUIntFromElement(IEnumerable<XElement> keyElements, string value)
        {
            string stringValue = GetStringFromElement(keyElements, value);

            if (stringValue != null)
            {
                int uintValue;
                if (int.TryParse(stringValue, out uintValue))
                    return uintValue;
                else
                    return null;
            }
            else
                return null;
        }

        private DateTime? GetDateFromElement(IEnumerable<XElement> keyElements, string value)
        {
            string stringValue = GetStringFromElement(keyElements, value);

            if (stringValue != null)
            {
                DateTime dateTimeValue;
                if (DateTime.TryParse(stringValue, out dateTimeValue))
                    return dateTimeValue;
                else
                    return null;
            }
            else
                return null;
        }




        // Old Parser

        public string CreateDatabase()
        {
            _message.Clear();

            //GetLibrary();

            using (var db = new LibraryContext())
            {
                db.Libraries.Add(Library);
                db.SaveChanges();
            }

            return _message.ToString();
        }

        public string UpdateDatabase()
        {
            _message.Clear();

            Library newLibrary = ReadLibraryFromXML();

            using (var db = new LibraryContext())
            {
                foreach (Library oldLibrary in db.Libraries)
                {
                    var deletedTracks = new List<Track>();
                    var insertedTracks = new List<Track>();

                    foreach (Track oldTrack in oldLibrary.Tracks)
                    {
                        Track newTrack = newLibrary.Tracks.Where(t => t.TrackID == oldTrack.TrackID).FirstOrDefault();

                        if (newTrack != null)
                        {
                            oldTrack.Compare(newTrack);
                        }
                        else
                        {
                            deletedTracks.Add(oldTrack);
                        }
                    }

                    foreach (Track newTrack in newLibrary.Tracks)
                    {
                        Track oldTrack = oldLibrary.Tracks.Where(t => t.TrackID == newTrack.TrackID).FirstOrDefault();

                        if (oldTrack == null)
                        {
                            insertedTracks.Add(newTrack);
                        }
                    }

                    foreach (Track track in deletedTracks)
                    {
                        oldLibrary.Tracks.Remove(track);
                    }

                    foreach (Track track in insertedTracks)
                    {
                        oldLibrary.Tracks.Add(track);
                    }
                }
                db.SaveChanges();
            }

            return _message.ToString();
        }

        private Library ReadLibraryFromXML()
        {
            var library = new Library();

            string fileName = Constant.ITunesMusicLibraryXml;

            var doc = new XmlDocument();

            doc.Load(fileName);

            XmlElement dictElement = doc.DocumentElement;

            if (dictElement is null) return _library;

            XmlNodeList list1 = dictElement.ChildNodes;

            if (list1.Count == 0) return _library;

            XmlNode dictNode = list1[0];

            if (dictNode is null) return _library;

            string propertyName = "";

            foreach (XmlNode node in dictNode.ChildNodes)
            {
                if (node.Name == "key")
                    propertyName = node.InnerText.Replace(" ", "");
                else if (node.Name == "integer")
                {
                    uint value = Convert.ToUInt32(node.InnerText);
                    library.GetType().GetProperty(propertyName).SetValue(library, value);
                }
                else if (node.Name == "string")
                {
                    library.GetType().GetProperty(propertyName).SetValue(library, node.InnerText);
                }
                else if (node.Name == "date")
                {
                    DateTime value = DateTime.Parse(node.InnerText);
                    library.GetType().GetProperty(propertyName).SetValue(library, value);
                }
                else if (node.Name == "true")
                    library.GetType().GetProperty(propertyName).SetValue(library, true);
                else if (node.Name == "false")
                    library.GetType().GetProperty(propertyName).SetValue(library, false);
                else if (node.Name == "dict")
                {
                    if (propertyName == "Tracks")
                    {
                        ProcessTracks(node, library);
                    }
                }
                else if (node.Name == "array")
                {
                    if (propertyName == "Playlists")
                    {
                        //ProcessPlaylists(node, Library);
                    }
                }
            }

            return library;
        }

        private void ProcessTracks(XmlNode dictNode, Library library)
        {
            Track track = null;

            foreach (XmlNode node in dictNode.ChildNodes)
            {
                if (node.Name == "key")
                {
                    int id = Convert.ToInt32(node.InnerText);
                    track = new Track { ID = id };
                    library.Tracks.Add(track);
                }
                else if (node.Name == "dict")
                {
                    ProcessTrack(node, track);
                }
            }
        }

        private void ProcessTrack(XmlNode dictNode, Track track)
        {
            string propertyName = "";
            PropertyInfo propertyInfo = null;
            bool @continue = false;

            foreach (XmlNode node in dictNode.ChildNodes)
            {
                if (node.Name == "key")
                {
                    if (@continue)
                    {
                        @continue = false;
                    }

                    propertyName = node.InnerText.Replace(" ", "");
                    propertyInfo = track.GetType().GetProperty(propertyName);
                    if (propertyInfo == null)
                    {
                        _message.AppendLine($"Track: {track.TrackID} {propertyName}");
                        @continue = true;
                    }
                }
                else
                {
                    if (@continue)
                    {
                        @continue = false;
                        continue;
                    }

                    if (node.Name == "integer")
                    {
                        uint value = Convert.ToUInt32(node.InnerText);
                        propertyInfo.SetValue(track, value);
                    }
                    else if (node.Name == "string")
                    {
                        propertyInfo.SetValue(track, node.InnerText);
                    }
                    else if (node.Name == "date")
                    {
                        DateTime value = DateTime.Parse(node.InnerText);
                        propertyInfo.SetValue(track, value);
                    }
                    else if (node.Name == "true")
                        propertyInfo.SetValue(track, true);
                    else if (node.Name == "false")
                        propertyInfo.SetValue(track, false);
                    else if (node.Name == "dict")
                    {
                    }
                    else if (node.Name == "array")
                    {
                    }
                }
            }
        }

        private void ProcessPlaylists(XmlNode arrayNode, Library library)
        {
            foreach (XmlNode node in arrayNode.ChildNodes)
            {
                if (node.Name == "dict")
                {
                    ProcessPlaylist(node, library);
                }
            }
        }

        private void ProcessPlaylist(XmlNode dictNode, Library library)
        {
            string propertyName = "";
            PropertyInfo propertyInfo = null;
            bool @continue = false;

            var playlist = new Playlist();
            library.Playlists.Add(playlist);

            foreach (XmlNode node in dictNode.ChildNodes)
            {
                if (node.Name == "key")
                {
                    if (@continue)
                    {
                        @continue = false;
                    }

                    propertyName = node.InnerText.Replace(" ", "");
                    propertyInfo = playlist.GetType().GetProperty(propertyName);
                    if (propertyInfo == null)
                    {
                        _message.AppendLine($"Playlist: {playlist.PlaylistID} {propertyName}");
                        @continue = true;
                    }
                }
                else
                {
                    if (@continue)
                    {
                        @continue = false;
                        continue;
                    }

                    if (node.Name == "integer")
                    {
                        uint value = Convert.ToUInt32(node.InnerText);
                        propertyInfo.SetValue(playlist, value);
                    }
                    else if (node.Name == "string")
                    {
                        propertyInfo.SetValue(playlist, node.InnerText);
                    }
                    else if (node.Name == "date")
                    {
                        DateTime value = DateTime.Parse(node.InnerText);
                        propertyInfo.SetValue(playlist, value);
                    }
                    else if (node.Name == "true")
                        propertyInfo.SetValue(playlist, true);
                    else if (node.Name == "false")
                        propertyInfo.SetValue(playlist, false);
                    else if (node.Name == "data")
                    {
                        propertyInfo.SetValue(playlist, node.InnerText);
                    }
                    else if (node.Name == "array")
                    {
                        if (propertyName == "PlaylistItems")
                        {
                            ProcessPlaylistItems(node, library, playlist);
                        }
                    }
                }
            }
        }

        private void ProcessPlaylistItems(XmlNode arrayNode, Library library, Playlist playlist)
        {
            foreach (XmlNode node in arrayNode.ChildNodes)
            {
                if (node.Name == "dict")
                {
                    ProcessPlaylistItem(node, library, playlist);
                }
            }
        }

        private void ProcessPlaylistItem(XmlNode dictNode, Library library, Playlist playlist)
        {
            string propertyName = "";

            foreach (XmlNode node in dictNode.ChildNodes)
            {
                if (node.Name == "key")
                {
                    propertyName = node.InnerText;
                    if (propertyName != "Track ID")
                    {
                        throw new Exception("key is not Track ID");
                    }
                }
                else
                {
                    if (node.Name == "integer")
                    {
                        uint value = Convert.ToUInt32(node.InnerText);

                        Track track = library.Tracks.Where(t => t.TrackID == value).FirstOrDefault();

                        if (track != null)
                        {
                            playlist.PlaylistItems.Add(track);
                            track.Playlists.Add(playlist);
                        }
                        else
                        {
                            throw new Exception("Track not found in the Library.Tracks");
                        }
                    }
                }
            }
        }
    }
}
