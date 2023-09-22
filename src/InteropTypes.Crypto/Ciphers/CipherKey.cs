using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using BYTES = System.ArraySegment<byte>;

namespace InteropTypes.Crypto.Ciphers
{
    public abstract class CipherKey
    {
        #region lifecycle

        public static Byte[] GetRandomBytes(int count)
        {
            using (var rnd = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                var data = new byte[count];
                rnd.GetBytes(data);
                return data;
            }
        }

        public static CipherKey FromRandom() { return AESCipherKey.FromRandom(); }

        public static CipherKey FromPlain() { return new NoCipherKey(); }

        public static CipherKey FromPassword(string clearPassword, byte[] salt = null)
        {
            if (string.IsNullOrEmpty(clearPassword)) return new NoCipherKey();
            return AESCipherKey.FromPassword(clearPassword, salt);
        }

        public static CipherKey Read(Stream stream)
        {
            using(var br = new BinaryReader(stream, Encoding.UTF8, true))
            {
                return Read(br);
            }
        }

        public static CipherKey Read(BinaryReader reader)
        {
            var type = reader.ReadString();

            CipherKey ckey = null;

            switch(type)
            {
                case "NONE": ckey = new NoCipherKey(); break;
                case "AES": ckey = new AESCipherKey(); break;
                default: throw new ArgumentException($"unknonwn {type}",nameof(reader));
            }

            ckey.ReadFrom(reader);
            return ckey;
        }

        #endregion

        #region API

        public void Write(Stream stream)
        {
            using (var wb = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                WriteTo(wb);
            }
        }

        public void Write(BinaryWriter bw) { WriteTo(bw); }

        protected abstract void WriteTo(BinaryWriter writer);

        protected abstract void ReadFrom(BinaryReader reader);

        public abstract CipherIO CreateInstance();

        public BYTES DecodeBytesFrom(System.IO.BinaryReader reader, int byteCount)
        {
            var cipherBytes = reader.ReadBytes(byteCount);

            // var content = Convert.ToBase64String(cipherBytes);

            using (var instance = CreateInstance())
            {
                return instance.DecryptBytes(cipherBytes);
            }
        }

        public long EncodeBytesTo(System.IO.BinaryWriter writer, BYTES plainBytes)
        {
            using (var instance = CreateInstance())
            {
                var cipherBytes = instance.EncryptBytes(plainBytes);
                
                // var content = Convert.ToBase64String(cipherBytes);

                writer.Write(cipherBytes);

                return cipherBytes.Count;
            }
        }

        public BYTES EncodeBytes(BYTES plainBytes)
        {
            using(var m = new System.IO.MemoryStream())
            {
                using (var w = new System.IO.BinaryWriter(m, System.Text.Encoding.UTF8, true))
                {
                    EncodeBytesTo(w, plainBytes);
                }

                return m.TryGetBuffer(out var buffer)
                    ? buffer
                    : m.ToArray();
            }
        }

        #endregion
    }

    sealed class NoCipherKey : CipherKey
    {
        protected override void ReadFrom(BinaryReader reader)
        {
            
        }

        protected override void WriteTo(BinaryWriter writer)
        {
            writer.Write("NONE");
        }

        public override CipherIO CreateInstance()
        {
            return new NoCipher();
        }
    }

    sealed class AESCipherKey : CipherKey
    {
        #region lifecycle

        public static AESCipherKey FromRandom()
        {
            using(var aes = Aes.Create())
            {
                // https://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.GenerateKey();
                aes.GenerateIV();

                var key = new AESCipherKey();

                key._Key = aes.Key;
                key._IV = aes.IV;
                key._Mode = aes.Mode; 
                key._Padding = aes.Padding;

                return key;
            }
        }        

        public static AESCipherKey FromPassword(string clearPassword, byte[] salt = null)
        {
            var passwordBytes = System.Text.Encoding.UTF8.GetBytes(clearPassword);
            
            return _Create(passwordBytes, salt);
        }

        private static AESCipherKey _Create(byte[] passwordBytes, byte[] salt = null)
        {
            var (aesKey, aesIV) = salt == null
                ? GetPasswordSHA256(passwordBytes)
                : GetPasswordRfc2898(passwordBytes, salt);

            if (aesKey.Length != 32) throw new InvalidOperationException();
            if (aesIV.Length != 16) throw new InvalidOperationException();

            var aes = new AESCipherKey();

            aes._Key = aesKey;
            aes._IV = aesIV;
            aes._Mode = CipherMode.CBC; // https://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
            aes._Padding = PaddingMode.PKCS7;

            return aes;
        }

        // https://stackoverflow.com/questions/70913958/best-way-to-store-passwords

        private static (Byte[] key, Byte[] iv) GetPasswordSHA256(byte[] passwordBytes)
        {
            using var sha256 = SHA256.Create();
            using var md5 = MD5.Create();

            var key = sha256.ComputeHash(passwordBytes);
            var iv = md5.ComputeHash(passwordBytes);

            return (key, iv);
        }

        private static (Byte[] key, Byte[] iv) GetPasswordRfc2898(byte[] passwordBytes, byte[] salt)
        {
            if (salt.Length != 16) throw new ArgumentOutOfRangeException(nameof(salt));

            // https://stackoverflow.com/questions/67574726/generating-aes-iv-from-rfc2898derivebytes
            // https://crypto.stackexchange.com/questions/26537/derive-cipher-iv-and-cipher-key-using-pbkdf2-with-random-salt
            // https://csharp.hotexamples.com/es/examples/System.Security.Cryptography/Rfc2898DeriveBytes/-/php-rfc2898derivebytes-class-examples.html

            using (var rfc = new Rfc2898DeriveBytes(passwordBytes, salt, 641487, HashAlgorithmName.SHA256))
            {
                var key = rfc.GetBytes(32);
                rfc.GetBytes(6);
                var iv = rfc.GetBytes(16);
                return (key, iv);
            }
        }

        #endregion

        #region data

        private CipherMode _Mode;
        private PaddingMode _Padding;
        private Byte[] _Key;
        private Byte[] _IV;

        #endregion

        #region API

        protected override void WriteTo(BinaryWriter writer)
        {
            writer.Write("AES");
            writer.Write(_Mode.ToString());
            writer.Write(_Padding.ToString());
            writer.Write(_Key.Length);            
            writer.Write(_Key);
            writer.Write(_IV.Length);
            writer.Write(_IV);
        }

        protected override void ReadFrom(BinaryReader reader)
        {
            _Mode = Enum.Parse<CipherMode>(reader.ReadString(), true);
            _Padding = Enum.Parse<PaddingMode>(reader.ReadString(), true);
            _Key = reader.ReadBytes(reader.ReadInt32());
            _IV = reader.ReadBytes(reader.ReadInt32());
        }

        public override CipherIO CreateInstance()
        {
            return new AESCipher(_Key, _IV, _Mode, _Padding);
        }

        #endregion
    }
}
