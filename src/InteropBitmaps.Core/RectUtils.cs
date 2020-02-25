using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    static class RectUtils
    {
        public static (int x, int y, int w, int h) Clamp(this in (int x, int y, int w, int h) rect, in (int x, int y, int w, int h) clamp)
        {
            var x = rect.x;
            var y = rect.y;
            var w = rect.w;
            var h = rect.h;

            if (x < clamp.x) { w -= (clamp.x - x); x = clamp.x; }
            if (y < clamp.y) { h -= (clamp.y - y); y = clamp.y; }

            if (x + w > clamp.x + clamp.w) w -= (x + w) - (clamp.x + clamp.w);
            if (y + h > clamp.y + clamp.h) h -= (y + h) - (clamp.y + clamp.h);

            w = Math.Abs(w);
            h = Math.Abs(h);

            return (x, y, w, h);
        }

        public static ((int w,int h) siz, (int x, int y) dst, (int x, int y) src) OverlapClamp(this (int w, int h) siz, (int w, int h) dstSiz, (int x, int y) dstOff, (int w, int h) srcSiz, (int x, int y) srcOff)
        {
            // clamp against source
            var src = (srcOff.x, srcOff.y, siz.w, siz.h).Clamp((0, 0, srcSiz.w, srcSiz.h));

            // clamp against destination
            var dst = (dstOff.x, dstOff.y, src.w, src.h).Clamp((0, 0, dstSiz.w, dstSiz.h));

            return ((dst.w, dst.h), (dst.x, dst.y), (src.x, src.y));
        }
    }


    struct RectangleOverlap
    {
        private (int w, int h) _SourceSize;
        private (int w, int h) _TargetSize;

        public int Width;
        public int Height;

        public int SourceX;
        public int SourceY;

        public int TargetX;
        public int TargetY;

        public (int x, int y, int w, int h) SourceRect => (SourceX, SourceY, Width, Height);
        public (int x, int y, int w, int h) TargetRect => (TargetX, TargetY, Width, Height);

        public bool IsEmpty => Width <= 0 || Height <= 0;

        public void SetSourceSize((int width, int height) size) { _SourceSize = size; }
        public void SetTargetSize((int width, int height) size) { _TargetSize = size; }

        public void SetSourceSize(int width, int height) { _SourceSize = (width, height); }
        public void SetTargetSize(int width, int height) { _TargetSize = (width, height); }

        public void SetTransfer((int x,int y) tgt, (int x, int y) src, (int w, int h) siz)
        {
            var xsrc = (src.x, src.y, siz.w, siz.h).Clamp((0, 0, _SourceSize.w, _SourceSize.h));

            SourceX = xsrc.x;
            SourceY = xsrc.y;

            var (x, y, w, h) = (tgt.x, tgt.y, xsrc.w, xsrc.h).Clamp((0, 0, _TargetSize.w, _TargetSize.h));

            Width = w;
            Height = h;

            TargetX = x;
            TargetY = y;            
        }
    }
}
