using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Platform;

using AVALONIABITMAP = Avalonia.Media.Imaging.Bitmap;
using WRITEABLEBITMAP = Avalonia.Media.Imaging.WriteableBitmap;

namespace InteropTypes.Graphics.Backends
{
    using INTERPIXFMT = Bitmaps.PixelFormat;
    
    partial class _AvaloniaResourceCache
    {
        #region data

        private static readonly INTERPIXFMT _BindablePixelFormat = Bitmaps.Pixel.BGRA32.Format;

        private readonly Dictionary<Object, AVALONIABITMAP> _ImagesCache = new Dictionary<Object, AVALONIABITMAP>();

        #endregion

        #region API

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

        #endregion

        #region core

        private static AVALONIABITMAP _UpdateDynamicBitmap(object imageKey, AVALONIABITMAP image)
        {
            if (!(image is WRITEABLEBITMAP dstWriteable)) return image;

            if (imageKey is Bitmaps.BindableBitmap srcBindable)
            {
                srcBindable.UpdateFromQueue(_BindablePixelFormat);
                return _CreateFromSource(srcBindable, dstWriteable);
            }

            return image;
        }

        private static AVALONIABITMAP _CreateFromSource(Bitmaps.SpanBitmap.ISource srcBindable, WRITEABLEBITMAP dstBmp = null)
        {
            if (dstBmp != null)
            {
                _Implementation.CopyPixels(srcBindable.AsSpanBitmap(), dstBmp);
                return dstBmp;
            }

            return _Implementation.CreateAvaloniaBitmap(srcBindable.AsSpanBitmap());
        }

        private static AVALONIABITMAP _CreateDynamicBitmap(object imageKey)
        {
            if (imageKey is Bitmaps.BindableBitmap srcBindable)
            {
                srcBindable.UpdateFromQueue(_BindablePixelFormat);
                return _CreateFromSource(srcBindable);
            }

            return null;
        }
        
        private static AVALONIABITMAP _CreateStaticBitmap(object imageKey)
        {
            if (imageKey is AVALONIABITMAP abitmap) return abitmap;

            if (imageKey is Bitmaps.SpanBitmap.ISource ibmp)
            {
                return _CreateFromSource(ibmp);
            }

            if (imageKey is string preImagePath && preImagePath.StartsWith("avares://"))
            {
                imageKey = new Uri(preImagePath);
            }

            if (imageKey is Uri uri)
            {
                if (uri.Scheme == "avares") return new AVALONIABITMAP(AssetLoader.Open(uri));
                else if (uri.IsFile) imageKey = new System.IO.FileInfo(uri.LocalPath);
                else throw new NotSupportedException(uri.ToString());
            }

            if (imageKey is System.IO.FileInfo finfo)
            {
                if (_TryCreateFromStreamFactory(finfo.OpenRead, out var bmp)) return bmp;
            }

            if (imageKey is Microsoft.Extensions.FileProviders.IFileInfo xinfo)
            {
                if (_TryCreateFromStreamFactory(xinfo.CreateReadStream, out var bmp)) return bmp;
            }            

            #if ANDROID

            if (imageKey is string imagePath)
            {
                var allFiles = System.IO.Directory.GetFiles(System.AppContext.BaseDirectory, "*", System.IO.SearchOption.AllDirectories);                             

                // https://github.com/xamarin/xamarin-android/issues/5052

                imagePath = imagePath.Replace("\\", "/");
                imagePath = imagePath.Replace("Assets", "assets");

                if (!System.IO.Path.IsPathRooted(imagePath))
                {
                    var mgr = 

                    imagePath = System.IO.Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, imagePath);

                    System.Diagnostics.Debug.Assert(System.IO.Path.IsPathRooted(imagePath));
                }                

                try
                {
                    System.Diagnostics.Debug.Assert(System.IO.File.Exists(imagePath), "path not found");
                    return new AVALONIABITMAP(imagePath);
                }
                catch(Exception ex)
                {
                    throw;
                }                
            }

            #endif

            if (imageKey is string url)
            {
                // url = System.IO.Path.Combine(AppContext.BaseDirectory, url);

                try { return new AVALONIABITMAP(url); }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    System.Diagnostics.Trace.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }

            // default
            return _TryCreateFromStreamFactory(() => Graphics.Drawing.ImageSource.TryOpenRead(imageKey), out var bmp2)
                ? bmp2
                : null;
        }

        private static bool _TryCreateFromStreamFactory(Func<System.IO.Stream> streamFactory, out AVALONIABITMAP bitmap)
        {
            bitmap = null;
            if (streamFactory == null) return false;

            using var s = streamFactory.Invoke();

            if (s == null) return false;

            bitmap = new AVALONIABITMAP(s);
            return true;
        }        

        #endregion
    }
}
