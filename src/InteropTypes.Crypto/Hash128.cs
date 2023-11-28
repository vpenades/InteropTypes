using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace InteropTypes.Crypto
{
    partial struct Hash128
    {
        public static Hash128 Md5FromFile(System.IO.FileInfo finfo)
        {
            using (var s = finfo.OpenRead())
            {
                return Md5FromStream(s);
            }
        }

        public static Hash128 Md5FromFile(Microsoft.Extensions.FileProviders.IFileInfo finfo, bool useCachedValues = true)
        {
            if (useCachedValues)
            {
                if (finfo is IServiceProvider srv)
                {
                    if (TryGetFromService(srv, "md5", out var r)) return r;
                }

                if (finfo is ISource src)
                {
                    var h = src.GetHash128Code();
                    if (!h.IsZero) return h;
                }
            }

            using (var s = finfo.CreateReadStream())
            {
                return Md5FromStream(s);
            }
        }

        public static Hash128 Md5FromStream(Func<System.IO.Stream> fileStream)
        {
            if (fileStream == null) return default;

            using(var s = fileStream.Invoke())
            {
                return Md5FromStream(s);
            }
        }

        public static Hash128 Md5FromStream(System.IO.Stream stream)
        {
            if (stream == null) return default;

            if (stream is System.IO.MemoryStream ms && ms.TryGetBuffer(out var buff))
            {
                return Md5FromBytes(buff);
            }

            var bytes = _HashEngines.Md5Engine.ComputeHash(stream);

            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash128(bytes);
        }

        public static Hash128 Md5FromList<TBytes>(TBytes collection)
            where TBytes : IEnumerable<Byte>
        {
            switch (collection)
            {
                case null: return default;
                case Byte[] array: return Md5FromBytes(array);
                case ArraySegment<Byte> segment: return Md5FromBytes(segment);
                #if NET6_0_OR_GREATER
                case List<Byte> list:                    
                    var bytes = CollectionsMarshal.AsSpan(list);
                    return Md5FromBytes(bytes);
                #endif
                default: return Md5FromBytes(collection.ToArray());
            }
        }

        public static Hash128 Md5FromBytes(ReadOnlySpan<Byte> bytes)
        {
            if (bytes.IsEmpty) return default;
            
            Span<Byte> hash = stackalloc byte[BYTESIZE];
            if (!_HashEngines.Md5Engine.TryComputeHash(bytes, hash, out _)) throw new InvalidOperationException();            

            System.Diagnostics.Debug.Assert(hash.Length == BYTESIZE);
            return new Hash128(hash);
        }

        public static Hash128 FromGuid(in Guid guid)
        {
            ref var guidRef = ref System.Runtime.CompilerServices.Unsafe.AsRef(guid);
            return System.Runtime.CompilerServices.Unsafe.As<Guid, Hash128>(ref guidRef);
        }

        public Guid ToGuid()
        {
            ref var hRef = ref System.Runtime.CompilerServices.Unsafe.AsRef(this);
            return System.Runtime.CompilerServices.Unsafe.As<Hash128, Guid>(ref hRef);
        }
    }
}
