using Helper;

namespace Business.Interfaces
{
    public interface ILibraryBuilder
    {
        Library GetLibrary();
        string CreateDatabase();
    }
}
