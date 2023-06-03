using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO
{
    public class ArchiveFileInfo
        : IFileInfo
        , IServiceProvider
    {
        public ArchiveFileInfo(SharpCompress.Archives.IArchiveEntry entry)
        {
            _Entry = entry;
        }        

        private readonly SharpCompress.Archives.IArchiveEntry _Entry;

        public bool Exists => true;

        public long Length => _Entry.Size;

        public string PhysicalPath => _Entry.Key;

        public string Name => System.IO.Path.GetFileName(_Entry.Key);

        public DateTimeOffset LastModified => _Entry.LastModifiedTime ?? DateTimeOffset.MinValue;

        public bool IsDirectory => _Entry.IsDirectory;

        public Stream CreateReadStream()
        {
            return _Entry.OpenEntryStream();
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(SharpCompress.Archives.IArchiveEntry)) return _Entry;

            if (_Entry is SharpCompress.Archives.Zip.ZipArchiveEntry zipEntry)
            {
                try
                {
                    if (serviceType == typeof(string)) return zipEntry.Comment;                    

                    if (serviceType == typeof(JsonDocument))
                    {
                        return string.IsNullOrWhiteSpace(zipEntry.Comment)
                            ? null
                            : (object)JsonDocument.Parse(zipEntry.Comment);
                    }

                    if (serviceType == typeof(System.Xml.Linq.XDocument))
                    {
                        return string.IsNullOrWhiteSpace(zipEntry.Comment)
                            ? null
                            : (object)System.Xml.Linq.XDocument.Parse(zipEntry.Comment);
                    }
                }
                catch { }
            }            

            return null;
        }
    }
}
