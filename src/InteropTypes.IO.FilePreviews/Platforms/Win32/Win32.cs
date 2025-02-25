using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Windows.Win32
{    
    internal partial class PInvoke
    {
        // https://learn.microsoft.com/es-es/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitemimagefactory-getimage

        [SupportedOSPlatform("windows5.1.2600")]
        internal static unsafe nuint SHGetFileInfo(string pszPath, Storage.FileSystem.FILE_FLAGS_AND_ATTRIBUTES dwFileAttributes, ref UI.Shell.SHFILEINFOW sfi, UI.Shell.SHGFI_FLAGS uFlags)
        {
            fixed (UI.Shell.SHFILEINFOW* psfi = &sfi)
            fixed (char* pszPathLocal = pszPath)
            {
                return SHGetFileInfo(pszPathLocal, dwFileAttributes, psfi, (uint)Unsafe.SizeOf<UI.Shell.SHFILEINFOW>(), uFlags);
            }
        }

        [SupportedOSPlatform("windows6.0.6000")]
        public static Windows.Win32.UI.Shell.IShellItemImageFactory GetShellItemImageFactory(string filePath)
        {
            var guid = new Guid("BCC18B79-BA16-442F-80C4-8A59C30C463B"); // this is the GUID of IShellItemImageFactory

            Windows.Win32.System.Com.IBindCtx bindContext = null;

            if (Windows.Win32.PInvoke.SHCreateItemFromParsingName(filePath, bindContext, guid, out var context).Failed) return null;

            return context as Windows.Win32.UI.Shell.IShellItemImageFactory;
        }

        internal static unsafe DeleteObjectSafeHandle GetImage(this UI.Shell.IShellItemImageFactory factory, Foundation.SIZE size, UI.Shell.SIIGBF flags)
        {
            // COMException 0x8004B200 < expected

            factory.GetImage(size, flags, out var safeHandle);
            if (safeHandle.IsInvalid)
            {
                safeHandle.Dispose();
                return null;
            }

            return safeHandle;
        }

        [SupportedOSPlatform("windows6.0.6000")]
        internal static unsafe Graphics.Gdi.HBITMAP GetThumbnail(this UI.Shell.IThumbnailProvider provider, uint cx, out UI.Shell.WTS_ALPHATYPE alpha)
        {
            var hbmp = default(Windows.Win32.Graphics.Gdi.HBITMAP);
            Graphics.Gdi.HBITMAP* pbmp = &hbmp;

            fixed (UI.Shell.WTS_ALPHATYPE* palpha = &alpha)
            {
                provider.GetThumbnail(cx, pbmp, palpha);
            }

            return hbmp;
        }
    }
}
