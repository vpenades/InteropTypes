using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Drawing;

using WPFBITMAPSOURCE = System.Windows.Media.Imaging.BitmapSource;
using WPFBITMAPIMAGE = System.Windows.Media.Imaging.BitmapImage;
using WPFBITMAPFRAME = System.Windows.Media.Imaging.BitmapFrame;
using WPFCREATEOPTIONS = System.Windows.Media.Imaging.BitmapCreateOptions;
using WPFCACHEOPTIONS = System.Windows.Media.Imaging.BitmapCacheOption;

namespace InteropTypes.Graphics.Backends
{
    internal class _WPFResourcesCache
    {
        #region data        

        private System.Windows.Media.Pen _TransparentPen;

        private readonly Dictionary<UInt32, System.Windows.Media.SolidColorBrush> _BrushesCache = new Dictionary<UInt32, System.Windows.Media.SolidColorBrush>();

        private readonly Dictionary<Object, WPFBITMAPSOURCE> _ImagesCache = new Dictionary<Object, WPFBITMAPSOURCE>();

        #endregion

        #region API

        public System.Windows.Media.Pen UseTransparentPen()
        {
            if (_TransparentPen == null) _TransparentPen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Transparent, 1);
            return _TransparentPen;
        }

        public System.Windows.Media.SolidColorBrush UseBrush(ColorStyle color)
        {
            if (!color.IsVisible) return null;

            if (_BrushesCache.TryGetValue(color.Packed, out System.Windows.Media.SolidColorBrush brush)) return brush;

            brush = color.ToGDI().ToDeviceBrush();

            _BrushesCache[color.Packed] = brush;

            return brush;
        }

        public System.Windows.Media.Pen UsePen(float thickness, LineStyle style)
        {
            if (!style.IsVisible) return null;
            if (thickness <= 0) return null;

            var fill = UseBrush(style.FillColor); if (fill == null) return null;

            var pen = new System.Windows.Media.Pen(fill, thickness)
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
            if (!(image is WriteableBitmap dstWriteable)) return image;

            if (imageKey is BindableBitmap srcBindable)
            {
                srcBindable.UpdateFromQueue();
                return _CreateFromSource(srcBindable, dstWriteable);
            }

            return image;

        }

        private static WPFBITMAPSOURCE _CreateDynamicBitmap(object imageKey)
        {
            if (imageKey is BindableBitmap srcBindable)
            {
                srcBindable.UpdateFromQueue();
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

            if (imageKey is SpanBitmap.ISource ibmp)
            {                
                return _CreateFromSource(ibmp);
            }

            return null;
        }

        private static WPFBITMAPSOURCE _CreateFromSource(SpanBitmap.ISource srcBindable, WriteableBitmap dstBmp = null)
        {            
            var srcBmp = srcBindable.AsSpanBitmap();
            if (srcBmp.IsEmpty) return dstBmp;

            srcBmp.WithWPF().CopyTo(ref dstBmp);
            return dstBmp;
        }

        #endregion
    }
}
