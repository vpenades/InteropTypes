using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

using FFmpeg.AutoGen;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Codecs
{
    interface IEncoder : IDisposable
    {
        void Encode(int idx, Action<AVFrame> frameUpdater);
        void Drain();
    }

    class VideoStreamEncoder : IDisposable
    {
        #region lifecycle

        public VideoStreamEncoder(string filePath, int fps)
        {
            _FilePath = filePath;
            _FramesPerSecond = fps;
        }        

        public void Dispose()
        {
            System.Threading.Interlocked.Exchange(ref _Encoder, null)?.Dispose();            
            System.Threading.Interlocked.Exchange(ref _Converter, null)?.Dispose();
            System.Threading.Interlocked.Exchange(ref _Stream, null)?.Dispose();
        }

        #endregion

        #region data

        private readonly string _FilePath;
        
        private readonly int _FramesPerSecond;

        private System.IO.Stream _Stream;
        private VideoFrameConverter _Converter;
        private IEncoder _Encoder;
        private BitmapInfo _Format;

        private long _FrameCount;        

        #endregion

        #region API        

        private (VideoFrameConverter converter, IEncoder encoder) GetEncoder(BitmapInfo format)
        {
            if (_Encoder != null) return (_Converter, _Encoder);

            var ss = new Size(format.Width, format.Height);

            _Converter = new VideoFrameConverter( ss, AVPixelFormat.AV_PIX_FMT_BGR24, ss, AVPixelFormat.AV_PIX_FMT_YUV420P);

            _Format = format;
            _Stream = File.OpenWrite(_FilePath);
            _Encoder = new H264VideoStreamEncoder(_Stream, _FramesPerSecond, ss);

            return (_Converter, _Encoder);
        }

        public unsafe void PushFrame(SpanBitmap inputFrame)
        {
            inputFrame.PinReadablePointer(ptr => PushFrame(ptr));
        }

        public unsafe void PushFrame(PointerBitmap inputFrame)
        {
            if (inputFrame.IsEmpty) return;            

            var (converter, encoder) = GetEncoder(inputFrame.Info);

            if (!inputFrame.Info.Equals(_Format)) return;

            void updater(AVFrame dstFrame)
            {
                converter.Convert(inputFrame, dstFrame);                
            }

            encoder.Encode((int)_FrameCount, updater);

            ++_FrameCount;
        }

        public void Drain() { _Encoder?.Drain(); }

        #endregion
    }
}
