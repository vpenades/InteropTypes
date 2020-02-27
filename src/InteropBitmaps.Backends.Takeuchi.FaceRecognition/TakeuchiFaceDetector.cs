using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InteropBitmaps
{
    public sealed class TakeuchiFaceDetector : IDisposable
    {
        #region lifecycle

        public TakeuchiFaceDetector(string modelsDirectoryPath)
        {
            if (IntPtr.Size != 8) throw new ArgumentException("Expected x64 architecture");

            _Recognizer = FaceRecognitionDotNet.FaceRecognition.Create(modelsDirectoryPath);
            _Images = new System.Threading.ThreadLocal<CachedImage>( ()=> new CachedImage() );
        }

        public void Dispose()
        {
            _Recognizer?.Dispose();
            _Recognizer = null;
            _Images?.Dispose();
            _Images = null;
        }

        #endregion

        #region data

        private FaceRecognitionDotNet.FaceRecognition _Recognizer;
        private System.Threading.ThreadLocal<CachedImage> _Images;

        #endregion

        #region API

        public IEnumerable<FaceRecognitionDotNet.Location> FindFaces(SpanBitmap bitmap)
        {
            using (var img = _UseTempImage(bitmap))
            {
                return _Recognizer.FaceLocations(img).ToArray();
            }            
        }

        public void FindLandmarks(SpanBitmap bitmap)
        {
            using (var img = _UseTempImage(bitmap))
            {
                var locs = _Recognizer.FaceLocations(img);
                var lnds = _Recognizer.FaceLandmark(img,locs).ToArray();               
            }
        }

        #endregion

        #region  nested types

        private FaceRecognitionDotNet.Image _UseTempImage(SpanBitmap bitmap)
        {
            var current = _Images.Value;
            current.Update(bitmap);
            return current.CreateClient();
        }

        private class CachedImage
        {
            public MemoryBitmap _Bitmap;

            public void Update(SpanBitmap src)
            {
                if (_Bitmap == null || _Bitmap.Width != src.Width || _Bitmap.Height != src.Height)
                {
                    _Bitmap = new MemoryBitmap(src.Width, src.Height, PixelFormat.Standard.RGB24);
                }

                var dst = _Bitmap.AsSpanBitmap();

                if (src.PixelFormat == dst.PixelFormat)
                {
                    dst.SetPixels(0, 0, src);
                    return;
                }

                PixelFormat.Convert(dst, src);                
            }

            public FaceRecognitionDotNet.Image CreateClient()
            {
                return FaceRecognitionDotNet.FaceRecognition.LoadImage(_Bitmap.ToArray(), _Bitmap.Height, _Bitmap.Width, _Bitmap.PixelSize);
            }
        }

        #endregion
    }
}
