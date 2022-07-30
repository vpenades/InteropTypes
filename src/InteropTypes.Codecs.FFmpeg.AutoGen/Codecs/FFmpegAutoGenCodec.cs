using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using InteropTypes.Graphics.Backends.Codecs;
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
        public static void EncodeFrames(Action<Action<System.IO.FileInfo>> saveCallback, IEnumerable<(PointerBitmap bitmap, long pts)> frames, int? fps)
        {
            saveCallback(finfo => EncodeFrames(finfo.FullName, frames, fps));
        }

        public static void EncodeFrames(string url, IEnumerable<(PointerBitmap bitmap, long pts)> frames, int? fps = null)
        {
            using(var encoder = new VideoStreamEncoder(url, fps ?? 30))
            {
                foreach (var (bitmap, pts) in frames)
                {
                    encoder.PushFrame(bitmap, fps.HasValue ? null : (long?)pts);
                }

                encoder.Drain();
            }
        }
        
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public static void ProcessFrames(string inputUrl, Action<Action<System.IO.FileInfo>> outputCallback, Action<MemoryBitmap> frameCallback, int? fps = null)
        {
            outputCallback(finfo => ProcessFrames(inputUrl, finfo.FullName, frameCallback, fps));
        }

        public static void ProcessFrames(string inputUrl, string outputUrl, Action<MemoryBitmap> frameCallback, int? fps = null)
        {
            // transcoding
            // https://gist.github.com/Ruslan-B/43d3a4219f39b99f0c9685290dcd23cc

            // encodes to jpgs, then re-encodes
            // https://github.com/Ruslan-B/FFmpeg.AutoGen/blob/master/FFmpeg.AutoGen.Example/Program.cs#L29

            // similar timing issues
            // https://github.com/Ruslan-B/FFmpeg.AutoGen.Questions/issues/31
            // https://github.com/Ruslan-B/FFmpeg.AutoGen.Questions/issues/7

            MemoryBitmap current = default;

            VideoStreamEncoder encoder = null;
            
            foreach (var (bitmap, state) in DecodeFrames(inputUrl))
            {
                bitmap.AsSpanBitmap().CopyTo(ref current);

                frameCallback(current);

                // get framerate from state.

                if (encoder == null) encoder = new VideoStreamEncoder(outputUrl, fps ?? 30);

                current.AsSpanBitmap().PinReadablePointer(ptr => encoder.PushFrame(ptr));
            }

            encoder?.Drain();

            encoder?.Dispose();
        }


    }
}
