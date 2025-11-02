using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;

namespace _24hplusdotnetcore.Extensions
{
    public static class ZipEntensions
    {
        public static byte[] CompressDirectory(IEnumerable<(byte[] bytes, string names)> files, int compressionLevel = 9)
        {
            using var stream = new MemoryStream();
            using ZipOutputStream outputStream = new ZipOutputStream(stream);

            // Define the compression level
            // 0 - store only to 9 - means best compression
            outputStream.SetLevel(compressionLevel);

            foreach (var (bytes, names) in files)
            {
                ZipEntry entry = new ZipEntry(names)
                {
                    DateTime = DateTime.Now
                };

                outputStream.PutNextEntry(entry);

                outputStream.Write(bytes, 0, bytes.Length);
            }

            outputStream.Finish();

            outputStream.Close();

            return stream.ToArray();
        }
    }
}
