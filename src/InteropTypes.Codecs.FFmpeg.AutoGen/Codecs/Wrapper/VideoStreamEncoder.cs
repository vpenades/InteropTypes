using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

using FFmpeg.AutoGen;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Codecs
{
    class VideoStreamEncoder : IDisposable
    {
        #region lifecycle

        public VideoStreamEncoder(string filePath, int fps)
        {
            _FilePath = filePath;            
            _LastPTS = -1;

            var fpsr = new AVRational { num = 1, den = fps };

            _BaseTime = fpsr;
            _FrameRate = new AVRational { num = 0, den = 1 };
        }

        public VideoStreamEncoder(string filePath, AVRational baseTime, AVRational frameRate)
        {
            _FilePath = filePath;
            _LastPTS = -1;            

            _BaseTime = baseTime;
            _FrameRate = frameRate;
        }

        public void Dispose()
        {
            System.Threading.Interlocked.Exchange(ref _Encoder, null)?.Dispose();
            System.Threading.Interlocked.Exchange(ref _Stream, null)?.Dispose();
            System.Threading.Interlocked.Exchange(ref _Converter, null)?.Dispose();
        }

        #endregion

        #region data

        private readonly string _FilePath;
        private readonly AVRational _BaseTime;
        private readonly AVRational _FrameRate;

        private System.IO.Stream _Stream;
        private VideoFrameConverter _Converter;
        private H264VideoStreamEncoder _Encoder;
        private BitmapInfo _Format;

        private long _FrameCount;
        private long _LastPTS;

        #endregion

        #region API

        private (VideoFrameConverter converter, H264VideoStreamEncoder encoder) GetEncoder(BitmapInfo format)
        {
            if (_Encoder != null) return (_Converter, _Encoder);

            var ss = new Size(format.Width, format.Height);

            _Converter = new VideoFrameConverter
                ( ss, AVPixelFormat.AV_PIX_FMT_BGR24
                , ss, AVPixelFormat.AV_PIX_FMT_YUV420P);

            _Format = format;
            _Stream = File.OpenWrite(_FilePath);
            _Encoder = new H264VideoStreamEncoder(_Stream, 30, ss);

            return (_Converter, _Encoder);
        }

        public unsafe void PushFrame(PointerBitmap inputFrame, long? presentationTimeStamp = null)
        {
            if (inputFrame.IsEmpty) return;

            var pts = presentationTimeStamp ?? _FrameCount * 30;
            if (pts <= _LastPTS) return;

            var (converter, encoder) = GetEncoder(inputFrame.Info);

            if (!inputFrame.Info.Equals(_Format)) return;            

            var frame = new AVFrame
            {
                data = new byte_ptrArray8 { [0] = (Byte*)inputFrame.Pointer.ToPointer() },
                linesize = new int_array8 { [0] = inputFrame.StepByteSize },
                height = inputFrame.Height
            };            

            var convertedFrame = converter.Convert(frame);

            convertedFrame.pts = pts;            
            _LastPTS = pts;

            encoder.Encode(convertedFrame);

            ++_FrameCount;
        }

        public void Drain() { _Encoder?.Drain(); }

        #endregion
    }
}
