using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using SharpCompress.Archives;

namespace InteropTypes.IO.Archives
{
    [System.Diagnostics.DebuggerDisplay("{_Path}")]
    public sealed partial class SharpCompressFileProvider 
        : Primitives.ArchiveFileProviderBase<IArchiveEntry>        
        , IDisposable
    {
        #region lifecycle

        public static SharpCompressFileProvider Open(System.IO.Stream stream)
        {
            try
            {
                var f = ArchiveFactory.Open(stream);
                if (f == null) return null;

                return Create(f, false, "<stream>");
            }
            catch { return null; }
        }

        public static SharpCompressFileProvider Open(string path)
        {
            try
            {
                var f = ArchiveFactory.Open(path);
                if (f == null) return null;

                return Create(f, false, path);
            }
            catch { return null; }
        }

        public static SharpCompressFileProvider Create(IArchive archive, bool leaveArchiveOpen, string path)
        {
            if (archive == null) throw new ArgumentNullException(nameof(archive));
            if (archive.IsSolid) throw new NotImplementedException("Solid archives not supported yet");

            var accessor = new SharpCompressArchiveAccessor(archive);

            return new SharpCompressFileProvider(archive, leaveArchiveOpen, accessor, path);
        }

        protected SharpCompressFileProvider(IArchive archive, bool leaveArchiveOpen, Primitives.IArchiveAccessor<IArchiveEntry> accessor, string path)
            : base(accessor)
        {
            _Archive = archive;
            _LeaveArchiveOpen = leaveArchiveOpen;
            _Path = path;
        }

        public void Dispose()
        {
            if (!_LeaveArchiveOpen) _Archive?.Dispose();
            _Archive = null;
        }

        #endregion

        #region data

        private string _Path;
        private IArchive _Archive;
        private bool _LeaveArchiveOpen;

        #endregion        
    }    
}
