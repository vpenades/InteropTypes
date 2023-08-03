using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO
{
    public interface IPhysicalFileFactory
    {
        PhysicalSystemInfo CreateInfo(FileSystemInfo xinfo);

        PhysicalFileInfo CreateFileInfo(FileInfo fileInfo);

        PhysicalDirectoryInfo CreateDirectoryInfo(DirectoryInfo dirInfo);
    }
}
