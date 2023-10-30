using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.FileProviders;

using SharpSvn;

namespace InteropTypes.IO
{
    [System.Diagnostics.DebuggerDisplay("{_Target.FileName}")]
    struct _DirectoryContents : IDirectoryContents
    {
        #region lifecycle

        public _DirectoryContents(SvnClient client, SvnTarget target)
        {
            _Client = client;
            _Target = target;
        }

        #endregion

        #region data

        private SvnClient _Client;
        private SvnTarget _Target;

        #endregion

        #region API

        public bool Exists => !(_Client?.IsDisposed ?? true);

        public IEnumerator<IFileInfo> GetEnumerator() { return _GetContents().GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return _GetContents().GetEnumerator(); }

        private IEnumerable<IFileInfo> _GetContents()
        {
            var opts = System.IO.SearchOption.AllDirectories;

            var list = _Client.FindFilesAndDirectories(_Target, opts).ToList();

            return _Populate(list, string.Empty, opts == System.IO.SearchOption.AllDirectories);
        }

        internal IEnumerable<IFileInfo> _Populate(IReadOnlyList<SvnListEventArgs> list, string basePath, bool recurse)
        {
            var entries = list.SelectPathSlice(basePath).ToArray();

            foreach (var entry in entries)
            {
                if (entry.Entry.NodeKind == SvnNodeKind.Directory)
                {
                    var dir = new _SVNClientDirectoryInfo(_Client, _Target + "/" + entry.Name, entry);

                    if (recurse)
                    {
                        var subPath = string.IsNullOrWhiteSpace(basePath)
                            ? entry.Name
                            : basePath + "/" + entry.Name;

                        var children = _Populate(list, subPath, true);

                        dir.SetDirectoryContents(children);
                    }

                    yield return dir;
                }

                if (entry.Entry.NodeKind == SvnNodeKind.File)
                {
                    yield return new _SVNClientFileInfo(_Client, _Target + "/" + entry.Name, entry);
                }
            }
        }

        #endregion
    }
}
