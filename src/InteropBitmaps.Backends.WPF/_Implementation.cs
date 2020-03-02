using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WIC_WRITABLE = System.Windows.Media.Imaging.WriteableBitmap;
using WIC_READABLE = System.Windows.Media.Imaging.BitmapSource;
using WIC_ENCODER = System.Windows.Media.Imaging.BitmapEncoder;

namespace InteropBitmaps
{
    
    static class _Implementation
    {
        #region constants

        private const System.Windows.Media.Imaging.BitmapCreateOptions LOAD_CREATE = System.Windows.Media.Imaging.BitmapCreateOptions.PreservePixelFormat;
        private const System.Windows.Media.Imaging.BitmapCacheOption LOAD_CACHE = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;

        #endregion

        #region blit

        public static void SetPixels(WIC_WRITABLE bmp, int dstX, int dstY, SpanBitmap spanSrc)
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.imaging.writeablebitmap.adddirtyrect?view=netframework-4.8

            var pfmt = bmp.Format.ToInteropFormat();            

            try
            {
                // Reserve the back buffer for updates.
                bmp.TryLock(System.Windows.Duration.Forever);

                var binfo = new BitmapInfo(bmp.PixelWidth, bmp.PixelHeight, pfmt, bmp.BackBufferStride);
                var spanDst = new SpanBitmap(bmp.BackBuffer, binfo);

                spanDst.SetPixels(dstX, dstY, spanSrc);

                var changed = true;

                // Specify the area of the bitmap that changed.
                if (changed)
                {
                    var rect = new System.Windows.Int32Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight);
                    bmp.AddDirtyRect(rect);
                }
            }
            finally
            {
                // Release the back buffer and make it available for display.
                bmp.Unlock();
            }
        }

        #endregion

        #region IO

        public static WIC_ENCODER CreateEncoder(string fileExt)
        {
            if (fileExt.EndsWith("bmp", StringComparison.OrdinalIgnoreCase)) return new System.Windows.Media.Imaging.BmpBitmapEncoder();
            if (fileExt.EndsWith("jpg", StringComparison.OrdinalIgnoreCase)) return new System.Windows.Media.Imaging.JpegBitmapEncoder();
            if (fileExt.EndsWith("png", StringComparison.OrdinalIgnoreCase)) return new System.Windows.Media.Imaging.PngBitmapEncoder();
            if (fileExt.EndsWith("wmp", StringComparison.OrdinalIgnoreCase)) return new System.Windows.Media.Imaging.WmpBitmapEncoder();
            if (fileExt.EndsWith("tif", StringComparison.OrdinalIgnoreCase)) return new System.Windows.Media.Imaging.TiffBitmapEncoder();

            throw new NotImplementedException();
        }

        public static System.Windows.Media.Imaging.BitmapFrame Load(string filePath)
        {
            var uri = new Uri(filePath, UriKind.RelativeOrAbsolute);

            return Load(uri);
        }

        public static System.Windows.Media.Imaging.BitmapFrame Load(Uri uri)
        {
            return System.Windows.Media.Imaging.BitmapFrame.Create(uri, LOAD_CREATE, LOAD_CACHE);
        }

        public static System.Windows.Media.Imaging.BitmapFrame Load(System.IO.Stream stream)
        {
            return System.Windows.Media.Imaging.BitmapFrame.Create(stream, LOAD_CREATE, LOAD_CACHE);
        }

        public static void Save(string filePath, WIC_READABLE src)
        {
            var encoder = CreateEncoder(System.IO.Path.GetExtension(filePath));
            Save(filePath, src, encoder);
        }

        public static void Save(string filePath, WIC_READABLE src, WIC_ENCODER encoder)
        {            
            // TODO: check extensions match: encoder.CodecInfo;

            using (var s = System.IO.File.Create(filePath))
            {
                Save(s, src, encoder);
            }
        }

        public static void Save(System.IO.FileInfo dst, WIC_READABLE src, WIC_ENCODER encoder)
        {
            // TODO: check extensions match: encoder.CodecInfo;

            using (var s = dst.Create())
            {
                Save(s, src, encoder);
            }
        }

        public static void Save(System.IO.Stream dst, WIC_READABLE src, WIC_ENCODER encoder)
        {
            var frame = System.Windows.Media.Imaging.BitmapFrame.Create(src);

            encoder.Frames.Add(frame);
            encoder.Save(dst);
        }

        #endregion
    }
}
