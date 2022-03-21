using System;
using System.Collections.Generic;
using System.Text;

using SharpAvi.Output;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Codecs
{
    public sealed partial class MJpegAviFrameWriter
    {
        #region lifecycle

        internal MJpegAviFrameWriter(IAviVideoStream writer)
        {
            _Writer = writer;
            _Cache = new MemoryBitmap<Pixel.BGRA32>(writer.Width, writer.Height, Pixel.BGRA32.Format);
        }

        #endregion

        #region data

        private readonly IAviVideoStream _Writer;
        private readonly MemoryBitmap<Pixel.BGRA32> _Cache;

        #endregion

        #region API

        public void WriteFrame(SpanBitmap bmp)
        {
            _Cache.AsTypeless().SetPixels(0, 0, bmp);

            if (_Cache.TryGetBuffer(out var segment))
            {
                _Writer.WriteFrame(true, segment.Array, segment.Offset, segment.Count);
            }            
        }

        #endregion
    }
}
