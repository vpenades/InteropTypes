using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using BYTES = System.ArraySegment<byte>;

namespace InteropTypes.Crypto
{
    /// <summary>
    /// Creates a context that can be used to encrypt and decrypt data.
    /// </summary>
    public abstract class CryptoSerializer : IDisposable
    {
        #region lifecycle

        public static CryptoSerializer CreateAES(string clearPassword, Hash128 publicSalt, int iterations)
        {
            return CreateAES(clearPassword, publicSalt, iterations, CipherMode.CBC, PaddingMode.ISO10126);
        }

        public static CryptoSerializer CreateAES(string clearPassword, Hash128 publicSalt, int iterations, CipherMode cipher, PaddingMode padding)
        {
            var secretKey = Hash256.Rfc2898KeyFromPasswordAndSalt(clearPassword, publicSalt, iterations);

            return AESCryptoSerializer.Create(secretKey, cipher, padding);
        }

        public static CryptoSerializer CreateAES(Hash256 secretKey, CipherMode cipher, PaddingMode padding)
        {
            return AESCryptoSerializer.Create(secretKey, cipher, padding);
        }

        protected CryptoSerializer() { }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CryptoSerializer()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing) { }

        #endregion        

        #region API - Encryptor        

        public BYTES Encrypt(BYTES plainSrc)
        {
            using (var m = new System.IO.MemoryStream(plainSrc.Array, plainSrc.Offset, plainSrc.Count, false))
            {
                return Encrypt(m);
            }
        }

        public BYTES Encrypt(System.IO.Stream plainSrc)
        {
            using (var m = new System.IO.MemoryStream())
            {
                Encrypt(plainSrc, m);

                return m.TryGetBuffer(out var buff)
                    ? buff
                    : m.ToArray();
            }
        }

        public BYTES Encrypt(Action<System.IO.Stream> plainSrc)
        {
            using (var m = new System.IO.MemoryStream())
            {
                Encrypt(plainSrc, m);

                return m.TryGetBuffer(out var buff)
                    ? buff
                    : m.ToArray();
            }
        }

        public void Encrypt(BYTES plainSrc, System.IO.Stream cipherDst)
        {
            using (var m = new System.IO.MemoryStream(plainSrc.Array, plainSrc.Offset, plainSrc.Count, false))
            {
                Encrypt(m, cipherDst);
            }
        }

        public void Encrypt(System.IO.Stream plainSrc, System.IO.Stream cipherDst)
        {
            Encrypt(w => plainSrc.CopyTo(w), cipherDst);
        }

        public void Encrypt(Action<System.IO.Stream> plainSrc, System.IO.Stream cipherDst)
        {
            using (var xform = CreateEncryptor(cipherDst))
            {
                using (var xs = new CryptoStream(cipherDst, xform, CryptoStreamMode.Write, true))
                {
                    plainSrc(xs);
                }
            }
        }        

        protected abstract ICryptoTransform CreateEncryptor(System.IO.Stream cipherDst);

        #endregion

        #region API - Decryptor        

        public BYTES Decrypt(BYTES cipherSrc)
        {
            using (var m = new System.IO.MemoryStream(cipherSrc.Array, cipherSrc.Offset, cipherSrc.Count, false))
            {
                return Decrypt(m);
            }
        }

        public BYTES Decrypt(System.IO.Stream cipherSrc)
        {
            using (var m = new System.IO.MemoryStream())
            {
                Decrypt(cipherSrc, m);

                return m.TryGetBuffer(out var buff)
                    ? buff
                    : m.ToArray();
            }
        }

        public void Decrypt(BYTES cipherSrc, System.IO.Stream plainDst)
        {
            using (var m = new System.IO.MemoryStream(cipherSrc.Array, cipherSrc.Offset, cipherSrc.Count, false))
            {
                Decrypt(m, plainDst);
            }
        }

        public void Decrypt(System.IO.Stream cipherSrc, System.IO.Stream plainDst)
        {
            Decrypt(cipherSrc, s => s.CopyTo(plainDst));
        }

        public void Decrypt(System.IO.Stream cipherSrc, Action<System.IO.Stream> plainDst)
        {
            using (var xform = CreateDecryptor(cipherSrc))
            {
                using (var xs = new CryptoStream(cipherSrc, xform, CryptoStreamMode.Read, true))
                {
                    plainDst(xs);
                }
            }
        }

        protected abstract ICryptoTransform CreateDecryptor(System.IO.Stream cipherSrc);

        #endregion
    }

    internal class AESCryptoSerializer : CryptoSerializer
    {
        #region lifecycle       

        public static AESCryptoSerializer Create(Hash256 secretKey, CipherMode cipher, PaddingMode padding)
        {
            #pragma warning disable CA5358 // Review cipher mode usage with cryptography experts
            System.Diagnostics.Debug.Assert(cipher != CipherMode.ECB, "this is an insecure mode, avoid using it");
            #pragma warning restore CA5358 // Review cipher mode usage with cryptography experts

            // https://stackoverflow.com/questions/67574726/generating-aes-iv-from-rfc2898derivebytes
            // https://crypto.stackexchange.com/questions/26537/derive-cipher-iv-and-cipher-key-using-pbkdf2-with-random-salt
            // https://csharp.hotexamples.com/es/examples/System.Security.Cryptography/Rfc2898DeriveBytes/-/php-rfc2898derivebytes-class-examples.html            

            var aes = Aes.Create();
            aes.Mode = cipher;
            aes.Padding = padding;
            aes.Key = secretKey.ToBytes();

            return new AESCryptoSerializer(aes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {                
                System.Threading.Interlocked.Exchange(ref _Algorythm, null)?.Dispose();
            }

            base.Dispose(disposing);
        }

        private AESCryptoSerializer(SymmetricAlgorithm algorythm)
        {            
            _Algorythm = algorythm;            
        }

        #endregion

        #region data

        private SymmetricAlgorithm _Algorythm;        

        #endregion

        #region API - Encryptor             

        protected override ICryptoTransform CreateEncryptor(System.IO.Stream cipherDst)
        {
            var xform = CreateEncryptor(out var iv);
            iv.WriteTo(cipherDst);
            return xform;
        }

        private ICryptoTransform CreateEncryptor(out Hash128 iv)
        {
            _Algorythm.GenerateIV();
            iv = new Hash128(_Algorythm.IV);
            return _Algorythm.CreateEncryptor();
        }

        #endregion

        #region API - Decryptor

        protected override ICryptoTransform CreateDecryptor(System.IO.Stream cipherSrc)
        {
            var iv = Hash128.ReadFrom(cipherSrc);
            return CreateDecryptor(iv);
        }

        private ICryptoTransform CreateDecryptor(Hash128 iv)
        {
            _Algorythm.IV = iv.ToBytes();
            return _Algorythm.CreateDecryptor();
        }

        #endregion
    }
}
