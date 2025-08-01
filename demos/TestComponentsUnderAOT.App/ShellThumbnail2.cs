using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace InteropTypes
{
    internal partial class ShellThumbnail2
    {
        // https://github.com/dotnet/runtime/issues/115753

        #region API

        public static Bitmap GetThumbnail(string path)
        {
            System.Threading.Thread.Sleep(1000);

            var guid = new Guid("BCC18B79-BA16-442F-80C4-8A59C30C463B"); // this is the GUID of IShellItemImageFactory

            UnmanagedMethods.SHCreateItemFromParsingName(path, default, guid, out var ppv);

            if (ppv == null) return null;

            ppv.GetImage(new SIZE(128, 128), SIIGBF.SIIGBF_BIGGERSIZEOK, out var hbitmap);

            var bmp = Bitmap.FromHbitmap(hbitmap);

            // Marshal.ReleaseComObject(ppv); // apparently not needed anymore?

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
