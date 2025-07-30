using Android.Content;
using Android.Media;
using Android.OS;
using Android.Provider;



using System.IO;
using System.Threading.Tasks;

namespace InteropTypes.Platforms.Android
{
    internal static class AndroidThumbnailService
    {
        public static async Task<System.IO.Stream?> GetStreamThumbnailAsync(string filePath, int width, int height)
        {            
            using var thumb = await GetAndroidThumbnailAsync(filePath, width, height);

            var ms = new MemoryStream();
            thumb.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, ms);
            ms.Position = 0;
            return ms;
        }

        public static async Task<Android.Graphics.Bitmap?> GetAndroidThumbnailAsync(string filePath, int width, int height)
        {
            var context = Android.App.Application.Context;
            var uri = Android.Net.Uri.Parse(filePath);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
            {
                // Android 10+: ContentResolver.LoadThumbnailAsync
                return = await context.ContentResolver
                    .LoadThumbnailAsync(uri, new Android.Util.Size(width, height), null)
                    .AsAsync<Android.Graphics.Bitmap>();
            }
            else
            {
                // Fallback for older releases
                long id = ContentUris.ParseId(uri);
                return MediaStore.Images.Thumbnails.GetThumbnail(context.ContentResolver, id, ThumbnailKind.MiniKind, null);
            }
        }
    }
}
