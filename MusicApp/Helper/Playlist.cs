using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Helper
{
    public class Playlist
    {
        [Key, Column(Order = 0)]
        public int ID { get; set; }

        [Column(Order = 1)]
        public bool? Master { get; set; }

        [Column(Order = 2)]
        public int? PlaylistID { get; set; }

        [Column(Order = 3)]
        public string ParentPersistentID { get; set; }

        [Column(Order = 4)]
        public string PlaylistPersistentID { get; set; }

        [Column(Order = 5)]
        public int? DistinguishedKind { get; set; }

        [Column(Order = 6)]
        public bool? Music { get; set; }

        [Column(Order = 7)]
        public bool? Movies { get; set; }

        [Column(Order = 8)]
        public bool? TVShows { get; set; }

        [Column(Order = 9)]
        public bool? Podcasts { get; set; }

        [Column(Order = 10)]
        public bool? iTunesU { get; set; }

        [Column(Order = 11)]
        public bool? Audiobooks { get; set; }

        [Column(Order = 12)]
        public bool? Books { get; set; }

        [Column(Order = 13)]
        public bool? PurchasedMusic { get; set; }

        [Column(Order = 14)]
        public bool? AllItems { get; set; }

        [Column(Order = 15)]
        public bool? Visible { get; set; }

        [Column(Order = 16)]
        public bool? Folder { get; set; }

        [Column(Order = 17)]
        public string Name { get; set; }

        [Column(Order = 18)]
        public string Description { get; set; }

        [Column(Order = 19)]
        public string SmartInfo { get; set; }

        [Column(Order = 20)]
        public string SmartCriteria { get; set; }

        public virtual List<int> PlaylistItemIDs { get; set; }
        public virtual List<Track> PlaylistItems { get; set; }

        [ForeignKey("Library"), Column(Order = 21)]
        //public uint LibraryID { get; set; }
        public string LibraryPersistentID { get; set; }
        public virtual Library Library { get; set; }


        public string CloudId { get; set; }


        public Playlist()
        {
            PlaylistItemIDs = new List<int>();
            //PlaylistItems = new List<Track>();
        }
    }
}
