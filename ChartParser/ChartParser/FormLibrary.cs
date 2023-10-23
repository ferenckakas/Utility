using Business;
using Common;
using Common.Models;
using Helper;
using Services.AppleMusic;
using Services.Discogs;
using Services.Spotify;
using Services.Youtube;
using SpotifyWebAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Apple = Services.AppleMusic.Models;
using Discogs = Services.Discogs.Models;

namespace ChartParser
{
    public partial class FormLibrary : Form
    {
        private readonly AppleMusicClient _appleMusicClient = new AppleMusicClient();
        private readonly YoutubeClient _youtubeClient = new YoutubeClient();
        private readonly SpotifyClient _spotifyClient = new SpotifyClient();
        private readonly DiscogsClient _discogsClient = new DiscogsClient();

        private ColumnIndex _gridViewIndex;
        private ColumnIndex _gridResultIndex;
        private Dictionary<int?, Track> _trackDictionary = new Dictionary<int?, Track>();

        private CancellationTokenSource _cancellationTokenSource;

        public FormLibrary()
        {
            InitializeComponent();
            BuildColumnIndices();
        }

        private async void FormLibrary_Load(object sender, EventArgs e)
        {
            _trackDictionary = Static.Library.Tracks.ToDictionary(t => t.TrackID);
            BuildColumnSelector();
            await BuildPlaylists();
            //await youtubeClient.LoadPlaylists();
        }

        private void BuildColumnSelector()
        {
            for (var i = 0; i < this.dataGridView.Columns.Count; i++)
            {
                DataGridViewColumn column = this.dataGridView.Columns[i];
                column.Name = $"{column.Name.Substring(0, column.Name.IndexOf("DataGridView"))}Column";
            }

            for (var i = 0; i < this.dataGridViewTrace.Columns.Count; i++)
            {
                DataGridViewColumn column = this.dataGridViewTrace.Columns[i];
                column.Name = $"{column.Name.Substring(0, column.Name.IndexOf("DataGridView"))}Column";
            }


            PropertyInfo[] propertyInfos = (new Track()).GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo p in propertyInfos)
            {
                if (p.Name != "LibraryPersistentID" && p.Name != "Library" && p.Name != "Playlists")
                {
                    bool visible = p.CustomAttributes.Any(a => a.AttributeType == typeof(VisibleAttribute));
                    this.checkedListBox.Items.Add(p.Name, visible);

                    string columnName = $"{p.Name.Substring(0, 1).ToLower()}{p.Name.Substring(1)}Column";

                    if (this.dataGridView.Columns[columnName] != null)
                    {
                        this.dataGridView.Columns[columnName].Visible = visible;
                    }

                    //{
                    //    //columnName = $"{p.Name.Substring(0, 1).ToLower()}{p.Name.Substring(1)}DataGridViewImageColumn";

                    //    //if (this.dataGridView1.Columns[columnName] != null)
                    //    //    this.dataGridView1.Columns[columnName].Visible = visible;
                    //}
                }
            }

            this.dataGridView.Columns["artworkUrlColumn"].Visible = false;
            this.dataGridView.Columns["thumbnailUrlColumn"].Visible = false;

            this.dataGridView.Columns["libraryPersistentIDColumn"].Visible = false;
            this.dataGridView.Columns["libraryColumn"].Visible = false;

            this.artworkDataGridViewImageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            this.thumbnailDataGridViewImageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;

            this.artworkDataGridViewImageColumn.HeaderText = "";
            this.thumbnailDataGridViewImageColumn.HeaderText = "";
            this.ordinalDataGridViewTextBoxColumn.HeaderText = "";

            this.dataGridViewTrace.Columns["thumbnailUrlColumn"].Visible = false;
            this.dataGridViewTrace.Columns["videoIdColumn"].Visible = false;
            this.dataGridViewTrace.Columns["channelIdColumn"].Visible = false;
            this.thumbnailDataGridViewImageColumn1.ImageLayout = DataGridViewImageCellLayout.Zoom;
            this.thumbnailDataGridViewImageColumn1.HeaderText = "";
            this.ordinalDataGridViewTextBoxColumn1.HeaderText = "";
        }

        private async Task BuildPlaylists()
        {
            List<Apple.Playlist> cloudPlaylists = await _appleMusicClient.GetAllPlaylists();

            foreach (Playlist playlist in Static.Library.Playlists)
            {
                Apple.Playlist cloudPlaylist = cloudPlaylists.FirstOrDefault(cp => cp.Attributes.Name == playlist.Name);
                if (cloudPlaylist != null)
                    playlist.CloudId = cloudPlaylist.Id;
            }

            var treeNodes = new List<TreeNode>();

            foreach (Playlist playlist in Static.Library.Playlists)
            {
                var treeNode = new TreeNode($"{playlist.Name} ({playlist.PlaylistItemIDs.Count})"); //{playlist.CloudId}
                treeNode.Tag = playlist;
                if (playlist.Folder.HasValue && playlist.Folder.Value)
                {
                    if (playlist.CloudId == null)
                    {
                        treeNode.ImageIndex = 0;
                        treeNode.SelectedImageIndex = 0;
                    }
                    else
                    {
                        treeNode.ImageIndex = 1;
                        treeNode.SelectedImageIndex = 1;
                    }
                }
                else
                {
                    if (playlist.SmartInfo != null)
                    {
                        if (playlist.CloudId == null)
                        {
                            treeNode.ImageIndex = 2;
                            treeNode.SelectedImageIndex = 2;
                        }
                        else
                        {
                            treeNode.ImageIndex = 3;
                            treeNode.SelectedImageIndex = 3;
                        }
                    }
                    else
                    {
                        if (playlist.CloudId == null)
                        {
                            treeNode.ImageIndex = 4;
                            treeNode.SelectedImageIndex = 4;
                        }
                        else
                        {
                            treeNode.ImageIndex = 5;
                            treeNode.SelectedImageIndex = 5;
                        }
                    }
                }
                treeNodes.Add(treeNode);
            }

            var rootNodes = new List<TreeNode>();

            foreach (TreeNode treeNode in treeNodes)
            {
                var playlist = (Playlist)treeNode.Tag;

                if (playlist.ParentPersistentID != null)
                {
                    TreeNode parentTreeNode = treeNodes.FirstOrDefault(n => ((Playlist)n.Tag).PlaylistPersistentID == playlist.ParentPersistentID);
                    if (parentTreeNode != null)
                        parentTreeNode.Nodes.Add(treeNode);
                }
                else
                {
                    rootNodes.Add(treeNode);
                }
            }

            this.treeView.Nodes.AddRange(rootNodes.ToArray());

            this.treeView.SelectedNode = treeNodes.FirstOrDefault(n => n.Text.StartsWith("y 2018-19 Int"));
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //List<Apple.Playlist> cloudPlaylists = await _appleMusicClient.GetAllPlaylists();

            var playlist = (Playlist)e.Node.Tag;

            if (playlist.PlaylistItems == null)
            {
                playlist.PlaylistItems = new List<Track>();

                foreach (int id in playlist.PlaylistItemIDs)
                {
                    if (_trackDictionary.TryGetValue(id, out Track Track))
                    {
                        playlist.PlaylistItems.Add(Track);
                        Track.Playlists.Add(playlist);
                    }
                    else
                    {
                        throw new Exception("Track not found in the Library.Tracks");
                    }
                }
            }

            //if (playlist.PlaylistItems.Count > 100)
            //this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            //else
            //    this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            //List<SpotifyWebAPI.PlaylistTrack> spotifyTracks = await _spotifyClient.GetAllPlaylistTracks(playlist.Name);

            this.dataGridView.DataSource = playlist.PlaylistItems;
        }

        private void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var checkedListBox = (CheckedListBox)sender;

            string text = checkedListBox.Items[e.Index].ToString();

            this.dataGridView.Columns[$"{text.Substring(0, 1).ToLower()}{text.Substring(1)}Column"].Visible = e.NewValue == CheckState.Checked;
        }

        private async void toolStripButtonRefreshArtworks_Click(object sender, EventArgs e)
        {
            var playlist = (Playlist)this.treeView.SelectedNode.Tag;

            if (playlist.CloudId != null)
            {
                List<Apple.Song> songs = await _appleMusicClient.GetSongs(playlist.CloudId);

                if (songs != null && songs.Count > 0)
                {
                    using (var wc = new WebClient())
                    {
                        int i = 0;

                        this.toolStripProgressBar.Maximum = playlist.PlaylistItems.Count + 1;

                        foreach (Track track in playlist.PlaylistItems)
                        {
                            Apple.Song song = songs[i];
                            track.CloudId = song.Id;
                            track.ArtworkUrl = song.Attributes.Artwork.Url;

                            //check artist, name

                            //if (song.Attributes.Artwork.Url == null)
                            //{
                            //    i++;
                            //    continue;
                            //}

                            //if (!File.Exists($@"C:\Users\Ferenc\Documents\AppleMusic\{track.CloudId}.jpeg"))
                            //{
                            //    byte[] data = wc.DownloadData(song.Attributes.Artwork.Url.Replace("{w}x{h}", "22x22"));
                            //    if (data == null)
                            //    {
                            //        i++;
                            //        continue;
                            //    }

                            //    using (var imgStream = new MemoryStream(data))
                            //    {
                            //        track.Artwork = Image.FromStream(imgStream);
                            //        if (track.Artwork != null)
                            //        {
                            //            track.Artwork.Save($@"C:\Users\Ferenc\Documents\AppleMusic\{track.CloudId}.jpeg");
                            //            //Tracks[i].VideoId = item.FoundTrack.VideoId;
                            //        }
                            //    }
                            //}

                            this.toolStripProgressBar.PerformStep();
                            i++;
                        }
                    }

                    this.dataGridView.DataSource = null;
                    this.dataGridView.DataSource = playlist.PlaylistItems;

                    this.toolStripProgressBar.PerformStep();
                    this.toolStripProgressBar.Value = 0;
                }
            }
        }

        private readonly List<CatalogTrack> _catalogTracks = new List<CatalogTrack>();

        private async void toolStripButtonCreateITunesPlaylist_Click(object sender, EventArgs e)
        {
            var youtubeTracks = new List<YoutubeTrack>(); //_catalogTracks = new List<CatalogTrack>();

            string playlistName = this.treeView.SelectedNode.Text.Substring(0, this.treeView.SelectedNode.Text.LastIndexOf(" "));

            var songIds = new List<string>(); //var videoIds = new List<string>();

            int maxHitResult = 0;
            int maxHit = 2;
            if (int.TryParse(this.toolStripTextBoxMaxHit.Text, out maxHitResult))
                maxHit = maxHitResult;

            //await youtubeClient.LoadPlaylists();

            var songs = (List<Track>)dataGridView.DataSource;
            List<Track> selectedSongs = GetSelectedSongs(songs);

            //var songtitles = Global.Mapper.Map<List<Track>, List<SongTitle>>(selectedSongs);
            this.toolStripProgressBar.Maximum = selectedSongs.Count + 1;

            int i = 1;

            foreach (Track song in selectedSongs)
            {
                string artist = song.Artist.RemoveAndFeat();
                string title = song.Name.RemovePars().RemoveBrackets().Replace("`", "'").Replace(",", " ");

                string feat = song.Name.ParseFeat();
                string version = song.Name.ParseSongVersion();

                uint? time = null;
                if (song.TotalTime.HasValue)
                    time = (uint?)Math.Round((double)song.TotalTime / 1000);

                string term = $"{artist} {title}".Trim();  //var term = $"{Artist} {Title} {Feat}".Trim();

                youtubeTracks.Add(new YoutubeTrack
                {
                    Ordinal = i.ToString(),
                    Name = song.Name,
                    Artist = song.Artist
                });

                youtubeTracks.Add(new YoutubeTrack
                {
                    Ordinal = i.ToString(),
                    Name = title,
                    Artist = artist,
                    Feat = feat,
                    SongVersion = version
                });

                string ArtistFeat = artist;
                if (!string.IsNullOrWhiteSpace(feat))
                    ArtistFeat = $"{artist} {feat}";

                youtubeTracks.Add(new YoutubeTrack
                {
                    Ordinal = i.ToString(),
                    Name = title,
                    Artist = ArtistFeat,
                    SongVersion = version
                });


                List<Apple.Song> results = await _appleMusicClient.SearchSongs($"{term}", maxHit, maxHit);

                if (!results.Any())
                {
                    artist = artist._ToWords()[0];
                    results = await _appleMusicClient.SearchSongs($"{artist} {title}", maxHit, maxHit);

                    //_catalogTracks.Add(new CatalogTrack
                    //{
                    //    Ordinal = results.Count.ToString(),
                    //    RawName = title,
                    //    RawArtist = artist
                    //});
                }

                List<CatalogTrack> ctracks0 = Global.Mapper.Map<List<Apple.Song>, List<CatalogTrack>>(results);

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

                    //_catalogTracks.Add(new CatalogTrack
                    //{
                    //    Ordinal = results.Count.ToString(),
                    //    RawName = title,
                    //    RawArtist = artist
                    //});

                    ctracks = Global.Mapper.Map<List<Apple.Song>, List<CatalogTrack>>(results);
                }

                var ytracks = new List<YoutubeTrack>();

                foreach (CatalogTrack ctrack in ctracks)
                {
                    var resultTrack = new YoutubeTrack
                    {
                        Title = ctrack.Name,
                        Name = ctrack.Name,
                        Artist = ctrack.Artist
                        //VideoId = searchResult.VideoId,
                        //ThumbnailUrl = searchResult.ThumbnailUrl,
                        //ChannelId = searchResult.ChannelId,
                        //ChannelTitle = searchResult.ChannelTitle,
                        //Channel = new YoutubeChannel(searchResult.ChannelId, searchResult.ChannelTitle)
                        //,
                        //Ordinal = searchResult == track ? "=" : "",
                    };

                    //resultTrack.Decompose(artist, title, feat, version);
                    
                    ytracks.Add(resultTrack);


                    ctrack.Decompose(title);
                }

                title = title.ToLower();
                artist = artist.ToLower();

                CatalogTrack track = null;


                ctracks.Filter("01", ref track, t => t.Name._Match(title) && t.Artist._WordMatchAny(artist), songIds, time);


                youtubeTracks.AddRange(ytracks); //_catalogTracks.AddRange(ctracks);

                toolStripProgressBar.PerformStep();
                i++;
            }

            this.dataGridViewTrace.DataSource = null;
            this.dataGridViewTrace.DataSource = youtubeTracks;

            Apple.Playlist playlist = await _appleMusicClient.CreatePlaylist(playlistName + " New", songIds);

            this.toolStripProgressBar.PerformStep();
            this.toolStripProgressBar.Value = 0;
        }

        private async void toolStripButtonCreateYoutubePlaylist_Click(object sender, EventArgs e)
        {
            var youtubeTracks = new List<YoutubeTrack>();

            string playListName = this.treeView.SelectedNode.Text.Substring(0, this.treeView.SelectedNode.Text.LastIndexOf(" "));

            var videoIds = new List<string>();

            int maxHitResult = 0;
            int maxHit = 2;
            if (int.TryParse(this.toolStripTextBoxMaxHit.Text, out maxHitResult))
                maxHit = maxHitResult;

            await _youtubeClient.LoadPlaylists();

            var songs = (List<Track>)dataGridView.DataSource;
            List<Track> selectedSongs = GetSelectedSongs(songs);

            List<SongTitle> songtitles = Global.Mapper.Map<List<Track>, List<SongTitle>>(selectedSongs);
            this.toolStripProgressBar.Maximum = songtitles.Count + 1;

            int i = 1;
            foreach (SongTitle song in songtitles)
            {
                string Artist = song.Artist.RemoveAndFeat();

                //string feat1 = song.Artist.ParseFeat();
                //string feat2 = song.Name.ParseFeat();

                string Feat = song.Name.ParseFeat();

                //var featList = new List<string>();
                //if (feat1 != null) featList.Add(feat1);
                //if (feat2 != null) featList.Add(feat2);
                //string Feat = featList.Any() ? string.Join(" ", featList) : null;

                string Version = song.Name.ParseSongVersion();
                string Title = song.Name.RemovePars().RemoveBrackets().Replace("`", "'").Replace(",", " ");

                string term = $"{Artist} {Title} {Feat}".Trim();

                youtubeTracks.Add(new YoutubeTrack
                {
                    Ordinal = i.ToString(),
                    Name = song.Name,
                    Artist = song.Artist
                });

                youtubeTracks.Add(new YoutubeTrack
                {
                    Ordinal = i.ToString(),
                    Name = Title,
                    Artist = Artist,
                    Feat = Feat,
                    SongVersion = Version
                });

                string ArtistFeat = Artist;
                if (!string.IsNullOrWhiteSpace(Feat))
                    ArtistFeat = $"{Artist} {Feat}";

                youtubeTracks.Add(new YoutubeTrack
                {
                    Ordinal = i.ToString(),
                    Name = Title,
                    Artist = ArtistFeat,
                    SongVersion = Version
                });

                List<SearchResult> results = await _youtubeClient.SearchVideos(term, maxHit);

                if (!results.Any())
                {
                    this.toolStripProgressBar.PerformStep();
                    i++;
                    continue;
                }

                var ytracks = new List<YoutubeTrack>();

                foreach (SearchResult searchResult in results)
                {
                    var resultTrack = new YoutubeTrack
                    {
                        Title = WebUtility.HtmlDecode(searchResult.Title),
                        VideoId = searchResult.VideoId,
                        ThumbnailUrl = searchResult.ThumbnailUrl,
                        ChannelId = searchResult.ChannelId,
                        ChannelTitle = searchResult.ChannelTitle,
                        Channel = new YoutubeChannel(searchResult.ChannelId, searchResult.ChannelTitle)
                        //,
                        //Ordinal = searchResult == track ? "=" : "",
                    };

                    resultTrack.Decompose(Artist, Title, Feat, Version);

                    ytracks.Add(resultTrack);
                }

                string title = Title.ToLower();
                string artist = ArtistFeat.ToLower();

                YoutubeTrack track = null;

                //ytracks.Filter(ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist.ToLower() == artist      && t.IsOfficial && t.IsVideo && t.Channel.Text.ToLower() == artist, videoIds);
                //ytracks.Filter(ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchMutual(artist) && t.IsOfficial && t.IsVideo && t.Channel.Text.ToLower() == artist, videoIds);
                //ytracks.Filter(ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchAny(artist)    && t.IsOfficial && t.IsVideo && t.Channel.Text.ToLower() == artist, videoIds);

                //ytracks.Filter(ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist.ToLower() == artist      && t.IsOfficial && t.IsVideo && t.Channel.Text._WordMatchMutual(artist), videoIds);
                //ytracks.Filter(ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchMutual(artist) && t.IsOfficial && t.IsVideo && t.Channel.Text._WordMatchMutual(artist), videoIds);
                //ytracks.Filter(ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchAny(artist)    && t.IsOfficial && t.IsVideo && t.Channel.Text._WordMatchMutual(artist), videoIds);

                ytracks.Filter("01", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist.ToLower() == artist      && t.IsOfficial && t.IsVideo && t.Channel.Text._WordMatchAny(artist), videoIds);
                ytracks.Filter("02", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchMutual(artist) && t.IsOfficial && t.IsVideo && t.Channel.Text._WordMatchAny(artist), videoIds);
                ytracks.Filter("03", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchAny(artist)    && t.IsOfficial && t.IsVideo && t.Channel.Text._WordMatchAny(artist), videoIds);

                ytracks.Filter("04", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist.ToLower() == artist      && !t.IsOfficial && t.IsVideo && t.Channel.Text._WordMatchAny(artist), videoIds);
                ytracks.Filter("05", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchMutual(artist) && !t.IsOfficial && t.IsVideo && t.Channel.Text._WordMatchAny(artist), videoIds);
                ytracks.Filter("06", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchAny(artist)    && !t.IsOfficial && t.IsVideo && t.Channel.Text._WordMatchAny(artist), videoIds);

                ytracks.Filter("07", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist.ToLower() == artist      && t.IsOfficial && t.IsVideo && t.Channel.IsPreferred, videoIds);
                ytracks.Filter("08", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchMutual(artist) && t.IsOfficial && t.IsVideo && t.Channel.IsPreferred, videoIds);
                ytracks.Filter("09", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchAny(artist)    && t.IsOfficial && t.IsVideo && t.Channel.IsPreferred, videoIds);

                ytracks.Filter("10", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist.ToLower() == artist      && !t.IsOfficial && t.IsVideo && t.Channel.IsPreferred, videoIds);
                ytracks.Filter("11", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchMutual(artist) && !t.IsOfficial && t.IsVideo && t.Channel.IsPreferred, videoIds);
                ytracks.Filter("12", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchAny(artist)    && !t.IsOfficial && t.IsVideo && t.Channel.IsPreferred, videoIds);

                ytracks.Filter("13", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist.ToLower() == artist      && t.IsOfficial && t.IsVideo, videoIds);
                ytracks.Filter("14", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchMutual(artist) && t.IsOfficial && t.IsVideo, videoIds);
                ytracks.Filter("15", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchAny(artist)    && t.IsOfficial && t.IsVideo, videoIds);

                ytracks.Filter("16", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist.ToLower() == artist      && !t.IsOfficial && t.IsVideo, videoIds);
                ytracks.Filter("17", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchMutual(artist) && !t.IsOfficial && t.IsVideo, videoIds);
                ytracks.Filter("18", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchAny(artist)    && !t.IsOfficial && t.IsVideo, videoIds);

                ytracks.Filter("19", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist.ToLower() == artist      && t.IsOfficial && t.IsLyric, videoIds);
                ytracks.Filter("20", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchMutual(artist) && t.IsOfficial && t.IsLyric, videoIds);
                ytracks.Filter("21", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchAny(artist)    && t.IsOfficial && t.IsLyric, videoIds);

                ytracks.Filter("22", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist.ToLower() == artist      && !t.IsOfficial && t.IsLyric, videoIds);
                ytracks.Filter("23", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchMutual(artist) && !t.IsOfficial && t.IsLyric, videoIds);
                ytracks.Filter("24", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchAny(artist)    && !t.IsOfficial && t.IsLyric, videoIds);

                ytracks.Filter("25", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist.ToLower() == artist      && t.IsOfficial && t.IsAudio, videoIds);
                ytracks.Filter("26", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchMutual(artist) && t.IsOfficial && t.IsAudio, videoIds);
                ytracks.Filter("27", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchAny(artist)    && t.IsOfficial && t.IsAudio, videoIds);

                ytracks.Filter("28", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist.ToLower() == artist      && !t.IsOfficial && t.IsAudio, videoIds);
                ytracks.Filter("29", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchMutual(artist) && !t.IsOfficial && t.IsAudio, videoIds);
                ytracks.Filter("30", ref track, t => t.Name.VEq(title) && t.SongVersion.VEq(Version) && t.Artist._WordMatchAny(artist)    && !t.IsOfficial && t.IsAudio, videoIds);

                ytracks.Filter("FF", ref track, t => true, videoIds);

                youtubeTracks.AddRange(ytracks);

                this.toolStripProgressBar.PerformStep();
                i++;
            }

            this.dataGridViewTrace.DataSource = null;
            this.dataGridViewTrace.DataSource = youtubeTracks;

            await _youtubeClient.CreatePlaylist(playListName, videoIds);

            this.toolStripProgressBar.PerformStep();
            this.toolStripProgressBar.Value = 0;
        }

        private void dataGridViewYoutubeResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var youtubeTracks = (List<YoutubeTrack>)this.dataGridViewTrace.DataSource;

            //youtubeTracks[e.RowIndex].Decompose();

            //this.dataGridViewYoutubeResults.DataSource = null;
            //this.dataGridViewYoutubeResults.DataSource = youtubeTracks;
        }

        private async void toolStripButtonCreateSpotifyPlaylist_Click(object sender, EventArgs e)
        {
            if (_spotifyClient.AuthenticationToken.HasExpired)
            {
                var formBrowser = new FormBrowser("webapp320161016012541.azurewebsites.net");
                formBrowser.ShowDialog();
            }

            await _spotifyClient.LoadPlaylists();

            //if (!spotifyClient.Playlists.Any())
            //{
            //    FormBrowser formBrowser = new FormBrowser("webapp320161016012541.azurewebsites.net");
            //    formBrowser.ShowDialog();
            //    await spotifyClient.LoadPlaylists();
            //}

            var youtubeTracks = new List<YoutubeTrack>();

            string playListName = this.treeView.SelectedNode.Text.Substring(0, this.treeView.SelectedNode.Text.LastIndexOf(" "));

            var videoIds = new List<string>();

            int maxHitResult = 0;
            int maxHit = 2;
            if (int.TryParse(this.toolStripTextBoxMaxHit.Text, out maxHitResult))
                maxHit = maxHitResult;

            var songs = (List<Track>)dataGridView.DataSource;
            List<Track> selectedSongs = GetSelectedSongs(songs);

            this.toolStripProgressBar.Maximum = selectedSongs.Count + 1;

            int i = 1;
            foreach (Track song in selectedSongs)
            {
                string Artist = song.Artist.RemoveAndFeat();

                //string feat1 = song.Artist.ParseFeat();
                //string feat2 = song.Name.ParseFeat();

                string Feat = song.Name.ParseFeat();

                //var featList = new List<string>();
                //if (feat1 != null) featList.Add(feat1);
                //if (feat2 != null) featList.Add(feat2);
                //string Feat = featList.Any() ? string.Join(" ", featList) : null;

                string Version = song.Name.ParseSongVersion();
                string Title = song.Name.RemovePars().RemoveBrackets().Replace("`", "'");

                string term = $"{Artist} {Title}";

                if (!string.IsNullOrWhiteSpace(Feat)) term = $"{term} {Feat}";
                if (!string.IsNullOrWhiteSpace(Version)) term = $"{term} {Version}";

                youtubeTracks.Add(new YoutubeTrack
                {
                    Ordinal = i.ToString(),
                    Name = song.Name,
                    Artist = song.Artist
                });

                youtubeTracks.Add(new YoutubeTrack
                {
                    Ordinal = i.ToString(),
                    Name = Title,
                    Artist = Artist,
                    Feat = Feat,
                    SongVersion = Version
                });

                string ArtistFeat = Artist;
                if (!string.IsNullOrWhiteSpace(Feat))
                    ArtistFeat = $"{Artist} {Feat}";

                youtubeTracks.Add(new YoutubeTrack
                {
                    Ordinal = i.ToString(),
                    Name = Title,
                    Artist = ArtistFeat,
                    SongVersion = Version
                });


                List<SpotifyWebAPI.Track> results = null;

                do
                {
                    try
                    {
                        results = await _spotifyClient.SearchTracks(term, maxHit);
                    }
                    catch (TooManySpotifyRequestException ex)
                    {
                        Color originalColorButton = this.toolStripButtonCreateSpotifyPlaylist.BackColor;
                        Color originalColorProgressBar = this.toolStripProgressBar.BackColor;

                        this.toolStripButtonCreateSpotifyPlaylist.BackColor = Color.Red;
                        this.toolStripProgressBar.BackColor = Color.Red;

                        Thread.Sleep(ex.RetryAfter * 1000);

                        this.toolStripButtonCreateSpotifyPlaylist.BackColor = originalColorButton;
                        this.toolStripProgressBar.BackColor = originalColorProgressBar;
                    }
                } while (results == null);


                if (!results.Any())
                {
                    this.toolStripProgressBar.PerformStep();
                    i++;
                    continue;
                }

                var ytracks = new List<YoutubeTrack>();

                foreach (SpotifyWebAPI.Track searchResult in results)
                {
                    var resultTrack = new YoutubeTrack
                    {
                        Title = searchResult.Name,
                        Artist = string.Join(" ", searchResult.Artists.Select(a => a.Name)),
                        VideoId = searchResult.Uri,
                        //ThumbnailUrl = searchResult.Album.Images.FirstOrDefault(im => im.Height == 64)?.Url
                        DurationInMilliSec = searchResult.Duration
                        //ChannelId = searchResult.ChannelId,
                        //ChannelTitle = searchResult.ChannelTitle,
                        //Channel = new YoutubeChannel(searchResult.ChannelId, searchResult.ChannelTitle)
                        //,
                        //Ordinal = searchResult == track ? "=" : "",
                    };

                    resultTrack.DecomposeSpotify(Artist, Title, Feat, Version);

                    ytracks.Add(resultTrack);
                }

                string title = Title.ToLower();
                string artist = ArtistFeat.ToLower();

                YoutubeTrack track = null;

                ytracks.Filter("01", ref track, t => t.Name._Match(title) && Version != null && t.SongVersion._Match(Version) && t.IsRemaster && t.Artist._Match(artist), videoIds, song);
                ytracks.Filter("02", ref track, t => t.Name._Match(title) && Version != null && t.SongVersion._Match(Version) && t.IsRemaster && t.Artist._WordMatchMutual(artist), videoIds, song);
                ytracks.Filter("03", ref track, t => t.Name._Match(title) && Version != null && t.SongVersion._Match(Version) && t.IsRemaster && t.Artist._WordMatchAny(artist), videoIds, song);

                ytracks.Filter("04", ref track, t => t.Name._Match(title) && Version != null && t.SongVersion._Match(Version) && t.Artist._Match(artist), videoIds, song);
                ytracks.Filter("05", ref track, t => t.Name._Match(title) && Version != null && t.SongVersion._Match(Version) && t.Artist._WordMatchMutual(artist), videoIds, song);
                ytracks.Filter("06", ref track, t => t.Name._Match(title) && Version != null && t.SongVersion._Match(Version) && t.Artist._WordMatchAny(artist), videoIds, song);

                ytracks.Filter("07", ref track, t => t.Name._Match(title) && t.SongVersion._Match(Version) && t.IsRemaster && t.Artist._Match(artist), videoIds, song);
                ytracks.Filter("08", ref track, t => t.Name._Match(title) && t.SongVersion._Match(Version) && t.IsRemaster && t.Artist._WordMatchMutual(artist), videoIds, song);
                ytracks.Filter("09", ref track, t => t.Name._Match(title) && t.SongVersion._Match(Version) && t.IsRemaster && t.Artist._WordMatchAny(artist), videoIds, song);

                ytracks.Filter("10", ref track, t => t.Name._Match(title) && t.IsRemaster && t.Artist._Match(artist), videoIds, song);
                ytracks.Filter("11", ref track, t => t.Name._Match(title) && t.IsRemaster && t.Artist._WordMatchMutual(artist), videoIds, song);
                ytracks.Filter("12", ref track, t => t.Name._Match(title) && t.IsRemaster && t.Artist._WordMatchAny(artist), videoIds, song);

                ytracks.Filter("13", ref track, t => t.Name._Match(title) && t.SongVersion._Match(Version) && t.Artist._Match(artist), videoIds, song);
                ytracks.Filter("14", ref track, t => t.Name._Match(title) && t.SongVersion._Match(Version) && t.Artist._WordMatchMutual(artist), videoIds, song);
                ytracks.Filter("15", ref track, t => t.Name._Match(title) && t.SongVersion._Match(Version) && t.Artist._WordMatchAny(artist), videoIds, song);

                ytracks.Filter("FF", ref track, t => true, videoIds);

                youtubeTracks.AddRange(ytracks);

                this.toolStripProgressBar.PerformStep();
                i++;
            }

            this.dataGridViewTrace.DataSource = null;
            this.dataGridViewTrace.DataSource = youtubeTracks;

            await _spotifyClient.CreatePlaylist(playListName, videoIds);

            this.toolStripProgressBar.PerformStep();
            this.toolStripProgressBar.Value = 0;
        }

        private List<Track> GetSelectedSongs(List<Track> songs)
        {
            if (dataGridView.SelectedRows.Count == 0 || dataGridView.SelectedRows.Count == songs.Count)
                return songs;

            var selectedSongs = new List<Track>();

            var indices = new List<int>();

            for (var i = 0; i < dataGridView.SelectedRows.Count; i++)
            {
                indices.Add(dataGridView.SelectedRows[i].Index);
            }

            indices.Sort();

            foreach (int i in indices)
            {
                selectedSongs.Add(songs[i]);
            }

            return selectedSongs;
        }

        private async void toolStripButtonValidateByDiscogs_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await ValidateByDiscogsAsync(_cancellationTokenSource.Token);
        }

        private async Task ValidateByDiscogsAsync(CancellationToken token)
        {
            var youtubeTracks = new List<YoutubeTrack>();

            var videoIds = new List<string>();

            int maxHitResult = 0;
            int maxHit = 2;
            if (int.TryParse(this.toolStripTextBoxMaxHit.Text, out maxHitResult))
                maxHit = maxHitResult;

            var songs = (List<Track>)dataGridView.DataSource;
            List<Track> selectedSongs = GetSelectedSongs(songs);

            this.toolStripProgressBar.Maximum = selectedSongs.Count + 1;

            int i = 1;
            foreach (Track song in selectedSongs)
            {
                if (token.IsCancellationRequested)
                    break;

                if (song.Year2 != null || song.Genre2 != null)
                {
                    this.toolStripProgressBar.PerformStep();
                    i++;
                    continue;
                }

                string Artist = song.Artist.Replace("ß", "ss").RemoveAndFeat();

                //string feat1 = song.Artist.ParseFeat();
                //string feat2 = song.Name.ParseFeat();

                string Feat = song.Name.Replace("ß", "ss").ParseFeat();

                //var featList = new List<string>();
                //if (feat1 != null) featList.Add(feat1);
                //if (feat2 != null) featList.Add(feat2);
                //string Feat = featList.Any() ? string.Join(" ", featList) : null;

                string Version = song.Name.Replace("ß", "ss").ParseSongVersion();
                string Title = song.Name.Replace("ß", "ss").RemovePars().RemoveBrackets().Replace("`", "'");

                string term = $"{Artist} {Title}";

                if (!string.IsNullOrWhiteSpace(Feat)) term = $"{term} {Feat}";
                if (!string.IsNullOrWhiteSpace(Version)) term = $"{term} {Version}";

                youtubeTracks.Add(new YoutubeTrack
                {
                    Ordinal = i.ToString(),
                    Name = song.Name,
                    Artist = song.Artist
                });

                youtubeTracks.Add(new YoutubeTrack
                {
                    Ordinal = i.ToString(),
                    Name = Title,
                    Artist = Artist,
                    Feat = Feat,
                    SongVersion = Version
                });

                string ArtistFeat = Artist;
                if (!string.IsNullOrWhiteSpace(Feat))
                    ArtistFeat = $"{Artist} {Feat}";

                string TitleVersion = Title;
                if (!string.IsNullOrWhiteSpace(Version))
                    TitleVersion = $"{Title} {Version}";

                youtubeTracks.Add(new YoutubeTrack
                {
                    Ordinal = i.ToString(),
                    Name = Title,
                    Artist = ArtistFeat,
                    SongVersion = Version,
                    Year = (uint?) song.Year
                });


                List<Discogs.Result> results = null;

                bool onlineSearch = this.toolStripButtonForceOnline.Checked;

                using (var musicDBEntities = new Entities.MusicDBEntities())
                {
                    Entities.DiscogsSearchQuery query = musicDBEntities.DiscogsSearchQueries.FirstOrDefault(r => r.Artist == ArtistFeat && r.Name == TitleVersion);

                    if (query != null)
                    {
                        if (query.DiscogsSearchResults.Any())
                        {
                            if (!onlineSearch)
                            {
                                ICollection<Entities.DiscogsSearchResult> discogsSearchResults = query.DiscogsSearchResults;
                                results = Global.Mapper.Map<ICollection<Entities.DiscogsSearchResult>, List<Discogs.Result>>(discogsSearchResults);
                            }
                            else
                                query.DiscogsSearchResults.Clear();
                        }
                        else
                            onlineSearch = true;
                    }
                    else // query == null
                    {
                        query = musicDBEntities.DiscogsSearchQueries.Add(new Entities.DiscogsSearchQuery
                        {
                            Artist = ArtistFeat,
                            Name = TitleVersion
                        });
                        onlineSearch = true;
                    }

                    if (onlineSearch) // online search
                    {
                        results = await _discogsClient.Search(ArtistFeat, TitleVersion, maxHit, true);
                        //var discogsSearchResult = Global.Mapper.Map<List<Discogs.Result>, List<Entities.DiscogsSearchResult>>(results);
                        //discogsSearchResult.ForEach(r => { r.QueryId = query.Id; });
                        //musicDBEntities.DiscogsSearchResults.AddRange(discogsSearchResult);
                        //await musicDBEntities.SaveChangesAsync();

                        if (!results.Any())
                        {
                            results = await _discogsClient.Search(ArtistFeat, TitleVersion, maxHit, false);
                        }
                    }

                    if (!results.Any())
                    {
                        song.Year2 = 0;
                        song.Genre2 = "No data";
                        await musicDBEntities.SaveChangesAsync();
                        this.toolStripProgressBar.PerformStep();
                        i++;
                        continue;
                    }

                    var ytracks = new List<YoutubeTrack>();

                    foreach (Discogs.Result searchResult in results)
                    {
                        uint yearResult = 0;
                        uint? year = null;
                        if (uint.TryParse(searchResult.Year, out yearResult))
                            year = yearResult;

                        var resultTrack = new YoutubeTrack
                        {
                            Title = searchResult.Title.Replace("ß", "ss"),
                            //Artist = string.Join(" ", searchResult.Genre),
                            //Name = string.Join(" ", searchResult.Style),
                            //SongVersion = year?.ToString(),
                            Year = year,
                            VideoVersion = searchResult.Country,
                            Official = string.Join(" ", searchResult.Genre) + " " + string.Join(" ", searchResult.Style),
                            ChannelTitle = string.Join(" ", searchResult.Format),
                            VideoId = searchResult.Id.ToString()
                            //ThumbnailUrl = searchResult.Album.Images.FirstOrDefault(im => im.Height == 64)?.Url
                            //DurationInMilliSec = searchResult.Duration
                            //ChannelId = searchResult.ChannelId,
                            //ChannelTitle = searchResult.ChannelTitle,
                            //Channel = new YoutubeChannel(searchResult.ChannelId, searchResult.ChannelTitle)
                            //,
                            //Ordinal = searchResult == track ? "=" : "",
                        };

                        resultTrack.DecomposeDiscogs(Artist, Title, Feat, Version);

                        ytracks.Add(resultTrack);
                    }

                    string title = Title.ToLower();
                    string artist = ArtistFeat.ToLower();

                    YoutubeTrack track = null;

                    ytracks.Filter("01", ref track, t => t.Names._ContainsMatch(title), videoIds, song);
                    ytracks.Filter("02", ref track, t => t.Names._ContainsWordMatchAny(title), videoIds, song);
                    ytracks.Filter("03", ref track, t => t.Title._WordMatchAny(title), videoIds, song);

                    // Format
                    // Country

                    ytracks.Filter("FF", ref track, t => true, videoIds);

                    if (track != null)
                    {
                        song.Year2 = (int?) track.Year;
                        song.Genre2 = $"{track.Ordinal}  {track.Artist} {track.Name}";
                    }
                    else
                    {
                        song.Year2 = 1;
                        song.Genre2 = $"{track.Ordinal}  No match";
                    }

                    var newTracks = new List<YoutubeTrack>();

                    if (ytracks.Count > maxHit)
                    {
                        newTracks.AddRange(ytracks.Take(maxHit));
                        if (!ytracks.Take(maxHit).Any(y => y == track))
                        {
                            newTracks.Add(track);
                            newTracks.Add(new YoutubeTrack { Title = $"{(ytracks.Count - maxHit - 1).ToString()} more items" });
                        }
                        else
                            newTracks.Add(new YoutubeTrack { Title = $"{(ytracks.Count - maxHit).ToString()} more items" });
                    }
                    else
                        newTracks = ytracks;

                    youtubeTracks.AddRange(newTracks);

                    List<Discogs.Result> res = results.Where(r => newTracks.Any(n => n.VideoId == r.Id.ToString())).ToList();

                    if (onlineSearch && res != null && res.Any())
                    {
                        List<Entities.DiscogsSearchResult> discogsSearchResult = Global.Mapper.Map<List<Discogs.Result>, List<Entities.DiscogsSearchResult>>(res);
                        discogsSearchResult.ForEach(r => { r.QueryId = query.Id; });
                        musicDBEntities.DiscogsSearchResults.AddRange(discogsSearchResult);
                        await musicDBEntities.SaveChangesAsync();
                    }
                }

                this.toolStripProgressBar.PerformStep();
                i++;
            }

            this.dataGridViewTrace.DataSource = null;
            this.dataGridViewTrace.DataSource = youtubeTracks;

            this.dataGridView.DataSource = null;
            this.dataGridView.DataSource = songs;

            this.toolStripProgressBar.PerformStep();

            this.toolStripProgressBar.Value = 0;
        }

        private void toolStripButtonStopValidation_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private void toolStripButtonCheckTag_Click(object sender, EventArgs e)
        {
            var songs = (List<Track>)dataGridView.DataSource;
            List<Track> selectedSongs = GetSelectedSongs(songs);

            this.toolStripProgressBar.Maximum = selectedSongs.Count + 1;

            var sb1 = new StringBuilder();
            var sb2 = new StringBuilder();
            foreach (Track song in selectedSongs)
            {
                var file = new MusicFile(Uri.UnescapeDataString(song.Location.Replace("file://localhost", "")));

                sb1.AppendLine($"Title: {song.Name}");
                sb1.AppendLine($"Artist: {song.Artist}");
                sb1.AppendLine($"Album: {song.Album}");
                sb1.AppendLine($"Album Artist: {song.AlbumArtist}");
                sb1.AppendLine($"Composer: {song.Composer}");
                sb1.AppendLine($"Grouping: {song.Grouping}");
                sb1.AppendLine($"Genre: {song.Genre}");
                sb1.AppendLine($"Year: {song.Year}");
                sb1.AppendLine($"TrackNumber: {song.TrackNumber}");
                sb1.AppendLine($"TrackCount: {song.TrackCount}");
                sb1.AppendLine($"DiscNumber: {song.DiscNumber}");
                sb1.AppendLine($"DiscCount: {song.DiscCount}");
                sb1.AppendLine($"Rating: {((!song.RatingComputed.HasValue || song.RatingComputed.HasValue && !song.RatingComputed.Value) && song.Rating != null ? (song.Rating / 20).ToString() : string.Empty)}");
                sb1.AppendLine($"BPM: {song.BPM}");
                sb1.AppendLine($"Comment: {song.Comments}");
                sb1.AppendLine();

                sb2.AppendLine($"Title: {file.Title}");
                sb2.AppendLine($"Artist: {file.Artist}");
                sb2.AppendLine($"Album: {file.Album}");
                sb2.AppendLine($"Album Artist: {file.AlbumArtist}");
                sb2.AppendLine($"Composer: {file.Composer}");
                sb2.AppendLine($"Grouping: {file.Grouping}");
                sb2.AppendLine($"Genre: {file.Genre}");
                sb2.AppendLine($"Year: {(file.Year != 0 ? file.Year.ToString() : string.Empty)}");
                sb2.AppendLine($"TrackNumber: {(file.TrackNumber != 0 ? file.TrackNumber.ToString() : string.Empty)}");
                sb2.AppendLine($"TrackCount: {(file.TrackCount != 0 ? file.TrackCount.ToString() : string.Empty)}");
                sb2.AppendLine($"DiscNumber: {(file.DiscNumber != 0 ? file.DiscNumber.ToString() : string.Empty)}");
                sb2.AppendLine($"DiscCount: {(file.DiscCount != 0 ? file.DiscCount.ToString() : string.Empty)}");
                sb2.AppendLine($"Rating: {(file.Rating != 0 ? file.Rating.ToString() : string.Empty)}");
                sb2.AppendLine($"BPM: {(file.Rating != 0 ? file.Rating.ToString() : string.Empty)}");
                sb2.AppendLine($"Comment: {file.Comment}");
                sb2.AppendLine();

                this.toolStripProgressBar.PerformStep();
            }

            System.IO.File.WriteAllText(Constant.TagsInLibraryFile, sb1.ToString());
            System.IO.File.WriteAllText(Constant.TagsInMusicFilesFile, sb2.ToString());

            this.toolStripProgressBar.PerformStep();
            this.toolStripProgressBar.Value = 0;
        }

        private void toolStripButtonUpdateTag_Click(object sender, EventArgs e)
        {
            var songs = (List<Track>)dataGridView.DataSource;
            List<Track> selectedSongs = GetSelectedSongs(songs);

            this.toolStripProgressBar.Maximum = selectedSongs.Count + 1;

            int i = 1;
            foreach (Track song in selectedSongs)
            {
                var file = new MusicFile(Uri.UnescapeDataString(song.Location.Replace("file://localhost", "")));

                if (file.Album != song.Album)
                    file.Album = song.Album;

                if (file.AlbumArtist != song.AlbumArtist)
                    file.AlbumArtist = song.AlbumArtist;

                if (file.Artist != song.Artist)
                    file.Artist = song.Artist;

                if (file.Comment != song.Comments)
                    file.Comment = song.Comments;

                if (file.Composer != song.Composer)
                    file.Composer = song.Composer;

                uint diskCount = song.DiscCount != null ? (uint)song.DiscCount : 0;
                if (file.DiscCount != diskCount)
                    file.DiscCount = diskCount;

                uint diskNumber = song.DiscNumber != null ? (uint)song.DiscNumber : 0;
                if (file.DiscNumber != diskNumber)
                    file.DiscNumber = diskNumber;

                if (file.Genre != song.Genre)
                    file.Genre = song.Genre;

                if (file.Grouping != song.Grouping)
                    file.Grouping = song.Grouping;

                uint rating = (!song.RatingComputed.HasValue || song.RatingComputed.HasValue && !song.RatingComputed.Value) && song.Rating != null ? (uint)song.Rating / 20 : 0;
                if (file.Rating != rating)
                    file.Rating = rating;

                if (file.Title != song.Name)
                    file.Title = song.Name;

                uint trackCount = song.TrackCount != null ? (uint)song.TrackCount : 0;
                if (file.TrackCount != trackCount)
                    file.TrackCount = trackCount;

                uint trackNumber = song.TrackNumber != null ? (uint)song.TrackNumber : 0;
                if (file.TrackNumber != trackNumber)
                    file.TrackNumber = trackNumber;

                uint year = song.Year != null ? (uint)song.Year : 0;
                if (file.Year != year)
                    file.Year = year;

                file.Save();

                this.toolStripProgressBar.PerformStep();
                i++;
            }

            this.toolStripProgressBar.PerformStep();
            this.toolStripProgressBar.Value = 0;
        }

        private void toolStripButtonCreateMp3_Click(object sender, EventArgs e)
        {
            var songs = (List<Track>)dataGridView.DataSource;
            List<Track> selectedSongs = GetSelectedSongs(songs);

            this.toolStripProgressBar.Maximum = selectedSongs.Count + 1;

            Directory.CreateDirectory(Constant.Mp3Directory);

            var folder = this.treeView.SelectedNode.Text.Substring(2, this.treeView.SelectedNode.Text.LastIndexOf("(") - 3);

            Parallel.ForEach(selectedSongs, song =>
            {
                try
                {
                    var file = new MusicFile(Uri.UnescapeDataString(song.Location.Replace("file://localhost", "")));

                    var filePath = file.SaveAsMp3(Constant.Mp3Directory);

                    var fileL = new MusicFile(filePath);
                    fileL.Copy2(folder);

                    this.toolStripProgressBar.PerformStep();
                }
                catch (Exception)
                {
                }
            });

            this.toolStripProgressBar.PerformStep();
            this.toolStripProgressBar.Value = 0;
        }

        private void BuildColumnIndices()
        {
            for (var i = 0; i < this.dataGridView.Columns.Count; i++)
            {
                DataGridViewColumn c = this.dataGridView.Columns[i];

                switch (c.HeaderText)
                {
                    case "Name":
                        _gridViewIndex.Name = i;
                        break;
                    case "Artist":
                        _gridViewIndex.Artist = i;
                        break;
                    case "Comments":
                        _gridViewIndex.Comments = i;
                        break;
                    case "Year":
                        _gridViewIndex.Year = i;
                        break;
                    case "Year2":
                        _gridViewIndex.Year2 = i;
                        break;
                }
            }

            for (var i = 0; i < this.dataGridViewTrace.Columns.Count; i++)
            {
                DataGridViewColumn c = this.dataGridViewTrace.Columns[i];

                switch (c.HeaderText)
                {
                    case "Ordinal":
                        _gridResultIndex.Ordinal = i;
                        break;
                    case "Year":
                        _gridResultIndex.Year3 = i;
                        break;
                }
            }
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int colIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            if (rowIndex >= 0 && colIndex >= 0 && colIndex == _gridViewIndex.Year)
            {
                DataGridViewRow row = this.dataGridView.Rows[rowIndex];

                string comments = (string)row.Cells[_gridViewIndex.Comments].Value;
                int? year = row.Cells[_gridViewIndex.Year].Value != null ? (int?)row.Cells[_gridViewIndex.Year].Value : null;
                int? year2 = row.Cells[_gridViewIndex.Year2].Value != null ? (int?)row.Cells[_gridViewIndex.Year2].Value : null;

                if (year2 != null && year != year2)
                {
                    if (comments != null && comments.Contains("by Wikipedia"))
                        e.CellStyle.ForeColor = Color.Blue;
                    else
                        e.CellStyle.ForeColor = Color.Red;
                }
                else if (year2 != null && year == year2)
                {
                    e.CellStyle.ForeColor = Color.Green;
                }
            }
        }

        private void dataGridViewYoutubeResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int colIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            if (rowIndex >= 0 && colIndex >= 0 && colIndex == _gridResultIndex.Year3)
            {
                DataGridViewRow row = this.dataGridViewTrace.Rows[rowIndex];

                var ordinal = (string)row.Cells[_gridResultIndex.Ordinal].Value;
                //var yearString = (string)theRow.Cells[gridResultIndex.Year3].Value;
                //uint? year = yearString != null ? yearString.ToNullableUInt() : null;
                var year = (uint?)row.Cells[_gridResultIndex.Year3].Value;

                if (ordinal != null && ordinal.StartsWith("="))
                {
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }
                else if (ordinal != null && year != null) //// year cell is not empty and number (uint)
                {
                    //var year = (uint?)e.Value;

                    int i = rowIndex + 1;
                    for (; i<this.dataGridViewTrace.Rows.Count; i++)
                    {
                        ordinal = (string)this.dataGridViewTrace.Rows[i].Cells[_gridResultIndex.Ordinal].Value;
                        if (ordinal != null && ordinal.StartsWith("="))
                            break;
                        else if (ordinal != null)
                            break;
                    }

                    if (ordinal != null && ordinal.StartsWith("="))
                    {
                        //yearString = (string)this.dataGridViewYoutubeResults.Rows[i].Cells[gridResultIndex.Year3].Value;
                        //uint? year3 = yearString != null ? yearString.ToNullableUInt() : null;
                        var year3 = (uint?)this.dataGridViewTrace.Rows[i].Cells[_gridResultIndex.Year3].Value;

                        if (year3 != null && year != year3)
                        {
                            e.CellStyle.ForeColor = Color.Red;
                        }
                        else if (year3 != null && year == year3)
                        {
                            e.CellStyle.ForeColor = Color.Green;
                        }
                    }
                }
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dataGridView.Rows[e.RowIndex];

            var artist = (string)row.Cells[_gridViewIndex.Artist].Value;
            var name = (string)row.Cells[_gridViewIndex.Name].Value;

            name = name.RemovePars().RemoveBrackets().Replace("`", "'");
            string version = name.ParseSongVersion();

            artist = WebUtility.UrlEncode(artist).Replace("%20", "+");
            name = WebUtility.UrlEncode(name).Replace("%20", "+");
            version = version != null ? WebUtility.UrlEncode(version).Replace("%20", "+") : "";

            string url = null;

            if (e.ColumnIndex == _gridViewIndex.Year)
            {
                url = $"https://www.bing.com/search?q={artist}+{name}+{version}+wikipedia.org&src=IE-SearchBox&FORM=IESR4S&pc=EUPP_";
                //url = $"https://www.bing.com/search?q={artist}+{name}+{version}+wikipedia.org&src=IE-SearchBox&FORM=IENDS2&pc=EUPP_";
            }
            else if (e.ColumnIndex == _gridViewIndex.Year2)
            {
                //url = $"https://www.discogs.com/search/?q=ABBA+Thank+You+For+the+Music&format_exact=Single&type=master";
                url = $"https://www.discogs.com/search/?q={name}+{version}&artist={artist}&type=master";
            }

            if (url != null)
            {
                FormBrowser formBrowser = new FormBrowser(url);
                formBrowser.ShowDialog();
            }
        }

        private void FormLibrary_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
