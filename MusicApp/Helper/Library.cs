using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Helper
{
    public class Library
    {
        //[Key, Column(Order = 0)]
        //public uint ID { get; set; }

        [Column(Order = 1)]
        public int? MajorVersion { get; set; }

        [Column(Order = 2)]
        public int? MinorVersion { get; set; }

        [Column(Order = 3)]
	    public string ApplicationVersion { get; set; }

        [Column(Order = 4)]
    	public DateTime? Date { get; set; }

        [Column(Order = 5)]
    	public int? Features { get; set; }

        [Column(Order = 6)]
    	public bool? ShowContentRatings { get; set; }

        [Key, Column(Order = 7)]
        public string LibraryPersistentID { get; set; }

        public virtual List<Track> Tracks { get; set; }
        public virtual List<Playlist> Playlists { get; set; }

        [Column(Order = 8)]
        public string MusicFolder { get; set; }

        public Library()
        {
            Tracks = new List<Track>();
            Playlists = new List<Playlist>();
        }
    }
}
