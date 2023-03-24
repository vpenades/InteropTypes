using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using InteropTypes.Graphics.Backends.Codecs;

using BITMAP = InteropTypes.Graphics.Bitmaps.MemoryBitmap;

namespace InteropTypes.Codecs
{
    public static class FFmpegAutoGen
    {
        public static IEnumerable<(BITMAP bitmap, VideoFrameMetadata state)> DecodeFrames(string url)
        {
            return _Implementation.DecodeFrames(url, FFmpeg.AutoGen.AVHWDeviceType.AV_HWDEVICE_TYPE_NONE);
        }

        public static void EncodeFrames(string url, int fps, IEnumerable<BITMAP> bitmaps)
        {
            using (var encoder = new VideoStreamEncoder(url, fps))
            {
                foreach (var bmp in bitmaps)
                {
                    encoder.PushFrame(bmp);
                }
                encoder.Drain();
            }
        }
        
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public static void EncodeFrames(Action<Action<System.IO.FileInfo>> saveCallback, IEnumerable<(BITMAP bitmap, long pts)> frames, int? fps)
        {
            saveCallback(finfo => EncodeFrames(finfo.FullName, frames, fps));
        }

        public static void EncodeFrames(string url, IEnumerable<(BITMAP bitmap, long pts)> frames, int? fps = null)
        {
            using(var encoder = new VideoStreamEncoder(url, fps ?? -1))
            {
                foreach (var (bmp, pts) in frames)
                {
                    encoder.PushFrame(bmp);
                }

                encoder.Drain();
            }
        }
        
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public static void ProcessFrames(string inputUrl, Action<Action<System.IO.FileInfo>> outputCallback, Action<BITMAP> frameCallback, int? fps = null)
        {
            outputCallback(finfo => ProcessFrames(inputUrl, finfo.FullName, frameCallback, fps));
        }

        public static void ProcessFrames(string inputUrl, string outputUrl, Action<BITMAP> frameCallback, int? fps = null)
        {
            // transcoding
            // https://gist.github.com/Ruslan-B/43d3a4219f39b99f0c9685290dcd23cc

            // encodes to jpgs, then re-encodes
            // https://github.com/Ruslan-B/FFmpeg.AutoGen/blob/master/FFmpeg.AutoGen.Example/Program.cs#L29

            // similar timing issues
            // https://github.com/Ruslan-B/FFmpeg.AutoGen.Questions/issues/31
            // https://github.com/Ruslan-B/FFmpeg.AutoGen.Questions/issues/7

            BITMAP current = default;

            VideoStreamEncoder encoder = null;
            
            foreach (var (bitmap, state) in DecodeFrames(inputUrl))
            {
                bitmap.AsSpanBitmap().CopyTo(ref current);

                frameCallback(current);

                // get framerate from state.

                if (encoder == null) encoder = new VideoStreamEncoder(outputUrl, fps ?? 30);

                encoder.PushFrame(current);
            }

            encoder?.Drain();

            encoder?.Dispose();
        }


    }
}
