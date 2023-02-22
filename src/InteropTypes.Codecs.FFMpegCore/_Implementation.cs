using System;
using System.Collections.Generic;
using System.Text;

using FFMpegCore;
using FFMpegCore.Arguments;
using FFMpegCore.Enums;
using FFMpegCore.Extend;
using FFMpegCore.Pipes;

using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Bitmaps;

using GDIWRAPPER = FFMpegCore.Extensions.System.Drawing.Common.BitmapVideoFrameWrapper;

namespace InteropTypes
{
    static class _Implementation
    {
        

        public static IEnumerable<GDIWRAPPER> WrapFrames(IEnumerable<PointerBitmap> frames)
        {
            MemoryBitmap tmp = default;

            foreach(var f in frames)
            {
                f.CopyTo(ref tmp);

                var bmp = tmp.ToGDIBitmap();

                yield return new GDIWRAPPER(bmp);
            }
        }       


        public static void Encode(string outFile, IEnumerable<IVideoFrame> frames)
        {
            Encode(outFile, frames, new VideoCodecArgument(VideoCodec.LibX264));
        }

        public static void Encode(string outFile, IEnumerable<IVideoFrame> frames, params IArgument[] inputArguments)
        {
            var videoFramesSource = new RawVideoPipeSource(frames);

            void _addArgs(FFMpegArgumentOptions ops)
            {
                foreach (var arg in inputArguments) ops.WithArgument(arg);
            }            

            var arguments = FFMpegArguments.FromPipeInput(videoFramesSource, _addArgs);
            

            var processor = arguments.OutputToFile(outFile);
            
            processor.ProcessSynchronously();
        }
    }
}
