using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InteropTypes.IO.Archives.Primitives
{
    public interface IArchiveAccessor<TEntry>
    {
        IEnumerable<TEntry> GetAllEntries();

        bool GetEntryIsDirectory(TEntry entry);

        /// <summary>
        /// Gets the subpath, or key string of the given entry.
        /// </summary>
        /// <param name="entry">the entry to query</param>
        /// <returns>a string representing an entry subpath or key</returns>
        string GetEntryKey(TEntry entry);

        long GetEntryFileLength(TEntry entry);

        DateTimeOffset GetEntryLastWriteTime(TEntry entry);

        string GetEntryComment(TEntry entry) { return null; }

        Stream OpenEntryReadStream(TEntry entry, FileMode mode);
    }
}
