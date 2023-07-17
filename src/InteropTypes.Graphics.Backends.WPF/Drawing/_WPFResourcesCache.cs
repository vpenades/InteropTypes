using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Drawing;

using VENDOR = System.Windows;


namespace InteropTypes.Graphics.Backends
{
    using WPFBITMAPSOURCE = VENDOR.Media.Imaging.BitmapSource;
    using WPFBITMAPIMAGE = VENDOR.Media.Imaging.BitmapImage;
    using WPFBITMAPFRAME = VENDOR.Media.Imaging.BitmapFrame;
    using WPFCREATEOPTIONS = VENDOR.Media.Imaging.BitmapCreateOptions;
    using WPFCACHEOPTIONS = VENDOR.Media.Imaging.BitmapCacheOption;

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

            var pen = new VENDOR.Media.Pen(fill, thickness)
            {
                StartLineCap = style.StartCap.ToDeviceCapStyle(),
                EndLineCap = style.EndCap.ToDeviceCapStyle()
            };

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
            if (!(image is VENDOR.Media.Imaging.WriteableBitmap dstWriteable)) return image;

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
                Uri.TryCreate(imagePath, UriKind.RelativeOrAbsolute, out var uri);

                return new WPFBITMAPIMAGE(uri);
            }

            if (imageKey is Microsoft.Extensions.FileProviders.IFileInfo xinfo)
            {
                using(var s = xinfo.CreateReadStream())
                {
                    if (s != null) return WPFBITMAPFRAME.Create(s);
                }
            }

            using (var s = ImageSource.TryOpenRead(imageKey))
            {
                if (s != null) return WPFBITMAPFRAME.Create(s);
            }

            if (imageKey is SpanBitmap.ISource ibmp)
            {                
                return _CreateFromSource(ibmp);
            }

            return null;
        }

        private static WPFBITMAPSOURCE _CreateFromSource(SpanBitmap.ISource srcBindable, VENDOR.Media.Imaging.WriteableBitmap dstBmp = null)
        {            
            var srcBmp = srcBindable.AsSpanBitmap();
            if (srcBmp.IsEmpty) return dstBmp;

            srcBmp.WithWPF().CopyTo(ref dstBmp);
            return dstBmp;
        }

        #endregion
    }
}
