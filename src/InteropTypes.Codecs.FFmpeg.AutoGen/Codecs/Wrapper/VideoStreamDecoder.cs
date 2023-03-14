using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

using FFmpeg.AutoGen;

namespace InteropTypes.Codecs
{
    sealed unsafe class VideoStreamDecoder : IDisposable
    {
        #region lifecycle

        public VideoStreamDecoder(string url, AVHWDeviceType HWDeviceType = AVHWDeviceType.AV_HWDEVICE_TYPE_NONE)
        {
            if (!System.IO.File.Exists(url)) throw new System.IO.FileNotFoundException(url);            

            _pFormatContext = ffmpeg.avformat_alloc_context();
            _receivedFrame = ffmpeg.av_frame_alloc();
            var pFormatContext = _pFormatContext;
            ffmpeg.avformat_open_input(&pFormatContext, url, null, null).ThrowExceptionIfError();
            ffmpeg.avformat_find_stream_info(_pFormatContext, null).ThrowExceptionIfError();
            AVCodec* codec = null;
            _streamIndex = ffmpeg.av_find_best_stream(_pFormatContext, AVMediaType.AVMEDIA_TYPE_VIDEO, -1, -1, &codec, 0).ThrowExceptionIfError();
            _pCodecContext = ffmpeg.avcodec_alloc_context3(codec);
            if (HWDeviceType != AVHWDeviceType.AV_HWDEVICE_TYPE_NONE)
            {
                ffmpeg.av_hwdevice_ctx_create(&_pCodecContext->hw_device_ctx, HWDeviceType, null, null, 0).ThrowExceptionIfError();
            }
            ffmpeg.avcodec_parameters_to_context(_pCodecContext, _pFormatContext->streams[_streamIndex]->codecpar).ThrowExceptionIfError();
            ffmpeg.avcodec_open2(_pCodecContext, codec, null).ThrowExceptionIfError();            

            CodecName = ffmpeg.avcodec_get_name(codec->id);
            FrameSize = new Size(_pCodecContext->width, _pCodecContext->height);            

            _pPacket = ffmpeg.av_packet_alloc();
            _pFrame = ffmpeg.av_frame_alloc();
        }

        public void Dispose()
        {
            if (_Disposed) return;
            _Disposed = true;

            ffmpeg.av_frame_unref(_pFrame);
            ffmpeg.av_free(_pFrame);

            ffmpeg.av_packet_unref(_pPacket);
            ffmpeg.av_free(_pPacket);

            ffmpeg.avcodec_close(_pCodecContext);
            var pFormatContext = _pFormatContext;
            ffmpeg.avformat_close_input(&pFormatContext);
        }

        #endregion

        #region data

        private bool _Disposed;

        private readonly AVCodecContext* _pCodecContext;
        private readonly AVFormatContext* _pFormatContext;
        private readonly int _streamIndex;
        private readonly AVFrame* _pFrame;
        private readonly AVFrame* _receivedFrame;
        private readonly AVPacket* _pPacket;

        public string CodecName { get; }
        public Size FrameSize { get; }
        public AVPixelFormat PixelFormat => _pCodecContext->pix_fmt;

        #endregion

        #region properties - time

        // https://blog.actorsfit.com/a?ID=01050-e67b0cc6-9e44-4421-a02a-be503e6fe24c


        /// <summary>
        /// This is the fundamental unit of time (in seconds) in terms of which frame timestamps
        //  are represented. For fixed-fps content, timebase should be 1/framerate and timestamp
        //  increments should be identically 1. This often, but not always is the inverse
        //  of the frame rate or field rate for video. 1/time_base is not the average frame
        //  rate if the frame rate is not constant.
        /// </summary>
        public AVRational TimeBase => _pCodecContext->time_base;

        /// <summary>
        /// - Decoding: For codecs that store a framerate value in the compressed bitstream,
        ///   the decoder may export it here. { 0, 1} when unknown.<br/>
        /// - Encoding: May be used to signal the framerate of CFR content to an encoder.
        /// </summary>
        public AVRational FrameRate => _pCodecContext->framerate;        

        /// <summary>
        /// For some codecs, the time base is closer to the field rate than the frame rate.
        /// Most notably, H.264 and MPEG-2 specify <see cref="TimeBase"/> as half of frame duration if
        /// no telecine is used ...
        /// </summary>
        public int TicksPerFrame => _pCodecContext->ticks_per_frame;

        public double VideoDuration
        {
            get
            {
                long duration = _pFormatContext->streams[_streamIndex]->duration;
                AVRational time_base = _pFormatContext->streams[_streamIndex]->time_base;

                if (duration == ffmpeg.AV_NOPTS_VALUE)
                {
                    duration = _pFormatContext->duration;
                    time_base = new AVRational() { num = 1, den = ffmpeg.AV_TIME_BASE };
                    if (duration == ffmpeg.AV_NOPTS_VALUE) return 0.0;
                }

                return (double)duration * time_base.num / time_base.den;
            }
        }

        #endregion

        #region API       

        public bool TryDecodeNextFrame(out AVFrame frame)
        {
            ffmpeg.av_frame_unref(_pFrame);
            ffmpeg.av_frame_unref(_receivedFrame);
            int error;

            do
            {
                try
                {
                    do
                    {
                        ffmpeg.av_packet_unref(_pPacket);
                        error = ffmpeg.av_read_frame(_pFormatContext, _pPacket);

                        if (error == ffmpeg.AVERROR_EOF)
                        {
                            frame = *_pFrame;
                            return false;
                        }

                        error.ThrowExceptionIfError();
                    } while (_pPacket->stream_index != _streamIndex);

                    ffmpeg.avcodec_send_packet(_pCodecContext, _pPacket).ThrowExceptionIfError();
                }
                finally
                {
                    ffmpeg.av_packet_unref(_pPacket);
                }

                error = ffmpeg.avcodec_receive_frame(_pCodecContext, _pFrame);
            } while (error == ffmpeg.AVERROR(ffmpeg.EAGAIN));

            error.ThrowExceptionIfError();

            if (_pCodecContext->hw_device_ctx != null)
            {
                ffmpeg.av_hwframe_transfer_data(_receivedFrame, _pFrame, 0).ThrowExceptionIfError();
                frame = *_receivedFrame;
            }
            else
                frame = *_pFrame;

            return true;
        }

        public IReadOnlyDictionary<string, string> GetContextInfo()
        {
            AVDictionaryEntry* tag = null;
            var result = new Dictionary<string, string>();
            while ((tag = ffmpeg.av_dict_get(_pFormatContext->metadata, "", tag, ffmpeg.AV_DICT_IGNORE_SUFFIX)) != null)
            {
                var key = Marshal.PtrToStringAnsi((IntPtr)tag->key);
                var value = Marshal.PtrToStringAnsi((IntPtr)tag->value);
                result.Add(key, value);
            }

            return result;
        }

        #endregion
    }
}
