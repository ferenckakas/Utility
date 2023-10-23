using Common;
using System.Drawing;

namespace Helper
{
    public class SpotifyTrack
    {
        public string Ordinal { get; set; }
        public string VideoId { get; set; }
        public string ThumbnailUrl { get; set; }

        private Image _thumbnail;

        public Image Thumbnail
        {
            get
            {
                string fileName = $@"C:\Users\Ferenc\OneDrive\Documents\My App Data\ChartParser\Spotify\{VideoId}_default.jpg";
                return Utils.CreateImageFileIfNotExists(_thumbnail, fileName, ThumbnailUrl);
            }

            set
            {
                _thumbnail = value;
            }
        }

        public string Title { get; set; }
        public string Artist { get; set; }
        public string Name { get; set; }
        public string Feat { get; set; }
        public string SongVersion { get; set; }
        public string VideoVersion { get; set; }
        public string Official { get; set; }

        public bool IsMix
        {
            get
            {
                return SongVersion != null &&
                       !SongVersion.ToLower()._ToWords().Contains("radio") &&
                       !SongVersion.ToLower().Contains("explicit version") &&
                       !SongVersion.ToLower().Contains("main version") &&
                       !SongVersion.ToLower().Contains("uk version") &&
                       !SongVersion.ToLower().Contains("us version") &&
                       !SongVersion.ToLower()._ToWords().Contains("video");
            }
        }
    }
}
