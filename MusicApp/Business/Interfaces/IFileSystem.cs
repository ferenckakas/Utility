using System.Collections.Generic;

namespace Business.Interfaces
{
    public interface IFileSystem
    {
        void GetFileCount(string directory, ref List<(string extension, int count)> list);
        string GetMissingFiles(string directory);
    }
}
