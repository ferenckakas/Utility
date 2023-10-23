using Business.Interfaces;
using Common;
using Services.AppleMusic;
using Services.AppleMusic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{
    public class MusicLibrary : IMusicLibrary
    {
        private readonly AppleMusicClient _appleMusicClient = new AppleMusicClient();

        public void LoadLibrary()
        {
            var _ = Static.Library;
        }

        public (int, string) GetLocalTrackInfo()
        {
            string s = "file://localhost";
            s += Constant.ITunesMusicDirectory.Replace(@"\", "/");
            int index = s.Length;
            int count = 0;
            bool showNoLocation = false;

            string message = "";

            foreach (Helper.Track track in Static.Library.Tracks)
            {
                if (track.Location != null)
                {
                    count++;

                    string fileName = Uri.UnescapeDataString(track.Location);
                    fileName = fileName.Substring(index);
                    fileName = $@"{Constant.ITunesMusicDirectory}{fileName}";

                    if (!File.Exists(fileName))
                        message += $" Not found: {fileName}{Environment.NewLine}";
                }
                else if (showNoLocation)
                {
                    message += $" No location: {track.ID}/{track.Name}/{track.Artist}{Environment.NewLine}";
                }
            }

            return (count, message);
        }

        public (int, string) GetLocalPlaylistInfo()
        {
            int count = Static.Library.Playlists.Count;

            List<string> playlistNames = Static.Library.Playlists.Select(p => p.Name).ToList();

            string m = "";
            foreach (Helper.Playlist playlist in Static.Library.Playlists)
            {
                int c = playlistNames.Count(name => name == playlist.Name);
                if (c > 1)
                {
                    m += $"{playlist.Name} {c}x{Environment.NewLine}";
                    playlistNames.RemoveAll(name => name == playlist.Name);
                }
            }

            return (count, m);
        }

        public async Task<(int, string)> GetCloudPlaylistInfoAsync()
        {
            List<Playlist> cloudPlaylists = await _appleMusicClient.GetAllPlaylists();

            int count = cloudPlaylists.Count;

            List<string> playlistNames = cloudPlaylists.Select(p => p.Attributes.Name).ToList();

            string m = "";
            foreach (Playlist playlist in cloudPlaylists)
            {
                int c = playlistNames.Count(name => name == playlist.Attributes.Name);
                if (c > 1)
                {
                    m += $"{playlist.Attributes.Name} {c}x{Environment.NewLine}";
                    playlistNames.RemoveAll(name => name == playlist.Attributes.Name);
                }
            }

            return (count, m);
        }
    }
}
