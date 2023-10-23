using Business.Interfaces;
using Helper;

namespace Business
{
    public static class Static
    {
        private static readonly ILibraryBuilder s_libraryBuilder = new LibraryBuilder();

        static Static()
        {
        }

        public static Library Library
        {
            get
            {
                return s_libraryBuilder.GetLibrary();
            }
        }

        public static string CreateDatabase()
        {
            return s_libraryBuilder.CreateDatabase();
        }
    }
}
