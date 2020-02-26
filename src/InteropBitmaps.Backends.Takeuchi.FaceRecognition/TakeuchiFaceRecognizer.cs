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
            // if (bitmap.PixelSize != 1 && bitmap.PixelSize != 3) throw new ArgumentException("Only Gray and RGB formats are valid", nameof(bitmap));

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

        #region  nested types.

        private class CachedImage
        {
            public Byte[] Data;
            public BitmapInfo Info;

            public void Update(SpanBitmap src)
            {
                if (Info.Width != src.Width || Info.Height != src.Height)
                {
                    
                    Info = new BitmapInfo(src.Width, src.Height, PixelFormat.Standard.RGB24);
                    Data = new byte[Info.BitmapByteSize];
                }

                var dst = new SpanBitmap(Data, Info);

                if (src.PixelFormat == dst.PixelFormat)
                {
                    dst.SetPixels(0, 0, src);
                    return;
                }

                PixelFormat.Convert(dst, src);                
            }

            public FaceRecognitionDotNet.Image CreateClient()
            {
                return FaceRecognitionDotNet.FaceRecognition.LoadImage(Data, Info.Height, Info.Width, Info.PixelSize);
            }
        }

        #endregion
    }
}
