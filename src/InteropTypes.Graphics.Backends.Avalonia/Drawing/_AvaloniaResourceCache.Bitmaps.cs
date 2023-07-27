using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Platform;

using VENDOR = Avalonia;

namespace InteropTypes.Graphics.Backends
{
    using INTERPIXFMT = Bitmaps.PixelFormat;

    using AVALONIABITMAP = VENDOR.Media.Imaging.Bitmap;

    partial class _AvaloniaResourceCache
    {
        private readonly Dictionary<Object, AVALONIABITMAP> _ImagesCache = new Dictionary<Object, AVALONIABITMAP>();

        private static readonly INTERPIXFMT _BindablePixelFormat = Bitmaps.Pixel.BGRA32.Format;

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

            if (imageKey is Bitmaps.BindableBitmap srcBindable)
            {
                srcBindable.UpdateFromQueue(_BindablePixelFormat);
                return _CreateFromSource(srcBindable, dstWriteable);
            }

            return image;

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

            if (imageKey is System.IO.FileInfo finfo) { imageKey = finfo.FullName; }

            if (imageKey is Microsoft.Extensions.FileProviders.IFileInfo xinfo)
            {
                using (var s = xinfo.CreateReadStream())
                {
                    if (s != null)
                    {
                        try { return new AVALONIABITMAP(s); }
                        catch { throw; }
                    }
                }
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

            using (var s = Graphics.Drawing.ImageSource.TryOpenRead(imageKey))
            {
                if (s != null)
                {
                    try { return new AVALONIABITMAP(s); }
                    catch { throw; }
                }
            }

            if (imageKey is Bitmaps.SpanBitmap.ISource ibmp)
            {
                return _CreateFromSource(ibmp);
            }

            return null;
        }


        private static AVALONIABITMAP _CreateFromSource(Bitmaps.SpanBitmap.ISource srcBindable, VENDOR.Media.Imaging.WriteableBitmap dstBmp = null)
        {
            if (dstBmp != null)
            {
                _Implementation.CopyPixels(srcBindable.AsSpanBitmap(), dstBmp);
                return dstBmp;
            }

            return _Implementation.CreateAvaloniaBitmap(srcBindable.AsSpanBitmap());
        }
    }
}
