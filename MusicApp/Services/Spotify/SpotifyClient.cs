using Services.Old;
using SpotifyWebAPI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Spotify
{
    public class SpotifyClient
    {
        private List<Playlist> _playlists { get; set; }

        private User _user { get; set; }

        public AuthenticationToken AuthenticationToken {
            get {
                return Application.AuthenticationToken;
            }
        }

        public async Task LoadPlaylists()
        {
            // get the user
            _user = await User.GetCurrentUserProfile(AuthenticationToken);

            _playlists = await Playlist.GetAllPlaylists(_user.Id, AuthenticationToken);
        }

        public async Task<List<Track>> SearchTracks(string term, int maxHit)
        {
            Page<Track> tracks = await Track.SearchAll(term, AuthenticationToken, maxHit);

            if (tracks == null || tracks.Items == null) return new List<Track>();

            return tracks.Items.Take(maxHit).ToList();
        }

        public async Task<List<PlaylistTrack>> GetAllPlaylistTracks(string playlistName)
        {
            Playlist playlist = _playlists.FirstOrDefault(p => p.Name == playlistName);

            if (playlist == null)
            {
                _playlists = await Playlist.GetAllPlaylists(_user.Id, AuthenticationToken);

                playlist = _playlists.FirstOrDefault(p => p.Name == playlistName);
            }

            if (playlist == null)
                return null;

            return await playlist.GetAllPlaylistTracks(AuthenticationToken);
        }

        public async Task CreatePlaylist(string playlistName, List<string> newTrackUris)
        {
            Playlist playlist = _playlists.Where(p => p.Name == playlistName).FirstOrDefault();

            bool update = false;

            if (playlist == null)
            {
                playlist = await Playlist.CreatePlaylist(_user.Id, playlistName, false, AuthenticationToken);
                //
                update = true;
            }
            else
            {
                List<PlaylistTrack> playlistTracks = await playlist.GetAllPlaylistTracks(AuthenticationToken);

                if (playlistTracks.Count == newTrackUris.Count)
                {
                    int i = 0;
                    foreach (PlaylistTrack playlistTrack in playlistTracks)
                    {
                        if (playlistTrack.Track.Uri != newTrackUris[i])
                            break;
                        i++;
                    }

                    if (i < playlistTracks.Count) update = true;
                }
                else
                    update = true;

                if (update)
                {
                    var tracks = new List<Track>();
                    foreach (PlaylistTrack playlistItem in playlistTracks)
                    {
                        tracks.Add(playlistItem.Track);
                    }

                    if (tracks.Any())
                        await playlist.RemoveTracks(AuthenticationToken, tracks);
                }
            }

            if (update)
            {
                //await playlist.ReplacePlaylistTracks(_user.Id, playlist.Id, AuthenticationToken, NewTracks);

                //foreach (string track in newTrackUris)
                //{
                //    await playlist.AddTrack(track, AuthenticationToken);
                //}

                await playlist.AddTracks(AuthenticationToken, newTrackUris);
            }
        }
    }
}
