using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InteropTypes.IO
{
    public interface IFileStreamFullAccess
    {
        public Stream CreateStream(FileMode mode)
        {
            return CreateStream(mode, (mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite));
        }

        public Stream CreateStream(FileMode mode, FileAccess access)
        {
            return CreateStream(mode, access, FileShare.None);
        }

        public System.IO.Stream CreateStream(FileMode mode, FileAccess access, FileShare share);
    }
}
