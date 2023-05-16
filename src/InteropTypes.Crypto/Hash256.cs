using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace InteropTypes.Crypto
{    
    partial struct Hash256
    {
        public static Hash256 Sha256FromFile(System.IO.FileInfo finfo)
        {
            using (var s = finfo.OpenRead())
            {
                return Sha256FromStream(s);
            }
        } 
        
        public static Hash256 Sha256FromFile(Microsoft.Extensions.FileProviders.IFileInfo finfo)
        {
            if (finfo is IServiceProvider srv)
            {
                if (TryGetFromService(srv, "sha256", out var r)) return r;
            }

            if (finfo is ISource src)
            {
                var h = src.GetHash256Code();
                if (!h.IsZero) return h;
            }

            using (var s = finfo.CreateReadStream())
            {
                return Sha256FromStream(s);
            }
        }

        public static Hash256 Sha256FromStream(Func<System.IO.Stream> fileStream)
        {
            if (fileStream == null) return default;

            using (var s = fileStream.Invoke())
            {
                return Sha256FromStream(s);
            }
        }

        public static Hash256 Sha256FromStream(System.IO.Stream stream)
        {
            if (stream == null) return default;

            if (stream is System.IO.MemoryStream ms && ms.TryGetBuffer(out var buff))
            {
                return Sha256FromBytes(buff);
            }

            var bytes = _HashEngines.Sha256Engine.ComputeHash(stream);

            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash256(bytes);
        }

        public static Hash256 Sha256FromList(List<Byte> list)
        {
            if (list == null) return default;

            #if NETSTANDARD
            var bytes = list.ToArray();
            #else
            var bytes = CollectionsMarshal.AsSpan(list);
            #endif
            return Sha256FromBytes(bytes);            
        }

        public static Hash256 Sha256FromBytes(ReadOnlySpan<Byte> bytes)
        {
            if (bytes.IsEmpty) return default;

            #if NETSTANDARD2_0
            var hash = _HashEngines.Sha256Engine.ComputeHash(bytes.ToArray());
            #else
            Span<Byte> hash = stackalloc byte[BYTESIZE];
            if (!_HashEngines.Sha256Engine.TryComputeHash(bytes, hash, out _)) throw new InvalidOperationException();
            #endif

            System.Diagnostics.Debug.Assert(hash.Length == BYTESIZE);
            return new Hash256(hash);
        }

        public Hash256(in Hash128 a, in Hash128 b)
        {
            this._A = a._A;
            this._B = a._B;
            this._C = b._A;
            this._D = b._B;

        }
    }
}