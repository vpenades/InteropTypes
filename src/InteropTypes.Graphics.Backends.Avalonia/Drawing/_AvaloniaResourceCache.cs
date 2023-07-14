using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Drawing;

using VENDOR = Avalonia;


namespace InteropTypes.Graphics.Backends
{
    using WPFBITMAPSOURCE = VENDOR.Media.Imaging.Bitmap;
    using WPFBITMAPIMAGE = VENDOR.Media.Imaging.Bitmap;

    internal class _WPFResourcesCache
    {
        #region data        

        private VENDOR.Media.Pen _TransparentPen;

        private readonly Dictionary<UInt32, VENDOR.Media.SolidColorBrush> _BrushesCache = new Dictionary<UInt32, VENDOR.Media.SolidColorBrush>();

        private readonly Dictionary<Object, WPFBITMAPSOURCE> _ImagesCache = new Dictionary<Object, WPFBITMAPSOURCE>();

        private readonly Dictionary<System.Drawing.RectangleF, VENDOR.Media.RectangleGeometry> _ClipCache = new Dictionary<System.Drawing.RectangleF, VENDOR.Media.RectangleGeometry>();

        private static readonly PixelFormat _BindablePixelFormat = Pixel.BGRA32.Format;

        #endregion

        #region API

        public VENDOR.Media.RectangleGeometry UseClipRectangle(System.Drawing.RectangleF rect)
        {
            if (_ClipCache.TryGetValue(rect, out var dstRect)) return dstRect;

            var srcRect = new VENDOR.Rect(rect.X, rect.Y, rect.Width, rect.Height);
            dstRect = new VENDOR.Media.RectangleGeometry(srcRect);
            _ClipCache[rect] = dstRect;
            return dstRect;
        }

        public VENDOR.Media.Pen UseTransparentPen()
        {
            if (_TransparentPen == null) _TransparentPen = new VENDOR.Media.Pen(VENDOR.Media.Brushes.Transparent, 1);
            return _TransparentPen;
        }

        public VENDOR.Media.SolidColorBrush UseBrush(ColorStyle color)
        {
            if (!color.IsVisible) return null;

            if (_BrushesCache.TryGetValue(color.Packed, out VENDOR.Media.SolidColorBrush brush)) return brush;

            brush = color.ToGDI().ToDeviceBrush();

            _BrushesCache[color.Packed] = brush;

            return brush;
        }

        public VENDOR.Media.Pen UsePen(float thickness, LineStyle style)
        {
            if (!style.IsVisible) return null;
            if (thickness <= 0) return null;

            var fill = UseBrush(style.FillColor); if (fill == null) return null;

            var pen = new VENDOR.Media.Pen(fill, thickness, null, style.StartCap.ToDeviceCapStyle());

            return pen;
        }

        public WPFBITMAPSOURCE UseImage(object imageKey)
        {
            // check if image is already in the cache
            if (_ImagesCache.TryGetValue(imageKey, out WPFBITMAPSOURCE oldImage))
            {
                // if the source image is dynamic we should update it
                var newImage = _UpdateDynamicBitmap(imageKey, oldImage);

                if (oldImage != newImage)
                {
                    if (newImage == null) _ImagesCache.Remove(imageKey);
                    else _ImagesCache[imageKey] = newImage;
                }

                return newImage;
            }

            // create the new image, either dynamic or static.

            var image = _CreateDynamicBitmap(imageKey) ?? _CreateStaticBitmap(imageKey);

            if (image != null) _ImagesCache[imageKey] = image;

            return image;
        }

        private static WPFBITMAPSOURCE _UpdateDynamicBitmap(object imageKey, WPFBITMAPSOURCE image)
        {
            if (!(image is VENDOR.Media.Imaging.Bitmap dstWriteable)) return image;

            if (imageKey is BindableBitmap srcBindable)
            {
                srcBindable.UpdateFromQueue(_BindablePixelFormat);
                return _CreateFromSource(srcBindable, dstWriteable);
            }

            return image;

        }

        private static WPFBITMAPSOURCE _CreateDynamicBitmap(object imageKey)
        {
            if (imageKey is BindableBitmap srcBindable)
            {
                srcBindable.UpdateFromQueue(_BindablePixelFormat);                
                return _CreateFromSource(srcBindable);

            }

            return null;
        }

        private static WPFBITMAPSOURCE _CreateStaticBitmap(object imageKey)
        {
            if (imageKey is System.IO.FileInfo finfo) { imageKey = finfo.FullName; }

            if (imageKey is string imagePath)
            {
                return new WPFBITMAPIMAGE(imagePath);
            }

            if (imageKey is SpanBitmap.ISource ibmp)
            {
                return _CreateFromSource(ibmp);
            }

            return null;
        }

        
        private static WPFBITMAPSOURCE _CreateFromSource(SpanBitmap.ISource srcBindable, WPFBITMAPSOURCE dstBmp = null)
        {
            return null;

            /*
            var srcBmp = srcBindable.AsSpanBitmap();
            if (srcBmp.IsEmpty) return dstBmp;

            srcBmp.WithAvalon().CopyTo(ref dstBmp);
            return dstBmp;
            */
        }

        #endregion
    }
}
