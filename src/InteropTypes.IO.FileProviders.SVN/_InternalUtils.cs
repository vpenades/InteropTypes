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

        public static SvnDepth ToSvnDepth(this System.IO.SearchOption option)
        {
            switch (option)
            {
                case SearchOption.TopDirectoryOnly: return SvnDepth.Children;
                case SearchOption.AllDirectories: return SvnDepth.Infinity;
                default: throw new NotSupportedException(option.ToString());
            }
        }
        
        public static System.IO.MemoryStream GetMemoryStream(ref WeakReference<Byte[]> bytes, Func<MemoryStream> create)
        {
            if (bytes != null && bytes.TryGetTarget(out var array))
            {
                return new System.IO.MemoryStream(array, false);
            }

            var m = create();
            bytes = new WeakReference<byte[]>(m.ToArray());

            return m;            
        }

        public static SvnTarget Concat(this SvnTarget target, string relativePath)
        {
            if (relativePath == null) return target;

            if (!string.IsNullOrEmpty(relativePath) && !relativePath.StartsWith("/")) relativePath = "/" + relativePath;
            return target + relativePath;
        }

        public static System.IO.MemoryStream OpenReadStream(this SvnClient client, SvnTarget target)
        {
            if (client == null || client.IsDisposed) throw new ObjectDisposedException(nameof(client));            

            var m = new System.IO.MemoryStream();
            client.Write(target, m);
            m.Position = 0;
            return m;
        }

        public static IEnumerable<SvnListEventArgs> FindFilesAndDirectories(this SvnClient client, SvnTarget target, SearchOption searchOpt)
        {
            if (client == null || client.IsDisposed) throw new ObjectDisposedException("SVNClient");

            // Create a SvnListArgs object and set some options
            var args = new SvnListArgs();
            args.Depth = searchOpt.ToSvnDepth();
            args.RetrieveEntries = SvnDirEntryItems.AllFieldsV15; // retrieve all information for each entry
            // args.Revision = target.Revision; ?? is this needed?

            

            // Get the list of files and directories from the remote repository
            if (client.GetList(target, args, out var list))
            {
                foreach (var entry in list)
                {
                    if (string.IsNullOrEmpty(entry.Name)) continue;

                    yield return entry;
                }
            }
        }

    }
}
