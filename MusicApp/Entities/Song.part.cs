using Common;
using System.Linq;

namespace Entities
{
    public partial class Song
    {
        public static Song GetSong(Album album, string title)
        {
            using (var entities = new MusicEntities())
            {
                AlbumSong albumSong = album.AlbumSongs.FirstOrDefault(a => a.Song == new Song() { Title = title });
                Song song;
                if (albumSong == null)
                {
                    song = new Song();
                    albumSong = new AlbumSong();
                   //albumSong.Album = album;
                    albumSong.Song = song;
                    //albumSong.Number =
                    album.AlbumSongs.Add(albumSong);
                }
                else
                    song = albumSong.Song;

                return song;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            Song a = (Song)obj;
            string aName = a.Title.ToLower().Replace(" ", "").RemoveSigns();
            string tName = Title.ToLower().Replace(" ", "").RemoveSigns();
            if (!aName.Equals(tName)) return false;
            return true;
        }
    }
}
