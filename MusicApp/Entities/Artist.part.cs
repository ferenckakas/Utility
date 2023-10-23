using Common;
using System.Linq;

namespace Entities
{
    public partial class Artist
    {
        public static Artist GetArtist(string name)
        {
            using (var entities = new MusicEntities())
            {
                Artist artist = entities.Artists.FirstOrDefault(a => a == new Artist() { Name = name });
                if (artist == null)
                {
                    artist = new Artist();
                }

                return artist;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            Artist a = (Artist)obj;
            string aName = a.Name.ToLower().Replace(" ", "").RemoveSigns();
            string tName = Name.ToLower().Replace(" ", "").RemoveSigns();
            if (!aName.Equals(tName)) return false;
            return true;
        }
    }
}
