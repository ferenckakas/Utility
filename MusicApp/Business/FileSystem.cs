using Business.Interfaces;
using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Business
{
    public class FileSystem : IFileSystem
    {
        public void GetFileCount(string directory, ref List<(string extension, int count)> list)
        {
            if (!Directory.Exists(directory))
                return;

            string[] files = Directory.GetFiles(directory);

            if (files.Length > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    string extension = list[i].extension;
                    int count = list[i].count;

                    if (string.IsNullOrEmpty(extension))
                        list[i] = (extension, count + files.Length);
                    else
                        list[i] = (extension, count + files.Count(f => f.EndsWith(extension)));
                }
            }

            string[] directories = Directory.GetDirectories(directory);

            if (directories.Length == 0)
                return;
            else
            {
                foreach (string d in directories)
                {
                    GetFileCount(d, ref list);
                }
                return;
            }
        }

        public string GetMissingFiles(string directory)
        {
            string message = string.Empty;
            int index = $"file://localhost{Constant.ITunesMusicDirectory.Replace(@"\", "/")}".Length;

            var fileList = new List<string>();

            foreach (Helper.Track track in Static.Library.Tracks)
            {
                if (track.Location != null)
                {
                    string fileName = Uri.UnescapeDataString(track.Location);
                    fileName = fileName.Substring(index);
                    fileName = fileName.Replace("/", "\\");
                    fileName = $@"{Constant.ITunesMusicDirectory}{fileName}";

                    fileList.Add(fileName.ToLower());
                }
            }

            GetMissingFilesRecursive(directory, fileList, ref message);

            return message;
        }

        private void GetMissingFilesRecursive(string directory, List<string> fileList, ref string message)
        {
            if (!Directory.Exists(directory))
                return;

            string[] files = Directory.GetFiles(directory);

            string[] musicFileExtensions = { ".m4a", ".mp3", ".wav" };
            foreach (string file in files.Where(f => musicFileExtensions.Any(e => f.EndsWith(e))))
            {
                bool found = false;
                string fileLowerCase = file.ToLower();
                foreach (string file2 in fileList)
                {
                    if (fileLowerCase == file2)
                    {
                        found = true;
                        fileList.Remove(file2);
                        break;
                    }
                }

                if (!found)
                    message += $" Not found: {file}{Environment.NewLine}";
            }

            string[] directories = Directory.GetDirectories(directory);

            if (directories.Length == 0)
                return;
            else
            {
                foreach (string d in directories)
                {
                    GetMissingFilesRecursive(d, fileList, ref message);
                }
                return;
            }
        }
    }
}
