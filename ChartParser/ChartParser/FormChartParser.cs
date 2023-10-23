using Common;
using Helper;
using Services.AppleMusic;
using Services.Chart;
using Services.Discogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Discogs = Services.Discogs.Models;

namespace ChartParser
{
    public partial class FormChartParser : Form
    {
        private readonly AppleMusicClient _appleMusicClient = new AppleMusicClient();
        private readonly DiscogsClient _discogsClient = new DiscogsClient();

        private List<CatalogTrack> _catalogTracks = new List<CatalogTrack>();

        private List<CSong> _songs;

        private readonly string _logDir = $@"C:\Users\Ferenc\OneDrive\Documents\My App Data\ChartParser\Log";

        private CChartBase _chart;

        private readonly int _toYear = 2022;

        private readonly TreeNode _nodeNow = new TreeNode
        {
            Text = "Now"
        };

        private readonly TreeNode _nodeBravoHits = new TreeNode
        {
            Text = "Bravo Hits"
        };

        private readonly TreeNode _nodeClubRotation = new TreeNode
        {
            Text = "Club Rotation"
        };

        private readonly TreeNode _nodeClubSounds = new TreeNode
        {
            Text = "Club Sounds"
        };

        public FormChartParser()
        {
            InitializeComponent();

            this.artworkDataGridViewImageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            this.artworkDataGridViewImageColumn.HeaderText = "";
            this.ordinalDataGridViewTextBoxColumn.HeaderText = "";

            var nodeMAHASZ = new TreeNode
            {
                Text = "MAHASZ"
            };

            this.treeView.Nodes.Add(nodeMAHASZ);

            var nodeMAHASZLatest = new TreeNode
            {
                Text = "Latest"
            };

            nodeMAHASZ.Nodes.Add(nodeMAHASZLatest);

            var chartNodes = new List<ChartNode>();

            chartNodes.Add(new ChartNode
            {
                Type = "MAHASZ Latest",
                Text = "MAHASZ Dance Top 40",
                Url = "https://slagerlistak.hu/dance-top-40-slagerlista",
                FileName = "MAHASZ_DanceTop40"
            });

            chartNodes.Add(new ChartNode
            {
                Type = "MAHASZ Latest",
                Text = "MAHASZ Radio Top 40",
                Url = "https://slagerlistak.hu/radios-top-40-slagerlista",
                FileName = "MAHASZ_RadioTop40"
            });

            chartNodes.Add(new ChartNode
            {
                Type = "MAHASZ Latest",
                Text = "MAHASZ Magyar Radio Top 40",
                Url = "https://slagerlistak.hu/magyar-radios-top-40-slagerlista",
                FileName = "MAHASZ_MagyarRadioTop40"
            });

            chartNodes.Add(new ChartNode
            {
                Type = "MAHASZ Latest",
                Text = "MAHASZ Editor's Choice Radio Top 40",
                Url = "https://slagerlistak.hu/editors-choice-top-40-slagerlista",
                FileName = "MAHASZ_EditorsChoiceRadioTop40"
            });

            chartNodes.Add(new ChartNode
            {
                Type = "MAHASZ Latest",
                Text = "MAHASZ Single Top 40",
                Url = "https://slagerlistak.hu/single-top-40-slagerlista",
                FileName = "MAHASZ_SingleTop40"
            });

            chartNodes.Add(new ChartNode
            {
                Type = "MAHASZ Latest",
                Text = "MAHASZ Stream Top 40",
                Url = "https://slagerlistak.hu/stream-top-40-slagerlista",
                FileName = "MAHASZ_StreamTop40"
            });

            foreach (ChartNode chartNode in chartNodes)
            {
                var node = new TreeNode
                {
                    Text = chartNode.Text,
                    Tag = chartNode
                };

                nodeMAHASZLatest.Nodes.Add(node);
            }


            var nodeMAHASZAggregated = new TreeNode
            {
                Text = "Aggregated"
            };

            nodeMAHASZ.Nodes.Add(nodeMAHASZAggregated);


            var nodeMAHASZAggregatedDanceTop100 = new TreeNode
            {
                Text = "Dance Top 100"
            };

            nodeMAHASZAggregated.Nodes.Add(nodeMAHASZAggregatedDanceTop100);

            for (var year = 2006; year <= _toYear; year++)
            {
                var node = new TreeNode
                {
                    Text = year.ToString(),
                    Tag = new ChartNode
                    {
                        Type = "MAHASZ Aggregated",
                        Text = $"MAHASZ Dance Top 100 ({year})",
                        Url = $"https://slagerlistak.hu/archivum/eves-osszesitett-listak/dance/{year}",
                        FileName = ""
                    }
                };
                nodeMAHASZAggregatedDanceTop100.Nodes.Add(node);
            }


            var nodeMAHASZAggregatedRadioTop100 = new TreeNode
            {
                Text = "Radio Top 100"
            };

            nodeMAHASZAggregated.Nodes.Add(nodeMAHASZAggregatedRadioTop100);

            for (var year = 2004; year <= _toYear; year++)
            {
                var node = new TreeNode
                {
                    Text = year.ToString(),
                    Tag = new ChartNode
                    {
                        Type = "MAHASZ Aggregated",
                        Text = $"MAHASZ Radio Top 100 ({year})",
                        Url = $"https://slagerlistak.hu/archivum/eves-osszesitett-listak/radios/{year}",
                        FileName = ""
                    }
                };
                nodeMAHASZAggregatedRadioTop100.Nodes.Add(node);
            }


            var nodeMAHASZAggregatedMagyarRadioTop1000 = new TreeNode
            {
                Text = "Magyar Radio Top 100"
            };

            nodeMAHASZAggregated.Nodes.Add(nodeMAHASZAggregatedMagyarRadioTop1000);

            for (var year = 2017; year <= _toYear; year++)
            {
                var node = new TreeNode
                {
                    Text = year.ToString(),
                    Tag = new ChartNode
                    {
                        Type = "MAHASZ Aggregated",
                        Text = $"MAHASZ Magyar Radio Top 100 ({year})",
                        Url = $"https://slagerlistak.hu/archivum/eves-osszesitett-listak/radios_magyar/{year}",
                        FileName = ""
                    }
                };
                nodeMAHASZAggregatedMagyarRadioTop1000.Nodes.Add(node);
            }


            var nodeMAHASZAggregatedEditorsChoiceRadioTop100 = new TreeNode
            {
                Text = "Editor's Choice Radio Top 100"
            };

            nodeMAHASZAggregated.Nodes.Add(nodeMAHASZAggregatedEditorsChoiceRadioTop100);

            for (var year = 2004; year <= _toYear; year++)
            {
                var node = new TreeNode
                {
                    Text = year.ToString(),
                    Tag = new ChartNode
                    {
                        Type = "MAHASZ Aggregated",
                        Text = $"MAHASZ Editor's Choice Radio Top 100 ({year})",
                        Url = $"https://slagerlistak.hu/archivum/eves-osszesitett-listak/editor/{year}",
                        FileName = ""
                    }
                };
                nodeMAHASZAggregatedEditorsChoiceRadioTop100.Nodes.Add(node);
            }


            var nodeMAHASZAggregatedSingleTop100ByCopiesSold = new TreeNode
            {
                Text = "Single Top 100 (By Copies sold)"
            };

            nodeMAHASZAggregated.Nodes.Add(nodeMAHASZAggregatedSingleTop100ByCopiesSold);

            for (var year = 2014; year <= _toYear; year++)
            {
                var node = new TreeNode
                {
                    Text = year.ToString(),
                    Tag = new ChartNode
                    {
                        Type = "MAHASZ Aggregated",
                        Text = $"MAHASZ Single Top 100 (By Copies sold) ({year})",
                        Url = $"https://slagerlistak.hu/archivum/eves-osszesitett-listak/single_db/{year}",
                        FileName = ""
                    }
                };
                nodeMAHASZAggregatedSingleTop100ByCopiesSold.Nodes.Add(node);
            }


            var nodeMAHASZAggregatedSingleTop100ByPlace = new TreeNode
            {
                Text = "Single Top 100 (By Place)"
            };

            nodeMAHASZAggregated.Nodes.Add(nodeMAHASZAggregatedSingleTop100ByPlace);

            for (var year = 2014; year <= _toYear; year++)
            {
                var node = new TreeNode
                {
                    Text = year.ToString(),
                    Tag = new ChartNode
                    {
                        Type = "MAHASZ Aggregated",
                        Text = $"MAHASZ Single Top 100 (By Place) ({year})",
                        Url = $"https://slagerlistak.hu/archivum/eves-osszesitett-listak/single_chart/{year}",
                        FileName = ""
                    }
                };
                nodeMAHASZAggregatedSingleTop100ByPlace.Nodes.Add(node);
            }


            var nodeMAHASZAggregatedStreamTop100ByCopiesSold = new TreeNode
            {
                Text = "Stream Top 100 (By Copies sold)"
            };

            nodeMAHASZAggregated.Nodes.Add(nodeMAHASZAggregatedStreamTop100ByCopiesSold);

            for (var year = 2019; year <= _toYear; year++)
            {
                var node = new TreeNode
                {
                    Text = year.ToString(),
                    Tag = new ChartNode
                    {
                        Type = "MAHASZ Aggregated",
                        Text = $"MAHASZ Stream Top 100 (By Copies sold) ({year})",
                        Url = $"https://slagerlistak.hu/archivum/eves-osszesitett-listak/stream_db/{year}",
                        FileName = ""
                    }
                };
                nodeMAHASZAggregatedStreamTop100ByCopiesSold.Nodes.Add(node);
            }


            var nodeMAHASZAggregatedStreamTop100ByPlace = new TreeNode
            {
                Text = "Stream Top 100 (By Place)"
            };

            nodeMAHASZAggregated.Nodes.Add(nodeMAHASZAggregatedStreamTop100ByPlace);

            for (var year = 2014; year <= _toYear; year++)
            {
                var node = new TreeNode
                {
                    Text = year.ToString(),
                    Tag = new ChartNode
                    {
                        Type = "MAHASZ Aggregated",
                        Text = $"MAHASZ Stream Top 100 (By Place) ({year})",
                        Url = $"https://slagerlistak.hu/archivum/eves-osszesitett-listak/stream/{year}",
                        FileName = ""
                    }
                };
                nodeMAHASZAggregatedStreamTop100ByPlace.Nodes.Add(node);
            }

            this.treeView.Nodes.Add(_nodeNow);
            this.treeView.Nodes.Add(_nodeBravoHits);
            this.treeView.Nodes.Add(_nodeClubRotation);
            this.treeView.Nodes.Add(_nodeClubSounds);
        }

        private async Task LoadNodes(TreeNode parentNode, string type, int labelId, string contains)
        {
            List<Discogs.Release> releases = await _discogsClient.GetReleasesByLabel(labelId);

            releases = releases.Where(r => r.Status == "Accepted" && r.Title.Contains(contains) && (r.Format.Contains("2xCD, Comp") || r.Format.Contains("2xLP, Comp"))).ToList(); //.Sort(new Comparison<Release>());

            foreach (Discogs.Release release in releases)
            {
                var node = new TreeNode
                {
                    Text = release.Title,
                    Tag = new ChartNode
                    {
                        Type = type,
                        Text = release.Title,
                        Url = release.ResourceUrl,
                        FileName = ""
                    }
                };
                parentNode.Nodes.Add(node);
            }
        }

        private async void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                string text = e.Node.Text;
                e.Node.Text += " (loading ...)";
                if (text == "Now")
                {
                    await LoadNodes(_nodeNow, text, 563691, "Music"); //Now-Thats-What-I-Call-Music!"
                }
                else if (text == "Bravo Hits")
                {
                    await LoadNodes(_nodeBravoHits, text, 293910, "");
                }
                else if (text == "Club Rotation")
                {
                    await LoadNodes(_nodeClubRotation, text, 343401, "");
                }
                else if (text == "Club Sounds")
                {
                    await LoadNodes(_nodeClubSounds, text, 350516, "");
                }
                e.Node.Text = text;

                return;
            }


            _catalogTracks = new List<CatalogTrack>();

            var node = (ChartNode)e.Node.Tag;

            if (node == null)
                return;

            string[] labels = { "Now", "Bravo Hits", "Club Rotation", "Club Sounds" };

            if (labels.Contains(node.Type)) _chart = new DiscogsRelease(node.Type);
            else
            {
                if (node.Type == "MAHASZ Latest") _chart = new HUMAHASZLatest();
                else if (node.Type == "MAHASZ Aggregated") _chart = new HUMAHASZAggregated();
                else return;
                this.webBrowser.Navigate(node.Url);
            }

            _songs = await _chart.GetChart(((ChartNode)this.treeView.SelectedNode.Tag).Text, node.Url);

            int i = 1;
            int artistMaxLength = 0;
            int titleMaxLength = 0;
            bool timeExists = false;
            foreach (CSong song in _songs)
            {
                //var artist = song.sArtist.Replace(",", " ").Replace(" & ", " ").Replace(" x ", " ")
                //    .Replace(" feat. ", " ").Replace(" feat ", " ").Replace(" featuring ", " ").Replace(" vs. ", " ");

                //var title = song.sTitle.Replace(",", " ");

                //var line = $"{artist},{title}";

                song.Artist = song.Artist.Replace("\t", " ").RemoveMultipleSpaces();
                song.Title = song.Title.Replace("\t", " ").RemoveMultipleSpaces();

                _catalogTracks.Add(new CatalogTrack
                {
                    Ordinal = i++.ToString(),
                    RawName = song.Title,
                    RawArtist = song.Artist,
                    TotalTime = song.TotalTime
                });

                if (song.Artist.Length > artistMaxLength) artistMaxLength = song.Artist.Length;
                if (song.Title.Length > titleMaxLength) titleMaxLength = song.Title.Length;
                if (song.TotalTime != null) timeExists = true;
            }

            var sb = new StringBuilder();
            foreach (CSong song in _songs)
            {
                if (timeExists)
                    sb.AppendLine(string.Format($"{{0,{-artistMaxLength}}}\t{{1,{-titleMaxLength}}}\t{{2}}", song.Artist, song.Title, song.DisplayTime));
                else
                    sb.AppendLine(string.Format($"{{0,{-artistMaxLength}}}\t{{1}}", song.Artist, song.Title)); 
            }

            if (!File.Exists(_chart.FileName))
            {
                string dir = Path.GetDirectoryName(_chart.FileName);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(_chart.FileName, sb.ToString());
            }

            this.dataGridViewResults.DataSource = null;
            this.dataGridViewResults.DataSource = _catalogTracks;
        }

        private async void toolStripButtonCreateITunesPlaylist_Click(object sender, EventArgs e)
        {
            _catalogTracks = new List<CatalogTrack>();

            //var playlistName = ((ChartNode)this.treeView.SelectedNode.Tag).Text + " " + $"(Week {chart.Year.Trim()}/{chart.Week.Trim()})";
            string playlistName = _chart.Name;

            var songIds = new List<string>();

            int maxHitResult = 0;
            int maxHit = 2;
            if (int.TryParse(this.toolStripTextBoxMaxHit.Text, out maxHitResult))
                maxHit = maxHitResult;

            List<CSong> songs = _songs;
            toolStripProgressBar.Maximum = songs.Count + 1;

            int i = 1;
            foreach (CSong song in songs)
            {
                //string artist = song.sArtist.Replace(",", " ").Replace(" & ", " ").Replace(" x ", " ")
                //                         .Replace(" feat. ", " ").Replace(" feat ", " ").Replace(" featuring ", " ").Replace(" vs. ", " ");

                _catalogTracks.Add(new CatalogTrack
                {
                    Ordinal = $"{i}.",
                    RawName = song.Title,
                    RawArtist = song.Artist
                });

                string artist = song.Artist.RemoveAndFeat();
                string title = song.Title.Replace(",", " ");
                if (title.IndexOf("[") > -1) title = title.Substring(0, title.IndexOf("[")).Trim();
                if (title.IndexOf("(") > -1) title = title.Substring(0, title.IndexOf("(")).Trim();

                uint? time = song.TotalTime;

                //string term = $"{artist} {title}"; //.Replace(" ", "+");

                List<Services.AppleMusic.Models.Song> results = await _appleMusicClient.SearchSongs($"{artist} {title}", maxHit, maxHit);

                _catalogTracks.Add(new CatalogTrack
                {
                    Ordinal = results.Count.ToString(),
                    RawName = title,
                    RawArtist = artist
                });

                if (!results.Any())
                {
                    artist = artist._ToWords()[0];
                    results = await _appleMusicClient.SearchSongs($"{artist} {title}", maxHit, maxHit);

                    _catalogTracks.Add(new CatalogTrack
                    {
                        Ordinal = results.Count.ToString(),
                        RawName = title,
                        RawArtist = artist
                    });
                }

                //string missingId = null;
                //if (!results.Any())
                //{
                //    artist = "Conal Flood";
                //    title = "Missing Track";
                //    results = await appleMusicClient.SearchSongs($"{artist} {title}", maxHit, maxHit);

                //    if (results.Any())
                //        missingId = results[0].Id;

                //    this.catalogTracks.Add(new CatalogTrack
                //    {
                //        Ordinal = results.Count.ToString(),
                //        RawName = title,
                //        RawArtist = artist
                //    });
                //}

                //if (results.Any())
                //{
                    List<CatalogTrack> ctracks0 = Global.Mapper.Map<List<Services.AppleMusic.Models.Song>, List<CatalogTrack>>(results);

                    var ctracks = new List<CatalogTrack>();

                    ctracks0.ForEach(t => {
                        /*if (time == null || (t.TotalTime != null && (Math.Abs((int)(t.TotalTime - time))) < 3))*/
                            ctracks.Add(t);
                    });

                    if (!ctracks.Any())
                    {
                        artist = "Conal Flood";
                        title = "Missing Track";
                        results = await _appleMusicClient.SearchSongs($"{artist} {title}", maxHit, maxHit);

                        _catalogTracks.Add(new CatalogTrack
                        {
                            Ordinal = results.Count.ToString(),
                            RawName = title,
                            RawArtist = artist
                        });

                        ctracks = Global.Mapper.Map<List<Services.AppleMusic.Models.Song>, List<CatalogTrack>>(results);
                    }

                    foreach (CatalogTrack ctrack in ctracks)
                    {
                        ctrack.Decompose(title);
                    }

                    title = title.ToLower();
                    artist = artist.ToLower();

                    CatalogTrack track = null;

                //ctracks.Filter(ref track, t => t.Name.ToLower() == title && t.Artist.ToLower() == artist && t.Album.ToLower() == $"{title} - single", songIds);
                //ctracks.Filter(ref track, t => t.Name.ToLower() == title && t.Artist.ToLower() == artist && t.Album.ToLower().Contains("- single"), songIds);
                //ctracks.Filter(ref track, t => t.Name.ToLower() == title && t.Artist.ToLower() == artist, songIds);

                //ctracks.Filter(ref track, t => t.Name.ToLower() == title && t.Artist._WordMatchMutual(artist) && t.Album.ToLower() == $"{title} - single", songIds);
                //ctracks.Filter(ref track, t => t.Name.ToLower() == title && t.Artist._WordMatchMutual(artist) && t.Album.ToLower().Contains("- single"), songIds);
                //ctracks.Filter(ref track, t => t.Name.ToLower() == title && t.Artist._WordMatchMutual(artist), songIds);

                //ctracks.Filter(ref track, t => t.Name.ToLower() == title && t.Artist._WordMatchAny(artist) && t.Album.ToLower() == $"{title} - single", songIds);
                //ctracks.Filter(ref track, t => t.Name.ToLower() == title && t.Artist._WordMatchAny(artist) && t.Album.ToLower().Contains("- single"), songIds);
                //ctracks.Filter(ref track, t => t.Name.ToLower() == title && t.Artist._WordMatchAny(artist), songIds);


                if (true)
                {
                    ctracks.Filter("01", ref track, t => t.Name._Match(title) && !t.IsMix && t.Artist._Match(artist) && t.Album.ToLower() == $"{title} - single", songIds);
                    ctracks.Filter("02", ref track, t => t.Name._Match(title) && !t.IsMix && t.Artist._Match(artist) && t.Album.ToLower().Contains("- single"), songIds);
                    ctracks.Filter("03", ref track, t => t.Name._Match(title) && !t.IsMix && t.Artist._Match(artist), songIds);

                    ctracks.Filter("04", ref track, t => t.Name._Match(title) && !t.IsMix && t.Artist._WordMatchMutual(artist) && t.Album.ToLower() == $"{title} - single", songIds);
                    ctracks.Filter("05", ref track, t => t.Name._Match(title) && !t.IsMix && t.Artist._WordMatchMutual(artist) && t.Album.ToLower().Contains("- single"), songIds);
                    ctracks.Filter("06", ref track, t => t.Name._Match(title) && !t.IsMix && t.Artist._WordMatchMutual(artist), songIds);

                    ctracks.Filter("07", ref track, t => t.Name._Match(title) && !t.IsMix && t.Artist._WordMatchAny(artist) && t.Album.ToLower() == $"{title} - single", songIds);
                    ctracks.Filter("08", ref track, t => t.Name._Match(title) && !t.IsMix && t.Artist._WordMatchAny(artist) && t.Album.ToLower().Contains("- single"), songIds);
                    ctracks.Filter("09", ref track, t => t.Name._Match(title) && !t.IsMix && t.Artist._WordMatchAny(artist), songIds);


                    ctracks.Filter("10", ref track, t => t.Name._Match(title) && t.Artist._Match(artist) && t.Album.ToLower() == $"{title} - single", songIds);
                    ctracks.Filter("11", ref track, t => t.Name._Match(title) && t.Artist._Match(artist) && t.Album.ToLower().Contains("- single"), songIds);
                    ctracks.Filter("12", ref track, t => t.Name._Match(title) && t.Artist._Match(artist), songIds);

                    ctracks.Filter("13", ref track, t => t.Name._Match(title) && t.Artist._WordMatchMutual(artist) && t.Album.ToLower() == $"{title} - single", songIds);
                    ctracks.Filter("14", ref track, t => t.Name._Match(title) && t.Artist._WordMatchMutual(artist) && t.Album.ToLower().Contains("- single"), songIds);
                    ctracks.Filter("15", ref track, t => t.Name._Match(title) && t.Artist._WordMatchMutual(artist), songIds);

                    ctracks.Filter("16", ref track, t => t.Name._Match(title) && t.Artist._WordMatchAny(artist) && t.Album.ToLower() == $"{title} - single", songIds);
                    ctracks.Filter("17", ref track, t => t.Name._Match(title) && t.Artist._WordMatchAny(artist) && t.Album.ToLower().Contains("- single"), songIds);
                    ctracks.Filter("18", ref track, t => t.Name._Match(title) && t.Artist._WordMatchAny(artist), songIds);

                    ctracks.Filter("19", ref track, t => t.Name._Match(title) && t.Artist._WordMatchPartial(artist) && t.Album.ToLower() == $"{title} - single", songIds);
                    ctracks.Filter("20", ref track, t => t.Name._Match(title) && t.Artist._WordMatchPartial(artist) && t.Album.ToLower().Contains("- single"), songIds);
                    ctracks.Filter("21", ref track, t => t.Name._Match(title) && t.Artist._WordMatchPartial(artist), songIds);

                    ctracks.Filter("22", ref track, t => t.Name._Match(title) && t.Album.ToLower() == $"{title} - Single", songIds);
                    ctracks.Filter("23", ref track, t => t.Name._Match(title) && t.Album.ToLower().Contains("- single"), songIds);

                    ctracks.Filter("24", ref track, t => t.Name._Match(title), songIds);

                    ctracks.Filter("FF", ref track, t => true, songIds);
                }
                else
                {
                    //ctracks.Filter("01", ref track, t => t.Name._Match(title) && t.Artist._Match(artist) && (Math.Abs((int)(t.TotalTime - time))) < diff, songIds);
                    //ctracks.Filter("02", ref track, t => t.Name._Match(title) && t.Artist._Match(artist), songIds, time);

                    //ctracks.Filter("06", ref track, t => t.Name._Match(title) && t.Artist._WordMatchMutual(artist), songIds, time);

                    ctracks.Filter("01", ref track, t => t.Name._Match(title) && t.Artist._WordMatchAny(artist), songIds, time);
                }

                    _catalogTracks.AddRange(ctracks);
                //}

                toolStripProgressBar.PerformStep();
                i++;
            }


            int nameMaxLength = 0;
            int artistMaxLength = 0;
            int albumMaxLength = 0;
            foreach (CatalogTrack ctrack in _catalogTracks)
            {
                if (ctrack.RawName != null && ctrack.RawName.Length > nameMaxLength) nameMaxLength = ctrack.RawName.Length;
                if (ctrack.Name != null && ctrack.Name.Length > nameMaxLength) nameMaxLength = ctrack.Name.Length;

                if (ctrack.RawArtist != null && ctrack.RawArtist.Length > artistMaxLength) artistMaxLength = ctrack.RawArtist.Length;
                if (ctrack.Artist != null && ctrack.Artist.Length > artistMaxLength) artistMaxLength = ctrack.Artist.Length;

                if (ctrack.Album != null && ctrack.Album.Length > albumMaxLength) albumMaxLength = ctrack.Album.Length;
            }

            var sb = new StringBuilder();
            bool header = false;
            foreach (CatalogTrack ctrack in _catalogTracks)
            {
                if (ctrack.Ordinal != null && (int.TryParse(ctrack.Ordinal, out int result) || ctrack.Ordinal.EndsWith(".")))
                {
                    if (!header)
                    {
                        sb.AppendLine();
                        sb.AppendLine(new string('-', 5 + 2 + nameMaxLength + 2 + artistMaxLength + 2 + albumMaxLength));
                    }
                    sb.AppendLine(string.Format($"{{0,5}}  {{1,{-nameMaxLength}}}  {{2, {-artistMaxLength}}}", ctrack.Ordinal, ctrack.RawName, ctrack.RawArtist));
                    header = true;
                }
                else
                {
                    sb.AppendLine();
                    sb.AppendLine(string.Format($"{{0,-5}}  {{1,{-nameMaxLength}}}  {{2, {-artistMaxLength}}}", ctrack.Ordinal, ctrack.RawName, ctrack.RawArtist));
                    sb.AppendLine(string.Format($"{{0,-5}}  {{1,{-nameMaxLength}}}  {{2, {-artistMaxLength}}}  {{3, {-albumMaxLength}}}", "     ", ctrack.Name, ctrack.Artist, ctrack.Album));
                    header = false;
                }
            }

            File.WriteAllText(_chart.FileName.Replace(".txt", " - Log.txt"), sb.ToString());


            this.dataGridViewResults.DataSource = null;
            this.dataGridViewResults.DataSource = _catalogTracks;

            Services.AppleMusic.Models.Playlist playlist = await _appleMusicClient.CreatePlaylist(playlistName, songIds);

            toolStripProgressBar.PerformStep();
            toolStripProgressBar.Value = 0;
        }

        private void FormChartParser_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
