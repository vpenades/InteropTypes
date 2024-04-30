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
using System.Security.Cryptography;

namespace InteropTypes.Crypto
{    
    partial struct Hash256
    {
        /// <summary>
        /// Creates a secret key from a user password and a salt.
        /// </summary>
        public static Hash256 Rfc2898KeyFromPasswordAndSalt(string clearPassword, Hash128 publicSalt, int iterations)
        {
            if (string.IsNullOrEmpty(clearPassword)) throw new ArgumentNullException(nameof(clearPassword));

            // https://stackoverflow.com/questions/70913958/best-way-to-store-passwords

            using (var rfc = new Rfc2898DeriveBytes(clearPassword, publicSalt.ToBytes(), iterations, HashAlgorithmName.SHA256))
            {
                return new Hash256(rfc.GetBytes(BYTESIZE));
            }
        }

        public static Hash256 Sha256FromFile(System.IO.FileInfo finfo)
        {
            using (var s = finfo.OpenRead())
            {
                return Sha256FromStream(s);
            }
        } 
        
        public static Hash256 Sha256FromFile(Microsoft.Extensions.FileProviders.IFileInfo finfo, bool useCachedValues = true)
        {
            if (useCachedValues)
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

            var position = stream.CanSeek ? stream.Position : -1;

            var bytes = _HashEngines.Sha256Engine.ComputeHash(stream);

            // restore original position after reading
            if (position >= 0) stream.TrySetPosition(position);

            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash256(bytes);
        }

        public static Hash256 Sha256FromList<TBytes>(TBytes collection)
            where TBytes: IEnumerable<Byte>
        {
            switch(collection)
            {
                case null: return default;
                case Byte[] array: return Sha256FromBytes(array);
                case ArraySegment<Byte> segment: return Sha256FromBytes(segment);
                #if NET6_0_OR_GREATER
                case List<Byte> list:                    
                    var bytes = CollectionsMarshal.AsSpan(list);
                    return Sha256FromBytes(bytes);
                #endif
                    default: return Sha256FromBytes(collection.ToArray());
            }
        }

        public static Hash256 Sha256FromBytes(ReadOnlySpan<Byte> bytes)
        {
            if (bytes.IsEmpty) return default;
            
            Span<Byte> hash = stackalloc byte[BYTESIZE];
            if (!_HashEngines.Sha256Engine.TryComputeHash(bytes, hash, out _)) throw new InvalidOperationException();            

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