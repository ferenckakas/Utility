using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IMusicLibrary
    {
        void LoadLibrary();

        (int, string) GetLocalTrackInfo();

        (int, string) GetLocalPlaylistInfo();

        Task<(int, string)> GetCloudPlaylistInfoAsync();
    }
}
