using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using NUnit.Framework;

using Xamarin.Essentials;

namespace InteropTypes.Droid.Tests
{
    static class UnitTestUtils
    {
        private static async Task<Java.IO.File> GetPicturesPath(string fileName)
        {
            fileName = System.IO.Path.Combine("TestResults", fileName);

            var r = await CheckAndRequestPermission<Permissions.StorageWrite>();
            if (r != PermissionStatus.Granted) return null;

            var sdCardPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            var filePath = new Java.IO.File(sdCardPath, fileName);

            filePath.ParentFile.Mkdirs();

            return filePath;
        }


        public static async Task AttachToCurrentTest(this TestContext context, string fileName, InteropBitmaps.MemoryBitmap bmp)
        {
            var filePath = await GetPicturesPath(fileName);

            if (filePath == null) return;

            bmp.Save(filePath.AbsolutePath, InteropBitmaps.Codecs.AndroidBitmapCodec.Default);

            TestContext.AddTestAttachment(filePath.AbsolutePath);
        }        

        public static async Task AttachToCurrentTest(this TestContext context, string fileName, Android.Graphics.Bitmap bmp)
        {
            var filePath = await GetPicturesPath(fileName);

            if (filePath == null) return;

            _SaveBitmap(filePath, bmp);

            TestContext.AddTestAttachment(filePath.AbsolutePath);
        }

        static void _SaveBitmap(Java.IO.File filePath, Android.Graphics.Bitmap bitmap)
        {
            Android.Graphics.Bitmap.CompressFormat cfmt = null;

            switch(System.IO.Path.GetExtension(filePath.AbsolutePath).ToLower())
            {
                case ".png": cfmt = Android.Graphics.Bitmap.CompressFormat.Png; break;
                case ".jpg": cfmt = Android.Graphics.Bitmap.CompressFormat.Jpeg; break;
                case ".webp": cfmt = Android.Graphics.Bitmap.CompressFormat.WebpLossless; break;
                default: throw new ArgumentException(nameof(filePath));
            }
            
            using var stream = System.IO.File.Create(filePath.AbsolutePath);
            bitmap.Compress(cfmt, 100, stream);
        }

        static async Task<PermissionStatus> CheckAndRequestPermission<TPermission>()
            where TPermission : Permissions.BasePermission, new()
        {
            var status = await Permissions.CheckStatusAsync<TPermission>();

            if (status == PermissionStatus.Granted) return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<TPermission>())
            {
                // Prompt the user with additional information as to why the permission is needed

                var page = Xamarin.Forms.Application.Current.MainPage;

                await page.DisplayAlert("Error",$"status", "Cancel");
            }

            status = await Permissions.RequestAsync<TPermission>();

            return status;
        }
    }
}