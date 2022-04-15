using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Tensors.Imaging
{
    readonly ref struct SpanTensorBitmap<TPixel>
        where TPixel: unmanaged
    {
        internal readonly int _Width;
        internal readonly int _Height;

        internal readonly Span<TPixel> _ChannelX;
        internal readonly Span<TPixel> _ChannelY;
        internal readonly Span<TPixel> _ChannelZ;
        internal readonly Span<TPixel> _ChannelW;

        internal readonly ColorEncoding _Encoding;        
    }
}
