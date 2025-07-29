using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#nullable disable

// https://github.com/dotnet/linker/issues/1989

// https://stackoverflow.com/questions/1439719/c-sharp-get-thumbnail-from-file-via-windows-api

namespace InteropTypes
{
    /// <summary>
    /// utility class to get shell preview thumbnails
    /// </summary>
    /// <remarks>
    /// requires <BuiltInComInteropSupport>true</BuiltInComInteropSupport>
    /// </remarks>
    public class ShellThumbnail : IDisposable
    {
        #region lifecycle

        ~ShellThumbnail()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!disposed)
            {
                if (alloc != null)
                {
                    Marshal.ReleaseComObject(alloc);
                }
                alloc = null;
                _thumbNail?.Dispose();
                _thumbNail = null;
                disposed = true;
            }
        }

        #endregion

        #region data

        private IMalloc alloc = null;
        private bool disposed = false;
        private Size _desiredSize = new Size(100, 100);
        private Bitmap _thumbNail;

        #endregion

        #region API

        public Bitmap ThumbNail => _thumbNail;

        public Size DesiredSize
        {
            get { return _desiredSize; }
            set { _desiredSize = value; }
        }
        private IMalloc Allocator
        {
            get
            {
                if (!disposed)
                {
                    if (alloc == null)
                    {
                        UnmanagedMethods.SHGetMalloc(ref alloc);
                    }
                }
                else
                {
                    Debug.Assert(false, "Object has been disposed.");
                }
                return alloc;
            }
        }

        public Bitmap GetThumbnail(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            if (!File.Exists(fileName) && !Directory.Exists(fileName))
            {
                throw new FileNotFoundException(string.Format("The file '{0}' does not exist", fileName), fileName);
            }

            if (_thumbNail != null)
            {
                _thumbNail.Dispose();
                _thumbNail = null;
            }

            IShellFolder folder = null;
            try
            {
                folder = getDesktopFolder;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (folder != null)
            {
                IntPtr pidlMain = IntPtr.Zero;
                try
                {
                    int cParsed = 0;
                    int pdwAttrib = 0;
                    string filePath = Path.GetDirectoryName(fileName);
                    folder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, filePath, ref cParsed, ref pidlMain, ref pdwAttrib);
                }
                catch (Exception ex)
                {
                    Marshal.ReleaseComObject(folder);
                    throw ex;
                }
                if (pidlMain != IntPtr.Zero)
                {
                    Guid iidShellFolder = new Guid("000214E6-0000-0000-C000-000000000046");
                    IShellFolder item = null;
                    try
                    {
                        folder.BindToObject(pidlMain, IntPtr.Zero, ref iidShellFolder, ref item);
                    }
                    catch (Exception ex)
                    {
                        Marshal.ReleaseComObject(folder);
                        Allocator.Free(pidlMain);
                        throw ex;
                    }
                    if (item != null)
                    {
                        IEnumIDList idEnum = null;
                        try
                        {
                            item.EnumObjects(IntPtr.Zero, (ESHCONTF.SHCONTF_FOLDERS | ESHCONTF.SHCONTF_NONFOLDERS), ref idEnum);
                        }
                        catch (Exception ex)
                        {
                            Marshal.ReleaseComObject(folder);
                            Allocator.Free(pidlMain);
                            throw ex;
                        }
                        if (idEnum != null)
                        {
                            int hRes = 0;
                            IntPtr pidl = IntPtr.Zero;
                            int fetched = 0;
                            bool complete = false;
                            while (!complete)
                            {
                                hRes = idEnum.Next(1, ref pidl, ref fetched);
                                if (hRes != 0)
                                {
                                    pidl = IntPtr.Zero;
                                    complete = true;
                                }
                                else
                                {
                                    if (_getThumbNail(fileName, pidl, item))
                                    {
                                        complete = true;
                                    }
                                }
                                if (pidl != IntPtr.Zero)
                                {
                                    Allocator.Free(pidl);
                                }
                            }
                            Marshal.ReleaseComObject(idEnum);
                        }
                        Marshal.ReleaseComObject(item);
                    }
                    Allocator.Free(pidlMain);
                }
                Marshal.ReleaseComObject(folder);
            }
            return ThumbNail;
        }

        private bool _getThumbNail(string file, IntPtr pidl, IShellFolder item)
        {
            IntPtr hBmp = IntPtr.Zero;
            IExtractImage extractImage = null;
            try
            {
                string pidlPath = PathFromPidl(pidl);
                if (Path.GetFileName(pidlPath).ToUpper().Equals(Path.GetFileName(file).ToUpper()))
                {
                    IUnknown iunk = null;
                    int prgf = 0;
                    Guid iidExtractImage = new Guid("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1");
                    item.GetUIObjectOf(IntPtr.Zero, 1, ref pidl, ref iidExtractImage, ref prgf, ref iunk);
                    extractImage = (IExtractImage)iunk;
                    if (extractImage != null)
                    {
                        Console.WriteLine("Got an IExtractImage object!");
                        SIZE sz = new SIZE();
                        sz.cx = DesiredSize.Width;
                        sz.cy = DesiredSize.Height;
                        StringBuilder location = new StringBuilder(260, 260);
                        int priority = 0;
                        int requestedColourDepth = 32;
                        EIEIFLAG flags = EIEIFLAG.IEIFLAG_ASPECT | EIEIFLAG.IEIFLAG_SCREEN;
                        int uFlags = (int)flags;
                        try
                        {
                            extractImage.GetLocation(location, location.Capacity, ref priority, ref sz, requestedColourDepth, ref uFlags);
                            extractImage.Extract(ref hBmp);
                        }
                        catch (System.Runtime.InteropServices.COMException ex)
                        {

                        }
                        if (hBmp != IntPtr.Zero)
                        {
                            _thumbNail = Bitmap.FromHbitmap(hBmp);
                        }
                        Marshal.ReleaseComObject(extractImage);
                        extractImage = null;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (hBmp != IntPtr.Zero)
                {
                    UnmanagedMethods.DeleteObject(hBmp);
                }
                if (extractImage != null)
                {
                    Marshal.ReleaseComObject(extractImage);
                }
                throw ex;
            }
        }

        private string PathFromPidl(IntPtr pidl)
        {
            StringBuilder path = new StringBuilder(260, 260);
            int result = UnmanagedMethods.SHGetPathFromIDList(pidl, path);
            if (result == 0)
            {
                return string.Empty;
            }
            else
            {
                return path.ToString();
            }
        }

        private IShellFolder getDesktopFolder
        {
            get
            {
                IShellFolder ppshf = null;
                int r = UnmanagedMethods.SHGetDesktopFolder(ref ppshf);
                return ppshf;
            }
        }

        #endregion

        #region nested types

        [Flags]
        private enum ESTRRET
        {
            STRRET_WSTR = 0,
            STRRET_OFFSET = 1,
            STRRET_CSTR = 2
        }

        [Flags]
        private enum ESHCONTF
        {
            SHCONTF_FOLDERS = 32,
            SHCONTF_NONFOLDERS = 64,
            SHCONTF_INCLUDEHIDDEN = 128,
        }

        [Flags]
        private enum ESHGDN
        {
            SHGDN_NORMAL = 0,
            SHGDN_INFOLDER = 1,
            SHGDN_FORADDRESSBAR = 16384,
            SHGDN_FORPARSING = 32768
        }

        [Flags]
        private enum ESFGAO
        {
            SFGAO_CANCOPY = 1,
            SFGAO_CANMOVE = 2,
            SFGAO_CANLINK = 4,
            SFGAO_CANRENAME = 16,
            SFGAO_CANDELETE = 32,
            SFGAO_HASPROPSHEET = 64,
            SFGAO_DROPTARGET = 256,
            SFGAO_CAPABILITYMASK = 375,
            SFGAO_LINK = 65536,
            SFGAO_SHARE = 131072,
            SFGAO_READONLY = 262144,
            SFGAO_GHOSTED = 524288,
            SFGAO_DISPLAYATTRMASK = 983040,
            SFGAO_FILESYSANCESTOR = 268435456,
            SFGAO_FOLDER = 536870912,
            SFGAO_FILESYSTEM = 1073741824,
            SFGAO_HASSUBFOLDER = -2147483648,
            SFGAO_CONTENTSMASK = -2147483648,
            SFGAO_VALIDATE = 16777216,
            SFGAO_REMOVABLE = 33554432,
            SFGAO_COMPRESSED = 67108864,
        }

        private enum EIEIFLAG
        {
            IEIFLAG_ASYNC = 1,
            IEIFLAG_CACHE = 2,
            IEIFLAG_ASPECT = 4,
            IEIFLAG_OFFLINE = 8,
            IEIFLAG_GLEAM = 16,
            IEIFLAG_SCREEN = 32,
            IEIFLAG_ORIGSIZE = 64,
            IEIFLAG_NOSTAMP = 128,
            IEIFLAG_NOBORDER = 256,
            IEIFLAG_QUALITY = 512
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0, CharSet = CharSet.Auto)]
        private struct STRRET_CSTR
        {
            public ESTRRET uType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 520)]
            public byte[] cStr;
        }

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
        private struct STRRET_ANY
        {
            [FieldOffset(0)]
            public ESTRRET uType;
            [FieldOffset(4)]
            public IntPtr pOLEString;
        }
        [StructLayoutAttribute(LayoutKind.Sequential)]
        private struct SIZE
        {
            public int cx;
            public int cy;
        }

        [ComImport(), Guid("00000000-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IUnknown
        {

            [PreserveSig()]
            IntPtr QueryInterface(ref Guid riid, ref IntPtr pVoid);

            [PreserveSig()]
            IntPtr AddRef();

            [PreserveSig()]
            IntPtr Release();
        }

        [ComImportAttribute()]
        [GuidAttribute("00000002-0000-0000-C000-000000000046")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IMalloc
        {

            [PreserveSig()]
            IntPtr Alloc(int cb);

            [PreserveSig()]
            IntPtr Realloc(IntPtr pv, int cb);

            [PreserveSig()]
            void Free(IntPtr pv);

            [PreserveSig()]
            int GetSize(IntPtr pv);

            [PreserveSig()]
            int DidAlloc(IntPtr pv);

            [PreserveSig()]
            void HeapMinimize();
        }

        [ComImportAttribute()]
        [GuidAttribute("000214F2-0000-0000-C000-000000000046")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IEnumIDList
        {

            [PreserveSig()]
            int Next(int celt, ref IntPtr rgelt, ref int pceltFetched);

            void Skip(int celt);

            void Reset();

            void Clone(ref IEnumIDList ppenum);
        }

        [ComImportAttribute()]
        [GuidAttribute("000214E6-0000-0000-C000-000000000046")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellFolder
        {

            void ParseDisplayName(IntPtr hwndOwner, IntPtr pbcReserved,
              [MarshalAs(UnmanagedType.LPWStr)] string lpszDisplayName,
              ref int pchEaten, ref IntPtr ppidl, ref int pdwAttributes);

            void EnumObjects(IntPtr hwndOwner,
              [MarshalAs(UnmanagedType.U4)] ESHCONTF grfFlags,
              ref IEnumIDList ppenumIDList);

            void BindToObject(IntPtr pidl, IntPtr pbcReserved, ref Guid riid,
              ref IShellFolder ppvOut);

            void BindToStorage(IntPtr pidl, IntPtr pbcReserved, ref Guid riid, IntPtr ppvObj);

            [PreserveSig()]
            int CompareIDs(IntPtr lParam, IntPtr pidl1, IntPtr pidl2);

            void CreateViewObject(IntPtr hwndOwner, ref Guid riid,
              IntPtr ppvOut);

            void GetAttributesOf(int cidl, IntPtr apidl,
              [MarshalAs(UnmanagedType.U4)] ref ESFGAO rgfInOut);

            void GetUIObjectOf(IntPtr hwndOwner, int cidl, ref IntPtr apidl, ref Guid riid, ref int prgfInOut, ref IUnknown ppvOut);

            void GetDisplayNameOf(IntPtr pidl,
              [MarshalAs(UnmanagedType.U4)] ESHGDN uFlags,
              ref STRRET_CSTR lpName);

            void SetNameOf(IntPtr hwndOwner, IntPtr pidl,
              [MarshalAs(UnmanagedType.LPWStr)] string lpszName,
              [MarshalAs(UnmanagedType.U4)] ESHCONTF uFlags,
              ref IntPtr ppidlOut);
        }
        [ComImportAttribute(), GuidAttribute("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IExtractImage
        {
            void GetLocation([Out(), MarshalAs(UnmanagedType.LPWStr)]
        StringBuilder pszPathBuffer, int cch, ref int pdwPriority, ref SIZE prgSize, int dwRecClrDepth, ref int pdwFlags);

            void Extract(ref IntPtr phBmpThumbnail);
        }

        private class UnmanagedMethods
        {

            [DllImport("shell32", CharSet = CharSet.Auto)]
            internal extern static int SHGetMalloc(ref IMalloc ppMalloc);

            [DllImport("shell32", CharSet = CharSet.Auto)]
            internal extern static int SHGetDesktopFolder(ref IShellFolder ppshf);

            [DllImport("shell32", CharSet = CharSet.Auto)]
            internal extern static int SHGetPathFromIDList(IntPtr pidl, StringBuilder pszPath);

            [DllImport("gdi32", CharSet = CharSet.Auto)]
            internal extern static int DeleteObject(IntPtr hObject);
        }

        #endregion

    }
}
