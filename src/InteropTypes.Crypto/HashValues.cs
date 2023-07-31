// GENERATED CODE
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InteropTypes.Crypto
{



    /// <summary>
    /// Represents a hash value of 96 bits
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{ToHexString()}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack=1)]
    [System.Text.Json.Serialization.JsonConverter(typeof(Hash96.JsonConverter))]
    public readonly partial struct Hash96 : IEquatable<Hash96> , IHashValue
    {
        #region constants

        public const int BYTESIZE = 12;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a random hash.
        /// </summary>
        /// <returns>A new hash</returns>
        public static Hash96 FromRandom()
        {            
            Span<Byte> bytes = stackalloc Byte[BYTESIZE];            

            _HashEngines.Randomizer.GetBytes(bytes);

            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash96(bytes);
        }

        public static bool TryGetFromService(IServiceProvider service, string jsonProperty, out Hash96 result)
        {
            // direct
            if (service.GetService(typeof(Hash96)) is Hash96 h) { result= h; return true;}

            // indirect
            if (service.GetService(typeof(ISource)) is ISource hs) { result= hs.GetHash96Code(); return true;}

            // from json document
            if (!string.IsNullOrWhiteSpace(jsonProperty))
            {
                if (service.GetService(typeof(System.Text.Json.JsonDocument)) is System.Text.Json.JsonDocument doc)
                {
                    if (TryGetFromJson(doc.RootElement, jsonProperty, out result)) return true;
                }
            }

            result = default;
            return false;            
        }

        public static bool TryGetFromJson(System.Text.Json.JsonElement element, string jsonProperty, out Hash96 result)
        {
            var val = element.FindFirstOrDefault(jsonProperty);
            return TryParse(val, out result);
        }

        /// <summary>
        /// Detects if the string is hex or base64 and parses it.
        /// </summary>
        public static bool TryParse(string value, out Hash96 result)
        {
            if (string.IsNullOrWhiteSpace(value)) { result = default; return false; }

            try // try parse hexadecimal
            {                
                if (value.Length == BYTESIZE * 2 && !value.Contains('='))
                {
                    result = ParseHex(value);
                    return true;
                }
            }
            catch { }     

            try // try parse base64
            {
                result = ParseBase64(value);
                return true;
            }
            catch { }   
            
            result = default;
            return false;
        }

        public static Hash96 ParseHex(string value)
        {
            if (string.IsNullOrEmpty(value)) return default;

            var bytes = value.ParseHexBytes();
            
            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash96(bytes);
        }

        public static Hash96 ParseBase64(string value)
        {
            if (string.IsNullOrEmpty(value)) return default;

            var bytes = value.ParseBase64Bytes();

            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash96(bytes);
        }        

        public Hash96(ReadOnlySpan<Byte> bytes)
        {
            if (bytes.Length != BYTESIZE) throw new ArgumentException($"Byte len is {bytes.Length}, expected {BYTESIZE}", nameof(bytes));
            this = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte,Hash96>(bytes)[0];
        }

        public Hash96(ulong a, uint b)
        {
            _A = a;
            _B = b;
        }

        #endregion

        #region data

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _A;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly uint _B;
        
        /// <Inheritdoc/>
        public override int GetHashCode()
        {            
            return HashCode.Combine(_A, _B);         
        }

        /// <Inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is HashValue href) obj = href._Hash;
            if (obj is Hash96 other) return Equals(other);
            return false;
        }

        /// <Inheritdoc/>
        public bool Equals(IHashValue obj)
        {
            if (obj is HashValue href) obj = href._Hash;
            if (obj is Hash96 other) return Equals(other);
            return false;
        }

        /// <Inheritdoc/>
        public bool Equals(Hash96 other)
        {
            if (this._A != other._A) return false;
            if (this._B != other._B) return false;
            return true;
        }

        public static bool operator ==(in Hash96 a, in Hash96 b) { return a.Equals(b); }

        public static bool operator !=(in Hash96 a, in Hash96 b) { return !a.Equals(b); }

        #endregion

        #region operator

        public Byte this[int index]
        {
            get
            {                
                var bytes = AsReadOnlyBytes();
                return bytes[index];
            }        
        }

        public static Hash96 operator ^(in Hash96 left, in Hash96 right)
        {
            return new Hash96(left._A ^ right._A, left._B ^ right._B);
        }

        public static Hash96 operator &(in Hash96 left, in Hash96 right)
        {
            return new Hash96(left._A & right._A, left._B & right._B);
        }

        public static Hash96 operator |(in Hash96 left, in Hash96 right)
        {
            return new Hash96(left._A | right._A, left._B | right._B);
        }

        public static Hash96 operator ~(in Hash96 value)
        {
            return new Hash96(~value._A, ~value._B);
        }

        #endregion

        #region API        

        public int ByteCount => BYTESIZE;

        public bool IsZero
        {
            get
            {
                if (this._A != 0) return false;
                if (this._B != 0) return false;
                return true;            
            }
        }

        private ReadOnlySpan<Byte> AsReadOnlyBytes()
        {            
            ref var hRef = ref System.Runtime.CompilerServices.Unsafe.AsRef(this);
            var span = System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref hRef, 1);
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Hash96, Byte>(span);
        }

        public Byte[] ToBytes()
        {
            var bytes = new Byte[BYTESIZE];
            CopyTo(bytes);
            return bytes;
        }

        public string ToBase64String()
        {            
            var bytes = AsReadOnlyBytes();         
            return bytes.ToBase64String();
        }

        public string ToHexString()
        {            
            var bytes = AsReadOnlyBytes();         
            return bytes.ToHexString();
        }

        public void CopyTo(Span<Byte> target)
        {
            System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, Hash96>(target)[0] = this;
        }

        /// <remarks>
        /// Reads a hash value from the stream <paramref name="reader"/>
        /// </remarks>
        public static Hash96 ReadFrom(System.IO.Stream reader)
        {            
            Span<Byte> bytes = stackalloc byte[BYTESIZE];

            bytes.ReadBytesFrom(reader);

            return new Hash96(bytes);
        }

        /// <remarks>
        /// Writes this hash value to the stream <paramref name="writer"/>
        /// </remarks>
        public void WriteTo(System.IO.Stream writer)
        {
            var bytes = AsReadOnlyBytes();
            bytes.WriteBytesTo(writer);
        }

        public static Hash96 ReadFrom(System.IO.BinaryReader reader)
        {            
            Span<Byte> bytes = stackalloc byte[BYTESIZE];
            bytes.ReadBytesFrom(reader);         

            return new Hash96(bytes);
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {            
            var bytes = AsReadOnlyBytes();         

            bytes.WriteBytesTo(writer);
        }

        /// <summary>
        /// Gets the number of enabled bits.
        /// </summary>
        /// <returns>A count of enabled bits.</returns>
        public int GetEnabledBitsCount()
        {            
            var bytes = AsReadOnlyBytes();         

            int count = 0;            

            foreach (var e in bytes)
            {
                for (int i = 0; i < 8; ++i)
                {
                    if (((e >> i) & 1) == 1) ++count;
                }
            }

            return count;
        }

        /// <summary>
        /// Checks whether the two values are similar, given how many bits match.
        /// </summary>
        public static bool AreSimilar(in Hash96 left, in Hash96 right, int minBits)
        {
            System.Diagnostics.Debug.Assert(minBits >= 0);
            return (left ^ right).GetEnabledBitsCount() <= minBits;
        }

        public static IEqualityComparer<Hash96> GetComparer(int minBits, int hashIndex = -1)
        {
            if (minBits < 0) throw new ArgumentOutOfRangeException(nameof(minBits));

            return new _SimilarityComparer(minBits, hashIndex);
        }

        /// <remarks>
        /// <para>
        /// Gets the list of objects pairs that are considered equal, given their hashes discrepancy bits is lower than minBits.
        /// </para>
        /// <para>
        /// Strategies for faster collision check in dictionaries:        
        /// we cannot create a full hash from all the bytes because it would
        /// prevent a collision when there's a few bits that are different.
        /// </para>
        /// <para>
        /// But we can run multiple searches using PARTIAL hashes, that is,
        /// run the search using the 1st byte as dictionary hash,
        /// then run the search using the 2nd byte as dictionary hash and so on.        
        /// </para>
        /// </remarks>
        public static IEnumerable<(T, T)> FindCollisions<T>(IEnumerable<(T Item, Hash96 Hash)> items, int minBits = 1)
        {
            System.Diagnostics.Debug.Assert(minBits >= 0);

            var result = new HashSet<(T, T)>();

            for (int i = 0; i < BYTESIZE; ++i) // run the search for every byte.
            {
                // this dictionary stores hashes that are considered "equal" as long as:
                // - the given byte index is the same (used to calculate the partial hash)
                // - the number of divergent bits is equal or below minBits
                var dict = new Dictionary<Hash96, T>(new _SimilarityComparer(minBits, i));

                foreach (var (item, hash) in items)
                {
                    if (dict.TryGetValue(hash, out var collision))
                    {
                        result.Add((item, collision));
                    }
                    else
                    {
                        dict[hash] = item;
                    }
                }
            }

            return result;
        }

        #endregion

        #region nested types

        public interface ISource
        {
            Hash96 GetHash96Code();
        }

        /// <summary>
        /// Helper class used in Json serialization.
        /// </summary>
        /// <remarks>
        /// <see cref="System.Text.Json.Serialization.JsonConverterAttribute"/>
        /// </remarks>
        public class JsonConverter : JsonConverter<Hash96>
        {
            public override Hash96 Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                var bytes = reader.GetBytesFromBase64();
                return new Hash96(bytes);
            }

            public override void Write(
                Utf8JsonWriter writer,
                Hash96 value,
                JsonSerializerOptions options)
            {
                var bytes = value.ToBytes();
                writer.WriteBase64StringValue(bytes);
            }
        }

        /// <summary>
        /// Compares whether two values are similar, if the number of mismatching bits is lower than the given value.
        /// </summary>
        sealed class _SimilarityComparer : IEqualityComparer<Hash96>
        {
            /// <summary>
            /// Creates a new comparer
            /// </summary>
            /// <param name="mbc">the minimum number of bits required to consider two instances as equal.</param>
            /// <param name="hashIndex">The index of the byte to use as a hash, or -1 to use default hash.</param>
            /// <exception cref="ArgumentOutOfRangeException"></exception>
            public _SimilarityComparer(int mbc, int hashIndex = -1)
            {
                if (hashIndex >= BYTESIZE) throw new ArgumentOutOfRangeException(nameof(hashIndex));

                _MinBitCount = mbc;
                _HashIndex = hashIndex;
            }

            private readonly int _MinBitCount;
            private readonly int _HashIndex;

            public bool Equals(Hash96 x, Hash96 y)
            {
                return AreSimilar(x, y, _MinBitCount);
            }

            public int GetHashCode(Hash96 obj)
            {
                return _HashIndex < 0
                ? obj.GetHashCode()
                : obj[_HashIndex];
            }
        }

        #endregion
    }




    /// <summary>
    /// Represents a hash value of 128 bits
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{ToHexString()}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack=1)]
    [System.Text.Json.Serialization.JsonConverter(typeof(Hash128.JsonConverter))]
    public readonly partial struct Hash128 : IEquatable<Hash128> , IHashValue
    {
        #region constants

        public const int BYTESIZE = 16;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a random hash.
        /// </summary>
        /// <returns>A new hash</returns>
        public static Hash128 FromRandom()
        {            
            Span<Byte> bytes = stackalloc Byte[BYTESIZE];            

            _HashEngines.Randomizer.GetBytes(bytes);

            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash128(bytes);
        }

        public static bool TryGetFromService(IServiceProvider service, string jsonProperty, out Hash128 result)
        {
            // direct
            if (service.GetService(typeof(Hash128)) is Hash128 h) { result= h; return true;}

            // indirect
            if (service.GetService(typeof(ISource)) is ISource hs) { result= hs.GetHash128Code(); return true;}

            // from json document
            if (!string.IsNullOrWhiteSpace(jsonProperty))
            {
                if (service.GetService(typeof(System.Text.Json.JsonDocument)) is System.Text.Json.JsonDocument doc)
                {
                    if (TryGetFromJson(doc.RootElement, jsonProperty, out result)) return true;
                }
            }

            result = default;
            return false;            
        }

        public static bool TryGetFromJson(System.Text.Json.JsonElement element, string jsonProperty, out Hash128 result)
        {
            var val = element.FindFirstOrDefault(jsonProperty);
            return TryParse(val, out result);
        }

        /// <summary>
        /// Detects if the string is hex or base64 and parses it.
        /// </summary>
        public static bool TryParse(string value, out Hash128 result)
        {
            if (string.IsNullOrWhiteSpace(value)) { result = default; return false; }

            try // try parse hexadecimal
            {                
                if (value.Length == BYTESIZE * 2 && !value.Contains('='))
                {
                    result = ParseHex(value);
                    return true;
                }
            }
            catch { }     

            try // try parse base64
            {
                result = ParseBase64(value);
                return true;
            }
            catch { }   
            
            result = default;
            return false;
        }

        public static Hash128 ParseHex(string value)
        {
            if (string.IsNullOrEmpty(value)) return default;

            var bytes = value.ParseHexBytes();
            
            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash128(bytes);
        }

        public static Hash128 ParseBase64(string value)
        {
            if (string.IsNullOrEmpty(value)) return default;

            var bytes = value.ParseBase64Bytes();

            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash128(bytes);
        }        

        public Hash128(ReadOnlySpan<Byte> bytes)
        {
            if (bytes.Length != BYTESIZE) throw new ArgumentException($"Byte len is {bytes.Length}, expected {BYTESIZE}", nameof(bytes));
            this = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte,Hash128>(bytes)[0];
        }

        public Hash128(ulong a, ulong b)
        {
            _A = a;
            _B = b;
        }

        #endregion

        #region data

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _A;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _B;
        
        /// <Inheritdoc/>
        public override int GetHashCode()
        {            
            return HashCode.Combine(_A, _B);         
        }

        /// <Inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is HashValue href) obj = href._Hash;
            if (obj is Hash128 other) return Equals(other);
            return false;
        }

        /// <Inheritdoc/>
        public bool Equals(IHashValue obj)
        {
            if (obj is HashValue href) obj = href._Hash;
            if (obj is Hash128 other) return Equals(other);
            return false;
        }

        /// <Inheritdoc/>
        public bool Equals(Hash128 other)
        {
            if (this._A != other._A) return false;
            if (this._B != other._B) return false;
            return true;
        }

        public static bool operator ==(in Hash128 a, in Hash128 b) { return a.Equals(b); }

        public static bool operator !=(in Hash128 a, in Hash128 b) { return !a.Equals(b); }

        #endregion

        #region operator

        public Byte this[int index]
        {
            get
            {                
                var bytes = AsReadOnlyBytes();
                return bytes[index];
            }        
        }

        public static Hash128 operator ^(in Hash128 left, in Hash128 right)
        {
            return new Hash128(left._A ^ right._A, left._B ^ right._B);
        }

        public static Hash128 operator &(in Hash128 left, in Hash128 right)
        {
            return new Hash128(left._A & right._A, left._B & right._B);
        }

        public static Hash128 operator |(in Hash128 left, in Hash128 right)
        {
            return new Hash128(left._A | right._A, left._B | right._B);
        }

        public static Hash128 operator ~(in Hash128 value)
        {
            return new Hash128(~value._A, ~value._B);
        }

        #endregion

        #region API        

        public int ByteCount => BYTESIZE;

        public bool IsZero
        {
            get
            {
                if (this._A != 0) return false;
                if (this._B != 0) return false;
                return true;            
            }
        }

        private ReadOnlySpan<Byte> AsReadOnlyBytes()
        {            
            ref var hRef = ref System.Runtime.CompilerServices.Unsafe.AsRef(this);
            var span = System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref hRef, 1);
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Hash128, Byte>(span);
        }

        public Byte[] ToBytes()
        {
            var bytes = new Byte[BYTESIZE];
            CopyTo(bytes);
            return bytes;
        }

        public string ToBase64String()
        {            
            var bytes = AsReadOnlyBytes();         
            return bytes.ToBase64String();
        }

        public string ToHexString()
        {            
            var bytes = AsReadOnlyBytes();         
            return bytes.ToHexString();
        }

        public void CopyTo(Span<Byte> target)
        {
            System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, Hash128>(target)[0] = this;
        }

        /// <remarks>
        /// Reads a hash value from the stream <paramref name="reader"/>
        /// </remarks>
        public static Hash128 ReadFrom(System.IO.Stream reader)
        {            
            Span<Byte> bytes = stackalloc byte[BYTESIZE];

            bytes.ReadBytesFrom(reader);

            return new Hash128(bytes);
        }

        /// <remarks>
        /// Writes this hash value to the stream <paramref name="writer"/>
        /// </remarks>
        public void WriteTo(System.IO.Stream writer)
        {
            var bytes = AsReadOnlyBytes();
            bytes.WriteBytesTo(writer);
        }

        public static Hash128 ReadFrom(System.IO.BinaryReader reader)
        {            
            Span<Byte> bytes = stackalloc byte[BYTESIZE];
            bytes.ReadBytesFrom(reader);         

            return new Hash128(bytes);
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {            
            var bytes = AsReadOnlyBytes();         

            bytes.WriteBytesTo(writer);
        }

        /// <summary>
        /// Gets the number of enabled bits.
        /// </summary>
        /// <returns>A count of enabled bits.</returns>
        public int GetEnabledBitsCount()
        {            
            var bytes = AsReadOnlyBytes();         

            int count = 0;            

            foreach (var e in bytes)
            {
                for (int i = 0; i < 8; ++i)
                {
                    if (((e >> i) & 1) == 1) ++count;
                }
            }

            return count;
        }

        /// <summary>
        /// Checks whether the two values are similar, given how many bits match.
        /// </summary>
        public static bool AreSimilar(in Hash128 left, in Hash128 right, int minBits)
        {
            System.Diagnostics.Debug.Assert(minBits >= 0);
            return (left ^ right).GetEnabledBitsCount() <= minBits;
        }

        public static IEqualityComparer<Hash128> GetComparer(int minBits, int hashIndex = -1)
        {
            if (minBits < 0) throw new ArgumentOutOfRangeException(nameof(minBits));

            return new _SimilarityComparer(minBits, hashIndex);
        }

        /// <remarks>
        /// <para>
        /// Gets the list of objects pairs that are considered equal, given their hashes discrepancy bits is lower than minBits.
        /// </para>
        /// <para>
        /// Strategies for faster collision check in dictionaries:        
        /// we cannot create a full hash from all the bytes because it would
        /// prevent a collision when there's a few bits that are different.
        /// </para>
        /// <para>
        /// But we can run multiple searches using PARTIAL hashes, that is,
        /// run the search using the 1st byte as dictionary hash,
        /// then run the search using the 2nd byte as dictionary hash and so on.        
        /// </para>
        /// </remarks>
        public static IEnumerable<(T, T)> FindCollisions<T>(IEnumerable<(T Item, Hash128 Hash)> items, int minBits = 1)
        {
            System.Diagnostics.Debug.Assert(minBits >= 0);

            var result = new HashSet<(T, T)>();

            for (int i = 0; i < BYTESIZE; ++i) // run the search for every byte.
            {
                // this dictionary stores hashes that are considered "equal" as long as:
                // - the given byte index is the same (used to calculate the partial hash)
                // - the number of divergent bits is equal or below minBits
                var dict = new Dictionary<Hash128, T>(new _SimilarityComparer(minBits, i));

                foreach (var (item, hash) in items)
                {
                    if (dict.TryGetValue(hash, out var collision))
                    {
                        result.Add((item, collision));
                    }
                    else
                    {
                        dict[hash] = item;
                    }
                }
            }

            return result;
        }

        #endregion

        #region nested types

        public interface ISource
        {
            Hash128 GetHash128Code();
        }

        /// <summary>
        /// Helper class used in Json serialization.
        /// </summary>
        /// <remarks>
        /// <see cref="System.Text.Json.Serialization.JsonConverterAttribute"/>
        /// </remarks>
        public class JsonConverter : JsonConverter<Hash128>
        {
            public override Hash128 Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                var bytes = reader.GetBytesFromBase64();
                return new Hash128(bytes);
            }

            public override void Write(
                Utf8JsonWriter writer,
                Hash128 value,
                JsonSerializerOptions options)
            {
                var bytes = value.ToBytes();
                writer.WriteBase64StringValue(bytes);
            }
        }

        /// <summary>
        /// Compares whether two values are similar, if the number of mismatching bits is lower than the given value.
        /// </summary>
        sealed class _SimilarityComparer : IEqualityComparer<Hash128>
        {
            /// <summary>
            /// Creates a new comparer
            /// </summary>
            /// <param name="mbc">the minimum number of bits required to consider two instances as equal.</param>
            /// <param name="hashIndex">The index of the byte to use as a hash, or -1 to use default hash.</param>
            /// <exception cref="ArgumentOutOfRangeException"></exception>
            public _SimilarityComparer(int mbc, int hashIndex = -1)
            {
                if (hashIndex >= BYTESIZE) throw new ArgumentOutOfRangeException(nameof(hashIndex));

                _MinBitCount = mbc;
                _HashIndex = hashIndex;
            }

            private readonly int _MinBitCount;
            private readonly int _HashIndex;

            public bool Equals(Hash128 x, Hash128 y)
            {
                return AreSimilar(x, y, _MinBitCount);
            }

            public int GetHashCode(Hash128 obj)
            {
                return _HashIndex < 0
                ? obj.GetHashCode()
                : obj[_HashIndex];
            }
        }

        #endregion
    }




    /// <summary>
    /// Represents a hash value of 224 bits
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{ToHexString()}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack=1)]
    [System.Text.Json.Serialization.JsonConverter(typeof(Hash224.JsonConverter))]
    public readonly partial struct Hash224 : IEquatable<Hash224> , IHashValue
    {
        #region constants

        public const int BYTESIZE = 28;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a random hash.
        /// </summary>
        /// <returns>A new hash</returns>
        public static Hash224 FromRandom()
        {            
            Span<Byte> bytes = stackalloc Byte[BYTESIZE];            

            _HashEngines.Randomizer.GetBytes(bytes);

            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash224(bytes);
        }

        public static bool TryGetFromService(IServiceProvider service, string jsonProperty, out Hash224 result)
        {
            // direct
            if (service.GetService(typeof(Hash224)) is Hash224 h) { result= h; return true;}

            // indirect
            if (service.GetService(typeof(ISource)) is ISource hs) { result= hs.GetHash224Code(); return true;}

            // from json document
            if (!string.IsNullOrWhiteSpace(jsonProperty))
            {
                if (service.GetService(typeof(System.Text.Json.JsonDocument)) is System.Text.Json.JsonDocument doc)
                {
                    if (TryGetFromJson(doc.RootElement, jsonProperty, out result)) return true;
                }
            }

            result = default;
            return false;            
        }

        public static bool TryGetFromJson(System.Text.Json.JsonElement element, string jsonProperty, out Hash224 result)
        {
            var val = element.FindFirstOrDefault(jsonProperty);
            return TryParse(val, out result);
        }

        /// <summary>
        /// Detects if the string is hex or base64 and parses it.
        /// </summary>
        public static bool TryParse(string value, out Hash224 result)
        {
            if (string.IsNullOrWhiteSpace(value)) { result = default; return false; }

            try // try parse hexadecimal
            {                
                if (value.Length == BYTESIZE * 2 && !value.Contains('='))
                {
                    result = ParseHex(value);
                    return true;
                }
            }
            catch { }     

            try // try parse base64
            {
                result = ParseBase64(value);
                return true;
            }
            catch { }   
            
            result = default;
            return false;
        }

        public static Hash224 ParseHex(string value)
        {
            if (string.IsNullOrEmpty(value)) return default;

            var bytes = value.ParseHexBytes();
            
            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash224(bytes);
        }

        public static Hash224 ParseBase64(string value)
        {
            if (string.IsNullOrEmpty(value)) return default;

            var bytes = value.ParseBase64Bytes();

            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash224(bytes);
        }        

        public Hash224(ReadOnlySpan<Byte> bytes)
        {
            if (bytes.Length != BYTESIZE) throw new ArgumentException($"Byte len is {bytes.Length}, expected {BYTESIZE}", nameof(bytes));
            this = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte,Hash224>(bytes)[0];
        }

        public Hash224(ulong a, ulong b, ulong c, uint d)
        {
            _A = a;
            _B = b;
            _C = c;
            _D = d;
        }

        #endregion

        #region data

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _A;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _B;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _C;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly uint _D;
        
        /// <Inheritdoc/>
        public override int GetHashCode()
        {            
            return HashCode.Combine(_A, _B, _C, _D);         
        }

        /// <Inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is HashValue href) obj = href._Hash;
            if (obj is Hash224 other) return Equals(other);
            return false;
        }

        /// <Inheritdoc/>
        public bool Equals(IHashValue obj)
        {
            if (obj is HashValue href) obj = href._Hash;
            if (obj is Hash224 other) return Equals(other);
            return false;
        }

        /// <Inheritdoc/>
        public bool Equals(Hash224 other)
        {
            if (this._A != other._A) return false;
            if (this._B != other._B) return false;
            if (this._C != other._C) return false;
            if (this._D != other._D) return false;
            return true;
        }

        public static bool operator ==(in Hash224 a, in Hash224 b) { return a.Equals(b); }

        public static bool operator !=(in Hash224 a, in Hash224 b) { return !a.Equals(b); }

        #endregion

        #region operator

        public Byte this[int index]
        {
            get
            {                
                var bytes = AsReadOnlyBytes();
                return bytes[index];
            }        
        }

        public static Hash224 operator ^(in Hash224 left, in Hash224 right)
        {
            return new Hash224(left._A ^ right._A, left._B ^ right._B, left._C ^ right._C, left._D ^ right._D);
        }

        public static Hash224 operator &(in Hash224 left, in Hash224 right)
        {
            return new Hash224(left._A & right._A, left._B & right._B, left._C & right._C, left._D & right._D);
        }

        public static Hash224 operator |(in Hash224 left, in Hash224 right)
        {
            return new Hash224(left._A | right._A, left._B | right._B, left._C | right._C, left._D | right._D);
        }

        public static Hash224 operator ~(in Hash224 value)
        {
            return new Hash224(~value._A, ~value._B, ~value._C, ~value._D);
        }

        #endregion

        #region API        

        public int ByteCount => BYTESIZE;

        public bool IsZero
        {
            get
            {
                if (this._A != 0) return false;
                if (this._B != 0) return false;
                if (this._C != 0) return false;
                if (this._D != 0) return false;
                return true;            
            }
        }

        private ReadOnlySpan<Byte> AsReadOnlyBytes()
        {            
            ref var hRef = ref System.Runtime.CompilerServices.Unsafe.AsRef(this);
            var span = System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref hRef, 1);
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Hash224, Byte>(span);
        }

        public Byte[] ToBytes()
        {
            var bytes = new Byte[BYTESIZE];
            CopyTo(bytes);
            return bytes;
        }

        public string ToBase64String()
        {            
            var bytes = AsReadOnlyBytes();         
            return bytes.ToBase64String();
        }

        public string ToHexString()
        {            
            var bytes = AsReadOnlyBytes();         
            return bytes.ToHexString();
        }

        public void CopyTo(Span<Byte> target)
        {
            System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, Hash224>(target)[0] = this;
        }

        /// <remarks>
        /// Reads a hash value from the stream <paramref name="reader"/>
        /// </remarks>
        public static Hash224 ReadFrom(System.IO.Stream reader)
        {            
            Span<Byte> bytes = stackalloc byte[BYTESIZE];

            bytes.ReadBytesFrom(reader);

            return new Hash224(bytes);
        }

        /// <remarks>
        /// Writes this hash value to the stream <paramref name="writer"/>
        /// </remarks>
        public void WriteTo(System.IO.Stream writer)
        {
            var bytes = AsReadOnlyBytes();
            bytes.WriteBytesTo(writer);
        }

        public static Hash224 ReadFrom(System.IO.BinaryReader reader)
        {            
            Span<Byte> bytes = stackalloc byte[BYTESIZE];
            bytes.ReadBytesFrom(reader);         

            return new Hash224(bytes);
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {            
            var bytes = AsReadOnlyBytes();         

            bytes.WriteBytesTo(writer);
        }

        /// <summary>
        /// Gets the number of enabled bits.
        /// </summary>
        /// <returns>A count of enabled bits.</returns>
        public int GetEnabledBitsCount()
        {            
            var bytes = AsReadOnlyBytes();         

            int count = 0;            

            foreach (var e in bytes)
            {
                for (int i = 0; i < 8; ++i)
                {
                    if (((e >> i) & 1) == 1) ++count;
                }
            }

            return count;
        }

        /// <summary>
        /// Checks whether the two values are similar, given how many bits match.
        /// </summary>
        public static bool AreSimilar(in Hash224 left, in Hash224 right, int minBits)
        {
            System.Diagnostics.Debug.Assert(minBits >= 0);
            return (left ^ right).GetEnabledBitsCount() <= minBits;
        }

        public static IEqualityComparer<Hash224> GetComparer(int minBits, int hashIndex = -1)
        {
            if (minBits < 0) throw new ArgumentOutOfRangeException(nameof(minBits));

            return new _SimilarityComparer(minBits, hashIndex);
        }

        /// <remarks>
        /// <para>
        /// Gets the list of objects pairs that are considered equal, given their hashes discrepancy bits is lower than minBits.
        /// </para>
        /// <para>
        /// Strategies for faster collision check in dictionaries:        
        /// we cannot create a full hash from all the bytes because it would
        /// prevent a collision when there's a few bits that are different.
        /// </para>
        /// <para>
        /// But we can run multiple searches using PARTIAL hashes, that is,
        /// run the search using the 1st byte as dictionary hash,
        /// then run the search using the 2nd byte as dictionary hash and so on.        
        /// </para>
        /// </remarks>
        public static IEnumerable<(T, T)> FindCollisions<T>(IEnumerable<(T Item, Hash224 Hash)> items, int minBits = 1)
        {
            System.Diagnostics.Debug.Assert(minBits >= 0);

            var result = new HashSet<(T, T)>();

            for (int i = 0; i < BYTESIZE; ++i) // run the search for every byte.
            {
                // this dictionary stores hashes that are considered "equal" as long as:
                // - the given byte index is the same (used to calculate the partial hash)
                // - the number of divergent bits is equal or below minBits
                var dict = new Dictionary<Hash224, T>(new _SimilarityComparer(minBits, i));

                foreach (var (item, hash) in items)
                {
                    if (dict.TryGetValue(hash, out var collision))
                    {
                        result.Add((item, collision));
                    }
                    else
                    {
                        dict[hash] = item;
                    }
                }
            }

            return result;
        }

        #endregion

        #region nested types

        public interface ISource
        {
            Hash224 GetHash224Code();
        }

        /// <summary>
        /// Helper class used in Json serialization.
        /// </summary>
        /// <remarks>
        /// <see cref="System.Text.Json.Serialization.JsonConverterAttribute"/>
        /// </remarks>
        public class JsonConverter : JsonConverter<Hash224>
        {
            public override Hash224 Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                var bytes = reader.GetBytesFromBase64();
                return new Hash224(bytes);
            }

            public override void Write(
                Utf8JsonWriter writer,
                Hash224 value,
                JsonSerializerOptions options)
            {
                var bytes = value.ToBytes();
                writer.WriteBase64StringValue(bytes);
            }
        }

        /// <summary>
        /// Compares whether two values are similar, if the number of mismatching bits is lower than the given value.
        /// </summary>
        sealed class _SimilarityComparer : IEqualityComparer<Hash224>
        {
            /// <summary>
            /// Creates a new comparer
            /// </summary>
            /// <param name="mbc">the minimum number of bits required to consider two instances as equal.</param>
            /// <param name="hashIndex">The index of the byte to use as a hash, or -1 to use default hash.</param>
            /// <exception cref="ArgumentOutOfRangeException"></exception>
            public _SimilarityComparer(int mbc, int hashIndex = -1)
            {
                if (hashIndex >= BYTESIZE) throw new ArgumentOutOfRangeException(nameof(hashIndex));

                _MinBitCount = mbc;
                _HashIndex = hashIndex;
            }

            private readonly int _MinBitCount;
            private readonly int _HashIndex;

            public bool Equals(Hash224 x, Hash224 y)
            {
                return AreSimilar(x, y, _MinBitCount);
            }

            public int GetHashCode(Hash224 obj)
            {
                return _HashIndex < 0
                ? obj.GetHashCode()
                : obj[_HashIndex];
            }
        }

        #endregion
    }




    /// <summary>
    /// Represents a hash value of 256 bits
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{ToHexString()}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack=1)]
    [System.Text.Json.Serialization.JsonConverter(typeof(Hash256.JsonConverter))]
    public readonly partial struct Hash256 : IEquatable<Hash256> , IHashValue
    {
        #region constants

        public const int BYTESIZE = 32;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a random hash.
        /// </summary>
        /// <returns>A new hash</returns>
        public static Hash256 FromRandom()
        {            
            Span<Byte> bytes = stackalloc Byte[BYTESIZE];            

            _HashEngines.Randomizer.GetBytes(bytes);

            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash256(bytes);
        }

        public static bool TryGetFromService(IServiceProvider service, string jsonProperty, out Hash256 result)
        {
            // direct
            if (service.GetService(typeof(Hash256)) is Hash256 h) { result= h; return true;}

            // indirect
            if (service.GetService(typeof(ISource)) is ISource hs) { result= hs.GetHash256Code(); return true;}

            // from json document
            if (!string.IsNullOrWhiteSpace(jsonProperty))
            {
                if (service.GetService(typeof(System.Text.Json.JsonDocument)) is System.Text.Json.JsonDocument doc)
                {
                    if (TryGetFromJson(doc.RootElement, jsonProperty, out result)) return true;
                }
            }

            result = default;
            return false;            
        }

        public static bool TryGetFromJson(System.Text.Json.JsonElement element, string jsonProperty, out Hash256 result)
        {
            var val = element.FindFirstOrDefault(jsonProperty);
            return TryParse(val, out result);
        }

        /// <summary>
        /// Detects if the string is hex or base64 and parses it.
        /// </summary>
        public static bool TryParse(string value, out Hash256 result)
        {
            if (string.IsNullOrWhiteSpace(value)) { result = default; return false; }

            try // try parse hexadecimal
            {                
                if (value.Length == BYTESIZE * 2 && !value.Contains('='))
                {
                    result = ParseHex(value);
                    return true;
                }
            }
            catch { }     

            try // try parse base64
            {
                result = ParseBase64(value);
                return true;
            }
            catch { }   
            
            result = default;
            return false;
        }

        public static Hash256 ParseHex(string value)
        {
            if (string.IsNullOrEmpty(value)) return default;

            var bytes = value.ParseHexBytes();
            
            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash256(bytes);
        }

        public static Hash256 ParseBase64(string value)
        {
            if (string.IsNullOrEmpty(value)) return default;

            var bytes = value.ParseBase64Bytes();

            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash256(bytes);
        }        

        public Hash256(ReadOnlySpan<Byte> bytes)
        {
            if (bytes.Length != BYTESIZE) throw new ArgumentException($"Byte len is {bytes.Length}, expected {BYTESIZE}", nameof(bytes));
            this = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte,Hash256>(bytes)[0];
        }

        public Hash256(ulong a, ulong b, ulong c, ulong d)
        {
            _A = a;
            _B = b;
            _C = c;
            _D = d;
        }

        #endregion

        #region data

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _A;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _B;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _C;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _D;
        
        /// <Inheritdoc/>
        public override int GetHashCode()
        {            
            return HashCode.Combine(_A, _B, _C, _D);         
        }

        /// <Inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is HashValue href) obj = href._Hash;
            if (obj is Hash256 other) return Equals(other);
            return false;
        }

        /// <Inheritdoc/>
        public bool Equals(IHashValue obj)
        {
            if (obj is HashValue href) obj = href._Hash;
            if (obj is Hash256 other) return Equals(other);
            return false;
        }

        /// <Inheritdoc/>
        public bool Equals(Hash256 other)
        {
            if (this._A != other._A) return false;
            if (this._B != other._B) return false;
            if (this._C != other._C) return false;
            if (this._D != other._D) return false;
            return true;
        }

        public static bool operator ==(in Hash256 a, in Hash256 b) { return a.Equals(b); }

        public static bool operator !=(in Hash256 a, in Hash256 b) { return !a.Equals(b); }

        #endregion

        #region operator

        public Byte this[int index]
        {
            get
            {                
                var bytes = AsReadOnlyBytes();
                return bytes[index];
            }        
        }

        public static Hash256 operator ^(in Hash256 left, in Hash256 right)
        {
            return new Hash256(left._A ^ right._A, left._B ^ right._B, left._C ^ right._C, left._D ^ right._D);
        }

        public static Hash256 operator &(in Hash256 left, in Hash256 right)
        {
            return new Hash256(left._A & right._A, left._B & right._B, left._C & right._C, left._D & right._D);
        }

        public static Hash256 operator |(in Hash256 left, in Hash256 right)
        {
            return new Hash256(left._A | right._A, left._B | right._B, left._C | right._C, left._D | right._D);
        }

        public static Hash256 operator ~(in Hash256 value)
        {
            return new Hash256(~value._A, ~value._B, ~value._C, ~value._D);
        }

        #endregion

        #region API        

        public int ByteCount => BYTESIZE;

        public bool IsZero
        {
            get
            {
                if (this._A != 0) return false;
                if (this._B != 0) return false;
                if (this._C != 0) return false;
                if (this._D != 0) return false;
                return true;            
            }
        }

        private ReadOnlySpan<Byte> AsReadOnlyBytes()
        {            
            ref var hRef = ref System.Runtime.CompilerServices.Unsafe.AsRef(this);
            var span = System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref hRef, 1);
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Hash256, Byte>(span);
        }

        public Byte[] ToBytes()
        {
            var bytes = new Byte[BYTESIZE];
            CopyTo(bytes);
            return bytes;
        }

        public string ToBase64String()
        {            
            var bytes = AsReadOnlyBytes();         
            return bytes.ToBase64String();
        }

        public string ToHexString()
        {            
            var bytes = AsReadOnlyBytes();         
            return bytes.ToHexString();
        }

        public void CopyTo(Span<Byte> target)
        {
            System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, Hash256>(target)[0] = this;
        }

        /// <remarks>
        /// Reads a hash value from the stream <paramref name="reader"/>
        /// </remarks>
        public static Hash256 ReadFrom(System.IO.Stream reader)
        {            
            Span<Byte> bytes = stackalloc byte[BYTESIZE];

            bytes.ReadBytesFrom(reader);

            return new Hash256(bytes);
        }

        /// <remarks>
        /// Writes this hash value to the stream <paramref name="writer"/>
        /// </remarks>
        public void WriteTo(System.IO.Stream writer)
        {
            var bytes = AsReadOnlyBytes();
            bytes.WriteBytesTo(writer);
        }

        public static Hash256 ReadFrom(System.IO.BinaryReader reader)
        {            
            Span<Byte> bytes = stackalloc byte[BYTESIZE];
            bytes.ReadBytesFrom(reader);         

            return new Hash256(bytes);
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {            
            var bytes = AsReadOnlyBytes();         

            bytes.WriteBytesTo(writer);
        }

        /// <summary>
        /// Gets the number of enabled bits.
        /// </summary>
        /// <returns>A count of enabled bits.</returns>
        public int GetEnabledBitsCount()
        {            
            var bytes = AsReadOnlyBytes();         

            int count = 0;            

            foreach (var e in bytes)
            {
                for (int i = 0; i < 8; ++i)
                {
                    if (((e >> i) & 1) == 1) ++count;
                }
            }

            return count;
        }

        /// <summary>
        /// Checks whether the two values are similar, given how many bits match.
        /// </summary>
        public static bool AreSimilar(in Hash256 left, in Hash256 right, int minBits)
        {
            System.Diagnostics.Debug.Assert(minBits >= 0);
            return (left ^ right).GetEnabledBitsCount() <= minBits;
        }

        public static IEqualityComparer<Hash256> GetComparer(int minBits, int hashIndex = -1)
        {
            if (minBits < 0) throw new ArgumentOutOfRangeException(nameof(minBits));

            return new _SimilarityComparer(minBits, hashIndex);
        }

        /// <remarks>
        /// <para>
        /// Gets the list of objects pairs that are considered equal, given their hashes discrepancy bits is lower than minBits.
        /// </para>
        /// <para>
        /// Strategies for faster collision check in dictionaries:        
        /// we cannot create a full hash from all the bytes because it would
        /// prevent a collision when there's a few bits that are different.
        /// </para>
        /// <para>
        /// But we can run multiple searches using PARTIAL hashes, that is,
        /// run the search using the 1st byte as dictionary hash,
        /// then run the search using the 2nd byte as dictionary hash and so on.        
        /// </para>
        /// </remarks>
        public static IEnumerable<(T, T)> FindCollisions<T>(IEnumerable<(T Item, Hash256 Hash)> items, int minBits = 1)
        {
            System.Diagnostics.Debug.Assert(minBits >= 0);

            var result = new HashSet<(T, T)>();

            for (int i = 0; i < BYTESIZE; ++i) // run the search for every byte.
            {
                // this dictionary stores hashes that are considered "equal" as long as:
                // - the given byte index is the same (used to calculate the partial hash)
                // - the number of divergent bits is equal or below minBits
                var dict = new Dictionary<Hash256, T>(new _SimilarityComparer(minBits, i));

                foreach (var (item, hash) in items)
                {
                    if (dict.TryGetValue(hash, out var collision))
                    {
                        result.Add((item, collision));
                    }
                    else
                    {
                        dict[hash] = item;
                    }
                }
            }

            return result;
        }

        #endregion

        #region nested types

        public interface ISource
        {
            Hash256 GetHash256Code();
        }

        /// <summary>
        /// Helper class used in Json serialization.
        /// </summary>
        /// <remarks>
        /// <see cref="System.Text.Json.Serialization.JsonConverterAttribute"/>
        /// </remarks>
        public class JsonConverter : JsonConverter<Hash256>
        {
            public override Hash256 Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                var bytes = reader.GetBytesFromBase64();
                return new Hash256(bytes);
            }

            public override void Write(
                Utf8JsonWriter writer,
                Hash256 value,
                JsonSerializerOptions options)
            {
                var bytes = value.ToBytes();
                writer.WriteBase64StringValue(bytes);
            }
        }

        /// <summary>
        /// Compares whether two values are similar, if the number of mismatching bits is lower than the given value.
        /// </summary>
        sealed class _SimilarityComparer : IEqualityComparer<Hash256>
        {
            /// <summary>
            /// Creates a new comparer
            /// </summary>
            /// <param name="mbc">the minimum number of bits required to consider two instances as equal.</param>
            /// <param name="hashIndex">The index of the byte to use as a hash, or -1 to use default hash.</param>
            /// <exception cref="ArgumentOutOfRangeException"></exception>
            public _SimilarityComparer(int mbc, int hashIndex = -1)
            {
                if (hashIndex >= BYTESIZE) throw new ArgumentOutOfRangeException(nameof(hashIndex));

                _MinBitCount = mbc;
                _HashIndex = hashIndex;
            }

            private readonly int _MinBitCount;
            private readonly int _HashIndex;

            public bool Equals(Hash256 x, Hash256 y)
            {
                return AreSimilar(x, y, _MinBitCount);
            }

            public int GetHashCode(Hash256 obj)
            {
                return _HashIndex < 0
                ? obj.GetHashCode()
                : obj[_HashIndex];
            }
        }

        #endregion
    }




    /// <summary>
    /// Represents a hash value of 384 bits
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{ToHexString()}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack=1)]
    [System.Text.Json.Serialization.JsonConverter(typeof(Hash384.JsonConverter))]
    public readonly partial struct Hash384 : IEquatable<Hash384> , IHashValue
    {
        #region constants

        public const int BYTESIZE = 48;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a random hash.
        /// </summary>
        /// <returns>A new hash</returns>
        public static Hash384 FromRandom()
        {            
            Span<Byte> bytes = stackalloc Byte[BYTESIZE];            

            _HashEngines.Randomizer.GetBytes(bytes);

            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash384(bytes);
        }

        public static bool TryGetFromService(IServiceProvider service, string jsonProperty, out Hash384 result)
        {
            // direct
            if (service.GetService(typeof(Hash384)) is Hash384 h) { result= h; return true;}

            // indirect
            if (service.GetService(typeof(ISource)) is ISource hs) { result= hs.GetHash384Code(); return true;}

            // from json document
            if (!string.IsNullOrWhiteSpace(jsonProperty))
            {
                if (service.GetService(typeof(System.Text.Json.JsonDocument)) is System.Text.Json.JsonDocument doc)
                {
                    if (TryGetFromJson(doc.RootElement, jsonProperty, out result)) return true;
                }
            }

            result = default;
            return false;            
        }

        public static bool TryGetFromJson(System.Text.Json.JsonElement element, string jsonProperty, out Hash384 result)
        {
            var val = element.FindFirstOrDefault(jsonProperty);
            return TryParse(val, out result);
        }

        /// <summary>
        /// Detects if the string is hex or base64 and parses it.
        /// </summary>
        public static bool TryParse(string value, out Hash384 result)
        {
            if (string.IsNullOrWhiteSpace(value)) { result = default; return false; }

            try // try parse hexadecimal
            {                
                if (value.Length == BYTESIZE * 2 && !value.Contains('='))
                {
                    result = ParseHex(value);
                    return true;
                }
            }
            catch { }     

            try // try parse base64
            {
                result = ParseBase64(value);
                return true;
            }
            catch { }   
            
            result = default;
            return false;
        }

        public static Hash384 ParseHex(string value)
        {
            if (string.IsNullOrEmpty(value)) return default;

            var bytes = value.ParseHexBytes();
            
            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash384(bytes);
        }

        public static Hash384 ParseBase64(string value)
        {
            if (string.IsNullOrEmpty(value)) return default;

            var bytes = value.ParseBase64Bytes();

            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash384(bytes);
        }        

        public Hash384(ReadOnlySpan<Byte> bytes)
        {
            if (bytes.Length != BYTESIZE) throw new ArgumentException($"Byte len is {bytes.Length}, expected {BYTESIZE}", nameof(bytes));
            this = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte,Hash384>(bytes)[0];
        }

        public Hash384(ulong a, ulong b, ulong c, ulong d, ulong e, ulong f)
        {
            _A = a;
            _B = b;
            _C = c;
            _D = d;
            _E = e;
            _F = f;
        }

        #endregion

        #region data

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _A;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _B;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _C;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _D;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _E;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _F;
        
        /// <Inheritdoc/>
        public override int GetHashCode()
        {            
            return HashCode.Combine(_A, _B, _C, _D, _E, _F);         
        }

        /// <Inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is HashValue href) obj = href._Hash;
            if (obj is Hash384 other) return Equals(other);
            return false;
        }

        /// <Inheritdoc/>
        public bool Equals(IHashValue obj)
        {
            if (obj is HashValue href) obj = href._Hash;
            if (obj is Hash384 other) return Equals(other);
            return false;
        }

        /// <Inheritdoc/>
        public bool Equals(Hash384 other)
        {
            if (this._A != other._A) return false;
            if (this._B != other._B) return false;
            if (this._C != other._C) return false;
            if (this._D != other._D) return false;
            if (this._E != other._E) return false;
            if (this._F != other._F) return false;
            return true;
        }

        public static bool operator ==(in Hash384 a, in Hash384 b) { return a.Equals(b); }

        public static bool operator !=(in Hash384 a, in Hash384 b) { return !a.Equals(b); }

        #endregion

        #region operator

        public Byte this[int index]
        {
            get
            {                
                var bytes = AsReadOnlyBytes();
                return bytes[index];
            }        
        }

        public static Hash384 operator ^(in Hash384 left, in Hash384 right)
        {
            return new Hash384(left._A ^ right._A, left._B ^ right._B, left._C ^ right._C, left._D ^ right._D, left._E ^ right._E, left._F ^ right._F);
        }

        public static Hash384 operator &(in Hash384 left, in Hash384 right)
        {
            return new Hash384(left._A & right._A, left._B & right._B, left._C & right._C, left._D & right._D, left._E & right._E, left._F & right._F);
        }

        public static Hash384 operator |(in Hash384 left, in Hash384 right)
        {
            return new Hash384(left._A | right._A, left._B | right._B, left._C | right._C, left._D | right._D, left._E | right._E, left._F | right._F);
        }

        public static Hash384 operator ~(in Hash384 value)
        {
            return new Hash384(~value._A, ~value._B, ~value._C, ~value._D, ~value._E, ~value._F);
        }

        #endregion

        #region API        

        public int ByteCount => BYTESIZE;

        public bool IsZero
        {
            get
            {
                if (this._A != 0) return false;
                if (this._B != 0) return false;
                if (this._C != 0) return false;
                if (this._D != 0) return false;
                if (this._E != 0) return false;
                if (this._F != 0) return false;
                return true;            
            }
        }

        private ReadOnlySpan<Byte> AsReadOnlyBytes()
        {            
            ref var hRef = ref System.Runtime.CompilerServices.Unsafe.AsRef(this);
            var span = System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref hRef, 1);
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Hash384, Byte>(span);
        }

        public Byte[] ToBytes()
        {
            var bytes = new Byte[BYTESIZE];
            CopyTo(bytes);
            return bytes;
        }

        public string ToBase64String()
        {            
            var bytes = AsReadOnlyBytes();         
            return bytes.ToBase64String();
        }

        public string ToHexString()
        {            
            var bytes = AsReadOnlyBytes();         
            return bytes.ToHexString();
        }

        public void CopyTo(Span<Byte> target)
        {
            System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, Hash384>(target)[0] = this;
        }

        /// <remarks>
        /// Reads a hash value from the stream <paramref name="reader"/>
        /// </remarks>
        public static Hash384 ReadFrom(System.IO.Stream reader)
        {            
            Span<Byte> bytes = stackalloc byte[BYTESIZE];

            bytes.ReadBytesFrom(reader);

            return new Hash384(bytes);
        }

        /// <remarks>
        /// Writes this hash value to the stream <paramref name="writer"/>
        /// </remarks>
        public void WriteTo(System.IO.Stream writer)
        {
            var bytes = AsReadOnlyBytes();
            bytes.WriteBytesTo(writer);
        }

        public static Hash384 ReadFrom(System.IO.BinaryReader reader)
        {            
            Span<Byte> bytes = stackalloc byte[BYTESIZE];
            bytes.ReadBytesFrom(reader);         

            return new Hash384(bytes);
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {            
            var bytes = AsReadOnlyBytes();         

            bytes.WriteBytesTo(writer);
        }

        /// <summary>
        /// Gets the number of enabled bits.
        /// </summary>
        /// <returns>A count of enabled bits.</returns>
        public int GetEnabledBitsCount()
        {            
            var bytes = AsReadOnlyBytes();         

            int count = 0;            

            foreach (var e in bytes)
            {
                for (int i = 0; i < 8; ++i)
                {
                    if (((e >> i) & 1) == 1) ++count;
                }
            }

            return count;
        }

        /// <summary>
        /// Checks whether the two values are similar, given how many bits match.
        /// </summary>
        public static bool AreSimilar(in Hash384 left, in Hash384 right, int minBits)
        {
            System.Diagnostics.Debug.Assert(minBits >= 0);
            return (left ^ right).GetEnabledBitsCount() <= minBits;
        }

        public static IEqualityComparer<Hash384> GetComparer(int minBits, int hashIndex = -1)
        {
            if (minBits < 0) throw new ArgumentOutOfRangeException(nameof(minBits));

            return new _SimilarityComparer(minBits, hashIndex);
        }

        /// <remarks>
        /// <para>
        /// Gets the list of objects pairs that are considered equal, given their hashes discrepancy bits is lower than minBits.
        /// </para>
        /// <para>
        /// Strategies for faster collision check in dictionaries:        
        /// we cannot create a full hash from all the bytes because it would
        /// prevent a collision when there's a few bits that are different.
        /// </para>
        /// <para>
        /// But we can run multiple searches using PARTIAL hashes, that is,
        /// run the search using the 1st byte as dictionary hash,
        /// then run the search using the 2nd byte as dictionary hash and so on.        
        /// </para>
        /// </remarks>
        public static IEnumerable<(T, T)> FindCollisions<T>(IEnumerable<(T Item, Hash384 Hash)> items, int minBits = 1)
        {
            System.Diagnostics.Debug.Assert(minBits >= 0);

            var result = new HashSet<(T, T)>();

            for (int i = 0; i < BYTESIZE; ++i) // run the search for every byte.
            {
                // this dictionary stores hashes that are considered "equal" as long as:
                // - the given byte index is the same (used to calculate the partial hash)
                // - the number of divergent bits is equal or below minBits
                var dict = new Dictionary<Hash384, T>(new _SimilarityComparer(minBits, i));

                foreach (var (item, hash) in items)
                {
                    if (dict.TryGetValue(hash, out var collision))
                    {
                        result.Add((item, collision));
                    }
                    else
                    {
                        dict[hash] = item;
                    }
                }
            }

            return result;
        }

        #endregion

        #region nested types

        public interface ISource
        {
            Hash384 GetHash384Code();
        }

        /// <summary>
        /// Helper class used in Json serialization.
        /// </summary>
        /// <remarks>
        /// <see cref="System.Text.Json.Serialization.JsonConverterAttribute"/>
        /// </remarks>
        public class JsonConverter : JsonConverter<Hash384>
        {
            public override Hash384 Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                var bytes = reader.GetBytesFromBase64();
                return new Hash384(bytes);
            }

            public override void Write(
                Utf8JsonWriter writer,
                Hash384 value,
                JsonSerializerOptions options)
            {
                var bytes = value.ToBytes();
                writer.WriteBase64StringValue(bytes);
            }
        }

        /// <summary>
        /// Compares whether two values are similar, if the number of mismatching bits is lower than the given value.
        /// </summary>
        sealed class _SimilarityComparer : IEqualityComparer<Hash384>
        {
            /// <summary>
            /// Creates a new comparer
            /// </summary>
            /// <param name="mbc">the minimum number of bits required to consider two instances as equal.</param>
            /// <param name="hashIndex">The index of the byte to use as a hash, or -1 to use default hash.</param>
            /// <exception cref="ArgumentOutOfRangeException"></exception>
            public _SimilarityComparer(int mbc, int hashIndex = -1)
            {
                if (hashIndex >= BYTESIZE) throw new ArgumentOutOfRangeException(nameof(hashIndex));

                _MinBitCount = mbc;
                _HashIndex = hashIndex;
            }

            private readonly int _MinBitCount;
            private readonly int _HashIndex;

            public bool Equals(Hash384 x, Hash384 y)
            {
                return AreSimilar(x, y, _MinBitCount);
            }

            public int GetHashCode(Hash384 obj)
            {
                return _HashIndex < 0
                ? obj.GetHashCode()
                : obj[_HashIndex];
            }
        }

        #endregion
    }




    /// <summary>
    /// Represents a hash value of 512 bits
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{ToHexString()}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack=1)]
    [System.Text.Json.Serialization.JsonConverter(typeof(Hash512.JsonConverter))]
    public readonly partial struct Hash512 : IEquatable<Hash512> , IHashValue
    {
        #region constants

        public const int BYTESIZE = 64;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a random hash.
        /// </summary>
        /// <returns>A new hash</returns>
        public static Hash512 FromRandom()
        {            
            Span<Byte> bytes = stackalloc Byte[BYTESIZE];            

            _HashEngines.Randomizer.GetBytes(bytes);

            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash512(bytes);
        }

        public static bool TryGetFromService(IServiceProvider service, string jsonProperty, out Hash512 result)
        {
            // direct
            if (service.GetService(typeof(Hash512)) is Hash512 h) { result= h; return true;}

            // indirect
            if (service.GetService(typeof(ISource)) is ISource hs) { result= hs.GetHash512Code(); return true;}

            // from json document
            if (!string.IsNullOrWhiteSpace(jsonProperty))
            {
                if (service.GetService(typeof(System.Text.Json.JsonDocument)) is System.Text.Json.JsonDocument doc)
                {
                    if (TryGetFromJson(doc.RootElement, jsonProperty, out result)) return true;
                }
            }

            result = default;
            return false;            
        }

        public static bool TryGetFromJson(System.Text.Json.JsonElement element, string jsonProperty, out Hash512 result)
        {
            var val = element.FindFirstOrDefault(jsonProperty);
            return TryParse(val, out result);
        }

        /// <summary>
        /// Detects if the string is hex or base64 and parses it.
        /// </summary>
        public static bool TryParse(string value, out Hash512 result)
        {
            if (string.IsNullOrWhiteSpace(value)) { result = default; return false; }

            try // try parse hexadecimal
            {                
                if (value.Length == BYTESIZE * 2 && !value.Contains('='))
                {
                    result = ParseHex(value);
                    return true;
                }
            }
            catch { }     

            try // try parse base64
            {
                result = ParseBase64(value);
                return true;
            }
            catch { }   
            
            result = default;
            return false;
        }

        public static Hash512 ParseHex(string value)
        {
            if (string.IsNullOrEmpty(value)) return default;

            var bytes = value.ParseHexBytes();
            
            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash512(bytes);
        }

        public static Hash512 ParseBase64(string value)
        {
            if (string.IsNullOrEmpty(value)) return default;

            var bytes = value.ParseBase64Bytes();

            System.Diagnostics.Debug.Assert(bytes.Length == BYTESIZE);
            return new Hash512(bytes);
        }        

        public Hash512(ReadOnlySpan<Byte> bytes)
        {
            if (bytes.Length != BYTESIZE) throw new ArgumentException($"Byte len is {bytes.Length}, expected {BYTESIZE}", nameof(bytes));
            this = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte,Hash512>(bytes)[0];
        }

        public Hash512(ulong a, ulong b, ulong c, ulong d, ulong e, ulong f, ulong g, ulong h)
        {
            _A = a;
            _B = b;
            _C = c;
            _D = d;
            _E = e;
            _F = f;
            _G = g;
            _H = h;
        }

        #endregion

        #region data

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _A;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _B;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _C;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _D;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _E;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _F;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _G;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly ulong _H;
        
        /// <Inheritdoc/>
        public override int GetHashCode()
        {            
            return HashCode.Combine(_A, _B, _C, _D, _E, _F, _G, _H);         
        }

        /// <Inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is HashValue href) obj = href._Hash;
            if (obj is Hash512 other) return Equals(other);
            return false;
        }

        /// <Inheritdoc/>
        public bool Equals(IHashValue obj)
        {
            if (obj is HashValue href) obj = href._Hash;
            if (obj is Hash512 other) return Equals(other);
            return false;
        }

        /// <Inheritdoc/>
        public bool Equals(Hash512 other)
        {
            if (this._A != other._A) return false;
            if (this._B != other._B) return false;
            if (this._C != other._C) return false;
            if (this._D != other._D) return false;
            if (this._E != other._E) return false;
            if (this._F != other._F) return false;
            if (this._G != other._G) return false;
            if (this._H != other._H) return false;
            return true;
        }

        public static bool operator ==(in Hash512 a, in Hash512 b) { return a.Equals(b); }

        public static bool operator !=(in Hash512 a, in Hash512 b) { return !a.Equals(b); }

        #endregion

        #region operator

        public Byte this[int index]
        {
            get
            {                
                var bytes = AsReadOnlyBytes();
                return bytes[index];
            }        
        }

        public static Hash512 operator ^(in Hash512 left, in Hash512 right)
        {
            return new Hash512(left._A ^ right._A, left._B ^ right._B, left._C ^ right._C, left._D ^ right._D, left._E ^ right._E, left._F ^ right._F, left._G ^ right._G, left._H ^ right._H);
        }

        public static Hash512 operator &(in Hash512 left, in Hash512 right)
        {
            return new Hash512(left._A & right._A, left._B & right._B, left._C & right._C, left._D & right._D, left._E & right._E, left._F & right._F, left._G & right._G, left._H & right._H);
        }

        public static Hash512 operator |(in Hash512 left, in Hash512 right)
        {
            return new Hash512(left._A | right._A, left._B | right._B, left._C | right._C, left._D | right._D, left._E | right._E, left._F | right._F, left._G | right._G, left._H | right._H);
        }

        public static Hash512 operator ~(in Hash512 value)
        {
            return new Hash512(~value._A, ~value._B, ~value._C, ~value._D, ~value._E, ~value._F, ~value._G, ~value._H);
        }

        #endregion

        #region API        

        public int ByteCount => BYTESIZE;

        public bool IsZero
        {
            get
            {
                if (this._A != 0) return false;
                if (this._B != 0) return false;
                if (this._C != 0) return false;
                if (this._D != 0) return false;
                if (this._E != 0) return false;
                if (this._F != 0) return false;
                if (this._G != 0) return false;
                if (this._H != 0) return false;
                return true;            
            }
        }

        private ReadOnlySpan<Byte> AsReadOnlyBytes()
        {            
            ref var hRef = ref System.Runtime.CompilerServices.Unsafe.AsRef(this);
            var span = System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref hRef, 1);
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Hash512, Byte>(span);
        }

        public Byte[] ToBytes()
        {
            var bytes = new Byte[BYTESIZE];
            CopyTo(bytes);
            return bytes;
        }

        public string ToBase64String()
        {            
            var bytes = AsReadOnlyBytes();         
            return bytes.ToBase64String();
        }

        public string ToHexString()
        {            
            var bytes = AsReadOnlyBytes();         
            return bytes.ToHexString();
        }

        public void CopyTo(Span<Byte> target)
        {
            System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, Hash512>(target)[0] = this;
        }

        /// <remarks>
        /// Reads a hash value from the stream <paramref name="reader"/>
        /// </remarks>
        public static Hash512 ReadFrom(System.IO.Stream reader)
        {            
            Span<Byte> bytes = stackalloc byte[BYTESIZE];

            bytes.ReadBytesFrom(reader);

            return new Hash512(bytes);
        }

        /// <remarks>
        /// Writes this hash value to the stream <paramref name="writer"/>
        /// </remarks>
        public void WriteTo(System.IO.Stream writer)
        {
            var bytes = AsReadOnlyBytes();
            bytes.WriteBytesTo(writer);
        }

        public static Hash512 ReadFrom(System.IO.BinaryReader reader)
        {            
            Span<Byte> bytes = stackalloc byte[BYTESIZE];
            bytes.ReadBytesFrom(reader);         

            return new Hash512(bytes);
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {            
            var bytes = AsReadOnlyBytes();         

            bytes.WriteBytesTo(writer);
        }

        /// <summary>
        /// Gets the number of enabled bits.
        /// </summary>
        /// <returns>A count of enabled bits.</returns>
        public int GetEnabledBitsCount()
        {            
            var bytes = AsReadOnlyBytes();         

            int count = 0;            

            foreach (var e in bytes)
            {
                for (int i = 0; i < 8; ++i)
                {
                    if (((e >> i) & 1) == 1) ++count;
                }
            }

            return count;
        }

        /// <summary>
        /// Checks whether the two values are similar, given how many bits match.
        /// </summary>
        public static bool AreSimilar(in Hash512 left, in Hash512 right, int minBits)
        {
            System.Diagnostics.Debug.Assert(minBits >= 0);
            return (left ^ right).GetEnabledBitsCount() <= minBits;
        }

        public static IEqualityComparer<Hash512> GetComparer(int minBits, int hashIndex = -1)
        {
            if (minBits < 0) throw new ArgumentOutOfRangeException(nameof(minBits));

            return new _SimilarityComparer(minBits, hashIndex);
        }

        /// <remarks>
        /// <para>
        /// Gets the list of objects pairs that are considered equal, given their hashes discrepancy bits is lower than minBits.
        /// </para>
        /// <para>
        /// Strategies for faster collision check in dictionaries:        
        /// we cannot create a full hash from all the bytes because it would
        /// prevent a collision when there's a few bits that are different.
        /// </para>
        /// <para>
        /// But we can run multiple searches using PARTIAL hashes, that is,
        /// run the search using the 1st byte as dictionary hash,
        /// then run the search using the 2nd byte as dictionary hash and so on.        
        /// </para>
        /// </remarks>
        public static IEnumerable<(T, T)> FindCollisions<T>(IEnumerable<(T Item, Hash512 Hash)> items, int minBits = 1)
        {
            System.Diagnostics.Debug.Assert(minBits >= 0);

            var result = new HashSet<(T, T)>();

            for (int i = 0; i < BYTESIZE; ++i) // run the search for every byte.
            {
                // this dictionary stores hashes that are considered "equal" as long as:
                // - the given byte index is the same (used to calculate the partial hash)
                // - the number of divergent bits is equal or below minBits
                var dict = new Dictionary<Hash512, T>(new _SimilarityComparer(minBits, i));

                foreach (var (item, hash) in items)
                {
                    if (dict.TryGetValue(hash, out var collision))
                    {
                        result.Add((item, collision));
                    }
                    else
                    {
                        dict[hash] = item;
                    }
                }
            }

            return result;
        }

        #endregion

        #region nested types

        public interface ISource
        {
            Hash512 GetHash512Code();
        }

        /// <summary>
        /// Helper class used in Json serialization.
        /// </summary>
        /// <remarks>
        /// <see cref="System.Text.Json.Serialization.JsonConverterAttribute"/>
        /// </remarks>
        public class JsonConverter : JsonConverter<Hash512>
        {
            public override Hash512 Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                var bytes = reader.GetBytesFromBase64();
                return new Hash512(bytes);
            }

            public override void Write(
                Utf8JsonWriter writer,
                Hash512 value,
                JsonSerializerOptions options)
            {
                var bytes = value.ToBytes();
                writer.WriteBase64StringValue(bytes);
            }
        }

        /// <summary>
        /// Compares whether two values are similar, if the number of mismatching bits is lower than the given value.
        /// </summary>
        sealed class _SimilarityComparer : IEqualityComparer<Hash512>
        {
            /// <summary>
            /// Creates a new comparer
            /// </summary>
            /// <param name="mbc">the minimum number of bits required to consider two instances as equal.</param>
            /// <param name="hashIndex">The index of the byte to use as a hash, or -1 to use default hash.</param>
            /// <exception cref="ArgumentOutOfRangeException"></exception>
            public _SimilarityComparer(int mbc, int hashIndex = -1)
            {
                if (hashIndex >= BYTESIZE) throw new ArgumentOutOfRangeException(nameof(hashIndex));

                _MinBitCount = mbc;
                _HashIndex = hashIndex;
            }

            private readonly int _MinBitCount;
            private readonly int _HashIndex;

            public bool Equals(Hash512 x, Hash512 y)
            {
                return AreSimilar(x, y, _MinBitCount);
            }

            public int GetHashCode(Hash512 obj)
            {
                return _HashIndex < 0
                ? obj.GetHashCode()
                : obj[_HashIndex];
            }
        }

        #endregion
    }

}
