using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

using Microsoft.Extensions.FileProviders;

using STREAM = System.IO.Stream;
using BYTESSEGMENT = System.ArraySegment<byte>;

using READWRITESTREAM1 = System.Func<System.IO.FileMode, System.IO.Stream>;
using READWRITESTREAM2 = System.Func<System.IO.FileMode, System.IO.FileAccess, System.IO.Stream>;
using READWRITESTREAM3 = System.Func<System.IO.FileMode, System.IO.FileAccess, System.IO.FileShare, System.IO.Stream>;

namespace InteropTypes.IO
{
    /// <summary>
    /// exposes file streaming services for the given type.
    /// </summary>    
    public abstract class StreamProvider
        #if NETSTANDARD
        <T>
        #else
        <[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>
        #endif
    {
        #region API

        public static StreamProvider<T> Default { get; } = _Initialize();       

        public System.IO.BinaryReader CreateBinaryReader(T src, Encoding encoding)
        {
            var s = CreateReadStreamFrom(src);
            if (s == null) return null;
            return new System.IO.BinaryReader(s, encoding, false);
        }
        public System.IO.BinaryWriter CreateBinaryWriter(T dst, Encoding encoding)
        {
            var s = CreateWriteStreamFrom(dst);
            if (s == null) return null;
            return new System.IO.BinaryWriter(s, encoding, false);
        }

        public BYTESSEGMENT ReadAllBytesFrom(T src)
        {
            using var s = CreateReadStreamFrom(src);
            return XStream.ReadAllBytes(s);
        }        

        public void WriteAllBytesTo(T dst, BYTESSEGMENT bytes)
        {
            using var s = CreateWriteStreamFrom(dst);
            XStream.WriteAllBytes(s, bytes);
        }

        public string ReadAllTextFrom(T src)
        {
            using var s = CreateReadStreamFrom(src);
            return XStream.ReadAllText(s);
        }

        public string ReadAllTextFrom(T src, Encoding encoding)
        {
            using var s = CreateReadStreamFrom(src);
            return XStream.ReadAllText(s, encoding);
        }

        public void WriteAllTextTo(T dst, string contents)
        {
            using var s = CreateWriteStreamFrom(dst);
            XStream.WriteAllText(s, contents);
        }

        public void WriteAllTextTo(T dst, string contents, Encoding encoding)
        {
            using var s = CreateWriteStreamFrom(dst);
            XStream.WriteAllText(s, contents, encoding);
        }

        #endregion

        #region abstract API

        // [RequiresUnreferencedCode("Calls InteropTypes.IO.StreamProvider<T>._DefaultProvider._TryGetReadStream<TSrv>(T, out Stream)")]
        public abstract STREAM CreateReadStreamFrom(T obj);

        // [RequiresUnreferencedCode("Calls InteropTypes.IO.StreamProvider<T>._DefaultProvider._TryGetReadStream<TSrv>(T, out Stream)")]
        public abstract STREAM CreateWriteStreamFrom(T obj);

        #endregion

        #region nested types

        private static StreamProvider<T> _Initialize()
        {
            if (_MSPhysicalFileProvider.IsMatch())
            {
                return new _MSPhysicalFileProvider();
            }

            if (_SharpCompressEntryProvider.IsMatch())
            {
                return new _SharpCompressEntryProvider();
            }

            return new _DefaultProvider();
        }

        class _DefaultProvider : StreamProvider<T>
        {
            // TODO: Support IReadOnlyList<Byte> for both Read & List<Byte> for write
            
            public override STREAM CreateReadStreamFrom(T obj)
            {
                switch (obj)
                {
                    case null: throw new ArgumentNullException(nameof(obj));
                    case Uri uri: return XStream.TryGetFileInfo(uri, out var fileInfo) ? fileInfo.OpenRead() : throw new ArgumentException($"Unsupported {uri}");
                    case FileInfo finfo: return finfo.OpenRead();
                    case IFileInfo finfo when !finfo.IsDirectory: return finfo.CreateReadStream();
                    case Byte[] array: return XStream.CreateMemoryStream(array);
                    case BYTESSEGMENT segment: return XStream.CreateMemoryStream(segment, false);
                    case IReadOnlyList<Byte> list: return XStream.WrapList(list);
                    case System.IO.Compression.ZipArchiveEntry zipEntry: return XStream.OpenArchive(zipEntry, FileMode.Open);
                    case READWRITESTREAM1 lambda: return lambda.Invoke(FileMode.Open);
                    case READWRITESTREAM2 lambda: return lambda.Invoke(FileMode.Open, FileAccess.Read);
                    case READWRITESTREAM3 lambda: return lambda.Invoke(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);                    
                }                

                if (typeof(IServiceProvider).IsAssignableFrom(typeof(T)))
                {
                    if (_TryGetReadStream<Uri>(obj, out var s0)) return s0;
                    if (_TryGetReadStream<FileInfo>(obj, out var s1)) return s1;
                    if (_TryGetReadStream<READWRITESTREAM1>(obj, out var s2)) return s2;
                    if (_TryGetReadStream<READWRITESTREAM2>(obj, out var s3)) return s3;
                }

                // runtime type evaluation                

                if (_TryGetFromMethod(obj, "CreateReadStream", out var sx)) return sx;

                throw new NotSupportedException(typeof(T).GetType().FullName);
            }

            public override STREAM CreateWriteStreamFrom(T obj)
            {
                switch(obj)
                {
                    case null: throw new ArgumentNullException(nameof(obj));
                    case Uri uri: return XStream.TryGetFileInfo(uri, out var fileInfo) ? fileInfo.OpenWrite() : throw new ArgumentException($"Unsupported {uri}");
                    case FileInfo finfo: finfo.Directory.Create(); return finfo.Create();
                    case List<Byte> list: return XStream.WrapList(list, FileMode.Create);
                    case System.IO.Compression.ZipArchiveEntry zipEntry: return XStream.OpenArchive(zipEntry, FileMode.CreateNew);
                    case READWRITESTREAM1 lambda: return lambda.Invoke(FileMode.Create);
                    case READWRITESTREAM2 lambda: return lambda.Invoke(FileMode.Create, FileAccess.Write);
                    case READWRITESTREAM3 lambda: return lambda.Invoke(FileMode.Create, FileAccess.Write, FileShare.None);
                }                

                if (typeof(IServiceProvider).IsAssignableFrom(typeof(T)))
                {
                    if (_TryGetWriteStream<Uri>(obj, out var s0)) return s0;
                    if (_TryGetWriteStream<FileInfo>(obj, out var s1)) return s1;
                    if (_TryGetWriteStream<READWRITESTREAM1>(obj, out var s2)) return s2;
                    if (_TryGetWriteStream<READWRITESTREAM2>(obj, out var s3)) return s3;
                }

                // runtime type evaluation                

                if (_TryGetFromMethod(obj, "CreateWriteStream", out var sx)) return sx;

                throw new NotSupportedException(typeof(T).GetType().FullName);
            }

            private static bool _TryGetReadStream
                #if NETSTANDARD
                <TSrv>
                #else
                <[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TSrv>
                #endif
                (T obj, out STREAM stream)
            {
                stream = default;

                if (!(obj is IServiceProvider sprv)) return false;

                var val = sprv.GetService(typeof(TSrv));
                if (val is TSrv xval)
                {
                    stream = StreamProvider<TSrv>.Default.CreateReadStreamFrom(xval);
                    return true;
                }

                return false;
            }

            private static bool _TryGetWriteStream
                #if NETSTANDARD
                <TSrv>
                #else
                <[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TSrv>
                #endif
                (T obj, out STREAM stream)
            {
                stream = default;

                if (!(obj is IServiceProvider sprv)) return false;

                var val = sprv.GetService(typeof(TSrv));
                if (val is TSrv xval)
                {
                    stream = StreamProvider<TSrv>.Default.CreateWriteStreamFrom(xval);
                    return true;
                }

                return false;
            }

            #if !NETSTANDARD
            // Prevent these from being trimmed:
            [DynamicDependency(DynamicallyAccessedMemberTypes.All, "SharpCompress.Archives.IArchiveEntry", "SharpCompress")]
            [DynamicDependency(DynamicallyAccessedMemberTypes.All, "SharpCompress.Archives.Zip.ZipArchiveEntry", "SharpCompress")]
            [DynamicDependency(DynamicallyAccessedMemberTypes.All, "SharpCompress.Archives.Rar.RarArchiveEntry", "SharpCompress")]
            [DynamicDependency(DynamicallyAccessedMemberTypes.All, "SharpCompress.Archives.SevenZip.SevenZipArchiveEntry", "SharpCompress")]
            #endif
            protected static bool _TryGetFromMethod(T obj, string methodName, out STREAM stream)
            {
                stream = default;

                if (obj == null) return false;

                var method = obj.GetType().GetMethod(methodName);
                if (method == null) return false;
                if (method.GetParameters().Length != 0) return false;
                if (method.ReturnType != typeof(STREAM)) return false;
                stream = method.Invoke(obj, Array.Empty<object>()) as STREAM;
                return stream != null;
            }
        }
        
        class _MSPhysicalFileProvider : _DefaultProvider        
        {
            public static bool IsMatch()
            {
                return typeof(T).FullName == "Microsoft.Extensions.FileProviders.Physical.PhysicalFileInfo";
            }

            public override STREAM CreateWriteStreamFrom(T obj)
            {
                if (obj is IFileInfo xinfo)
                {
                    if (xinfo.IsDirectory) throw new ArgumentException("Expected a file, got a directory", nameof(obj));
                    var finfo = new System.IO.FileInfo(xinfo.PhysicalPath);
                    return StreamProvider<FileInfo>.Default.CreateWriteStreamFrom(finfo);
                }

                return base.CreateWriteStreamFrom(obj);
            }
        }
        
        class _SharpCompressEntryProvider : _DefaultProvider
        {
            public static bool IsMatch()
            {
                if (typeof(T).FullName == "SharpCompress.Archives.IArchiveEntry") return true;
                return false;
            }            
            public override STREAM CreateReadStreamFrom(T obj)
            {
                if (_TryGetFromMethod(obj, "OpenEntryStream", out var sx)) return sx;

                return base.CreateReadStreamFrom(obj);
            }

            public override STREAM CreateWriteStreamFrom(T obj)
            {
                return base.CreateWriteStreamFrom(obj);
            }
        }

        #endregion
    }    
}
