using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace Helper
{
    static class Utils
    {
        public static Image CreateImageFileIfNotExists(Image image, string fileName, string url)
        {
            if (image == null)
            {
                string fileNameEmpty = $@"C:\Users\Ferenc\OneDrive\Documents\My App Data\ChartParser\AppleMusic\empty.jpeg";

                try
                {
                    if (!File.Exists(fileName))
                    {
                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            using (var wc = new WebClient())
                            {
                                byte[] data = wc.DownloadData(url);
                                if (data == null)
                                    return Image.FromFile(fileNameEmpty);

                                using (var imgStream = new MemoryStream(data))
                                {
                                    image = Image.FromStream(imgStream);
                                    if (image == null)
                                        return Image.FromFile(fileNameEmpty);
                                    image.Save(fileName);
                                }
                            }
                        }
                        else
                            return Image.FromFile(fileNameEmpty);
                    }
                    else
                    {
                        image = Image.FromFile(fileName);
                    }
                }
                catch (OutOfMemoryException ex)
                {
                }
            }

            return image;
        }
    }
}
