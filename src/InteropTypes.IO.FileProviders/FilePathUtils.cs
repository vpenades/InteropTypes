using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Primitives;

namespace InteropTypes.IO
{
    internal static class FilePathUtils
    {
        #region static

        public static readonly StringComparison PathComparisonMode = _GetIsCaseSensitive()
            ? StringComparison.Ordinal
            : StringComparison.OrdinalIgnoreCase;

        private static bool _GetIsCaseSensitive()
        {
            // Proposal: Directory.GetCaseSensitivity() -> https://github.com/dotnet/runtime/issues/34235
            // Issue with System.IO.GetIsCaseSensitive -> https://github.com/dotnet/runtime/issues/54313
            // Change PathInternal.IsCaseSensitive to a constant -> https://github.com/dotnet/runtime/pull/54340

            // another way to determine case sensitivity is to pick a random file or directory,
            // then invert the letters capitalization, and check for its existence.

            #if !NETSTANDARD
            if (OperatingSystem.IsWindows()) return false;
            if (OperatingSystem.IsMacOS()) return false;
            if (OperatingSystem.IsMacCatalyst()) return false;
            if (OperatingSystem.IsIOS()) return false;
            if (OperatingSystem.IsTvOS()) return false;
            if (OperatingSystem.IsWatchOS()) return false;
            #endif

            return false;
        }

        private static char[] _GetInvalidFileNameChars() => Path.GetInvalidFileNameChars();

        private static char[] _GetInvalidPathChars() => Path
            .GetInvalidPathChars()
            .Where(c => c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar)
            .ToArray();

        private static char[] _GetInvalidFilterChars() => _GetInvalidPathChars()
            .Where(c => c != '*' && c != '|' && c != '?')
            .ToArray();

        /*
        #if NET8_0_OR_GREATER
        private static readonly System.Buffers.SearchValues<char> _invalidPathChars = System.Buffers.SearchValues.Create(_GetInvalidPathChars());
        private static readonly System.Buffers.SearchValues<char> _invalidNameChars = System.Buffers.SearchValues.Create(_GetInvalidFileNameChars());
        private static readonly System.Buffers.SearchValues<char> _invalidFilterChars = System.Buffers.SearchValues.Create(_GetInvalidFilterChars());        

        private static bool Intesects(string text, System.Buffers.SearchValues<char> chars)
        {
            return text.AsSpan().IndexOfAny(chars) >= 0;
        }

        #else
        */

        private static readonly char[] _invalidPathChars = _GetInvalidPathChars();
        private static readonly char[] _invalidNameChars = _GetInvalidFileNameChars();
        private static readonly char[] _invalidFilterChars = _GetInvalidFilterChars();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool _Intesects(string text, char[] chars)
        {
            return text.AsSpan().IndexOfAny(chars) >= 0;
        }

        // #endif

        private static readonly char[] _pathSeparators = new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        #endregion

        #region API

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool ContainsDirectorySeparator(string path) { return path.Any(IsDirectorySeparator); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsDirectorySeparator(this char c)
        {
            // https://github.com/dotnet/runtime/blob/main/src/libraries/Common/src/System/IO/PathInternal.Windows.cs
            return c == System.IO.Path.DirectorySeparatorChar || c == System.IO.Path.AltDirectorySeparatorChar;
        }

        internal static bool HasInvalidNameChars(string name)
        {
            // technically, a name with whitespaces is valid, but not recomended.
            return name == null || _Intesects(name, _invalidNameChars);
        }

        internal static bool HasInvalidPathChars(string path)
        {
            // technically, a name with whitespaces is valid, but not recomended.
            return path == null || _Intesects(path, _invalidPathChars);
        }

        internal static bool HasInvalidFilterChars(string filter)
        {
            // technically, a name with whitespaces is valid, but not recomended.
            return filter == null || _Intesects(filter, _invalidFilterChars);
        }

        internal static IEnumerable<string> SplitPath(string path)
        {
            var sb = new StringBuilder();

            foreach(var c in path)
            {
                if (c.IsDirectorySeparator())
                {
                    if (sb.Length > 0) yield return sb.ToString();
                    sb.Clear();
                    continue;
                }

                sb.Append(c);
            }

            if (sb.Length > 0) yield return sb.ToString();
        }

        

        [Obsolete("This method is ambiguous because it can't infer which separator to use.")]
        internal static string EnsureTrailingSeparator(string path)
        {
            if (!string.IsNullOrEmpty(path) &&
                path[path.Length - 1] != Path.DirectorySeparatorChar)
            {
                return path + Path.DirectorySeparatorChar;
            }

            return path;
        }

        internal static bool PathNavigatesAboveRoot(string path)
        {
            var tokenizer = new StringTokenizer(path, _pathSeparators);
            int depth = 0;

            foreach (StringSegment segment in tokenizer)
            {
                if (segment.Equals(".") || segment.Equals(""))
                {
                    continue;
                }
                else if (segment.Equals(".."))
                {
                    depth--;

                    if (depth == -1)
                    {
                        return true;
                    }
                }
                else
                {
                    depth++;
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to get a composite extension of a file.
        /// </summary>
        /// <param name="fileName">the filename from where to get the extension.</param>
        /// <param name="dots">the number of dots used by the extension.</param>
        /// <param name="extension">the resulting extension.</param>
        /// <returns>true if an extension was found</returns>        
        public static bool TryGetCompositedExtension(string fileName, int dots, out string extension)
        {
            if (dots < 1) throw new ArgumentOutOfRangeException(nameof(dots), "Must be equal or greater than 1");

            var l = fileName.Length - 1;
            var r = -1;

            while (dots > 0 && l >= 0)
            {
                var c = fileName[l];

                if (IsDirectorySeparator(c) || c == ':' || c=='?' || c=='*') break;

                if (c == '.')
                {
                    r = l;
                    --dots;
                }

                --l;
            }

            if (dots != 0)
            {
                extension = null;
                return false;
            }

            extension = fileName.Substring(r);
            return true;
        }

        #endregion
    }
}
