using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InteropTypes.IO
{
    #if !NETSTANDARD
    [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All)]
    #endif
    [System.Diagnostics.DebuggerDisplay("{_Finfo.FullName,nq}")]
    public readonly struct FileInfoServices : IServiceProvider
    {
        #region constructors

        public FileInfoServices(System.IO.DirectoryInfo dinfo,params string[] filePath)
        {            
            _Finfo = dinfo.DefineFileInfo(filePath);
        }

        public FileInfoServices(string filePath)
        {
            _Finfo = new System.IO.FileInfo(filePath);
        }

        public FileInfoServices(FileInfo finfo)
        {
            _Finfo = finfo;
        }

        #endregion

        #region data

        private readonly System.IO.FileInfo _Finfo;

        #endregion

        #region API        

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(Func<FileMode, Stream>)) return (Func<FileMode, Stream>)_Open1;
            if (serviceType == typeof(Func<FileMode, FileAccess, Stream>)) return (Func<FileMode, FileAccess, Stream>)_Open2;
            if (serviceType == typeof(Func<FileMode, FileAccess, FileShare, Stream>)) return (Func<FileMode, FileAccess, FileShare, Stream>)CreateStream;

            if (serviceType == typeof(FileInfo)) return _Finfo;

            return null;
        }        

        private Stream _Open1(FileMode mode)
        {
            return _Open2(mode, (mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite));
        }        
        private Stream _Open2(FileMode mode, FileAccess access)
        {
            return CreateStream(mode, access, FileShare.None);
        }        
        public Stream CreateStream(FileMode mode, FileAccess access, FileShare share)
        {
            if (_Finfo == null) return null;

            _EnsureDirectory(mode);

            var s = _Finfo?.Open(mode, access, share);

            var f = _Finfo;
            return XStream.WrapWithCloseActions(s, len => f.Refresh());
        }

        private void _EnsureDirectory(FileMode mode)
        {
            if (mode == FileMode.Create || mode == FileMode.CreateNew || mode == FileMode.OpenOrCreate)
            {
                _Finfo?.Directory?.Create();
            }
        }

        #endregion
    }
}
