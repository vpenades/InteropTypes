using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial struct PixelFormat
    {
        private static unsafe bool _TryIdentifyThirdPartyFormat<TPixel>(out PixelFormat fmt)
            where TPixel:unmanaged
        {
            var name = typeof(TPixel).FullName;

            fmt = default;

            switch (name)
            {
                // Unity (https://docs.unity3d.com/ScriptReference/TextureFormat.html)
                case "UnityEngine.TextureFormat.Alpha8": { fmt = Pixel.Alpha8.Format; break; }
                case "UnityEngine.TextureFormat.R8": { fmt = Pixel.Luminance8.Format; break; }
                case "UnityEngine.TextureFormat.R16": { fmt = Pixel.Luminance16.Format; break; }
                case "UnityEngine.TextureFormat.RFloat": { fmt = Pixel.Luminance32F.Format; break; }
                case "UnityEngine.TextureFormat.RGB24": { fmt = Pixel.RGB24.Format; break; }
                case "UnityEngine.TextureFormat.ARGB32": { fmt = Pixel.ARGB32.Format; break; }
                case "UnityEngine.TextureFormat.BGRA32": { fmt = Pixel.BGRA32.Format; break; }
                case "UnityEngine.TextureFormat.RGBAFloat": { fmt = Pixel.RGBA128F.Format; break; }

                // XNA & Monogame
                case "Microsoft.XNA.Framework.Color": { fmt = Pixel.RGBA32.Format; break; }

                // Ultraviolet
                case "Ultraviolet.Color": { fmt = Pixel.RGBA32.Format; break; }

                // Stride engine
                case "Stride.Core.Mathematics.Color": { fmt = Pixel.RGBA32.Format; break; }
                case "Stride.Core.Mathematics.ColorBGRA": { fmt = Pixel.BGRA32.Format; break; }
                case "Stride.Core.Mathematics.Color3": { fmt = Pixel.RGB96F.Format; break; }
                case "Stride.Core.Mathematics.Color4": { fmt = Pixel.RGBA128F.Format; break; }

                // ImageSharp
                case "SixLabors.ImageSharp.PixelFormats.A8": { fmt = Pixel.Alpha8.Format; break; }
                case "SixLabors.ImageSharp.PixelFormats.L8": { fmt = Pixel.Luminance8.Format; break; }
                case "SixLabors.ImageSharp.PixelFormats.L16": { fmt = Pixel.Luminance16.Format; break; }
                case "SixLabors.ImageSharp.PixelFormats.Bgr565": { fmt = Pixel.BGR565.Format; break; }
                case "SixLabors.ImageSharp.PixelFormats.Bgra5551": { fmt = Pixel.BGRA5551.Format; break; }
                case "SixLabors.ImageSharp.PixelFormats.Bgra4444": { fmt = Pixel.BGRA4444.Format; break; }
                case "SixLabors.ImageSharp.PixelFormats.Bgr24": { fmt = Pixel.BGR24.Format; break; }
                case "SixLabors.ImageSharp.PixelFormats.Rgb24": { fmt = Pixel.RGB24.Format; break; }
                case "SixLabors.ImageSharp.PixelFormats.Bgra32": { fmt = Pixel.BGRA32.Format; break; }
                case "SixLabors.ImageSharp.PixelFormats.Rgba32": { fmt = Pixel.RGBA32.Format; break; }
                case "SixLabors.ImageSharp.PixelFormats.Argb32": { fmt = Pixel.ARGB32.Format; break; }
                case "SixLabors.ImageSharp.PixelFormats.RgbaVector": { fmt = Pixel.RGBA128F.Format; break; }
            }

            return fmt.Code != 0 && sizeof(TPixel) == fmt.ByteCount;
        }
    }
}
