using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

using SharpAvi.Output;

namespace InteropTypes.Codecs
{
    partial class MJpegAviFrameWriter
    {
        public static void SaveToAVI(string filePath, IEnumerable<MemoryBitmap> frames, decimal fps, IBitmapEncoder jpgEncoder)
        {
            MJpegAviFrameWriter writer = null;

            System.Drawing.Size frameSize = System.Drawing.Size.Empty;

            using (var avi = new AviWriter(filePath))
            {
                avi.FramesPerSecond = fps;

                foreach (var frame in frames)
                {
                    if (frameSize.IsEmpty) frameSize = frame.Size;
                    else if (frame.Size != frameSize) throw new ArgumentException("all frames must have the same size.",nameof(frames));

                    if (writer == null) writer = AddVideoFrameWriter(avi, frame.Width, frame.Height, jpgEncoder);
                    writer.WriteFrame(frame);
                }
            }
        }

        public static MJpegAviFrameWriter AddVideoFrameWriter(AviWriter aviWriter, int width, int height, IBitmapEncoder encoder)
        {
            // Argument.IsNotNull(writer, nameof(writer));
            // Argument.IsPositive(width, nameof(width));
            // Argument.IsPositive(height, nameof(height));
            // Argument.IsInRange(quality, 1, 100, nameof(quality));

            var mjpegEncoder = new _MJpegAviEncoder(width, height, encoder);
            var videoStream = aviWriter.AddEncodingVideoStream(mjpegEncoder, true, width, height);

            return new MJpegAviFrameWriter(videoStream);
        }
    }
}
