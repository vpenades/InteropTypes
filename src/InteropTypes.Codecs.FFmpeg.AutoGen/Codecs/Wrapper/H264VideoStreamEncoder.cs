using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

using FFmpeg.AutoGen;

namespace InteropTypes.Codecs
{
    /// <summary>
    /// Based on https://ffmpeg.org/doxygen/5.0/encode_video_8c-example.html
    /// </summary>
    public sealed unsafe class H264VideoStreamEncoder : IEncoder
    {
        #region lifecycle

        public H264VideoStreamEncoder(Stream stream, Size frameSize)
        {
            _Stream = stream;

            // find the mpeg1video encoder
            _Codec = ffmpeg.avcodec_find_encoder(AVCodecID.AV_CODEC_ID_H264);
            if (_Codec == null) throw new InvalidOperationException();

            _Context = ffmpeg.avcodec_alloc_context3(_Codec);
            if (_Context == null) throw new InvalidOperationException();

            _Packet = ffmpeg.av_packet_alloc();
            if (_Packet == null) throw new InvalidOperationException();

            // put sample parameters
            _Context->bit_rate = 400000;

            // resolution must be a multiple of two
            _Context->width = frameSize.Width;
            _Context->height = frameSize.Height;

            /* emit one intra frame every ten frames
             * check frame pict_type before passing frame
             * to encoder, if frame->pict_type is AV_PICTURE_TYPE_I
             * then gop_size is ignored and the output of encoder
             * will always be I frame irrespective to gop_size
             */
            _Context->gop_size = 10;
            _Context->max_b_frames = 1;
            _Context->pix_fmt = AVPixelFormat.AV_PIX_FMT_YUV420P;

            if (_Codec->id == AVCodecID.AV_CODEC_ID_H264)
                ffmpeg.av_opt_set(_Context->priv_data, "preset", "slow", 0);            
        }        

        public void Dispose()
        {
            // we should use an interlocked exchange here, but it does not work with pointers
            var ctx = _Context;
            _Context = null;

            if (ctx == null) return;

            ffmpeg.avcodec_close(ctx);
            ffmpeg.av_free(ctx);
        }

        #endregion

        #region data

        private AVCodec* _Codec;
        private AVCodecContext* _Context;
        private AVPacket* _Packet;

        private AVFrame* _Frame;

        private Stream _Stream;

        private static readonly byte[] _EndCode = { 0, 0, 1, 0xb7 };

        #endregion

        #region API

        /// <summary>
        /// Setups the codec to encode frames at fixed frame rate
        /// </summary>
        /// <remarks>
        /// must be set before calling <see cref="Open"/>
        /// </remarks>
        public void SetFixedFrameRate(int fps)
        {
            if (fps <= 1) throw new ArgumentOutOfRangeException(nameof(fps));
            _Context->time_base = new AVRational { num = 1, den = fps };
            _Context->framerate = new AVRational { num = fps, den = 1 };
        }

        public void Open()
        {
            // open it
            var ret = ffmpeg.avcodec_open2(_Context, _Codec, null);
            if (ret < 0) throw new InvalidOperationException();

            // create working frame

            _Frame = ffmpeg.av_frame_alloc();
            if (_Frame == null) throw new InvalidOperationException();

            _Frame->format = (int)_Context->pix_fmt;
            _Frame->width = _Context->width;
            _Frame->height = _Context->height;

            ret = ffmpeg.av_frame_get_buffer(_Frame, 0);
            if (ret < 0) throw new InvalidOperationException();
        }

        public void Encode(int idx, Action<AVFrame> frameUpdater)
        {
            /*
            Make sure the frame data is writable.
            On the first round, the frame is fresh from av_frame_get_buffer()
            and therefore we know it is writable.
            But on the next rounds, encode() will have called
            avcodec_send_frame(), and the codec may have kept a reference to
            the frame in its internal structures, that makes the frame
            unwritable.
            av_frame_make_writable() checks that and allocates a new buffer
            for the frame only if necessary.
            */

            var ret = ffmpeg.av_frame_make_writable(_Frame);
            if (ret < 0) throw new InvalidOperationException();            

            // write the frame pixels
            frameUpdater.Invoke(*_Frame);

            _Frame->pts = idx;

            // _Frame->pkt_dts = idx * 248; no effect

            // encode the image
            _Encode(_Context, _Frame, _Packet, _Stream);
        }

        public void Drain()
        {
            /* flush the encoder */
            _Encode(_Context, null, _Packet, _Stream);

            /* Add sequence end code to have a real MPEG file.
               It makes only sense because this tiny examples writes packets
               directly. This is called "elementary stream" and only works for some
               codecs. To create a valid file, you usually need to write packets
               into a proper file format or protocol; see muxing.c.
             */

            if (_Codec->id == AVCodecID.AV_CODEC_ID_MPEG1VIDEO || _Codec->id == AVCodecID.AV_CODEC_ID_MPEG2VIDEO)
            {
                _Stream.Write(_EndCode, 0, _EndCode.Length);
            }
        }

        private static void _Encode(AVCodecContext* enc_ctx, AVFrame* frame, AVPacket* pkt, Stream outfile)
        {
            /* send the frame to the encoder */
            // if (frame != null)  printf("Send frame %3"PRId64"\n", frame->pts);

            var ret = ffmpeg.avcodec_send_frame(enc_ctx, frame);
            if (ret < 0)
            {
                return;
            }

            while (ret >= 0)
            {
                ret = ffmpeg.avcodec_receive_packet(enc_ctx, pkt);

                if (ret == ffmpeg.AVERROR(ffmpeg.EAGAIN) || ret == ffmpeg.AVERROR_EOF) return;

                else if (ret < 0) return;

                // printf("Write packet %3"PRId64" (size=%5d)\n", pkt->pts, pkt->size);

                using var packetStream = new UnmanagedMemoryStream(pkt->data, pkt->size);
                packetStream.CopyTo(outfile);

                ffmpeg.av_packet_unref(pkt);
            }
        }

        #endregion
    }
}
