using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Backends
{
    public class GDIFontGlyphFactory : IDisposable
    {
        // https://stackoverflow.com/questions/61584477/efficient-text-rendering-on-bitmap-in-c-sharp-with-system-drawing

        #region lifecycle

        public GDIFontGlyphFactory()
        {
            _FontSource = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 20);

            _Bitmap = new System.Drawing.Bitmap(128, 128, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            _Graphics = System.Drawing.Graphics.FromImage(_Bitmap);
        }

        public void Dispose()
        {
            System.Threading.Interlocked.Exchange(ref _Graphics, null)?.Dispose();
            System.Threading.Interlocked.Exchange(ref _Bitmap, null)?.Dispose();
            System.Threading.Interlocked.Exchange(ref _FontSource, null)?.Dispose();

        }
        #endregion

        #region data

        private System.Drawing.Font _FontSource;
        private System.Drawing.Bitmap _Bitmap;
        private System.Drawing.Graphics _Graphics;

        private readonly Dictionary<char,MemoryBitmap<Pixel.Alpha8>> _Glyphs = new Dictionary<char,MemoryBitmap<Pixel.Alpha8>>();

        public MemoryBitmap<Pixel.Alpha8> GetGlyph(char c)
        {
            if (_Glyphs.TryGetValue(c, out var bmp)) return bmp;

            _Graphics.Clear(System.Drawing.Color.Transparent);
            _Graphics.DrawString(c.ToString(), _FontSource, System.Drawing.Brushes.White, new System.Drawing.PointF(0, 0));            

            bmp = _Bitmap.ToMemoryBitmap(Pixel.Alpha8.Format).OfType<Pixel.Alpha8>();

            // bmp = bmp.CropVisible().Clone();

            _Glyphs[c] = bmp;

            return bmp;
        }

        #endregion
    }
}
