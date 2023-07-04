using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Primitives;

namespace InteropTypes.IO.FileProviders
{
    internal static class PathUtils
    {
        #region static

        private static char[] GetInvalidFileNameChars() => Path.GetInvalidFileNameChars()
            .Where(c => c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar)
            .ToArray();

        private static char[] GetInvalidFilterChars() => GetInvalidFileNameChars()
            .Where(c => c != '*' && c != '|' && c != '?')
            .ToArray();

        #if NET8_0_OR_GREATER
        private static readonly SearchValues<char> _invalidFileNameChars = SearchValues.Create(GetInvalidFileNameChars());
        private static readonly SearchValues<char> _invalidFilterChars = SearchValues.Create(GetInvalidFilterChars());
        #else
        private static readonly char[] _invalidFileNameChars = GetInvalidFileNameChars();
        private static readonly char[] _invalidFilterChars = GetInvalidFilterChars();
        #endif

        private static readonly char[] _pathSeparators = new[]
            {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};

        #endregion

        #region functions

        internal static bool IsPathSeparator(this char c)
        {
            if (c == Path.DirectorySeparatorChar) return true;
            if (c == Path.AltDirectorySeparatorChar) return true;
            return false;
        }

        internal static IEnumerable<string> SplitPath(string path)
        {
            var sb = new StringBuilder();

            foreach(var c in path)
            {
                if (c.IsPathSeparator())
                {
                    if (sb.Length > 0) yield return sb.ToString();
                    sb.Clear();
                    continue;
                }

                sb.Append(c);
            }

            if (sb.Length > 0) yield return sb.ToString();
        }

        internal static bool HasInvalidPathChars(string path)
        {
            return path.AsSpan().IndexOfAny(_invalidFileNameChars) >= 0;
        }

        internal static bool HasInvalidFilterChars(string path)
        {
            return path.AsSpan().IndexOfAny(_invalidFilterChars) >= 0;
        }

        internal static string EnsureTrailingSlash(string path)
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

        internal static bool ContainsSeparator(string path)
        {
            return path.Any(IsPathSeparator);
        }

        

        #endregion
    }
}
