using System;
using System.Collections.Generic;
using System.Text;

using SharpAvi.Output;

namespace InteropTypes.Graphics.Video
{    
    public static class VideoEncoderFactory
    {
        public static void SaveToAVI(this IEnumerable<InteropBitmaps.MemoryBitmap> frames, string filePath, decimal fps, InteropBitmaps.Codecs.IBitmapEncoder encoder)
        {
            VideoFrameWriter writer = null;

            using(var avi = new AviWriter(filePath))
            {
                avi.FramesPerSecond = fps;

                foreach (var frame in frames)
                {
                    if (writer == null) writer = avi.AddVideoFrameWriter(frame.Width, frame.Height, encoder);
                    writer.WriteFrame(frame);
                }
            }
        }

        public static VideoFrameWriter AddVideoFrameWriter(this AviWriter writer, int width, int height, InteropBitmaps.Codecs.IBitmapEncoder encoder)
        {
            // Argument.IsNotNull(writer, nameof(writer));
            // Argument.IsPositive(width, nameof(width));
            // Argument.IsPositive(height, nameof(height));
            // Argument.IsInRange(quality, 1, 100, nameof(quality));

            var mjpegEncoder = new MJpegVideoEncoder(width, height, encoder);
            var videoStream = writer.AddEncodingVideoStream(mjpegEncoder, true, width, height);

            return new VideoFrameWriter(videoStream);
        }
    }

    public sealed class VideoFrameWriter
    {
        internal VideoFrameWriter(IAviVideoStream writer)
        {
            _Writer = writer;
            _Cache = new InteropBitmaps.MemoryBitmap<InteropBitmaps.Pixel.BGRA32>(writer.Width, writer.Height, InteropBitmaps.Pixel.BGRA32.Format);
        }

        private readonly IAviVideoStream _Writer;
        private readonly InteropBitmaps.MemoryBitmap<InteropBitmaps.Pixel.BGRA32> _Cache;

        public void WriteFrame(InteropBitmaps.SpanBitmap bmp)
        {
            _Cache.AsTypeless().SetPixels(0, 0, bmp);

            if (_Cache.TryGetBuffer(out var segment))
            {
                _Writer.WriteFrame(true, segment.Array, segment.Offset, segment.Count);
            }            
        }
    }
}
