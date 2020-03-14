using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Codecs
{
    public class CodecException : System.IO.IOException { }


    public interface IBitmapDecoding
    {
        MemoryBitmap Read(System.IO.Stream s);
    }

    public interface IBitmapEncoding
    {
        void Write(System.IO.Stream s, CodecFormat format, SpanBitmap bmp);
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

        Emf,
        Wmf,
        Wbmp,
        Webp,
    }    
}
