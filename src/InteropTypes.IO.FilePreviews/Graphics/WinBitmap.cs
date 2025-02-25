using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace InteropTypes.Graphics
{
    /// <summary>
    /// This is a special kind of bitmap that is stored in memory in a way that can be used to access the pixel data,
    /// or as a BMP file that can be opened as a Stream
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Width}x{Height}x{BytesPerPixel} ... {_Data.Length}")]
    public class WindowsBitmap
    {
        #region lifecycle
        public WindowsBitmap(int width, int height, int bpp)
        {
            _Data = Initialize(width, height, bpp);
        }

        private static byte[] Initialize(int width, int height, int bytesPerPixel)
        {
            var stride = _PadTo4(width * bytesPerPixel);

            var hdrSize = BITMAPHEADER.GetSize() + BITMAPINFOHEADER.GetSize();
            var imgSize = stride * Math.Abs(height);
            var length = hdrSize + imgSize;

            var bmph = new BITMAPHEADER
            {
                Header0 = 0x42, // B
                Header1 = 0x4D, // M
                Size = (uint)length,
                Reserved0 = 0,
                Reserved1 = 0,
                OffsetToPixels = (uint)hdrSize,
            };

            var bih = new BITMAPINFOHEADER
            {
                biSize = (uint)BITMAPINFOHEADER.GetSize(),
                biWidth = width,
                biHeight = -height, // Negative to indicate a top-down DIB
                biPlanes = 1,
                biBitCount = (ushort)(bytesPerPixel * 8),
                biCompression = 0, // BI_RGB, no compression
                biSizeImage = (uint)imgSize,
                biXPelsPerMeter = 0,
                biYPelsPerMeter = 0,
                biClrUsed = 0,
                biClrImportant = 0
            };

            var bytes = new byte[length];
            bmph.CopyTo(bytes);
            bih.CopyTo(bytes.AsSpan(BITMAPHEADER.GetSize()));

            return bytes;
        }

        #endregion

        #region data

        private readonly byte[] _Data;

        #endregion

        #region properties

        public int Width => GetDib().biWidth;
        public int Height => Math.Abs( GetDib().biHeight );
        public bool IsTopBottom => GetDib().biHeight < 0;
        public int BytesPerPixel => GetDib().biBitCount / 8;
        public int Stride => _PadTo4(Width * BytesPerPixel);

        #endregion

        #region API

        public ReadOnlySpan<byte> GetRowSpan(int y)
        {
            return GetRow(y);
        }

        public ArraySegment<byte> GetRow(int y)
        {
            var dib = GetDib();

            if (dib.biHeight > 0)
            {
                y = dib.biHeight - y - 1;
            }

            var w = dib.biWidth * dib.biBitCount / 8;
            return GetPixelsData().Slice(_PadTo4(w) * y, w);
        }

        private ArraySegment<byte> GetPixelsData()
        {
            var offset = (int)GetHeader().OffsetToPixels;
            var data = new ArraySegment<byte>(_Data);
            return data.Slice(offset);
        }

        private static int _PadTo4(int number)
        {
            int remainder = number % 4;
            if (remainder == 0)
            {
                return number;
            }
            return number + (4 - remainder);
        }        

        public void Save(string fileName)
        {
            System.IO.File.WriteAllBytes(fileName, _Data);
        }

        public void WriteToStream(System.IO.Stream s)
        {
            using (var m = OpenRead())
            {
                m.CopyTo(s);
            }
        }

        public System.IO.Stream OpenRead()
        {
            return new System.IO.MemoryStream(_Data, false);
        }

        private ref readonly BITMAPHEADER GetHeader()
        {
            ref readonly var header = ref MemoryMarshal.Cast<byte, BITMAPHEADER>(_Data)[0];
            if (header.Header0 != 0x42 || header.Header1 != 0x4D) throw new ArgumentException("Not a BMP");
            return ref header;
        }

        private unsafe ref readonly BITMAPINFOHEADER GetDib()
        {
            var data = _Data.AsSpan(sizeof(BITMAPHEADER));
            ref readonly var dib = ref MemoryMarshal.Cast<byte, BITMAPINFOHEADER>(data)[0];
            if (dib.biCompression != 0 || dib.biPlanes != 1) throw new ArgumentException("Not supported");
            return ref dib;
        }

        #endregion
    }



    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct BITMAPHEADER
    {
        public byte Header0;
        public byte Header1;
        // size of the complete file, including this header
        public uint Size;
        public ushort Reserved0;
        public ushort Reserved1;

        // offset to the pixels from the beginning of the file
        public uint OffsetToPixels;

        public static int GetSize() => Marshal.SizeOf<BITMAPHEADER>();
        public void CopyTo(Span<byte> dst)
        {
            MemoryMarshal.Cast<byte, BITMAPHEADER>(dst)[0] = this;
        }
    }

    /// <summary>
    /// Header of a DIB file
    /// </summary>
    /// <remarks>
    /// <see href="https://en.wikipedia.org/wiki/BMP_file_format">BMP_file_format</see>
    /// <see href="https://learn.microsoft.com/en-us/windows/win32/gdi/bitmap-header-types">bitmap-header-types</see>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct BITMAPINFOHEADER
    {
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;

        public static int GetSize() => Marshal.SizeOf<BITMAPINFOHEADER>();
        public void CopyTo(Span<byte> dst)
        {
            MemoryMarshal.Cast<byte, BITMAPINFOHEADER>(dst)[0] = this;
        }
    }
}
