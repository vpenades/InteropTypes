#if NET8_0_OR_GREATER
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Versioning;

using InteropTypes.Graphics;

#nullable disable


namespace InteropTypes.Platforms.Win32
{
    [SupportedOSPlatform("windows6.0.6000")]
    internal static partial class FilePreview_GeneratedCOM
    {
        // https://github.com/dotnet/runtime/issues/115753

        #region public API

        public static System.IO.Stream GetStreamOrDefault(System.IO.FileInfo finfo, IO.FilePreviewOptions clientOptions = null)
        {
            try
            {
                using (var bmp = GetThumbnail(finfo.FullName, clientOptions))
                {
                    if (bmp == null) return null;

                    var mem = new System.IO.MemoryStream();

                    bmp.Save(mem, ImageFormat.Bmp);

                    return mem;
                }
            }
            catch (COMException ex)
            {
                return null;
            }
        }

        public static WindowsBitmap GetPreviewOrDefault(System.IO.FileInfo finfo, IO.FilePreviewOptions clientOptions = null)
        {
            try
            {
                using (var bmp = GetThumbnail(finfo.FullName, clientOptions))
                {
                    if (bmp == null) return null;

                    return bmp.GetWindowsBitmap(PixelFormat.Format24bppRgb);
                }
            }
            catch (COMException ex)
            {
                return null;
            }
        }

        #endregion

        #region API

        public static Bitmap GetThumbnail(string path, IO.FilePreviewOptions clientOptions = null)
        {
            clientOptions ??= IO.FilePreviewOptions._Default;            

            var guid = new Guid("BCC18B79-BA16-442F-80C4-8A59C30C463B"); // this is the GUID of IShellItemImageFactory

            UnmanagedMethods.SHCreateItemFromParsingName(path, default, guid, out var ppv);

            if (ppv == null) return null;

            var flags = SIIGBF.SIIGBF_RESIZETOFIT;

            if (clientOptions.CachedOnly) flags |= SIIGBF.SIIGBF_INCACHEONLY;
            if (clientOptions.AllowBigger) flags |= SIIGBF.SIIGBF_BIGGERSIZEOK;

            ppv.GetImage(new SIZE(clientOptions.Width, clientOptions.Height), flags, out var hbitmap);

            if (hbitmap == 0L || hbitmap == -1L) return null;

            var bmp = Bitmap.FromHbitmap(hbitmap);

            // do we require to release ppv?            

            return bmp;
        }

        #endregion

        #region nested types        

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            public int cx;
            public int cy;

            public SIZE(int cx, int cy)
            {
                this.cx = cx;
                this.cy = cy;
            }
        }

        [Flags]
        public enum SIIGBF
        {
            SIIGBF_RESIZETOFIT = 0x00,
            SIIGBF_BIGGERSIZEOK = 0x01,
            SIIGBF_MEMORYONLY = 0x02,
            SIIGBF_ICONONLY = 0x04,
            SIIGBF_THUMBNAILONLY = 0x08,
            SIIGBF_INCACHEONLY = 0x10,
        }

        // [ComImport]
        [GeneratedComInterface]
        [Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
        public partial interface IShellItemImageFactory
        {
            void GetImage(
            SIZE size,
            SIIGBF flags,
            out IntPtr phbm);
        }

        static partial class UnmanagedMethods
        {
            // https://pinvoke.net/default.aspx/Interfaces/IShellItem.html

            [LibraryImport("SHELL32")]
            [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
            public static unsafe partial void SHCreateItemFromParsingName(
                [MarshalAs(UnmanagedType.LPWStr)] string pszPath,
                IntPtr pbc,
                Guid riid,
                out IShellItemImageFactory ppv);
        }

        #endregion
    }
}

#endif