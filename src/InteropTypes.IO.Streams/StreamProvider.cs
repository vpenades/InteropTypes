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

        // [RequiresUnreferencedCode("Calls InteropTypes.IO.StreamProvider<T>._DefaultProvider._TryGetReadStream<TSrv>(T, out Stream)")]
        public abstract STREAM CreateReadStreamFrom(T obj);

        // [RequiresUnreferencedCode("Calls InteropTypes.IO.StreamProvider<T>._DefaultProvider._TryGetReadStream<TSrv>(T, out Stream)")]
        public abstract STREAM CreateWriteStreamFrom(T obj);

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
            using(var s = CreateReadStreamFrom(src))
            {
                return XStream.ReadAllBytes(s);
            }
        }        

        public void WriteAllBytesTo(T dst, BYTESSEGMENT bytes)
        {
            using(var s = CreateWriteStreamFrom(dst))
            {
                XStream.WriteAllBytes(s, bytes);
            }
        }

        public string ReadAllTextFrom(T src)
        {
            using (var s = CreateReadStreamFrom(src))
            {
                return XStream.ReadAllText(s);
            }
        }

        public string ReadAllTextFrom(T src, Encoding encoding)
        {
            using (var s = CreateReadStreamFrom(src))
            {
                return XStream.ReadAllText(s, encoding);
            }
        }

        public void WriteAllTextTo(T dst, string contents)
        {
            using (var s = CreateWriteStreamFrom(dst))
            {
                XStream.WriteAllText(s, contents);
            }
        }

        public void WriteAllTextTo(T dst, string contents, Encoding encoding)
        {
            using (var s = CreateWriteStreamFrom(dst))
            {
                XStream.WriteAllText(s, contents, encoding);
            }
        }        

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
                if (obj == null) throw new ArgumentNullException(nameof(obj));

                // static type evaluation

                if (typeof(T) == typeof(Uri))
                {
                    var uri = Unsafe.As<T, Uri>(ref obj);
                    if (uri.IsFile)
                    {
                        var finfo = new FileInfo(uri.LocalPath);
                        return StreamProvider<FileInfo>.Default.CreateReadStreamFrom(finfo);
                    }                    

                    throw new ArgumentException($"Unsupported {uri}");
                }

                if (typeof(T) == typeof(FileInfo)) return Unsafe.As<T, FileInfo>(ref obj).OpenRead();

                if (typeof(T) == typeof(Byte[]))
                {
                    var array = Unsafe.As<T, Byte[]>(ref obj);
                    return array.Length == 0
                        ? new System.IO.MemoryStream(Array.Empty<byte>(),false)
                        : (Stream)new System.IO.MemoryStream(array,0, array.Length, false);
                }

                if (typeof(T) == typeof(BYTESSEGMENT))
                {
                    var segment = Unsafe.As<T, BYTESSEGMENT>(ref obj);
                    return XStream.CreateFrom(segment, false);
                }

                if (typeof(T) == typeof(IReadOnlyList<Byte>))
                {
                    var list = Unsafe.As<T, IReadOnlyList<Byte>>(ref obj);
                    return XStream.CreateFrom(list);
                }                

                if (typeof(T) == typeof(List<Byte>))
                {
                    var list = Unsafe.As<T, List<Byte>>(ref obj);
                    return XStream.CreateFrom(list, FileMode.Open);
                }

                if (typeof(T) == typeof(System.IO.Compression.ZipArchiveEntry))
                {
                    var zentry = Unsafe.As<T, System.IO.Compression.ZipArchiveEntry>(ref obj);
                    return XStream.CreateFrom(zentry, FileMode.Open);
                }                

                if (typeof(T) == typeof(READWRITESTREAM1)) return Unsafe.As<T, READWRITESTREAM1>(ref obj).Invoke(FileMode.Open);

                if (typeof(T) == typeof(READWRITESTREAM2)) return Unsafe.As<T, READWRITESTREAM2>(ref obj).Invoke(FileMode.Open, FileAccess.Read);                            

                if (typeof(IFileInfo).IsAssignableFrom(typeof(T)))
                {
                    var xinfo = Unsafe.As<T, IFileInfo>(ref obj);
                    if (xinfo.IsDirectory) throw new ArgumentException("Expected a file, got a directory", nameof(obj));
                    return xinfo.CreateReadStream();
                }

                if (typeof(IServiceProvider).IsAssignableFrom(typeof(T)))
                {
                    if (_TryGetReadStream<Uri>(obj, out var s0)) return s0;
                    if (_TryGetReadStream<FileInfo>(obj, out var s1)) return s1;
                    if (_TryGetReadStream<READWRITESTREAM1>(obj, out var s2)) return s2;
                    if (_TryGetReadStream<READWRITESTREAM2>(obj, out var s3)) return s3;
                }

                // runtime type evaluation

                if (obj is FileInfo fi) return StreamProvider<FileInfo>.Default.CreateReadStreamFrom(fi);
                if (obj is IReadOnlyList<Byte> lb) return StreamProvider<IReadOnlyList<Byte>>.Default.CreateReadStreamFrom(lb);
                if (obj is IFileInfo xfi) return StreamProvider<IFileInfo>.Default.CreateReadStreamFrom(xfi);
                if (obj is IServiceProvider srv) return StreamProvider<IServiceProvider>.Default.CreateReadStreamFrom(srv);

                if (_TryGetFromMethod(obj, "CreateReadStream", out var sx)) return sx;

                throw new NotSupportedException(typeof(T).GetType().FullName);
            }

            public override STREAM CreateWriteStreamFrom(T obj)
            {
                if (obj == null) throw new ArgumentNullException(nameof(obj));

                // static type evaluation

                if (typeof(T) == typeof(Uri))
                {
                    var uri = Unsafe.As<T, Uri>(ref obj);
                    if (uri.IsFile)
                    {
                        var finfo = new FileInfo(uri.LocalPath);
                        return StreamProvider<FileInfo>.Default.CreateWriteStreamFrom(finfo);
                    }

                    throw new ArgumentException($"Unsupported {uri}");
                }

                if (typeof(T) == typeof(FileInfo))
                {
                    var finfo = Unsafe.As<T, FileInfo>(ref obj);
                    finfo.Directory.Create();                    
                    return finfo.Create();
                }

                if (typeof(T) == typeof(List<Byte>))
                {
                    var list = Unsafe.As<T, List<Byte>>(ref obj);
                    return XStream.CreateFrom(list, FileMode.Create);
                }

                if (typeof(T) == typeof(System.IO.Compression.ZipArchiveEntry))
                {
                    var zentry = Unsafe.As<T, System.IO.Compression.ZipArchiveEntry>(ref obj);
                    return XStream.CreateFrom(zentry, FileMode.CreateNew);
                }

                if (typeof(T) == typeof(READWRITESTREAM1)) return Unsafe.As<T, READWRITESTREAM1>(ref obj).Invoke(FileMode.Create);

                if (typeof(T) == typeof(READWRITESTREAM2)) return Unsafe.As<T, READWRITESTREAM2>(ref obj).Invoke(FileMode.Create, FileAccess.Write);                

                if (typeof(IServiceProvider).IsAssignableFrom(typeof(T)))
                {
                    if (_TryGetWriteStream<Uri>(obj, out var s0)) return s0;
                    if (_TryGetWriteStream<FileInfo>(obj, out var s1)) return s1;
                    if (_TryGetWriteStream<READWRITESTREAM1>(obj, out var s2)) return s2;
                    if (_TryGetWriteStream<READWRITESTREAM2>(obj, out var s3)) return s3;
                }

                // runtime type evaluation

                if (obj is FileInfo fi) return StreamProvider<FileInfo>.Default.CreateWriteStreamFrom(fi);
                if (obj is List<Byte> lb) return StreamProvider<List<Byte>>.Default.CreateWriteStreamFrom(lb);
                if (obj is IServiceProvider srv) return StreamProvider<IServiceProvider>.Default.CreateWriteStreamFrom(srv);

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
