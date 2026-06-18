using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace InteropTypes.Graphics
{
    // rename to ManagedBitmap


    /// <summary>
    /// This is a special kind of bitmap that is stored in memory in a way that can be used to access the pixel data,
    /// or as a BMP file that can be opened as a Stream
    /// </summary>
    /// <remarks>
    /// Usage:<br/>
    /// 1. Create a WindowsBitmap with the desired dimensions and BPP<br/>
    /// 2. Fill Rows using UseRow
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{Width}x{Height}x{BytesPerPixel} {Compression} ... {_Data.Length}")]
    public class WindowsBitmap
    {
        #region lifecycle
        public WindowsBitmap(int width, int height, int bytesPerPixel)
        {
            _Data = InitHeader(width, height, bytesPerPixel);
        }

        private static byte[] InitHeader(int width, int height, int bytesPerPixel)
        {
            var compression = WindowsBitmapCompression.BI_RGB;            

            var hdrSize = BITMAPHEADER.GetSize() + BITMAPINFOHEADER.GetSize();

            switch (compression)
            {
                case WindowsBitmapCompression.BI_RGB: break;
                case WindowsBitmapCompression.BI_BITFIELDS: hdrSize += BITMAPCOLORBITSMASK.GetSize(); break;
                case WindowsBitmapCompression.BI_ALPHABITFIELDS: hdrSize += BITMAPCOLORBITSMASK.GetSize(); break;
                default:  throw new NotImplementedException();
            }

            var imgSize = WindowsBitmapPixels.CalcBufferSize(width,height,bytesPerPixel);            

            var bih = new BITMAPINFOHEADER
            {
                biSize = (uint)BITMAPINFOHEADER.GetSize(),
                biWidth = width,
                biHeight = -height, // Negative to indicate a top-down DIB
                biPlanes = 1,
                biBitCount = (ushort)(bytesPerPixel * 8),
                biCompression = (uint)compression,
                biSizeImage = (uint)imgSize,
                biXPelsPerMeter = 0x2E23,
                biYPelsPerMeter = 0x2E23,
                biClrUsed = 0,
                biClrImportant = 0
            };
            
            var totalLength = hdrSize + imgSize;
            var bmph = new BITMAPHEADER
            {
                Header0 = 0x42, // B
                Header1 = 0x4D, // M
                Size = (uint)totalLength,
                Reserved0 = 0,
                Reserved1 = 0,
                OffsetToPixels = (uint)hdrSize,
            };

            var bytes = new byte[totalLength];
            var writer = bytes.AsSpan();
            bmph.CopyTo(writer); writer = writer.Slice(BITMAPHEADER.GetSize());
            bih.CopyTo(writer); writer = writer.Slice(BITMAPINFOHEADER.GetSize());

            switch(compression)
            {
                case WindowsBitmapCompression.BI_BITFIELDS:
                    BITMAPCOLORBITSMASK.Argb.CopyTo(writer);
                    writer = writer.Slice(BITMAPCOLORBITSMASK.GetSize());
                    break;

                case WindowsBitmapCompression.BI_ALPHABITFIELDS:
                    BITMAPCOLORBITSMASK.Argb.CopyTo(writer);
                    writer = writer.Slice(BITMAPCOLORBITSMASK.GetSize());
                    break;

            }

            return bytes;
        }

        #endregion

        #region data

        private readonly byte[] _Data;

        #endregion

        #region properties

        public int Width => GetDib().biWidth;
        public int Height => Math.Abs( GetDib().biHeight );
        public int BytesPerPixel => GetDib().biBitCount / 8;

        internal WindowsBitmapCompression Compression => (WindowsBitmapCompression)GetDib().biCompression;

        #endregion

        #region API

        public int GetValueHashCode()
        {
            if (_Data == null || _Data.Length == 0) return 0;
            return _Data.Aggregate(0, (h, b) => h = (h * 17) ^ b.GetHashCode());
        }

        /// <summary>
        /// Checks if the contents represent a JPG or PNG and returns an array representing the encoded image.
        /// </summary>
        /// <param name="embeddedImage">An array representing an encoded image.</param>
        /// <returns>true if the contents is a JPG or PNG image.</returns>
        public bool TryGetEmbeddedImage(out ArraySegment<byte> embeddedImage)
        {
            var dib = GetDib();            

            switch ((WindowsBitmapCompression)dib.biCompression)
            {
                case WindowsBitmapCompression.BI_JPEG: break;
                case WindowsBitmapCompression.BI_PNG: break;                
                default:
                    embeddedImage = ArraySegment<byte>.Empty;
                    return false;
            }

            var imageOffset = (int)GetHeader().OffsetToPixels;
            embeddedImage = new ArraySegment<byte>(_Data).Slice(imageOffset);
            return true;
        }

        /// <summary>
        /// Checks if the contents represents a raw, uncompressed pixel buffer and calls a delegate with the pixels accessor
        /// </summary>
        /// <param name="readPixelsAction">A delegate that performs operations over the pixels of the raw buffer</param>
        /// <returns>true if success</returns>
        public bool TryGetPixels(WindowsBitmapPixels.GetPixelsDelegate readPixelsAction)
        {
            var dib = GetDib();

            if (!WindowsBitmapPixels.IsCompatible((WindowsBitmapCompression)dib.biCompression)) return false;

            var pixelsOffset = (int)GetHeader().OffsetToPixels;
            var pixelsSpan = _Data.AsSpan(pixelsOffset);

            var api = new WindowsBitmapPixels(dib, pixelsSpan);

            readPixelsAction(api);

            return true;
        }

        /// <summary>
        /// Checks if the contents represents a raw, uncompressed pixel buffer and calls a delegate that converts the pixels to another type.
        /// </summary>
        /// <typeparam name="TResult">The result type from the pixels conversion.</typeparam>
        /// <param name="convertPixelsFunction">A delegate that converts the pixels to another type.</param>
        /// <param name="result">The resulting value.</param>
        /// <returns>true if success</returns>
        public bool TryConvertPixels<TResult>(WindowsBitmapPixels.ConvertPixelsDelegate<TResult> convertPixelsFunction, out TResult result)
        {
            result = default;

            var dib = GetDib();

            if (!WindowsBitmapPixels.IsCompatible((WindowsBitmapCompression)dib.biCompression)) return false;

            var pixelsOffset = (int)GetHeader().OffsetToPixels;
            var pixelsSpan = _Data.AsSpan(pixelsOffset);

            var api = new WindowsBitmapPixels(dib, pixelsSpan);

            result = convertPixelsFunction(api);
            return true;
        }        

        private static int _PadTo4(int width)
        {
            int remainder = width % 4;
            return remainder == 0
                ? width
                : width + (4 - remainder);
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

            System.Diagnostics.Debug.Assert(header.Header0 == 0x42 && header.Header1 == 0x4D, "Not a BMP");

            return ref header;
        }

        private unsafe ref readonly BITMAPINFOHEADER GetDib()
        {
            var data = _Data.AsSpan(sizeof(BITMAPHEADER));
            ref readonly var dib = ref MemoryMarshal.Cast<byte, BITMAPINFOHEADER>(data)[0];

            #if DEBUG

            System.Diagnostics.Debug.Assert(dib.biPlanes == 1, $"Unsupported: biPlanes {dib.biPlanes}");

            var format = (WindowsBitmapCompression)dib.biCompression;

            switch(format)
            {
                case WindowsBitmapCompression.BI_RGB: break;
                case WindowsBitmapCompression.BI_BITFIELDS: break;
                case WindowsBitmapCompression.BI_ALPHABITFIELDS: break;
                default: System.Diagnostics.Debug.Fail($"Unsupported: biCompression {format}"); break;
            }
            #endif


            return ref dib;
        }

        #endregion
    }

    

    [System.Diagnostics.DebuggerDisplay("{Width}x{Height}x{BytesPerPixel}")]
    public readonly ref struct WindowsBitmapPixels
    {
        #region delegates

        public delegate void GetPixelsDelegate(WindowsBitmapPixels pixels);

        public delegate TResult ConvertPixelsDelegate<TResult>(WindowsBitmapPixels pixels);

        #endregion

        #region lifecycle

        internal static int CalcBufferSize(int width, int height, int bytesPerPixel)
        {
            return _PadTo4(width * bytesPerPixel) * height;
        }

        internal static bool IsCompatible(WindowsBitmapCompression cmp)
        {
            switch (cmp)
            {
                case WindowsBitmapCompression.BI_RGB: break;
                case WindowsBitmapCompression.BI_CMYK: break;
                case WindowsBitmapCompression.BI_BITFIELDS: break;
                case WindowsBitmapCompression.BI_ALPHABITFIELDS: break;
                default: return false;
            }

            return true;
        }

        internal WindowsBitmapPixels(BITMAPINFOHEADER hdr, Span<Byte> pixelBuffer)
        {
            Width = (int)hdr.biWidth;
            _SignedHeight = hdr.biHeight;
            Height = Math.Abs(_SignedHeight);

            BytesPerPixel = hdr.biBitCount / 8;
            _PixelBuffer = pixelBuffer;

            _ScanByteLength = Width * BytesPerPixel;

            Stride = _PadTo4(_ScanByteLength);
        }

        private static int _PadTo4(int width)
        {
            int remainder = width % 4;
            return remainder == 0
                ? width
                : width + (4 - remainder);
        }

        #endregion

        #region data

        private readonly Span<Byte> _PixelBuffer;

        /// <summary>
        /// If negative it means the buffer is a bottom-to-top buffer
        /// </summary>
        private readonly int _SignedHeight;

        public int Width { get; }
        public int Height { get; }
        public int BytesPerPixel { get; }
        public int Stride { get; }

        public readonly int _ScanByteLength;

        #endregion

        #region API

        public System.Numerics.Tensors.TensorSpan<byte> AsTensorSpan()
        {
            var bhwc = new nint[4];
            bhwc[3] = this.BytesPerPixel;
            bhwc[2] = this.Width;
            bhwc[1] = this.Height;
            bhwc[0] = 1;            

            var strides = new nint[4];
            bhwc[3] = this.BytesPerPixel;
            bhwc[2] = this.Stride;
            bhwc[1] = bhwc[2] * this.Height;
            bhwc[0] = bhwc[1] * 1;

            return new System.Numerics.Tensors.TensorSpan<byte>(_PixelBuffer, bhwc, strides);
        }

        public unsafe void LockBits(Action<IntPtr, int> pixelData)
        {
            fixed (byte* pointer = _PixelBuffer)
            {
                IntPtr intPtr = (IntPtr)pointer;
                pixelData.Invoke(intPtr, _PixelBuffer.Length);
            }
        }

        public ReadOnlySpan<byte> GetRowSpan(int y)
        {
            return UseRowSpan(y);
        }

        public Span<byte> UseRowSpan(int y)
        {
            if (_SignedHeight > 0)
            {
                y = _SignedHeight - y - 1;
            }            
            return _PixelBuffer.Slice(Stride * y, _ScanByteLength);
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
        /// <summary>
        /// The biSize can be used as a "header version" to determine V4 and V5 header versions.
        /// </summary>
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

    /// <summary>
    /// Values to be set to <see cref="BITMAPINFOHEADER.biCompression"/>
    /// </summary>
    /// <remarks>
    /// <see href="https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-wmf/4e588f70-bd92-4a6f-b77f-35d0feaf7a57"/>
    /// </remarks>
    enum WindowsBitmapCompression
    {
        BI_RGB = 0x0000,
        BI_RLE8 = 0x0001,
        BI_RLE4 = 0x0002,
        BI_BITFIELDS = 0x0003,
        BI_JPEG = 0x0004,
        BI_PNG = 0x0005,
        BI_ALPHABITFIELDS = 0x0006,
        BI_CMYK = 0x000B,
        BI_CMYKRLE8 = 0x000C,
        BI_CMYKRLE4 = 0x000D
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct BITMAPCOLORBITSMASK
    {
        public static BITMAPCOLORBITSMASK Argb = new BITMAPCOLORBITSMASK
        {
            Red = 0x000000FF,
            Green = 0x0000FF00,
            Blue = 0x00FF0000,
            Alpha = 0xFF000000,
        };

        public uint Red;
        public uint Green;
        public uint Blue;
        public uint Alpha;

        public static int GetSize() => Marshal.SizeOf<BITMAPCOLORBITSMASK>();
        public void CopyTo(Span<byte> dst)
        {
            MemoryMarshal.Cast<byte, BITMAPCOLORBITSMASK>(dst)[0] = this;
        }
    }
}
