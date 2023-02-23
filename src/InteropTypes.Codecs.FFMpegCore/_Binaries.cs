using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;

using System.Net;

namespace InteropTypes.Graphics.Backends
{
    internal static class _Binaries
    {
        private static System.IO.DirectoryInfo UseTempPath() => new Lazy<System.IO.DirectoryInfo>(_GetInstallPath).Value;

        private static System.IO.DirectoryInfo _GetInstallPath()
        {
            return new System.IO.DirectoryInfo(System.IO.Path.GetTempPath()).CreateSubdirectory("FFmpegCore.Binaries");
        }

        public static System.IO.DirectoryInfo UseFFMpegDirectory()
        {
            var dir = UseTempPath();

            if (GetFFMpegDirectory() == null)
            {
                dir.Delete(true);
                
                var uri = new Uri("https://github.com/GyanD/codexffmpeg/releases/download/5.1.2/ffmpeg-5.1.2-full_build.zip");
                // var uri = new Uri("https://github.com/BtbN/FFmpeg-Builds/releases/download/autobuild-2023-02-23-12-36/ffmpeg-N-109902-gd5cc7acff1-win64-gpl-shared.zip");

                uri.TryUnzipTo(dir);
            }

            return GetFFMpegDirectory();
        }

        public static System.IO.DirectoryInfo GetFFMpegDirectory()
        {
            var exePath = UseTempPath().EnumerateFiles("ffmpeg.exe", System.IO.SearchOption.AllDirectories).FirstOrDefault();

            return exePath?.Directory;
        }
    }
}
