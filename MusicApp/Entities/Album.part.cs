using Common;
using System.Linq;

namespace Entities
{
    public partial class Album
    {
        public static Album GetAlbum(Artist artist, string name)
        {
            using (var entities = new MusicEntities())
            {
                Album album = artist.Albums.FirstOrDefault(a => a == new Album() { Name = name });
                if (album == null)
                {
                    album = new Album();
                }

                return album;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            Album a = (Album)obj;
            string aName = a.Name.ToLower().Replace(" ", "").RemoveSigns();
            string tName = Name.ToLower().Replace(" ", "").RemoveSigns();
            if (!aName.Equals(tName)) return false;
            return true;
        }
    }
}
