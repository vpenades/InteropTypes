using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InteropBitmaps.Detectors
{
    public sealed class TakeuchiFaceDetector : IDisposable
    {
        #region lifecycle

        public TakeuchiFaceDetector(string modelsDirectoryPath)
        {
            if (IntPtr.Size != 8) throw new ArgumentException("Expected x64 architecture");

            if (!System.IO.File.Exists(System.IO.Path.Combine(modelsDirectoryPath, "dlib_face_recognition_resnet_model_v1.dat"))) throw new ArgumentException("Model files not found");


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

        public IEnumerable<BitmapBounds> FindFaces(SpanBitmap bitmap)
        {
            using (var img = _UseTempImage(bitmap))
            {
                return _Recognizer.FaceLocations(img)
                    .ToArray()
                    .Select(r => new BitmapBounds(r.Left, r.Top, r.Right - r.Left, r.Bottom - r.Top));                    
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

                dst.SetPixels(0, 0, src);

                // PixelFormat.Convert(dst, src);                
            }

            public FaceRecognitionDotNet.Image CreateClient()
            {
                // TODO: check bitmap is continuous

                if (_Bitmap.TryGetBuffer(out ArraySegment<Byte> buffer))
                {
                    if (buffer.Offset != 0) throw new InvalidOperationException();

                    return FaceRecognitionDotNet.FaceRecognition.LoadImage(buffer.Array, _Bitmap.Height, _Bitmap.Width, _Bitmap.PixelSize);
                }

                throw new NotSupportedException();                
            }
        }

        #endregion
    }
}
