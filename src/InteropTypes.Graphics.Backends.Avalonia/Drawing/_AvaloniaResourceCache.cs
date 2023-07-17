using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Drawing;

using VENDOR = Avalonia;


namespace InteropTypes.Graphics.Backends
{
    using AVALONIABITMAP = VENDOR.Media.Imaging.Bitmap;    

    internal class _WPFResourcesCache
    {
        #region data        

        private VENDOR.Media.Pen _TransparentPen;

        private readonly Dictionary<UInt32, VENDOR.Media.SolidColorBrush> _BrushesCache = new Dictionary<UInt32, VENDOR.Media.SolidColorBrush>();

        private readonly Dictionary<Object, AVALONIABITMAP> _ImagesCache = new Dictionary<Object, AVALONIABITMAP>();        

        private static readonly PixelFormat _BindablePixelFormat = Pixel.BGRA32.Format;

        #endregion

        #region API        

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

        public AVALONIABITMAP UseImage(object imageKey)
        {
            // check if image is already in the cache
            if (_ImagesCache.TryGetValue(imageKey, out AVALONIABITMAP oldImage))
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

        private static AVALONIABITMAP _UpdateDynamicBitmap(object imageKey, AVALONIABITMAP image)
        {
            if (!(image is VENDOR.Media.Imaging.WriteableBitmap dstWriteable)) return image;

            if (imageKey is BindableBitmap srcBindable)
            {
                srcBindable.UpdateFromQueue(_BindablePixelFormat);
                return _CreateFromSource(srcBindable, dstWriteable);
            }

            return image;

        }

        private static AVALONIABITMAP _CreateDynamicBitmap(object imageKey)
        {
            if (imageKey is BindableBitmap srcBindable)
            {
                srcBindable.UpdateFromQueue(_BindablePixelFormat);                
                return _CreateFromSource(srcBindable);

            }

            return null;
        }

        private static AVALONIABITMAP _CreateStaticBitmap(object imageKey)
        {
            if (imageKey is AVALONIABITMAP abitmap) return abitmap;

            if (imageKey is System.IO.FileInfo finfo) { imageKey = finfo.FullName; }            

            if (imageKey is string imagePath)
            {
                var allFiles = System.IO.Directory.GetFiles(System.AppContext.BaseDirectory, "*", SearchOption.AllDirectories);

                #if ANDROID

                // https://github.com/xamarin/xamarin-android/issues/5052

                imagePath = imagePath.Replace("\\", "/");
                imagePath = imagePath.Replace("Assets", "assets");

                if (!System.IO.Path.IsPathRooted(imagePath))
                {
                    var mgr = 

                    imagePath = System.IO.Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, imagePath);

                    System.Diagnostics.Debug.Assert(System.IO.Path.IsPathRooted(imagePath));
                }

                #endif

                try
                {
                    return new AVALONIABITMAP(imagePath);
                }
                catch(Exception ex)
                {
                    throw;
                }

                
            }

            if (imageKey is Microsoft.Extensions.FileProviders.IFileInfo xinfo)
            {
                using(var s = xinfo.CreateReadStream())
                {
                    if (s != null)
                    {
                        try { return new AVALONIABITMAP(s); }
                        catch { throw; }
                    }
                }                
            }

            using (var s = ImageSource.TryOpenRead(imageKey))
            {
                if (s != null)
                {
                    try { return new AVALONIABITMAP(s); }
                    catch { throw; }
                }
            }

            if (imageKey is SpanBitmap.ISource ibmp)
            {
                return _CreateFromSource(ibmp);
            }

            return null;
        }

        
        private static AVALONIABITMAP _CreateFromSource(SpanBitmap.ISource srcBindable, VENDOR.Media.Imaging.WriteableBitmap dstBmp = null)
        {
            if (dstBmp != null)
            {
                _Implementation.CopyPixels(srcBindable.AsSpanBitmap(), dstBmp);
                return dstBmp;
            }
         
            return _Implementation.CreateAvaloniaBitmap(srcBindable.AsSpanBitmap());         
        }

        #endregion
    }
}
