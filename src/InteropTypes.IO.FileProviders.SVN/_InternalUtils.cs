using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpSvn;

namespace InteropTypes.IO
{
    internal static class _InternalUtils
    {
        public static IEnumerable<SvnListEventArgs> SelectPathSlice(this IEnumerable<SvnListEventArgs> list, string basePath)
        {
            foreach(var entry in list)
            {
                var path = entry.Path;

                if (!path.StartsWith(basePath)) continue;

                path = path.Substring(basePath.Length);
                path = path.TrimStart('/');

                if (string.IsNullOrEmpty(path)) continue; // skip parent from list
                if (path.Contains('/', StringComparison.OrdinalIgnoreCase)) continue; // skip children from list

                yield return entry;
            }            
        }

        public static SvnTarget Concat(this SvnTarget target, string relativePath)
        {
            if (relativePath == null) return target;

            if (!string.IsNullOrEmpty(relativePath) && !relativePath.StartsWith("/")) relativePath = "/" + relativePath;
            return target + relativePath;
        }        

        /// <summary>
        /// Retrieves the list of files of a given repository.
        /// </summary>
        /// <param name="client">The client context.</param>
        /// <param name="repositoryUrl">The repository url.</param>
        /// <param name="searchOpt">Directory listing options.</param>
        /// <param name="revision">The revision to query, or null for Head.</param>
        /// <returns>A list of file and directory entries</returns>        
        public static IEnumerable<SvnListEventArgs> FindFilesAndDirectories(this SvnClient client, SvnTarget repositoryUrl, SearchOption searchOpt, SvnRevision revision = null)
        {
            if (client == null || client.IsDisposed) throw new ObjectDisposedException("SVNClient");

            // Create a SvnListArgs object and set some options
            var args = new SvnListArgs();            

            args.Depth = searchOpt.ToSvnDepth();
            args.RetrieveEntries = SvnDirEntryItems.AllFieldsV15; // retrieve all information for each entry
            args.Revision = revision ?? args.Revision;

            // Get the list of files and directories from the remote repository
            if (client.GetList(repositoryUrl, args, out var list))
            {
                foreach (var entry in list)
                {
                    if (string.IsNullOrEmpty(entry.Name)) continue;

                    yield return entry;
                }
            }
        }

        public static SvnDepth ToSvnDepth(this System.IO.SearchOption option)
        {
            switch (option)
            {
                case SearchOption.TopDirectoryOnly: return SvnDepth.Children;
                case SearchOption.AllDirectories: return SvnDepth.Infinity;
                default: throw new NotSupportedException(option.ToString());
            }
        }

        /// <summary>
        /// Opens a file stream to a remote repository
        /// </summary>
        /// <param name="client">The client context.</param>
        /// <param name="repositoryUrl">The source repository.</param>
        /// <returns>A memory stream with the file contents.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public static System.IO.MemoryStream OpenReadStream(this SvnClient client, SvnTarget repositoryUrl, SvnRevision revision = null)
        {
            if (client == null || client.IsDisposed) throw new ObjectDisposedException(nameof(client));

            var args = new SvnWriteArgs();
            args.Revision = revision ?? args.Revision;

            var m = new System.IO.MemoryStream();
            client.Write(repositoryUrl, m, args);
            m.Position = 0;
            return m;
        }

        public static System.IO.MemoryStream GetCachedMemoryStream(ref WeakReference<Byte[]> bytes, Func<MemoryStream> create)
        {
            if (bytes != null && bytes.TryGetTarget(out var array))
            {
                return new System.IO.MemoryStream(array, false);
            }

            var m = create();
            bytes = new WeakReference<byte[]>(m.ToArray());

            return m;
        }
    }
}
