using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    struct _PixelBGRA32
    {
        #region data

        [System.Runtime.InteropServices.FieldOffset(0)]
        public UInt32 Packed;

        [System.Runtime.InteropServices.FieldOffset(0)]
        public Byte B;

        [System.Runtime.InteropServices.FieldOffset(1)]
        public Byte G;

        [System.Runtime.InteropServices.FieldOffset(2)]
        public Byte R;

        [System.Runtime.InteropServices.FieldOffset(3)]
        public Byte A;

        #endregion        
    }

    static class _PixelConverters
    {
        #region API

        public interface IConverter
        {
            void ConvertFrom(Span<_PixelBGRA32> dst, ReadOnlySpan<Byte> src);
            void ConvertTo(Span<Byte> dst, ReadOnlySpan<_PixelBGRA32> src);
        }

        public static IConverter GetConverter(PixelFormat fmt)
        {
            if (fmt == PixelFormat.Standard.BGRA32) return new _CvtBgra32();
            if (fmt == PixelFormat.Standard.ARGB32) return new _CvtArgb32();
            if (fmt == PixelFormat.Standard.RGBA32) return new _CvtRgba32();
            if (fmt == PixelFormat.Standard.RGB24) return new _CvtRgb24();
            if (fmt == PixelFormat.Standard.BGR24) return new _CvtBgr24();

            throw new NotImplementedException();
        }

        #endregion

        #region nested types

        private struct _CvtRgb24 : IConverter
        {
            const int SIZE = 3;

            public void ConvertFrom(Span<_PixelBGRA32> dst, ReadOnlySpan<byte> src)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    dst[i].R = src[0];
                    dst[i].G = src[1];
                    dst[i].B = src[2];
                    dst[i].A = 255;
                    src = src.Slice(SIZE);
                }
            }

            public void ConvertTo(Span<byte> dst, ReadOnlySpan<_PixelBGRA32> src)
            {
                for (int i = 0; i < src.Length; ++i)
                {
                    dst[0] = src[i].R;
                    dst[1] = src[i].G;
                    dst[2] = src[i].B;
                    dst = dst.Slice(SIZE);
                }
            }
        }

        private struct _CvtBgr24 : IConverter
        {
            const int SIZE = 3;

            public void ConvertFrom(Span<_PixelBGRA32> dst, ReadOnlySpan<byte> src)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    dst[i].B = src[0];
                    dst[i].G = src[1];
                    dst[i].R = src[2];
                    dst[i].A = 255;
                    src = src.Slice(SIZE);
                }
            }

            public void ConvertTo(Span<byte> dst, ReadOnlySpan<_PixelBGRA32> src)
            {
                for (int i = 0; i < src.Length; ++i)
                {
                    dst[0] = src[i].B;
                    dst[1] = src[i].G;
                    dst[2] = src[i].R;
                    dst = dst.Slice(SIZE);
                }
            }
        }

        private struct _CvtArgb32 : IConverter
        {
            const int SIZE = 4;

            public void ConvertFrom(Span<_PixelBGRA32> dst, ReadOnlySpan<byte> src)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    dst[i].A = src[0];
                    dst[i].R = src[1];
                    dst[i].G = src[2];
                    dst[i].B = src[3];
                    src = src.Slice(SIZE);
                }
            }

            public void ConvertTo(Span<byte> dst, ReadOnlySpan<_PixelBGRA32> src)
            {
                for (int i = 0; i < src.Length; ++i)
                {
                    dst[0] = src[i].A;
                    dst[1] = src[i].R;
                    dst[2] = src[i].G;
                    dst[3] = src[i].B;
                    dst = dst.Slice(SIZE);
                }
            }
        }

        private struct _CvtRgba32 : IConverter
        {
            const int SIZE = 4;

            public void ConvertFrom(Span<_PixelBGRA32> dst, ReadOnlySpan<byte> src)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    dst[i].R = src[0];
                    dst[i].G = src[1];
                    dst[i].B = src[2];
                    dst[i].A = src[3];
                    src = src.Slice(SIZE);
                }
            }

            public void ConvertTo(Span<byte> dst, ReadOnlySpan<_PixelBGRA32> src)
            {
                for (int i = 0; i < src.Length; ++i)
                {
                    dst[0] = src[i].R;
                    dst[1] = src[i].G;
                    dst[2] = src[i].B;
                    dst[3] = src[i].A;
                    dst = dst.Slice(SIZE);
                }
            }
        }

        private struct _CvtBgra32 : IConverter
        {
            const int SIZE = 4;

            public void ConvertFrom(Span<_PixelBGRA32> dst, ReadOnlySpan<byte> src)
            {
                var xsrc = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PixelBGRA32>(src);
                xsrc = xsrc.Slice(0, dst.Length);
                xsrc.CopyTo(dst);
            }

            public void ConvertTo(Span<byte> dst, ReadOnlySpan<_PixelBGRA32> src)
            {
                var xdst = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PixelBGRA32>(dst);
                src.CopyTo(xdst);
            }
        }

        #endregion
    }
}
