
// GENERATED CODE: using CodeGenUtils.t4

namespace InteropTypes.Graphics.Bitmaps.Processing
{    

        partial struct _PixelSampler<TPixel>
        {
            private Pixel.BGR24 _GetSampleBGR24()
            {
                var x = _X >> BITSHIFT;
                var y = _Y >> BITSHIFT;

                var a = _GetPixel<Pixel.BGR24>(x, y);
                var b = _GetPixel<Pixel.BGR24>(x + 1, y);
                var c = _GetPixel<Pixel.BGR24>(x, y + 1);
                var d = _GetPixel<Pixel.BGR24>(x + 1, y + 1);

                x = _X & BITMASK;
                y = _Y & BITMASK;

                return Pixel.BGR24.Lerp(a, b, c, d, (uint)x / 16, (uint)y / 16);
            }


            private Pixel.RGB24 _GetSampleRGB24()
            {
                var x = _X >> BITSHIFT;
                var y = _Y >> BITSHIFT;

                var a = _GetPixel<Pixel.RGB24>(x, y);
                var b = _GetPixel<Pixel.RGB24>(x + 1, y);
                var c = _GetPixel<Pixel.RGB24>(x, y + 1);
                var d = _GetPixel<Pixel.RGB24>(x + 1, y + 1);

                x = _X & BITMASK;
                y = _Y & BITMASK;

                return Pixel.RGB24.Lerp(a, b, c, d, (uint)x / 16, (uint)y / 16);
            }


            private Pixel.RGBA32 _GetSampleRGBA32()
            {
                var x = _X >> BITSHIFT;
                var y = _Y >> BITSHIFT;

                var a = _GetPixel<Pixel.RGBA32>(x, y);
                var b = _GetPixel<Pixel.RGBA32>(x + 1, y);
                var c = _GetPixel<Pixel.RGBA32>(x, y + 1);
                var d = _GetPixel<Pixel.RGBA32>(x + 1, y + 1);

                x = _X & BITMASK;
                y = _Y & BITMASK;

                return Pixel.RGBA32.Lerp(a, b, c, d, (uint)x / 16, (uint)y / 16);
            }


            private Pixel.BGRA32 _GetSampleBGRA32()
            {
                var x = _X >> BITSHIFT;
                var y = _Y >> BITSHIFT;

                var a = _GetPixel<Pixel.BGRA32>(x, y);
                var b = _GetPixel<Pixel.BGRA32>(x + 1, y);
                var c = _GetPixel<Pixel.BGRA32>(x, y + 1);
                var d = _GetPixel<Pixel.BGRA32>(x + 1, y + 1);

                x = _X & BITMASK;
                y = _Y & BITMASK;

                return Pixel.BGRA32.Lerp(a, b, c, d, (uint)x / 16, (uint)y / 16);
            }


            private Pixel.BGRP32 _GetSampleBGRP32()
            {
                var x = _X >> BITSHIFT;
                var y = _Y >> BITSHIFT;

                var a = _GetPixel<Pixel.BGRP32>(x, y);
                var b = _GetPixel<Pixel.BGRP32>(x + 1, y);
                var c = _GetPixel<Pixel.BGRP32>(x, y + 1);
                var d = _GetPixel<Pixel.BGRP32>(x + 1, y + 1);

                x = _X & BITMASK;
                y = _Y & BITMASK;

                return Pixel.BGRP32.Lerp(a, b, c, d, (uint)x / 16, (uint)y / 16);
            }


            private Pixel.RGBP32 _GetSampleRGBP32()
            {
                var x = _X >> BITSHIFT;
                var y = _Y >> BITSHIFT;

                var a = _GetPixel<Pixel.RGBP32>(x, y);
                var b = _GetPixel<Pixel.RGBP32>(x + 1, y);
                var c = _GetPixel<Pixel.RGBP32>(x, y + 1);
                var d = _GetPixel<Pixel.RGBP32>(x + 1, y + 1);

                x = _X & BITMASK;
                y = _Y & BITMASK;

                return Pixel.RGBP32.Lerp(a, b, c, d, (uint)x / 16, (uint)y / 16);
            }


        }
        
        }