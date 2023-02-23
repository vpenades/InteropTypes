using System;
using System.Collections.Generic;
using System.Text;

using FFMpegCore.Enums;
using FFMpegCore.Pipes;
using FFMpegCore;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Backends;

namespace InteropTypes.Codecs
{
    public class FFMpegCoreCodec
    {
        #region static

        static FFMpegCoreCodec()
        {
            GlobalFFOptions.Configure(_SetEnvironment);
        }

        private static void _SetEnvironment(FFOptions options)
        {
            // https://github.com/rosenbjerg/FFMpegCore#path-configuration

            options.BinaryFolder = _Binaries.UseFFMpegDirectory().FullName;
            options.TemporaryFilesFolder = System.IO.Path.GetTempPath() + "\\ffmpegcore";
        }

        #endregion

        #region properties

        public Codec WriteVideoCodec { get; set; } = VideoCodec.LibX264;

        public int FramesPerSecond { get; set; } = 25;

        #endregion

        #region  API

        public void Encode(string outFile, IEnumerable<PointerBitmap> frames)
        {
            Encode(outFile, PointerBitmapVideoFrame.WrapFrames(frames));
        }

        public void Encode(string outFile, IEnumerable<MemoryBitmap> frames)
        {
            Encode(outFile, MemoryBitmapVideoFrame.WrapFrames(frames));
        }

        public void Encode<TPixel>(string outFile, IEnumerable<MemoryBitmap<TPixel>> frames)
            where TPixel : unmanaged
        {
            Encode(outFile, MemoryBitmapVideoFrame.WrapFrames(frames));
        }        

        public void Encode(string outFile, IEnumerable<IVideoFrame> frames)
        {
            var videoFramesSource = new RawVideoPipeSource(frames);
            videoFramesSource.FrameRate = FramesPerSecond;            
            
            FFMpegArguments
                .FromPipeInput(videoFramesSource)                
                .OutputToFile(outFile, true, opts => opts
                    .WithVideoCodec(WriteVideoCodec))
                .ProcessSynchronously();
        }


        public void Transform(string inFile, string outFile)
        {
            // https://github.com/rosenbjerg/FFMpegCore/issues/133

            FFMpegArguments
                .FromFileInput(inFile)                
                .OutputToFile(outFile, true, opts
                => opts
                    .WithVideoCodec(WriteVideoCodec)                    
                    // .WithVideoFilters(vf => vf)
                    )
                
                .ProcessSynchronously();
        }

        #endregion
    }
}
