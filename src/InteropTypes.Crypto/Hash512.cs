using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace InteropTypes.Crypto
{
    partial struct Hash512
    {
        public static Hash512 Sha512FromFile(System.IO.FileInfo finfo)
        {
            using (var s = finfo.OpenRead())
            {
                return Sha512FromStream(s);
            }
        }

        public static Hash512 Sha512FromFile(Microsoft.Extensions.FileProviders.IFileInfo finfo, bool useCachedValues = true)
        {
            if (useCachedValues)
            {
                if (finfo is IServiceProvider srv)
                {
                    if (TryGetFromService(srv, "sha512", out var r)) return r;
                }

                if (finfo is ISource src)
                {
                    var h = src.GetHash512Code();
                    if (!h.IsZero) return h;
                }
            }

            using (var s = finfo.CreateReadStream())
            {
                return Sha512FromStream(s);
            }
        }

        public static Hash512 Sha512FromStream(Func<System.IO.Stream> fileStream)
        {
            if (fileStream == null) return default;

            using (var s = fileStream.Invoke())
            {
                return Sha512FromStream(s);
            }
        }

        public static Hash512 Sha512FromStream(System.IO.Stream stream)
        {
            if (stream == null) return default;

            if (stream is System.IO.MemoryStream ms && ms.TryGetBuffer(out var buff))
            {
                return Sha512FromBytes(buff);
            }

            var bytes = _HashEngines.Sha512Engine.ComputeHash(stream);

            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash512(bytes);
        }

        public static Hash512 Sha256FromList(List<Byte> list)
        {
            if (list == null) return default;

            #if NETSTANDARD
            var bytes = list.ToArray();
            #else
            var bytes = CollectionsMarshal.AsSpan(list);
            #endif
            return Sha512FromBytes(bytes);
        }

        public static Hash512 Sha512FromBytes(ReadOnlySpan<Byte> bytes)
        {
            if (bytes.IsEmpty) return default;
            
            Span<Byte> hash = stackalloc byte[BYTESIZE];
            if (!_HashEngines.Sha512Engine.TryComputeHash(bytes, hash, out _)) throw new InvalidOperationException();            

            System.Diagnostics.Debug.Assert(hash.Length == BYTESIZE);
            return new Hash512(hash);
        }

        public Hash512(in Hash256 a, in Hash256 b)
        {
            this._A = a._A;
            this._B = a._B;
            this._C = a._C;
            this._D = a._D;
            this._E = b._A;
            this._F = b._B;
            this._G = b._C;
            this._H = b._D;
        }

        public Hash512(in Hash384 a, in Hash128 b)
        {
            this._A = a._A;
            this._B = a._B;
            this._C = a._C;
            this._D = a._D;
            this._E = a._E;
            this._F = a._F;
            this._G = b._A;
            this._H = b._B;
        }
    }
}
