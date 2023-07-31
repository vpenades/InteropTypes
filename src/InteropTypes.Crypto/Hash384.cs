using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace InteropTypes.Crypto
{
    partial struct Hash384
    {
        public static Hash384 Sha384FromFile(System.IO.FileInfo finfo)
        {
            using (var s = finfo.OpenRead())
            {
                return Sha384FromStream(s);
            }
        }

        public static Hash384 Sha384FromFile(Microsoft.Extensions.FileProviders.IFileInfo finfo, bool useCachedValues = true)
        {
            if (useCachedValues)
            {
                if (finfo is IServiceProvider srv)
                {
                    if (TryGetFromService(srv, "sha384", out var r)) return r;
                }

                if (finfo is ISource src)
                {
                    var h = src.GetHash384Code();
                    if (!h.IsZero) return h;
                }
            }

            using (var s = finfo.CreateReadStream())
            {
                return Sha384FromStream(s);
            }
        }

        public static Hash384 Sha384FromStream(Func<System.IO.Stream> fileStream)
        {
            if (fileStream == null) return default;

            using (var s = fileStream.Invoke())
            {
                return Sha384FromStream(s);
            }
        }

        public static Hash384 Sha384FromStream(System.IO.Stream stream)
        {
            if (stream == null) return default;

            if (stream is System.IO.MemoryStream ms && ms.TryGetBuffer(out var buff))
            {
                return Sha384FromBytes(buff);
            }

            var bytes = _HashEngines.Sha384Engine.ComputeHash(stream);

            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash384(bytes);
        }

        public static Hash384 Sha256FromList(List<Byte> list)
        {
            if (list == null) return default;

            #if NETSTANDARD
            var bytes = list.ToArray();
            #else
            var bytes = CollectionsMarshal.AsSpan(list);
            #endif
            return Sha384FromBytes(bytes);
        }

        public static Hash384 Sha384FromBytes(ReadOnlySpan<Byte> bytes)
        {
            if (bytes.IsEmpty) return default;
            
            Span<Byte> hash = stackalloc byte[BYTESIZE];
            if (!_HashEngines.Sha384Engine.TryComputeHash(bytes, hash, out _)) throw new InvalidOperationException();            

            System.Diagnostics.Debug.Assert(hash.Length == BYTESIZE);
            return new Hash384(hash);
        }

        public Hash384(in Hash256 a, in Hash128 b)
        {
            this._A = a._A;
            this._B = a._B;
            this._C = a._C;
            this._D = a._D;
            this._E = b._A;
            this._F = b._B;
        }
    }
}
