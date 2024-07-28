using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

using SIZE = System.Drawing.Size;
using RECT = System.Drawing.Rectangle;

using XY = System.Numerics.Vector2;
using XFORM = System.Numerics.Matrix3x2;

namespace InteropTypes
{
    static partial class _Implementation
    {
        public static SixLabors.ImageSharp.Image<TPixel> CloneCropped<TPixel>(this SixLabors.ImageSharp.Image<TPixel> src, RECT srcRect)
            where TPixel : unmanaged, SixLabors.ImageSharp.PixelFormats.IPixel<TPixel>
        {
            var dst = new SixLabors.ImageSharp.Image<TPixel>(srcRect.Width, srcRect.Height);

            var p = new Point(-srcRect.X, -srcRect.Y);
            dst.Mutate(dc => dc.DrawImage(src, p, 1));
            return dst;

            // ImageSharp's Crop is very strict
            // var xRect = new SixLabors.ImageSharp.Rectangle(srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height);
            // return src.Clone(dc => dc.Crop(xRect));
        }

        public static Image<TPixel> CloneTransformed<TPixel>(this Image<TPixel> src, SIZE dstSize, XFORM xform, IResampler sampler = null)
            where TPixel : unmanaged, IPixel<TPixel>
        {            
            var srcRect = new SixLabors.ImageSharp.Rectangle(0, 0, src.Width, src.Height);
            var xform4 = new System.Numerics.Matrix4x4(xform);
            var dstSiz = new SixLabors.ImageSharp.Size(dstSize.Width, dstSize.Height);

            sampler ??= KnownResamplers.Bicubic;

            return src.Clone(dc => dc.Transform(srcRect, xform4, dstSiz, sampler));
        }       

        
        public static void DrawSpriteTo<TDstPixel, TSrcPixel>(Image<TDstPixel> target, XFORM spriteTransform, Image<TSrcPixel> sprite)
            where TDstPixel : unmanaged, IPixel<TDstPixel>
            where TSrcPixel : unmanaged, IPixel<TSrcPixel>
        {

            if (spriteTransform.M11 == 1 && spriteTransform.M12 == 0 && spriteTransform.M21 == 0 && spriteTransform.M22 == 1) // translation only
            {
                var x = (int)MathF.Round(spriteTransform.M31);
                var y = (int)MathF.Round(spriteTransform.M32);

                target.Mutate(dc => dc.DrawImage(sprite, new Point(x,y),1));
                return;
            }

            // var p = new Point((int)cmd.Transform.Translation.X, (int)cmd.Transform.Translation.Y);
            // target.Mutate(ctx => ctx.DrawImage(cmd.Image, p, 1));
            // continue;

            // https://github.com/SixLabors/ImageSharp/discussions/1764
            // https://github.com/SixLabors/ImageSharp.Drawing/discussions/124
            // but it seems it only works on vectors
            // var xform = System.Numerics.Matrix3x2.CreateScale(1, 1);
            // xform.Translation = -new System.Numerics.Vector2(cmd.X - x, cmd.Y - y);
            // target.Mutate(ctx => ctx.SetDrawingTransform(xform).DrawImage(cmd.Image, 1));

            /*
            var brush = new ImageBrush(sprite);
            var options = new DrawingOptions();
            options.Transform = spriteTransform;
            var rect = new RectangleF(0, 0, sprite.Width, sprite.Height);
            // target.Mutate(ctx => ctx.Fill(options, brush, rect));
            */

            // works for fonts and shapes, but NOT for images
            // target.Mutate(ctx => ctx.SetDrawingTransform(spriteTransform).DrawImage(sprite, 1));

            XFORM.Invert(spriteTransform, out var inverseTransform);

            void _process(Span<Vector4> dstSpan, Point value)
            {
                for (int i = 0; i < dstSpan.Length; i++)
                {
                    var v = new XY(i, value.Y);
                    v = XY.Transform(v, inverseTransform);

                    var xx = Math.Clamp((int)v.X, 0, sprite.Width - 1);
                    var yy = Math.Clamp((int)v.Y, 0, sprite.Height - 1);

                    var pixel = sprite.DangerousGetPixelRowMemory(yy).Span[xx].ToScaledVector4();

                    if (pixel.W == 0) continue;

                    dstSpan[i] = pixel;
                }
            }

            target.Mutate(ctx => ctx.ProcessPixelRowsAsVector4(_process));
        }
    }
}
