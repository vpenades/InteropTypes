using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace System.Net
{
    internal static class _Extensions
    {
        public static bool TryUnzipTo(this Uri srcUri, System.IO.DirectoryInfo dstPath)
        {
            using(var client = new Http.HttpClient())
            {
                return client.TryUnzipTo(srcUri, dstPath);
            }
        }

        public static bool TryUnzipTo(this Http.HttpClient client, Uri srcUri, System.IO.DirectoryInfo dstPath)
        {
            var bytes = System.Threading.Tasks.Task
                    .Run(async () => await client.GetByteArrayAsync(srcUri).ConfigureAwait(false))
                    .Result;

            dstPath.Create();

            using (var m = new System.IO.MemoryStream(bytes))
            {
                using (var z = new System.IO.Compression.ZipArchive(m))
                {
                    z.ExtractToDirectory(dstPath.FullName);
                }
            }

            return true;
        }

    }
}
