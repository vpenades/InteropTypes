using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace InteropTypes.IO.Reflection
{
    /// <summary>
    /// Represents an instance able to identify whether a file is of a given file format.
    /// </summary>    
    internal abstract partial class FileIdentification
    {
        #region static API        

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private static readonly Dictionary<FileIdentification, string> _Signatures = new Dictionary<FileIdentification, string>();

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private static int? _MinimumHeaderSize;

        /// <summary>
        /// Represents the minimum number of bytes to read from a file header to run against all registered signatures.
        /// </summary>
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

        internal static FileIdentification CreateFromAscii(string text)
        {
            var bytes = System.Text.Encoding.ASCII.GetBytes(text);
            return new _MaskedBytesHeader(bytes, null);
        }

        internal static FileIdentification CreateFromManySignatures(params string[] signatures)
        {
            var children = signatures.Select(CreateFromSignature);

            return new _CompositeHeader(children);
        }

        internal static FileIdentification CreateFromSignature(string signature)
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

            var rbytes = bytes.ToArray();
            var rmask = bytes.ToArray();

            if (rmask.All(x => x == 0xff)) return new _PlainBytesHeader(rbytes);
            else return new _MaskedBytesHeader(rbytes, rmask);
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

        private sealed class _CompositeHeader : FileIdentification, IEquatable<_CompositeHeader>
        {
            #region lifecycle
            public _CompositeHeader(IEnumerable<FileIdentification> chidren)
            {
                _Children = chidren.Distinct().OrderByDescending(item => item.MinimumHeaderSize).ToArray();
                MinimumHeaderSize = _Children.Max(x => x.MinimumHeaderSize);
            }

            #endregion

            #region data

            private readonly IReadOnlyList<FileIdentification> _Children;
            public override int GetHashCode()
            {
                int h = 0;
                foreach (var c in _Children) { h += c.GetHashCode(); h *= 224822519; }
                return (int)h;
            }

            public override bool Equals(object obj)
            {
                return obj is _CompositeHeader other && this.Equals(other);
            }

            public bool Equals(_CompositeHeader other)
            {
                return this._Children.SequenceEqual(other._Children);
            }

            #endregion

            #region API

            /// <summary>
            /// Gets the maximum number of bytes required to identify this header
            /// </summary>
            public override int MinimumHeaderSize { get; }

            /// <summary>
            /// checks if the passed bytes match this header
            /// </summary>
            /// <param name="header">the bytes of the file header. Must have a lenth of at least <see cref="MinimumHeaderSize"/></param>
            /// <returns>true if there's a match</returns>        
            public override bool IsMatch(ReadOnlySpan<Byte> header)
            {
                foreach(var c in _Children)
                {
                    if (c.IsMatch(header)) return true;
                }
                return false;
            }

            #endregion
        }

        private sealed class _PlainBytesHeader : FileIdentification, IEquatable<_PlainBytesHeader>
        {
            #region lifecycle
            public _PlainBytesHeader(byte[] bytes)
            {
                _Bytes = bytes;                
            }

            #endregion

            #region data

            private readonly Byte[] _Bytes;
            public override int GetHashCode()
            {
                uint h = 0;
                foreach (var b in _Bytes) { h += b; h *= 2246822519U; }
                return (int)h;
            }

            public override bool Equals(object obj)
            {
                return obj is _PlainBytesHeader other && this.Equals(other);
            }

            public bool Equals(_PlainBytesHeader other)
            {
                return this._Bytes.AsSpan().SequenceEqual(other._Bytes);
            }

            #endregion

            #region API

            /// <summary>
            /// Gets the maximum number of bytes required to identify this header
            /// </summary>
            public override int MinimumHeaderSize => _Bytes.Length;

            /// <summary>
            /// checks if the passed bytes match this header
            /// </summary>
            /// <param name="header">the bytes of the file header. Must have a lenth of at least <see cref="MinimumHeaderSize"/></param>
            /// <returns>true if there's a match</returns>        
            public override bool IsMatch(ReadOnlySpan<Byte> header)
            {
                if (header.Length < _Bytes.Length) return false;

                return _Bytes.AsSpan().SequenceEqual(header.Slice(0,header.Length));
            }

            #endregion
        }

        private sealed class _MaskedBytesHeader : FileIdentification, IEquatable<_MaskedBytesHeader>
        {
            #region lifecycle
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
