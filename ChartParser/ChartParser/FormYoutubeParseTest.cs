using Common;
using Common.Models;
using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ChartParser
{
    public partial class FormYoutubeParseTest : Form
    {
        public FormYoutubeParseTest()
        {
            InitializeComponent();
        }

        private readonly string _artistTerm = "Abba";
        private readonly string _nameTerm = "Dancing";
        private readonly string _featTerm = "Betty";

        private readonly List<SearchResult> _trackResults = new List<SearchResult>()
        {
            new SearchResult { Title = "Abba - Dancing" },
            new SearchResult { Title = "Abba - Dancing (Remix)" },
            new SearchResult { Title = "Abba - Dancing (DJ Kalid mix)" },
            new SearchResult { Title = "Abba - Dancing (feat. Betty)" },
            new SearchResult { Title = "Abba - Dancing (feat. Betty) (DJ Kalid mix)" },
            new SearchResult { Title = "Abba - Dancing (feat. Betty) [DJ Kalid mix]" },
            new SearchResult { Title = "Abba - Dancing (Queen)" },
            new SearchResult { Title = "Abba - Dancing (Queen)(Remix)" },
            new SearchResult { Title = "Abba - Dancing (Queen) (DJ Kalid mix)" },
            new SearchResult { Title = "Abba - Dancing (Queen) (feat. Betty)" },
            new SearchResult { Title = "Abba - Dancing (Queen) (feat. Betty)(DJ Kalid mix)" },
            new SearchResult { Title = "Abba - Dancing (Queen) (feat. Betty)[DJ Kalid mix]" },
            new SearchResult { Title = "Abba - Dancing Remix" },
            new SearchResult { Title = "Abba - Dancing DJ Kalid mix" },
            new SearchResult { Title = "Abba - Dancing feat. Betty" },
            new SearchResult { Title = "Abba - Dancing feat. Betty DJ Kalid mix" },
            new SearchResult { Title = "Abba - Dancing feat. Betty DJ Kalid mix" },
            new SearchResult { Title = "Abba - Dancing (Queen)" },
            new SearchResult { Title = "Abba - Dancing (Queen) Remix" },
            new SearchResult { Title = "Abba - Dancing (Queen) DJ Kalid mix" },
            new SearchResult { Title = "Abba - Dancing (Queen) feat. Betty" },
            new SearchResult { Title = "Abba - Dancing (Queen) feat. Betty DJ Kalid mix" },
            new SearchResult { Title = "Abba - Dancing (Queen) feat. Betty DJ Kalid mix" },

            new SearchResult { Title = "Dancing|Abba" },
            new SearchResult { Title = "Dancing|Abba (Remix)" },
            new SearchResult { Title = "Dancing|Abba (DJ Kalid mix)" },
            new SearchResult { Title = "Dancing|Abba (feat. Betty)" },
            new SearchResult { Title = "Dancing|Abba (feat. Betty) (DJ Kalid mix)" },
            new SearchResult { Title = "Dancing|Abba (feat. Betty) [DJ Kalid mix]" },
            new SearchResult { Title = "Dancing (Queen)|Abba" },
            new SearchResult { Title = "Dancing (Queen)|Abba (Remix)" },
            new SearchResult { Title = "Dancing (Queen)|Abba (DJ Kalid mix)" },
            new SearchResult { Title = "Dancing (Queen)|Abba (feat. Betty)" },
            new SearchResult { Title = "Dancing (Queen)|Abba (feat. Betty)(DJ Kalid mix)" },
            new SearchResult { Title = "Dancing (Queen)|Abba (feat. Betty)[DJ Kalid mix]" },
            new SearchResult { Title = "Dancing|Abba Remix" },
            new SearchResult { Title = "Dancing|Abba DJ Kalid mix" },
            new SearchResult { Title = "Dancing|Abba feat. Betty" },
            new SearchResult { Title = "Dancing|Abba feat. Betty DJ Kalid mix" },
            new SearchResult { Title = "Dancing|Abba feat. Betty DJ Kalid mix" },
            new SearchResult { Title = "Dancing (Queen)|Abba" },
            new SearchResult { Title = "Dancing (Queen)|Abba Remix" },
            new SearchResult { Title = "Dancing (Queen)|Abba DJ Kalid mix" },
            new SearchResult { Title = "Dancing (Queen)|Abba feat. Betty" },
            new SearchResult { Title = "Dancing (Queen)|Abba feat. Betty DJ Kalid mix" },
            new SearchResult { Title = "Dancing (Queen)|Abba feat. Betty DJ Kalid mix" }
        };

        private void FormYoutubeParseTest_Load(object sender, EventArgs e)
        {
            BuildColumnSelector();

            this.toolStripTextBox1.Text = _artistTerm;
            this.toolStripTextBox2.Text = _nameTerm;
            this.toolStripTextBox3.Text = _featTerm;
            this.toolStripTextBox4.Text = "Abba - Dancing (Queen) (feat. Betty)[DJ Kalid mix]";

            toolStripButtonCreateYoutubePlaylist_Click(null, null);
        }

        private void BuildColumnSelector()
        {
            for (var i = 0; i < this.dataGridViewYoutubeResults.Columns.Count; i++)
            {
                DataGridViewColumn column = this.dataGridViewYoutubeResults.Columns[i];
                column.Name = $"{column.Name.Substring(0, column.Name.IndexOf("DataGridView"))}Column";
            }

            this.thumbnailDataGridViewImageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;

            this.thumbnailDataGridViewImageColumn.HeaderText = "";
            this.ordinalDataGridViewTextBoxColumn.HeaderText = "";

            this.dataGridViewYoutubeResults.Columns["thumbnailUrlColumn"].Visible = false;
            this.dataGridViewYoutubeResults.Columns["videoIdColumn"].Visible = false;
            this.dataGridViewYoutubeResults.Columns["channelIdColumn"].Visible = false;
        }



        private void toolStripButtonCreateYoutubePlaylist_Click(object sender, EventArgs e)
        {
            var youtubeTracks = new List<YoutubeTrack>();

            var videoIds = new List<string>();

            var Tracks = new List<Track>() {
                        new Track
                        {
                            Artist = _artistTerm,
                            Name = _nameTerm
                        }
                    };

            List<SongTitle> songs = Global.Mapper.Map<List<Track>, List<SongTitle>>(Tracks);
            string feat = "";

            int i = 1;
            foreach (SongTitle song in songs)
            {
                string artist = song.Artist.RemoveAndFeat();

                string title = song.Name.Replace(",", " ");

                string term = $"{artist} {feat} {title}";

                youtubeTracks.Add(new YoutubeTrack
                {
                    Ordinal = i.ToString(),
                    Name = song.Name,
                    Artist = song.Artist,
                    Feat = _featTerm
                });

                List<SearchResult> results = _trackResults;

                if (results.Any())
                {
                    var ytracks = new List<YoutubeTrack>();

                    foreach (SearchResult searchResult in _trackResults)
                    {
                        var resultTrack = new YoutubeTrack
                        {
                            Title = searchResult.Title,
                            VideoId = searchResult.VideoId,
                            ThumbnailUrl = searchResult.ThumbnailUrl,
                            ChannelId = searchResult.ChannelId,
                            ChannelTitle = searchResult.ChannelTitle,
                            Channel = new YoutubeChannel(searchResult.ChannelId, searchResult.ChannelTitle)
                            //Ordinal = searchResult == track ? "=" : "",
                        };

                        resultTrack.Decompose(_artistTerm, _nameTerm, _featTerm, "");

                        resultTrack.Name = resultTrack.Name != null ? $"¤{resultTrack.Name}¤" : null;
                        resultTrack.Artist = resultTrack.Artist != null ? $"¤{resultTrack.Artist}¤" : null;
                        resultTrack.Feat = resultTrack.Feat != null ? $"¤{resultTrack.Feat}¤" : null;
                        resultTrack.SongVersion = resultTrack.SongVersion != null ? $"¤{resultTrack.SongVersion}¤" : null;
                        resultTrack.VideoVersion = resultTrack.VideoVersion != null ? $"¤{resultTrack.VideoVersion}¤" : null;
                        resultTrack.Official = resultTrack.Official != null ? $"¤{resultTrack.Official}¤" : null;

                        ytracks.Add(resultTrack);
                    }

                    YoutubeTrack track = null;


                    ytracks.Filter("FF", ref track, t => true, videoIds);

                    youtubeTracks.AddRange(ytracks);
                }

                i++;
            }

            this.dataGridViewYoutubeResults.DataSource = null;
            this.dataGridViewYoutubeResults.DataSource = youtubeTracks;
        }

        private void toolStripTextBox4_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            string artistTerm = this.toolStripTextBox1.Text;
            string nameTerm = this.toolStripTextBox2.Text;
            string featTerm = this.toolStripTextBox3.Text;
            string title = this.toolStripTextBox4.Text;

            var newTracks = new List<YoutubeTrack>();

            var origTrack = new YoutubeTrack
            {
                Ordinal = "1",
                Name = nameTerm,
                Artist = artistTerm,
                Feat = featTerm
            };

            newTracks.Add(origTrack);

            var resultTrack = new YoutubeTrack
            {
                Title = title//,
                //VideoId = searchResult.VideoId,
                //ThumbnailUrl = searchResult.ThumbnailUrl,
                //ChannelId = searchResult.ChannelId,
                //ChannelTitle = searchResult.ChannelTitle,
                //Ordinal = searchResult == track ? "=" : "",
            };

            //string artist = result.YSearchActs[0].Artist;
            //string title = result.YSearchActs[0].Title;

            resultTrack.Decompose(artistTerm, nameTerm, featTerm, "");

            resultTrack.Name = resultTrack.Name != null ? $"¤{resultTrack.Name}¤" : null;
            resultTrack.Artist = resultTrack.Artist != null ? $"¤{resultTrack.Artist}¤" : null;
            resultTrack.Feat = resultTrack.Feat != null ? $"¤{resultTrack.Feat}¤" : null;
            resultTrack.SongVersion = resultTrack.SongVersion != null ? $"¤{resultTrack.SongVersion}¤" : null;
            resultTrack.VideoVersion = resultTrack.VideoVersion != null ? $"¤{resultTrack.VideoVersion}¤" : null;
            resultTrack.Official = resultTrack.Official != null ? $"¤{resultTrack.Official}¤" : null;

            newTracks.Add(resultTrack);

            this.dataGridViewYoutubeResults.DataSource = null;
            this.dataGridViewYoutubeResults.DataSource = newTracks;
        }
    }
}
