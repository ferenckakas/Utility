using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace Helper
{
    public class Track
    {
        [Key, Column(Order = 0)]
        public int ID { get; set; }

        [Column(Order = 1)]
        public int? TrackID { get; set; }


        public string CloudId { get; set; }

        [Visible]
        public string Ordinal { get; set; }

        public string ArtworkUrl { get; set; }

        private Image _artwork;

        [NotMapped]
        [Visible]
        public Image Artwork
        {
            get
            {
                string fileName = $@"C:\Users\Ferenc\OneDrive\Documents\My App Data\ChartParser\AppleMusic\{CloudId}.jpeg";
                return Utils.CreateImageFileIfNotExists(_artwork, fileName, ArtworkUrl?.Replace("{w}x{h}", "50x50")); //600x600
            }

            set
            {
                _artwork = value;
            }
        }


        //public string SpotifyTrackId { get; set; }

        //public string SpotifyImageUrl { get; set; }

        //private Image _spotifyImage;

        //[Visible]
        //public Image SpotifyImage
        //{
        //    get
        //    {
        //        string fileName = $@"C:\Users\Ferenc\OneDrive\Documents\My App Data\ChartParser\Spotify\{SpotifyTrackId}.jpg";
        //        return Utils.CreateImageFileIfNotExists(_spotifyImage, fileName, SpotifyImageUrl);
        //    }

        //    set
        //    {
        //        _spotifyImage = value;
        //    }
        //}


        public string VideoId { get; set; }
        public string ThumbnailUrl { get; set; }

        private Image _thumbnail;

        [NotMapped]
        [Visible]
        public Image Thumbnail
        {
            get
            {
                string fileName = $@"C:\Users\Ferenc\OneDrive\Documents\My App Data\ChartParser\Youtube\{VideoId}_default.jpg";
                return Utils.CreateImageFileIfNotExists(_thumbnail, fileName, ThumbnailUrl);
            }

            set
            {
                _thumbnail = value;
            }
        }




        [Column(Order = 44), Visible]
        public string Name { get; set; }

        [Column(Order = 45), Visible]
        public string Artist { get; set; }

        [Column(Order = 46)]
        public string AlbumArtist { get; set; }

        [Column(Order = 47)]
        public string Composer { get; set; }

        [Column(Order = 48)]
        public string Album { get; set; }

        [Column(Order = 49)]
        public string Grouping { get; set; }

        [Column(Order = 50), Visible]
        public string Genre { get; set; }


        [Column(Order = 62), Visible]
        public string Genre2 { get; set; }



        [Column(Order = 51)]
        public string Kind { get; set; }

        [Column(Order = 52), Visible]
        public string Comments { get; set; }


        [Column(Order = 2)]
        public int? Size { get; set; }

        [Column(Order = 3)]
        public int? TotalTime { get; set; }

        [Column(Order = 4)]
        public int? StartTime { get; set; }

        [Column(Order = 5)]
        public int? DiscNumber { get; set; }

        [Column(Order = 6)]
        public int? DiscCount { get; set; }

        [Column(Order = 7)]
        public int? TrackNumber { get; set; }

        [Column(Order = 8)]
        public int? TrackCount { get; set; }

        [Column(Order = 9), Visible]
        public int? Year { get; set; }


        [Column(Order = 99), Visible]
        public int? Year2 { get; set; }



        [Column(Order = 10)]
        public int? BPM { get; set; }

        [Column(Order = 11)]
        public DateTime? DateModified { get; set; }

        [Column(Order = 12)]
        public DateTime? DateAdded { get; set; }

        [Column(Order = 13)]
        public int? BitRate { get; set; }

        [Column(Order = 14)]
        public int? SampleRate { get; set; }

        [Column(Order = 15)]
        public int? VolumeAdjustment { get; set; }

        [Column(Order = 16)]
        public bool? PartOfGaplessAlbum { get; set; }

        [Column(Order = 17)]
        public int? PlayCount { get; set; }

        [Column(Order = 18)]
        public int? PlayDate { get; set; }

        [Column(Order = 19)]
        public DateTime? PlayDateUTC { get; set; }

        [Column(Order = 20)]
        public int? SkipCount { get; set; }

        [Column(Order = 21)]
        public DateTime? SkipDate { get; set; }

        [Column(Order = 22), Visible]
        public int? Rating { get; set; }

        [Column(Order = 23)]
        public bool? RatingComputed { get; set; }

        [Column(Order = 24)]
        public int? AlbumRating { get; set; }

        [Column(Order = 25)]
        public bool? AlbumRatingComputed { get; set; }

        [Column(Order = 26)]
        public bool? Compilation { get; set; }

        [Column(Order = 27)]
        public DateTime? ReleaseDate { get; set; }

        [Column(Order = 28)]
        public int? ArtworkCount { get; set; }

        [Column(Order = 29)]
        public string PersistentID { get; set; }

        [Column(Order = 30)]
        public bool? Clean { get; set; }

        [Column(Order = 31)]
        public bool? Explicit { get; set; }

        [Column(Order = 32)]
        public string TrackType { get; set; }

        [Column(Order = 33)]
        public bool? AppleMusic { get; set; }

        [Column(Order = 34)]
        public bool? Podcast { get; set; }

        [Column(Order = 35)]
        public bool? Unplayed { get; set; }

        [Column(Order = 36)]
        public bool? Movie { get; set; }

        [Column(Order = 37)]
        public bool? MusicVideo { get; set; }

        [Column(Order = 38)]
        public bool? HasVideo { get; set; }

        [Column(Order = 39)]
        public bool? Protected { get; set; }

        [Column(Order = 40)]
        public bool? Purchased { get; set; }

        [Column(Order = 41)]
        public bool? PlaylistOnly { get; set; }

        [Column(Order = 42)]
        public int? FileFolderCount { get; set; }

        [Column(Order = 43)]
        public int? LibraryFolderCount { get; set; }



        [Column(Order = 53)]
        public string SortName { get; set; }

        [Column(Order = 54)]
        public string SortArtist { get; set; }

        [Column(Order = 55)]
        public string SortAlbumArtist { get; set; }

        [Column(Order = 56)]
        public string SortComposer { get; set; }

        [Column(Order = 57)]
        public string SortAlbum { get; set; }

        [Column(Order = 58)]
        public string ContentRating { get; set; }

        [Column(Order = 59), Visible]
        public string Work { get; set; }

        [Column(Order = 60)]
        public string Location { get; set; }



        [ForeignKey("Library"), Column(Order = 61)]
        //public uint LibraryID { get; set; }
        public string LibraryPersistentID { get; set; }
        public virtual Library Library { get; set; }

        public virtual List<Playlist> Playlists { get; set; }



        public Track()
        {
            Playlists = new List<Playlist>();
        }

        public void Compare(Track Track)
        {
            PropertyInfo[] propertyInfos = typeof(Track).GetProperties();

            foreach (PropertyInfo info in propertyInfos.Where(i => i.Name != "ID" && 
                                                                   i.Name != "LibraryPersistentID" &&
                                                                   i.Name != "Library"))
            {
                object ThisValue = info.GetValue(this);
                object TrackValue = info.GetValue(Track);
                if (ThisValue != null)
                {
                    if (!ThisValue.Equals(TrackValue))
                        info.SetValue(this, TrackValue);
                }
                else
                {
                    if (TrackValue != null)
                        info.SetValue(this, TrackValue);
                }
            }
        }
    }
}
