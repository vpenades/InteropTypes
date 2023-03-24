using System;
using System.Collections.Generic;
using System.Drawing;

using FFmpeg.AutoGen;

using InteropTypes.Graphics.Backends.Codecs;
using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Codecs
{
    /// <summary>
    /// copied directly from:
    /// <see href="https://github.com/Ruslan-B/FFmpeg.AutoGen/blob/master/FFmpeg.AutoGen.Example/VideoFrameConverter.cs"/> 
    /// </summary>
    public sealed unsafe class VideoFrameConverter : IDisposable
    {
        #region lifecycle

        public VideoFrameConverter(Size srcSize, AVPixelFormat srcPixelFormat, Size dstSize, AVPixelFormat dstPixelFormat)
        {
            _SrcSize = srcSize;

            _DstSize = dstSize;
            _DstFmt = dstPixelFormat;

            _pConvertContext = ffmpeg.sws_getContext(srcSize.Width,
                srcSize.Height,
                srcPixelFormat,
                dstSize.Width,
                dstSize.Height,
                dstPixelFormat,
                ffmpeg.SWS_FAST_BILINEAR,
                null,
                null,
                null);

            if (_pConvertContext == null)
                throw new ApplicationException("Could not initialize the conversion context.");                     
        }

        public void Dispose()
        {            
            ffmpeg.sws_freeContext(_pConvertContext);
        }

        #endregion

        #region data

        private readonly SwsContext* _pConvertContext;

        private Size _SrcSize;

        private Size _DstSize;
        private AVPixelFormat _DstFmt;

        #endregion

        #region API

        private void _VerifySource(int w, int h)
        {
            if (_SrcSize.Width != w || _SrcSize.Height != h) throw new InvalidOperationException("size mismatch");
        }

        private void _VerifyTarget(int w, int h)
        {
            if (_DstSize.Width != w || _DstSize.Height != h) throw new InvalidOperationException("size mismatch");
        }

        public unsafe void Convert(PointerBitmap src, PointerBitmap dst)
        {
            _VerifySource(src.Width, src.Height);
            _VerifyTarget(dst.Width, dst.Height);

            var srcData = new byte_ptrArray8 { [0] = (Byte*)src.Pointer.ToPointer() };
            var srcLine = new int_array8 { [0] = src.StepByteSize };

            var dstData = new byte_ptrArray8 { [0] = (Byte*)dst.Pointer.ToPointer() };
            var dstLine = new int_array8 { [0] = dst.StepByteSize };

            ffmpeg.sws_scale(_pConvertContext, srcData, srcLine, 0, src.Height, dstData, dstLine);
        }

        public unsafe void Convert(AVFrame src, AVFrame dst)
        {
            _VerifySource(src.width, src.height);
            _VerifyTarget(dst.width, dst.height);

            ffmpeg.sws_scale(_pConvertContext, src.data, src.linesize, 0, src.height, dst.data, dst.linesize);
        }

        public unsafe void Convert(AVFrame src, PointerBitmap dst)
        {
            _VerifySource(src.width, src.height);
            _VerifyTarget(dst.Width, dst.Height);

            var dstData = new byte_ptrArray8 { [0] = (Byte*)dst.Pointer.ToPointer() };
            var dstLine = new int_array8 { [0] = dst.StepByteSize };

            ffmpeg.sws_scale(_pConvertContext, src.data, src.linesize, 0, src.height, dstData, dstLine);
        }

        public unsafe void Convert(PointerBitmap src, AVFrame dst)
        {
            _VerifySource(src.Width, src.Height);
            _VerifyTarget(dst.width, dst.height);

            var srcData = new byte_ptrArray8 { [0] = (Byte*)src.Pointer.ToPointer() };
            var srcLine = new int_array8 { [0] = src.StepByteSize };            

            ffmpeg.sws_scale(_pConvertContext, srcData, srcLine, 0, src.Height, dst.data, dst.linesize);
        }

        public MemoryBitmap ConvertToMemoryBitmap(AVFrame src)
        {
            var dstFmt = _Implementation.GetFormat(_DstFmt);

            var dst = new MemoryBitmap(_DstSize.Width, _DstSize.Height, dstFmt);

            dst.AsSpanBitmap().PinWritablePointer(ptr => Convert(src, ptr));

            return dst;
        }

        #endregion
    }
}
