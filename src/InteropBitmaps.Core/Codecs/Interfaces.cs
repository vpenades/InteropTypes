using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Codecs
{
    public class CodecException : System.IO.IOException { }


    public interface IBitmapDecoding
    {
        bool TryRead(System.IO.Stream s, out MemoryBitmap bitmap);
    }

    public interface IBitmapEncoding
    {
        bool TryWrite(System.IO.Stream s, CodecFormat format, SpanBitmap bmp);
    }
    
    public enum CodecFormat
    {        
        Bmp,
        Gif,
        Icon,
        Jpeg,
        Png,
        
        Pkm,
        Ktx,
        Astc,
        Dng,
        Heif,
        Tiff,
        Tga,

        Emf,
        Wmf,
        Wbmp,
        Webp,

        Psd,


    }    
}
