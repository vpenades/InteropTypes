using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace InteropTypes.Crypto
{
    class _CryptoStream : CryptoStream
    {
        #region lifecycle

        public static _CryptoStream CreateFrom(Stream plainStream, CryptoStreamMode streamMode, SymmetricAlgorithm algo)
        {
            var xform = streamMode == CryptoStreamMode.Read
                ? algo.CreateDecryptor()
                : algo.CreateEncryptor();

            return new _CryptoStream(plainStream, null, xform, streamMode, true);
        }

        public static _CryptoStream CreateFromAES(Stream plainStream, CryptoStreamMode streamMode, string password, bool useSalt)
        {
            if (!useSalt)
            {
                var factory = AESFactory.FromPassword(password);

                return CreateFromFactory(plainStream, streamMode, factory);
            }
            else
            {
                byte[] salt;

                switch (streamMode)
                {
                    case CryptoStreamMode.Read:
                        salt = new Byte[AESFactory.SALTSIZE];
                        if (TryReadBytesToEnd(plainStream, salt) != salt.Length) throw new EndOfStreamException();
                        break;
                    case CryptoStreamMode.Write:
                        salt = SymmetricFactory.GetRandomBytes(AESFactory.SALTSIZE);
                        plainStream.Write(salt, 0, salt.Length);
                        break;
                    default: throw new NotSupportedException(streamMode.ToString());
                }

                var factory = AESFactory.FromPassword(password, salt);

                return CreateFromFactory(plainStream, streamMode, factory);
            }
        }

        public static _CryptoStream CreateFromFactory(Stream plainStream, CryptoStreamMode streamMode, SymmetricFactory factory)
        {
            var aes = factory.CreateAlgorythm();

            var xform = streamMode == CryptoStreamMode.Read
                ? aes.CreateDecryptor()
                : aes.CreateEncryptor();

            return new _CryptoStream(plainStream, aes, xform, streamMode, true);
        }

        protected _CryptoStream(Stream stream, SymmetricAlgorithm algo, ICryptoTransform transform, CryptoStreamMode mode, bool leaveOpen)
            : base(stream, transform, mode, leaveOpen)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                System.Threading.Interlocked.Exchange(ref _Transform, null)?.Dispose();
                System.Threading.Interlocked.Exchange(ref _Algorythm, null)?.Dispose();
            }
        }

        #endregion

        #region data

        private ICryptoTransform _Transform;
        private SymmetricAlgorithm _Algorythm;

        #endregion

        #region API

        private static int TryReadBytesToEnd(Stream stream, ArraySegment<Byte> bytes)
        {


            var bbb = bytes;

            while (bbb.Count > 0)
            {
                var l = stream.Read(bbb.Array, bbb.Offset, bbb.Count);
                if (l <= 0) return bytes.Count - bbb.Count;

                bbb = bbb.Slice(l);
            }

            return bytes.Count;
        }

        #endregion
    }
}
