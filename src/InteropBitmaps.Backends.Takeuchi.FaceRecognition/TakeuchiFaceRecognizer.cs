using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InteropBitmaps
{
    public sealed class TakeuchiFaceRecognizer : IDisposable
    {
        #region lifecycle

        public TakeuchiFaceRecognizer(string modelsDirectoryPath)
        {
            if (IntPtr.Size != 8) throw new ArgumentException("Expected x64 architecture");

            _Recognizer = FaceRecognitionDotNet.FaceRecognition.Create(modelsDirectoryPath);
        }

        public void Dispose()
        {
            _Recognizer?.Dispose();
            _Recognizer = null;
        }

        #endregion

        #region data

        private FaceRecognitionDotNet.FaceRecognition _Recognizer;

        private CachedImage _Current;

        #endregion

        #region API

        private CachedImage _GetImage(SpanBitmap bitmap)
        {
            if (_Current == null) _Current = new CachedImage();

            _Current.Update(bitmap);

            return _Current;
        }        


        public IEnumerable<FaceRecognitionDotNet.Location> FindFaces(SpanBitmap bitmap)
        {
            var tmp = _GetImage(bitmap);

            using (var img = tmp.CreateClient())
            {
                return _Recognizer.FaceLocations(img).ToArray();
            }            
        }

        public void FindLandmarks(SpanBitmap bitmap)
        {
            var tmp = _GetImage(bitmap);

            using (var img = tmp.CreateClient())
            {
                var locs = _Recognizer.FaceLocations(img);
                var lnds = _Recognizer.FaceLandmark(img,locs).ToArray();               
            }
        }

        #endregion

        #region 

        private class CachedImage
        {
            public Byte[] Data;
            public BitmapInfo Info;

            public void Update(SpanBitmap src)
            {
                if (src.PixelSize != 1 && src.PixelSize != 3) throw new ArgumentException("Only Gray and RGB formats are valid", nameof(src));

                if (Info.Width != src.Width || Info.Height != src.Height || Info.PixelSize != src.PixelSize)
                {
                    Data = new byte[src.Width * src.Height * src.PixelSize];
                    Info = new BitmapInfo(src.Width, src.Height, src.PixelFormat);
                }

                var dst = new SpanBitmap(Data, Info);
                dst.SetPixels(0, 0, src);
            }

            public FaceRecognitionDotNet.Image CreateClient()
            {
                return FaceRecognitionDotNet.FaceRecognition.LoadImage(Data, Info.Height, Info.Width, Info.PixelSize);
            }
        }

        #endregion
    }
}
