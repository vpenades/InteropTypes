using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO
{
    public interface IDirectoryContentsEx : IDirectoryContents
    {
        // IFileInfo GetFile(string path, FileMode mode);

        // bool TryGetFile(string fileName, bool create, out IFileInfo file);

        // bool TryGetStreamAccess(string fileName, bool create, out IFileStreamFullAccess streamAccess);
    }

    public interface IFileInfoEx : IFileInfo, IFileStreamFullAccess
    {
        // https://github.com/dotnet/runtime/issues/82008
    }
}
