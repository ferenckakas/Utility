using Business;
using Business.Interfaces;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChartParser
{
    public partial class FormConsole : Form
    {
        private readonly IFileSystem _fileSystem = new FileSystem();
        private readonly IMusicLibrary _musicLibrary = new MusicLibrary();

        public FormConsole()
        {
            InitializeComponent();
        }

        private async void FormConsole_Shown(object sender, EventArgs e)
        {
            Task taskLoadLibrary = Task.Run(() => _musicLibrary.LoadLibrary());
            Task<(int count, string message)> taskCloudPlaylist = Task.Run(async () =>
            {
                return await _musicLibrary.GetCloudPlaylistInfoAsync();
            });

            var list1 = new List<(string extension, int count)>
            {
                ("", 0),
                (".m4a", 0),
                (".mp3", 0),
                (".wav", 0),
                (".jpg", 0),
                (".jpeg", 0),
                (".url", 0),
                (".DS_Store", 0),
                (".db", 0)
            };
            var list2 = new List<(string extension, int count)>(list1);

            Task taskGetFileCountMusic = Task.Run(() => _fileSystem.GetFileCount(Constant.ITunesMusicDirectory, ref list1));
            Task taskGetFileCountBackup = Task.Run(() => _fileSystem.GetFileCount(Constant.BackupMusicDirectory, ref list2));

            await taskGetFileCountMusic;
            ShowFileCount(Constant.ITunesMusicDirectory, list1);

            await taskGetFileCountBackup;
            ShowFileCount(Constant.BackupMusicDirectory, list2);

            ShowMusicFileCount(list1);

            await taskLoadLibrary;

            Task<(int count, string message)> taskGetLocalTrackInfo = Task.Run(() => _musicLibrary.GetLocalTrackInfo());
            Task<string> taskGetMissingFiles = Task.Run(() => _fileSystem.GetMissingFiles(Constant.ITunesMusicDirectory));

            (int count, string message) getLocalTrackInfoResult = await taskGetLocalTrackInfo;
            string missingFiles = await taskGetMissingFiles;
            ShowTrackInfo(getLocalTrackInfoResult.count, getLocalTrackInfoResult.message, missingFiles);

            ShowLocalPlaylistInfo();

            (int count, string message) taskCloudPlaylistResult = await taskCloudPlaylist;
            ShowCloudPlaylistInfo(taskCloudPlaylistResult.count, taskCloudPlaylistResult.message);
            //Static.CreateDatabase();
        }

        private void ShowFileCount(string directory, List<(string extension, int count)> list)
        {
            this.richTextBoxConsole.Text += $"Directory: {directory}{Environment.NewLine}";

            foreach (var item in list)
            {
                this.richTextBoxConsole.Text += $"{item.count} {item.extension} files.{Environment.NewLine}";
            }

            this.richTextBoxConsole.Text += $"{list.Where(i => i.extension != "").Sum(i => i.count)}{Environment.NewLine}";
            this.richTextBoxConsole.Text += $"{Environment.NewLine}";
            this.richTextBoxConsole.SelectionStart = this.richTextBoxConsole.Text.Length;
        }

        private void ShowMusicFileCount(List<(string extension, int count)> list)
        {
            string[] musicFileExtensions = { ".m4a", ".mp3", ".wav" };
            this.richTextBoxConsole.Text += $"{list.Where(i => musicFileExtensions.Contains(i.extension)).Sum(i => i.count)} music files.{Environment.NewLine}";
            this.richTextBoxConsole.SelectionStart = this.richTextBoxConsole.Text.Length;
        }

        private void ShowTrackInfo(int count, string message, string missingFiles)
        {
            this.richTextBoxConsole.Text += $"{count} file references found in the xml.{Environment.NewLine}";
            this.richTextBoxConsole.Text += message;

            this.richTextBoxConsole.Text += $"{Static.Library.Tracks.Count} tracks found in the xml.{Environment.NewLine}";

            this.richTextBoxConsole.Text += missingFiles;

            this.richTextBoxConsole.Text += $"{Environment.NewLine}";
            this.richTextBoxConsole.SelectionStart = this.richTextBoxConsole.Text.Length;
        }

        private void ShowLocalPlaylistInfo()
        {
            (int count, string message) = _musicLibrary.GetLocalPlaylistInfo();

            this.richTextBoxConsole.Text += $"{count} playlists found in the xml.{Environment.NewLine}";

            if (string.IsNullOrEmpty(message))
            {
                this.richTextBoxConsole.Text += $"All playlist names are unique.{Environment.NewLine}";
            }
            else
            {
                this.richTextBoxConsole.Text += $"Playlist names are not unique.{Environment.NewLine}";
                this.richTextBoxConsole.Text += $"{message}";
            }

            this.richTextBoxConsole.Text += $"{Environment.NewLine}";
            this.richTextBoxConsole.SelectionStart = this.richTextBoxConsole.Text.Length;
        }

        private void ShowCloudPlaylistInfo(int count, string message)
        {
            this.richTextBoxConsole.Text += $"{count} playlists found in the cloud.{Environment.NewLine}";

            if (string.IsNullOrEmpty(message))
            {
                this.richTextBoxConsole.Text += $"All playlist names are unique.{Environment.NewLine}";
            }
            else
            {
                this.richTextBoxConsole.Text += $"Playlist names are not unique.{Environment.NewLine}";
                this.richTextBoxConsole.Text += $"{message}";
            }

            this.richTextBoxConsole.Text += $"{Environment.NewLine}";
            this.richTextBoxConsole.SelectionStart = this.richTextBoxConsole.Text.Length;
        }
    }
}
