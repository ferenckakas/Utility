using System.IO;

namespace Common
{
    public static class Constant
    {
        private static readonly string MusicDirectory = @"\\Mac\Home\Music";
        private static readonly string ITunesXmlFile = "iTunes Library.xml"; // "iTunes Music Library.xml"
        private static readonly string ITunesFolder = "iTunesCloud"; //iTunes

        public static readonly string ITunesDirectory = $@"{MusicDirectory}\{ITunesFolder}";
        public static readonly string ITunesMediaDirectory = $@"{ITunesDirectory}\iTunes Media";
        public static readonly string ITunesMusicLibraryXml = $@"{ITunesDirectory}\{ITunesXmlFile}";
        public static readonly string ITunesMusicDirectory = $@"{ITunesMediaDirectory}\Music";
        public static readonly string MediaLibraryConfigDirectory = $@"{MusicDirectory}\MediaLibraryConfig";
        public static readonly string BackupMusicDirectory = $@"C:\Users\Ferenc\OneDrive\Documents\Backup\C Users Ferenc\Music\{ITunesFolder}\iTunes Media\Music";
        public static readonly string TextDBPath = @"C:\Projects\MusicApp\TextDB";
        public static readonly string EnglishAlphabet = "abcdefghijklmnopqrstuvwxyz";

        public static readonly string DataDirectory = Path.Combine(ITunesMediaDirectory, "Data");

        public static readonly string TagsInLibraryFile = Path.Combine(DataDirectory, "tagsinlibrary.txt");
        public static readonly string TagsInMusicFilesFile = Path.Combine(DataDirectory, "tagsinmusicfiles.txt");

        public static readonly string Mp3Directory = Path.Combine(ITunesDirectory, "Mp3");

    }
}
