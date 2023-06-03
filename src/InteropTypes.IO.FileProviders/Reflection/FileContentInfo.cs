using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO.Reflection
{
    public class FileContentInfo : IEquatable<FileContentInfo>
    {
        #region static data

        static FileContentInfo()
        {
            MetadataFiles.Add("file_id.diz"); // https://es.wikipedia.org/wiki/FILE_ID.DIZ

            GarbageFiles.Add("thumbs.db");
            GarbageFiles.Add("sthumbs.dat");
            GarbageFiles.Add(".ds_store");
        }

        public static ICollection<string> MetadataFiles { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public static ICollection<string> GarbageFiles { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public static ICollection<string> DangerousFiles { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        #endregion

        #region lifecycle

        public FileContentInfo(IFileInfo info)
        {
            _Info = info;
        }

        #endregion

        #region data

        private readonly IFileInfo _Info;

        public override int GetHashCode() { return _Info.GetHashCode(); }

        public bool Equals(FileContentInfo other)
        {
            if (object.ReferenceEquals(this, other)) return true;

            return Equals(this._Info, other._Info);
        }

        #endregion

        #region properties        

        public FileContentType FileType => _Info.IdentifyContentType();

        #endregion

        #region API

        public static FileContentType IdentifyFile(IFileInfo finfo)
        {
            return finfo.IdentifyContentType();
        }        

        #endregion
    }
}
