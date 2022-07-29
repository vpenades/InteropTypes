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
                encoder.Drain();
            }
        }
        
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public static void EncodeFrames(Action<Action<System.IO.FileInfo>> saveCallback, IEnumerable<(PointerBitmap bitmap, long pts)> frames)
        {
            saveCallback(finfo => EncodeFrames(finfo.FullName, frames));
        }

        public static void EncodeFrames(string url, IEnumerable<(PointerBitmap bitmap, long pts)> frames)
        {
            using(var encoder = new VideoStreamEncoder(url,30))
            {
                foreach (var (bitmap, pts) in frames) encoder.PushFrame(bitmap,pts);
                encoder.Drain();
            }
        }
        
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public static void ProcessFrames(string inputUrl, Action<Action<System.IO.FileInfo>> outputCallback, Action<MemoryBitmap> frameCallback)
        {
            outputCallback(finfo => ProcessFrames(inputUrl, finfo.FullName, frameCallback));
        }

        public static void ProcessFrames(string inputUrl, string outputUrl, Action<MemoryBitmap> frameCallback)
        {
            // transcoding
            // https://gist.github.com/Ruslan-B/43d3a4219f39b99f0c9685290dcd23cc

            // encodes to jpgs, then re-encodes
            // https://github.com/Ruslan-B/FFmpeg.AutoGen/blob/master/FFmpeg.AutoGen.Example/Program.cs#L29

            MemoryBitmap current = default;

            VideoStreamEncoder encoder = null;
            
            foreach (var (bitmap, state) in DecodeFrames(inputUrl))
            {
                bitmap.AsSpanBitmap().CopyTo(ref current);

                frameCallback(current);

                // get framerate from state.

                if (encoder == null) encoder = new VideoStreamEncoder(outputUrl, state.Times["time_base"], state.Times["frame_rate"]);

                current.AsSpanBitmap().PinReadablePointer(ptr => encoder.PushFrame(ptr));
            }

            encoder?.Drain();

            encoder?.Dispose();
        }


    }
}
