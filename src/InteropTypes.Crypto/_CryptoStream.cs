using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace InteropTypes.Crypto
{
    class _CryptoStream : CryptoStream
    {
        #region lifecycle
        
        public _CryptoStream(Stream stream, SymmetricAlgorithm algo, ICryptoTransform transform, CryptoStreamMode mode, bool leaveOpen)
            : base(stream, transform, mode, leaveOpen)
        {
            _Transform = transform;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                System.Threading.Interlocked.Exchange(ref _Transform, null)?.Dispose();
            }

            base.Dispose(disposing);            
        }

        #endregion

        #region data

        #pragma warning disable CA2213 // Disposable fields should be disposed
        private ICryptoTransform _Transform;
        #pragma warning restore CA2213 // Disposable fields should be disposed

        #endregion
    }
}
