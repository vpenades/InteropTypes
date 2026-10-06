using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Versioning;

using InteropTypes.Graphics;

namespace InteropTypes.Platforms.Win32
{
    /// <summary>
    /// uses the new Net8 Generated COM bindings.
    /// </summary>
    [SupportedOSPlatform("windows6.0.6000")]
    internal static partial class FilePreview_GeneratedCOM
    {
        // https://github.com/dotnet/runtime/issues/115753

        #region public API

        public static System.IO.Stream GetStreamOrNull(FILEINFO finfo, IO.FilePreviewOptions clientOptions = null)
        {
            clientOptions ??= IO.FilePreviewOptions._Default;

            try
            {
                using (var bmp = GetNativeBmpOrNull(finfo.FullName, clientOptions))
                {
                    if (bmp == null) return null;

                    var mem = new System.IO.MemoryStream();

                    bmp.Save(mem, ImageFormat.Bmp);
                    mem.Position = 0;

                    return mem;
                }
            }
            catch (COMException ex)
            {
                return null;
            }
        }

        public static WindowsBitmap GetManagedBmpOrNull(FILEINFO finfo, IO.FilePreviewOptions clientOptions = null)
        {
            clientOptions ??= IO.FilePreviewOptions._Default;

            try
            {
                using (var bmp = GetNativeBmpOrNull(finfo.FullName, clientOptions))
                {
                    if (bmp == null) return null;

                    return bmp.GetWindowsBitmap(clientOptions.GetPixelFormat());
                }
            }
            catch (COMException ex)
            {
                return null;
            }
        }        

        public static unsafe Bitmap GetNativeBmpOrNull(string path, IO.FilePreviewOptions clientOptions = null)
        {
            clientOptions ??= IO.FilePreviewOptions._Default;            

            var guid = new Guid("BCC18B79-BA16-442F-80C4-8A59C30C463B"); // this is the GUID of IShellItemImageFactory

            UnmanagedMethods.SHCreateItemFromParsingName(path, IntPtr.Zero, guid, out var factory);
            if (factory == null) return null;

            try
            {

                var flags = SIIGBF.SIIGBF_RESIZETOFIT;
                if (clientOptions.IconOnly) flags |= SIIGBF.SIIGBF_ICONONLY;
                if (clientOptions.CachedOnly) flags |= SIIGBF.SIIGBF_INCACHEONLY;
                if (clientOptions.ThumbnailOnly) flags |= SIIGBF.SIIGBF_THUMBNAILONLY;
                if (clientOptions.AllowBigger) flags |= SIIGBF.SIIGBF_BIGGERSIZEOK;

                factory.GetImage(new SIZE(clientOptions.Width, clientOptions.Height), flags, out var hbitmap);

                if (hbitmap == 0L || hbitmap == -1L) return null;

                var bmp = Image.FromHbitmap(hbitmap);

                return bmp;
            }
            finally
            {
                /* in case we need its pointer
                if (ComWrappers.TryGetComInstance(factory, out var handle))
                {
                    Marshal.Release(handle);                    
                }*/

                // this does not work
                // var unmanaged = UniqueComInterfaceMarshaller<IShellItemImageFactory>.ConvertToUnmanaged(factory);
                // if (unmanaged != null) { UniqueComInterfaceMarshaller<IShellItemImageFactory>.Free(unmanaged); }
                
                if (factory is IDisposable disposable)
                {
                    // lots of docs suggest this to be available, but it does not.
                    disposable.Dispose();
                }
                else if ((object)factory is ComObject comObject)
                {
                    // IT WORKS.
                    // it's the only one that upon inspecting Factory, it sets _Released to true.
                    // Must be paired with UniqueComInterfaceMarshaller<> at SHCreateItemFromParsingName declaration.                    
                    comObject.FinalRelease();
                }
            }
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
            SIIGBF_MEMORYONLY = 0x02,       // Only if already in RAM (do not access disk)
            SIIGBF_ICONONLY = 0x04,         // Only use cached thumbnails
            SIIGBF_THUMBNAILONLY = 0x08,    // Use icon if thumbnail unavailable
            SIIGBF_INCACHEONLY = 0x10,      // Critical for bulk operations
            SIIGBF_CROPTOSQUARE = 0x20,
            SIIGBF_WIDETHUMBNAILS = 0x40,
            SIIGBF_SCALEUP = 0x100,
        }

        // https://learn.microsoft.com/en-us/dotnet/standard/native-interop/comwrappers-source-generation

        
        [GeneratedComInterface]
        [Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]        
        public partial interface IShellItemImageFactory
        {
            void GetImage(SIZE size, SIIGBF flags, out IntPtr phbm);
        }

        static partial class UnmanagedMethods
        {
            // https://pinvoke.net/default.aspx/Interfaces/IShellItem.html

            [LibraryImport("SHELL32", StringMarshalling = StringMarshalling.Utf16)]
            [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
            public static unsafe partial void SHCreateItemFromParsingName(
                [MarshalAs(UnmanagedType.LPWStr)]
                string pszPath,
                IntPtr pbc,
                Guid riid,
                [MarshalUsing(typeof(UniqueComInterfaceMarshaller<IShellItemImageFactory>))] // paired with ComObject.FinalRelease()
                out IShellItemImageFactory ppv);
        }

        #endregion
    }
}