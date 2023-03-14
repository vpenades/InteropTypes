using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using FFmpeg.AutoGen;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Codecs;

namespace InteropTypes.Graphics.Backends.Codecs
{
    static class _Implementation
    {
        public static bool IsValid(this AVRational rational)
        {
            if (rational.num == 0) return false; // result must not be 0
            if (rational.den == 0) return false; // must not divide by 0            
            return true;
        }

        public static void _EnsureBinariesAreSet()
        {
            if (!FFmpegHelper.Initialize()) throw new InvalidOperationException("ffmpeg not initialized.");
        }

        public static unsafe AVFrame AsAVFrame(PointerBitmap bmp)
        {
            _EnsureBinariesAreSet();

            var frame = new AVFrame();

            frame.data = new byte_ptrArray8 { [0] = (Byte*)bmp.Pointer.ToPointer() };
            frame.linesize = new int_array8 { [0] = bmp.StepByteSize };
            frame.height = bmp.Height;
            frame.width = bmp.Width;
            frame.format = (int)GetFormat(bmp.PixelFormat);

            return frame;
        }

        public static unsafe PointerBitmap AsPointerBitmap(AVFrame frame)
        {
            _EnsureBinariesAreSet();

            var fmt = GetFormat((AVPixelFormat)frame.format);            

            var binfo = new BitmapInfo(frame.width, frame.height, fmt, frame.linesize[0]);
            return new PointerBitmap((IntPtr)frame.data[0], binfo, true);
        }

        public static PixelFormat GetFormat(AVPixelFormat fmt)
        {
            switch(fmt)
            {                
                case AVPixelFormat.AV_PIX_FMT_GRAY8: return Pixel.Luminance8.Format;
                case AVPixelFormat.AV_PIX_FMT_GRAY16LE: return Pixel.Luminance16.Format;
                case AVPixelFormat.AV_PIX_FMT_RGB24: return Pixel.RGB24.Format;
                case AVPixelFormat.AV_PIX_FMT_BGR24: return Pixel.BGR24.Format;
                case AVPixelFormat.AV_PIX_FMT_RGBA: return Pixel.RGBA32.Format;
                case AVPixelFormat.AV_PIX_FMT_ARGB: return Pixel.ARGB32.Format;
                case AVPixelFormat.AV_PIX_FMT_BGRA: return Pixel.BGRA32.Format;
                case AVPixelFormat.AV_PIX_FMT_BGR565LE: return Pixel.BGR565.Format;
                default: throw new NotSupportedException();
            }
        }

        public static AVPixelFormat GetFormat(PixelFormat fmt)
        {
            switch (fmt.Code)
            {
                case Pixel.Luminance8.Code: return AVPixelFormat.AV_PIX_FMT_GRAY8;
                case Pixel.Luminance16.Code: return AVPixelFormat.AV_PIX_FMT_GRAY16LE;
                case Pixel.RGB24.Code: return AVPixelFormat.AV_PIX_FMT_RGB24;
                case Pixel.BGR24.Code: return AVPixelFormat.AV_PIX_FMT_BGR24;
                case Pixel.RGBA32.Code: return AVPixelFormat.AV_PIX_FMT_RGBA;
                case Pixel.ARGB32.Code: return AVPixelFormat.AV_PIX_FMT_ARGB;
                case Pixel.BGRA32.Code: return AVPixelFormat.AV_PIX_FMT_BGRA;
                case Pixel.BGR565.Code: return AVPixelFormat.AV_PIX_FMT_BGR565LE;
                default: throw new NotSupportedException();
            }
        }

        public static unsafe PointerBitmap AsPointerBitmap(int frameW, int frameH, int lineSize, IntPtr data)
        {
            _EnsureBinariesAreSet();

            var binfo = new BitmapInfo(frameW, frameH, Pixel.BGR24.Format, lineSize);
            return new PointerBitmap(data, binfo, true);
        }

        public static IReadOnlyDictionary<string, string> DecodeInfo(string url)
        {
            _EnsureBinariesAreSet();

            using (var vsd = new VideoStreamDecoder(url, AVHWDeviceType.AV_HWDEVICE_TYPE_NONE))
            {
                return GetDecoderInfo(vsd);
            }
        }

        private static IReadOnlyDictionary<string, string> GetDecoderInfo(VideoStreamDecoder vsd)
        {
            _EnsureBinariesAreSet();

            var info = vsd.GetContextInfo();

            var dict = new Dictionary<string, string>();

            dict["CodecName"] = vsd.CodecName;

            foreach (var kvp in info) dict[kvp.Key] = kvp.Value;

            return dict;
        }

        public static IEnumerable<(MemoryBitmap bitmap, VideoFrameState state)> DecodeFrames(string url, AVHWDeviceType HWDevice)
        {
            _EnsureBinariesAreSet();

            // https://github.com/Ruslan-B/FFmpeg.AutoGen/blob/db7a6cdefb0227283a0e09e76ef10b7feb27a53f/FFmpeg.AutoGen.Example/Program.cs#L95

            using (var vsd = new VideoStreamDecoder(url, HWDevice))
            {
                var info = GetDecoderInfo(vsd);
                var state = new Dictionary<string, long>();
                var times = new Dictionary<string, AVRational>();

                times["time_base"] = vsd.TimeBase;
                times["frame_rate"] = vsd.FrameRate;
                state["ticks_per_frame"] = vsd.TicksPerFrame;

                var context = new VideoFrameState(info, state, times);

                var sourceSize = vsd.FrameSize;
                var sourcePixelFormat = HWDevice == AVHWDeviceType.AV_HWDEVICE_TYPE_NONE ? vsd.PixelFormat : GetHWPixelFormat(HWDevice);
                var destinationSize = sourceSize;
                var destinationPixelFormat = AVPixelFormat.AV_PIX_FMT_BGR24;

                long index = 0;

                using (var vfc = new VideoFrameConverter(sourceSize, sourcePixelFormat, destinationSize, destinationPixelFormat))
                {
                    while (vsd.TryDecodeNextFrame(out var frame))
                    {
                        state["index"] = index;

                        if (frame.time_base.IsValid()) times["time_base"] = frame.time_base;

                        state["pts"] = frame.pts;
                        state["pkt_dts"] = frame.pkt_dts;
                        state["pkt_pos"] = frame.pkt_pos;
                        state["pkt_size"] = frame.pkt_size;
                        state["pkt_duration"] = frame.pkt_duration;                        
                        state["best_effort_timestamp"] = frame.best_effort_timestamp;

                        state["display_picture_number"] = frame.display_picture_number;
                        state["coded_picture_number"] = frame.coded_picture_number;
                        state["decode_error_flags"] = frame.decode_error_flags;


                        var convertedFrame = vfc.ConvertToMemoryBitmap(frame);
                        
                        yield return (convertedFrame, context);

                        ++index;
                    }
                }
            }
        }

        private static AVPixelFormat GetHWPixelFormat(AVHWDeviceType hWDevice)
        {
            switch (hWDevice)
            {
                case AVHWDeviceType.AV_HWDEVICE_TYPE_NONE: return AVPixelFormat.AV_PIX_FMT_NONE;
                case AVHWDeviceType.AV_HWDEVICE_TYPE_VDPAU: return AVPixelFormat.AV_PIX_FMT_VDPAU;
                case AVHWDeviceType.AV_HWDEVICE_TYPE_CUDA: return AVPixelFormat.AV_PIX_FMT_CUDA;
                case AVHWDeviceType.AV_HWDEVICE_TYPE_VAAPI: return AVPixelFormat.AV_PIX_FMT_VAAPI;
                case AVHWDeviceType.AV_HWDEVICE_TYPE_DXVA2: return AVPixelFormat.AV_PIX_FMT_NV12;
                case AVHWDeviceType.AV_HWDEVICE_TYPE_QSV: return AVPixelFormat.AV_PIX_FMT_QSV;
                case AVHWDeviceType.AV_HWDEVICE_TYPE_VIDEOTOOLBOX: return AVPixelFormat.AV_PIX_FMT_VIDEOTOOLBOX;
                case AVHWDeviceType.AV_HWDEVICE_TYPE_D3D11VA: return AVPixelFormat.AV_PIX_FMT_NV12;
                case AVHWDeviceType.AV_HWDEVICE_TYPE_DRM: return AVPixelFormat.AV_PIX_FMT_DRM_PRIME;
                case AVHWDeviceType.AV_HWDEVICE_TYPE_OPENCL: return AVPixelFormat.AV_PIX_FMT_OPENCL;
                case AVHWDeviceType.AV_HWDEVICE_TYPE_MEDIACODEC: return AVPixelFormat.AV_PIX_FMT_MEDIACODEC;
                default: return AVPixelFormat.AV_PIX_FMT_NONE;
            }
        }
    }
}
