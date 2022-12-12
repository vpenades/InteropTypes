﻿using System;
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
            if (rational.den == 0) return false; // must not divide by 0
            if (rational.num == 0) return false; // result must not be 0
            return true;
        }

        public static void _EnsureBinariesAreSet()
        {
            FFmpegHelper.Initialize();
        }

        public static unsafe PointerBitmap AsPointerBitmap(AVFrame frame)
        {
            _EnsureBinariesAreSet();

            var binfo = new BitmapInfo(frame.width, frame.height, Pixel.BGR24.Format, frame.linesize[0]);
            return new PointerBitmap((IntPtr)frame.data[0], binfo, true);
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

        public static IEnumerable<(PointerBitmap bitmap, VideoFrameState state)> DecodeFrames(string url, AVHWDeviceType HWDevice)
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
                        var convertedFrame = vfc.Convert(frame);

                        state["index"] = index;

                        if (frame.time_base.IsValid()) times["time_base"] = frame.time_base;

                        state["pts"] = frame.pts;
                        // state["pkt_pts"] = frame.pkt_pts;
                        state["pkt_dts"] = frame.pkt_dts;
                        state["best_effort_timestamp"] = frame.best_effort_timestamp;

                        state["display_picture_number"] = frame.display_picture_number;
                        state["coded_picture_number"] = frame.coded_picture_number;
                        state["decode_error_flags"] = frame.decode_error_flags;

                        yield return (AsPointerBitmap(convertedFrame), context);

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