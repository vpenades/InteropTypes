using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace InteropTypes.IO.Reflection
{
    /// <summary>
    /// Represents an instance able to identify whether a file is of a given file format.
    /// </summary>    
    internal abstract class FileIdentification
    {
        #region static API

        static FileIdentification() { _Init(_Signatures); }

        private static void _Init(Dictionary<FileIdentification, string> signatures)
        {
            // https://en.wikipedia.org/wiki/List_of_file_signatures

            signatures[CreateFromSignature("425A68")] = "bz2";
            signatures[CreateFromSignature("474946383761")] = "gif"; // GIF87a
            signatures[CreateFromSignature("474946383961")] = "gif"; // GIF89a

            signatures[CreateFromSignature("49492A00")] = "tif"; // little endian
            signatures[CreateFromSignature("4D4D002A")] = "tif"; // big endian

            signatures[CreateFromSignature("762F3101")] = "exr";

            signatures[CreateFromSignature("FFD8FFDB")] = "jpg";
            signatures[CreateFromSignature("FFD8FFE0")] = "jpg";
            signatures[CreateFromSignature("FFD8FFEE")] = "jpg";
            signatures[CreateFromSignature("FFD8FFE000104A4649460001")] = "jpg";

            signatures[CreateFromSignature("FFD8FFE1????457869660000")] = "jpg";

            // https://github.com/link-u/avif-sample-images
            signatures[CreateFromSignature("000000206674797061766966")] = "avif"; // 'ftypavif' this image format is used to disguise JPGs from stock sites.

            signatures[CreateFromSignature("4D5A")] = "exe"; // exe,dll and many other

            signatures[CreateFromSignature("504B0304")] = "zip";
            signatures[CreateFromSignature("504B0506")] = "zip"; // empty
            signatures[CreateFromSignature("504B0708")] = "zip"; // spanned

            signatures[CreateFromSignature("526172211A0700")] = "rar"; // ver 1.5
            signatures[CreateFromSignature("526172211A070100")] = "rar"; // ver 5.0

            signatures[CreateFromSignature("89504E470D0A1A0A")] = "png";

            signatures[CreateFromSignature("FFFE")] = "txt";
            signatures[CreateFromSignature("FEFF")] = "txt";
            signatures[CreateFromSignature("EFBBBF")] = "txt";
            signatures[CreateFromSignature("FFFE0000")] = "txt";
            signatures[CreateFromSignature("0000FEFF")] = "txt";

            signatures[CreateFromSignature("2B2F7638")] = "txt";
            signatures[CreateFromSignature("2B2F7639")] = "txt";
            signatures[CreateFromSignature("2B2F762B")] = "txt";
            signatures[CreateFromSignature("2B2F762F")] = "txt";

            signatures[CreateFromSignature("0EFEFF")] = "txt";
            signatures[CreateFromSignature("DD736673")] = "txt";

            signatures[CreateFromSignature("255044462D")] = "pdf";

            signatures[CreateFromSignature("38425053")] = "psd";

            signatures[CreateFromSignature("52494646")] = "wav";

            signatures[CreateFromSignature("FFF2")] = "mp3";
            signatures[CreateFromSignature("FFF3")] = "mp3";
            signatures[CreateFromSignature("FFFB")] = "mp3";

            signatures[CreateFromSignature("424D")] = "bmp";

            signatures[CreateFromSignature("4344303031")] = "iso";

            signatures[CreateFromSignature("1F8B")] = "gz"; // also tar.gz
            signatures[CreateFromSignature("FD377A585A00")] = "xz"; // also tar.xz
            signatures[CreateFromSignature("7573746172003030")] = "tar";
            signatures[CreateFromSignature("7573746172202000")] = "tar";

            signatures[CreateFromSignature("377ABCAF271C")] = "7z";

            signatures[CreateFromSignature("04224D18")] = "lz4";

            signatures[CreateFromSignature("4D534346")] = "cab";

            signatures[CreateFromSignature("233F52414449414E43450A")] = "hdr";

            signatures[CreateFromText("<!doctype html")] = "html";
            signatures[CreateFromText("<!DOCTYPE html")] = "html";
        }

        private static readonly Dictionary<FileIdentification, string> _Signatures = new Dictionary<FileIdentification, string>();

        private static int? _MinimumHeaderSize;

        public static int SharedMinimumHeaderSize
        {
            get
            {
                if (!_MinimumHeaderSize.HasValue)
                {
                    _MinimumHeaderSize = _Signatures.Keys.Max(item => item.MinimumHeaderSize);
                }
                return _MinimumHeaderSize.Value;                
            }
        }

        public static bool TryIdentify(ReadOnlySpan<Byte> header, out string extension)
        {
            // 1.- Sort all signatures by < see cref = "ByteSignature.MinimumHeaderSize" /> in descending order,
            // 2.- Read as many bytes as required by the maxinmum header size.
            // 3.- call <see cref="ByteSignature.IsMatch(ReadOnlySpan{byte})"/> in descending order.

            foreach (var kvp in _Signatures.OrderByDescending(item => item.Key.MinimumHeaderSize))
            {                
                if (kvp.Key.MinimumHeaderSize <= 0) continue; // skip signatures that don't support per byte matching.

                if (kvp.Key.IsMatch(header)) { extension = kvp.Value; return true; }
            }

            extension = null;
            return false;
        }        

        #endregion

        #region lifecycle

        internal static FileIdentification CreateFromText(string text)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            return new _MaskedBytesHeader(bytes, null);
        }

        internal static FileIdentification CreateFromSignature(string signature)
        {
            return new _MaskedBytesHeader(signature);
        }

        #endregion

        #region API

        public abstract int MinimumHeaderSize { get; }

        /// <summary>
        /// checks if the passed bytes match this header
        /// </summary>
        /// <param name="header">the bytes of the file header. Must have a lenth of at least <see cref="MinimumHeaderSize"/></param>
        /// <returns>true if there's a match</returns>        
        public abstract bool IsMatch(ReadOnlySpan<Byte> header);

        // public bool IsMatch(System.IO.Stream stream); // for complex files that need a deeper analysis

        #endregion

        #region nested types        

        private sealed class _MaskedBytesHeader : FileIdentification, IEquatable<_MaskedBytesHeader>
        {
            #region lifecycle            

            public _MaskedBytesHeader(string signature)
            {
                if (signature == null) throw new ArgumentNullException(nameof(signature));
                if ((uint)signature.Length % 2 != 0) throw new FormatException("must be multiple of 2");

                var bytes = new List<Byte>();
                var mask = new List<Byte>();

                while (signature.Length > 0)
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

            public _MaskedBytesHeader(byte[] bytes, byte[] mask)
            {
                _Bytes = bytes;
                _Mask = mask ?? Array.Empty<Byte>();
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
                return obj is _MaskedBytesHeader other && this.Equals(other);
            }

            public bool Equals(_MaskedBytesHeader other)
            {
                return this._Bytes.AsSpan().SequenceEqual(other._Bytes) && this._Mask.AsSpan().SequenceEqual(other._Mask);
            }

            #endregion

            #region API

            /// <summary>
            /// Gets the maximum number of bytes required to identify this header
            /// </summary>
            public override int MinimumHeaderSize => Math.Max(_Bytes.Length, _Mask.Length);

            /// <summary>
            /// checks if the passed bytes match this header
            /// </summary>
            /// <param name="header">the bytes of the file header. Must have a lenth of at least <see cref="MinimumHeaderSize"/></param>
            /// <returns>true if there's a match</returns>        
            public override bool IsMatch(ReadOnlySpan<Byte> header)
            {
                if (header.Length < MinimumHeaderSize) return false;

                for(int i=0; i < _Bytes.Length; i++)
                {
                    if ((header[i] & _Mask[i]) != _Bytes[i]) return false;
                }

                return true;
            }

            #endregion
        }

        #endregion
    }
}
