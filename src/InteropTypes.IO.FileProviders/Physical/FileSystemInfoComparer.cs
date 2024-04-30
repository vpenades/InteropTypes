using System;
using System.Collections.Generic;
using System.IO;

namespace InteropTypes.IO
{
    /// <summary>
    /// Used to compare equality between <see cref="FileInfo"/> and <see cref="DirectoryInfo"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FileSystemInfoComparer<T> 
        where T : FileSystemInfo
    {
        private FileSystemInfoComparer() { }

        public static IEqualityComparer<T> Default { get; } = MatchCasing.PlatformDefault.GetFullNameComparer<T>();
    }
}
