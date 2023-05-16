using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace InteropTypes
{
    internal static class _StringExtensions
    {
        public static string ToHexString(this Span<Byte> bytes)
        {
            return ((ReadOnlySpan<Byte>)bytes).ToHexString();
        }

        public static string ToBase64String(this Span<Byte> bytes)
        {
            return ((ReadOnlySpan<Byte>)bytes).ToBase64String();
        }

        public static string ToHexString(this ReadOnlySpan<Byte> bytes)
        {
            #if NET5_0_OR_GREATER
            return Convert.ToHexString(bytes);
            #else
            var hex = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes) hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
            #endif
        }

        public static string ToBase64String(this ReadOnlySpan<Byte> bytes)
        {
            #if NETSTANDARD2_0
            return System.Convert.ToBase64String(bytes.ToArray());
            #else
            return System.Convert.ToBase64String(bytes);
            #endif
        }

        public static Byte[] ParseHexBytes(this string value)
        {
            if (string.IsNullOrEmpty(value)) return default;

            #if NET5_0_OR_GREATER
            return System.Convert.FromHexString(value);
            #else

            return Enumerable
                .Range(0, value.Length)
                .Where(x => x % 2 == 0)
                .Select(x => byte.Parse(value.Substring(x, 2), System.Globalization.NumberStyles.HexNumber))
                .ToArray();
            #endif
        }

        public static Byte[] ParseBase64Bytes(this string value)
        {
            if (string.IsNullOrEmpty(value)) return default;

            return System.Convert.FromBase64String(value);            
        }        

        public static string FindFirstOrDefault(this JsonElement element, string propertyName)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                foreach(var property in element.EnumerateObject())
                {
                    if (string.Equals(propertyName, property.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        if (property.Value.ValueKind == JsonValueKind.String) return property.Value.GetString();
                    }                    

                    var result = property.Value.FindFirstOrDefault(propertyName);
                    if (result != null) return result;
                }
            }

            if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (var child in element.EnumerateArray())
                {
                    var result = child.FindFirstOrDefault(propertyName);
                    if (result != null) return result;
                }
            }

            return null;
        }

    }
}
