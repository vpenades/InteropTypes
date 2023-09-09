using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using BYTES = System.ArraySegment<System.Byte>;

namespace InteropTypes.Crypto.Ciphers
{
    // https://github.com/iamvivekkumar/EncryptionDecryptionUsingSymmetricKey
    // https://security.stackexchange.com/questions/38828/how-can-i-securely-convert-a-string-password-to-a-key-used-in-aes
    // https://stackoverflow.com/questions/61159825/password-as-key-for-aes-encryption-decryption

    public abstract class CipherIO : IDisposable
    {
        public abstract void Dispose();
        public string EncryptStringToBase64(string plainText)
        {
            var bytes = Encoding.UTF8.GetBytes(plainText);
            bytes = EncryptBytes(new BYTES(bytes)).ToArray();
            return Convert.ToBase64String(bytes);
        }

        public string DecryptStringFromBase64(string cipherText)
        {
            var bytes = Convert.FromBase64String(cipherText);
            bytes = DecryptBytes(new BYTES(bytes)).ToArray();
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public abstract BYTES EncryptBytes(BYTES plainBytes);

        public abstract BYTES DecryptBytes(BYTES cipherBytes);
    }

    sealed class NoCipher : CipherIO
    {
        public override void Dispose() { }

        public override BYTES DecryptBytes(BYTES cipherBytes)
        {
            return cipherBytes;
        }        

        public override BYTES EncryptBytes(BYTES plainBytes)
        {
            return plainBytes;
        }
    }

    sealed class AESCipher : CipherIO, IDisposable
    {
        #region lifecycle
        public static AESCipher FromPassword(string password)
        {
            var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

            using var sha256 = SHA256.Create();
            using var md5 = MD5.Create();

            var aesKey = sha256.ComputeHash(passwordBytes);
            var aesIV = md5.ComputeHash(passwordBytes);

            // https://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption

            return new AESCipher(aesKey,aesIV, CipherMode.CBC, PaddingMode.PKCS7);
        }

        public AESCipher(byte[] key, byte[] iv, CipherMode cmode, PaddingMode pmode)
        {
            _Aes = Aes.Create();
            _Aes.Key = key;
            _Aes.IV = iv;
            _Aes.Mode = cmode;
            _Aes.Padding = pmode;
        }
        private AESCipher(Aes aes) { _Aes = aes; }
        public override void Dispose()
        {
            System.Threading.Interlocked.Exchange(ref _Aes, null)?.Dispose();
        }

        #endregion

        #region data

        private Aes _Aes;

        #endregion

        #region API        

        private ICryptoTransform _CreateEncryptor()
        {
            return _Aes.CreateEncryptor(_Aes.Key, _Aes.IV);
        }

        private ICryptoTransform _CreateDecryptor()
        {
            return _Aes.CreateDecryptor(_Aes.Key, _Aes.IV);
        }

        public override BYTES EncryptBytes(BYTES plainBytes)
        {
            using var encryptor = _CreateEncryptor();

            using (var outStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(outStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.WriteBytes(plainBytes);
                }

                return outStream.ToBytes();
            }
        }

        public override BYTES DecryptBytes(BYTES cipherBytes)
        {
            using var decryptor = _CreateDecryptor();

            using (var outStream = new MemoryStream())
            {
                using (var inStream = cipherBytes.ToStream())
                {
                    using (var cryptoStream = new CryptoStream(inStream, decryptor, CryptoStreamMode.Read))
                    {
                        cryptoStream.CopyTo(outStream);
                    }
                }

                return outStream.ToBytes();
            }
        }        

        #endregion
    }
}

