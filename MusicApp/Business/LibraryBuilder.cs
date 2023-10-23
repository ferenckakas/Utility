using Business.Interfaces;
using Common;
using Helper;

namespace Business
{
    public class LibraryBuilder : ILibraryBuilder
    {
        private readonly iTunesHelper _iTunesHelper;

        public LibraryBuilder()
        {
            _iTunesHelper = new iTunesHelper(Constant.ITunesDirectory);
        }

        public Library GetLibrary()
        {
            return _iTunesHelper.Library;
        }

        public string CreateDatabase()
        {
            return _iTunesHelper.CreateDatabase();
        }
    }
}
