using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace InteropTypes.Crypto
{
    /// <summary>
    /// Represents the base class for symmetric algorythm factories
    /// </summary>
    internal abstract class SymmetricFactory
    {
        public static Byte[] GetRandomBytes(int count)
        {
            using (var rnd = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                var data = new byte[count];
                rnd.GetBytes(data);
                return data;
            }
        }

        public abstract SymmetricAlgorithm CreateAlgorythm();
    }

    sealed class AESFactory : SymmetricFactory
    {
        #region lifecycle

        public const int SALTSIZE = 16;

        public static AESFactory FromRandom()
        {
            using (var aes = Aes.Create())
            {
                // https://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.GenerateKey();
                aes.GenerateIV();

                var key = new AESFactory();

                key._Key = aes.Key;
                key._IV = aes.IV;
                key._Mode = aes.Mode;
                key._Padding = aes.Padding;

                return key;
            }
        }

        public static AESFactory FromPassword(string clearPassword, byte[] salt = null)
        {
            var passwordBytes = System.Text.Encoding.UTF8.GetBytes(clearPassword);

            return _Create(passwordBytes, salt);
        }

        private static AESFactory _Create(byte[] passwordBytes, byte[] salt = null)
        {
            var (aesKey, aesIV) = salt == null
                ? GetPasswordSHA256(passwordBytes)
                : GetPasswordRfc2898(passwordBytes, salt);

            if (aesKey.Length != 32) throw new InvalidOperationException();
            if (aesIV.Length != 16) throw new InvalidOperationException();

            var aes = new AESFactory();

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

        public override SymmetricAlgorithm CreateAlgorythm()
        {
            var aes = Aes.Create();
            aes.Key = _Key;
            aes.IV = _IV;
            aes.Mode = _Mode;
            aes.Padding = _Padding;
            return aes;
        }        

        #endregion        

    }    
}
