using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Codecs
{
    public static class FFmpegAutoGen
    {
        public static IEnumerable<(PointerBitmap bitmap, VideoFrameState state)> DecodeFrames(string url)
        {
            return _Implementation.DecodeFrames(url, FFmpeg.AutoGen.AVHWDeviceType.AV_HWDEVICE_TYPE_NONE);
        }

        public static void EncodeFrames(string url, IEnumerable<PointerBitmap> frames)
        {
            using (var encoder = new VideoStreamEncoder(url, 20))
            {
                foreach (var f in frames) encoder.PushFrame(f);
            }
        }

        public static void EncodeFrames(string url, IEnumerable<(PointerBitmap bitmap, long pts)> frames)
        {
            using(var encoder = new VideoStreamEncoder(url,30))
            {
                foreach (var (bitmap, pts) in frames) encoder.PushFrame(bitmap,pts);
            }
        }

        public static void ProcessFrames(string inputUrl, string outputUrl, Action<MemoryBitmap> frameCallback)
        {
            MemoryBitmap current = default;

            VideoStreamEncoder encoder = null;
            
            foreach (var (bitmap, state) in DecodeFrames(inputUrl))
            {
                bitmap.AsSpanBitmap().CopyTo(ref current);

                frameCallback(current);

                // get framerate from state.

                if (encoder == null) encoder = new VideoStreamEncoder(outputUrl, 30);

                current.AsSpanBitmap().PinReadablePointer(ptr => encoder.PushFrame(ptr));
            }

            encoder?.Dispose();
        }
    }
}
