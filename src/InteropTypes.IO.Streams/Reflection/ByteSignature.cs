using System;
using System.Collections.Generic;

namespace InteropTypes.IO.Reflection
{
    /// <summary>
    /// Represents a file header's byte signature.
    /// </summary>
    internal readonly struct ByteSignature : IEquatable<ByteSignature>
    {
        #region static API

        static ByteSignature() { _Init(_Signatures); }

        private static readonly Dictionary<ByteSignature, string> _Signatures = new Dictionary<ByteSignature, string>();

        private static void _Init(Dictionary<ByteSignature, string> signatures)
        {
            // https://en.wikipedia.org/wiki/List_of_file_signatures

            signatures[new ByteSignature("425A68")] = "bz2";
            signatures[new ByteSignature("474946383761")] = "gif"; // GIF87a
            signatures[new ByteSignature("474946383961")] = "gif"; // GIF89a

            signatures[new ByteSignature("49492A00")] = "tif"; // little endian
            signatures[new ByteSignature("4D4D002A")] = "tif"; // big endian

            signatures[new ByteSignature("762F3101")] = "exr";

            signatures[new ByteSignature("FFD8FFDB")] = "jpg";
            signatures[new ByteSignature("FFD8FFE0")] = "jpg";
            signatures[new ByteSignature("FFD8FFEE")] = "jpg";
            signatures[new ByteSignature("FFD8FFE000104A4649460001")] = "jpg";

            signatures[new ByteSignature("FFD8FFE1????457869660000")] = "jpg";

            signatures[new ByteSignature("4D5A")] = "exe"; // exe,dll and many other

            signatures[new ByteSignature("504B0304")] = "zip";
            signatures[new ByteSignature("504B0506")] = "zip"; // empty
            signatures[new ByteSignature("504B0708")] = "zip"; // spanned

            signatures[new ByteSignature("526172211A0700")] = "rar"; // ver 1.5
            signatures[new ByteSignature("526172211A070100")] = "rar"; // ver 5.0

            signatures[new ByteSignature("89504E470D0A1A0A")] = "png";

            signatures[new ByteSignature("FFFE")] = "txt";
            signatures[new ByteSignature("FEFF")] = "txt";
            signatures[new ByteSignature("EFBBBF")] = "txt";
            signatures[new ByteSignature("FFFE0000")] = "txt";
            signatures[new ByteSignature("0000FEFF")] = "txt";

            signatures[new ByteSignature("2B2F7638")] = "txt";
            signatures[new ByteSignature("2B2F7639")] = "txt";
            signatures[new ByteSignature("2B2F762B")] = "txt";
            signatures[new ByteSignature("2B2F762F")] = "txt";

            signatures[new ByteSignature("0EFEFF")] = "txt";
            signatures[new ByteSignature("DD736673")] = "txt";

            signatures[new ByteSignature("255044462D")] = "pdf";

            signatures[new ByteSignature("38425053")] = "psd";

            signatures[new ByteSignature("52494646")] = "wav";

            signatures[new ByteSignature("FFF2")] = "mp3";
            signatures[new ByteSignature("FFF3")] = "mp3";
            signatures[new ByteSignature("FFFB")] = "mp3";

            signatures[new ByteSignature("424D")] = "bmp";

            signatures[new ByteSignature("4344303031")] = "iso";

            signatures[new ByteSignature("1F8B")] = "gz"; // also tar.gz
            signatures[new ByteSignature("FD377A585A00")] = "xz"; // also tar.xz
            signatures[new ByteSignature("7573746172003030")] = "tar";
            signatures[new ByteSignature("7573746172202000")] = "tar";

            signatures[new ByteSignature("377ABCAF271C")] = "7z";

            signatures[new ByteSignature("04224D18")] = "lz4";

            signatures[new ByteSignature("4D534346")] = "cab";

            signatures[new ByteSignature("233F52414449414E43450A")] = "hdr";
        }

        #endregion

        #region lifecycle

        public ByteSignature(string signature)
        {
            if (signature == null) throw new ArgumentNullException(nameof(signature));
            if ((uint)signature.Length % 2 != 0) throw new FormatException("must be multiple of 2");

            var bytes = new List<Byte>();
            var mask = new List<Byte>();

            while(signature.Length > 0)
            {
                var c = signature.Substring(0, 2);

                if (byte.TryParse(c, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out var b))
                {
                    bytes.Add(b);
                    mask.Add(0xff);
                }
                else
                {
                    // ?? = any value
                    // HHLLhhll = uint

                    bytes.Add(0);
                    mask.Add(0);
                }

                signature = signature.Substring(2);
            }

            _Bytes = bytes.ToArray();
            _Mask = bytes.ToArray();
        }

        #endregion

        #region data

        private readonly Byte[] _Bytes;
        private readonly Byte[] _Mask;

        public override int GetHashCode()
        {
            uint h = 0;
            foreach (var b in _Bytes) { h += b; h *= 2246822519U; }
            return (int)h;
        }

        public override bool Equals(object obj)
        {
            return obj is ByteSignature other && this.Equals(other);
        }

        public bool Equals(ByteSignature other)
        {
            return this._Bytes.AsSpan().SequenceEqual(other._Bytes) && this._Mask.AsSpan().SequenceEqual(other._Mask);
        }

        #endregion        
    }
}
