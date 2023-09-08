using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO.Reflection
{
    [System.Diagnostics.DebuggerDisplay("{FileType} {_Info.Name}")]
    public class FileContentInfo : IEquatable<FileContentInfo>
    {
        #region static data

        static FileContentInfo()
        {
            MetadataFiles.Add("file_id.diz"); // https://es.wikipedia.org/wiki/FILE_ID.DIZ

            GarbageFiles.Add("thumbs.db");
            GarbageFiles.Add("sthumbs.dat");
            GarbageFiles.Add(".ds_store");

            DangerousFiles.Add("rarbg_do_not_mirror.exe");
        }

        public static ICollection<string> MetadataFiles { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public static ICollection<string> GarbageFiles { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public static ICollection<string> DangerousFiles { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        #endregion

        #region lifecycle

        public static FileContentType IdentifyFile(IFileInfo finfo)
        {            
            XFile.GuardIsValidFile(finfo);
            return finfo.IdentifyContentType();
        }

        public static FileContentInfo CreateFrom(IFileInfo finfo)
        {
            XFile.GuardIsValidFile(finfo);
            return new FileContentInfo(finfo);
        }

        private FileContentInfo(IFileInfo info)
        {
            _Info = info;
        }

        #endregion

        #region data

        private readonly IFileInfo _Info;

        public override int GetHashCode() { return _Info.GetHashCode(); }

        public override bool Equals(object obj) { return obj is FileContentInfo other && Equals(other); }

        public bool Equals(FileContentInfo other)
        {
            if (object.ReferenceEquals(this, other)) return true;

            return Equals(this._Info, other._Info);
        }

        #endregion

        #region properties

        public IFileInfo File => _Info;

        public FileContentType FileType => _Info.IdentifyContentType();

        #endregion

        #region static

        public static bool ExtensionIsArchivePart(string extension)
        {
            bool _isMatch(string ext, string prefix, string suffix)
            {
                ext = ext.ToLower();
                if (ext.StartsWith(prefix)) ext = ext.Substring(prefix.Length);
                if (suffix.Length > 0 && ext.EndsWith(suffix)) ext = ext.Substring(0, ext.Length - suffix.Length);
                return int.TryParse(ext, out _);
            }

            if (_isMatch(extension, ".7z.", string.Empty)) return true;
            if (_isMatch(extension, ".zip.", string.Empty)) return true;
            if (_isMatch(extension, ".part.", ".rar")) return true;

            return false;
        }

        #endregion
    }
}
