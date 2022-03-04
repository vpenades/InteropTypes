using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

using InteropTypes.Graphics;
using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Adapters;
using InteropTypes.Graphics.Backends;

using InteropTypes.Tensors;
using InteropTypes.Vision;

using OPENCV2 = OpenCvSharp.Cv2;
using CVSTORAGE = OpenCvSharp.FileStorage;
using ARUCO = OpenCvSharp.Aruco;
using CVMATRIX = OpenCvSharp.Mat;
using CVDEPTHTYPE = OpenCvSharp.MatType;

namespace InteropTypes.Vision.Backends
{
    partial class MarkersContext
    {        
        public class ArucoEstimator :
            PointerBitmapInput.IInference<MarkersContext>,
            IInferenceContext<PointerBitmap, MarkersContext>,
            IDisposable

        {
            #region lifecycle

            public ArucoEstimator(bool horizontalMirror = false)
            {
                _HorizontalMirror = horizontalMirror;
                _MarkersDict = ARUCO.CvAruco.GetPredefinedDictionary(ARUCO.PredefinedDictionaryName.Dict4X4_100);
            }

            public void Dispose()
            {
                System.Threading.Interlocked.Exchange(ref _CameraTransform, null)?.Dispose();
                System.Threading.Interlocked.Exchange(ref _CameraDistortion, null)?.Dispose();
            }

            #endregion

            #region data

            private readonly ARUCO.Dictionary _MarkersDict;
            private int _MarkersLength = 80;

            private ARUCO.DetectorParameters _Parameters = ARUCO.DetectorParameters.Create();

            private bool _HorizontalMirror;

            private CVMATRIX _CameraTransform;
            private CVMATRIX _CameraDistortion;

            #endregion

            #region API

            public void SetCameraCalibrationDefault()
            {
                var bytes = System.Convert.FromBase64String(_DEFCAM);
                var utf8 = System.Text.Encoding.UTF8.GetString(bytes);

                SetCameraCalibrationFromXml(utf8);
            }

            public void SetCameraCalibrationFromFile(string filePath)
            {
                // TODO: we could create an IModelGraph from this.

                // Initialize Camera calibration matrix with distortion coefficients 
                // Calibration done with https://docs.opencv.org/3.4.3/d7/d21/tutorial_interactive_calibration.html

                using var fs = new CVSTORAGE(filePath, CVSTORAGE.Modes.Read);

                if (!fs.IsOpened()) return;

                if (_CameraTransform == null) _CameraTransform = new CVMATRIX(3, 3, CVDEPTHTYPE.CV_32F, 1);
                if (_CameraDistortion == null) _CameraDistortion = new CVMATRIX(1, 8, CVDEPTHTYPE.CV_32F, 1);

                fs["cameraMatrix"].ReadMat(_CameraTransform);
                fs["dist_coeffs"].ReadMat(_CameraDistortion);
            }

            public void SetCameraCalibrationFromXml(string xmlContent)
            {
                // TODO: we could create an IModelGraph from this.

                // Initialize Camera calibration matrix with distortion coefficients 
                // Calibration done with https://docs.opencv.org/3.4.3/d7/d21/tutorial_interactive_calibration.html

                using var fs = new CVSTORAGE(xmlContent, CVSTORAGE.Modes.Read | CVSTORAGE.Modes.Memory | CVSTORAGE.Modes.FormatXml);

                if (!fs.IsOpened()) return;

                if (_CameraTransform == null) _CameraTransform = new CVMATRIX(3, 3, CVDEPTHTYPE.CV_32F, 1);
                if (_CameraDistortion == null) _CameraDistortion = new CVMATRIX(1, 8, CVDEPTHTYPE.CV_32F, 1);

                fs["cameraMatrix"].ReadMat(_CameraTransform);
                fs["dist_coeffs"].ReadMat(_CameraDistortion);
            }

            public void Inference(MarkersContext result, PointerBitmapInput input, Rectangle? inputWindow = null)
            {
                input.PinInput(ptr => Inference(result, ptr, inputWindow));
            }

            public void Inference(MarkersContext result, InferenceInput<PointerBitmap> input, Rectangle? inputWindow = null)
            {
                using var inputMat = input
                    .GetClippedPointerBitmap(ref inputWindow)
                    .WrapAsMat();

                if (inputMat == null) return;

                Inference(result, inputMat);
            }

            public void Inference(MarkersContext result, CVMATRIX src)
            {
                result._Detected.Clear();

                if (src.Empty()) return;

                int[] ids; // name/id of the detected markers
                OpenCvSharp.Point2f[][] corners; // corners of the detected marker
                OpenCvSharp.Point2f[][] rejected; // rejected contours

                if (_HorizontalMirror)
                {
                    using var mirror = new CVMATRIX();
                    OPENCV2.Flip(src, mirror, OpenCvSharp.FlipMode.X);
                    ARUCO.CvAruco.DetectMarkers(mirror, _MarkersDict, out corners, out ids, _Parameters, out rejected);
                }
                else
                {
                    ARUCO.CvAruco.DetectMarkers(src, _MarkersDict, out corners, out ids, _Parameters, out rejected);
                }

                var count = Math.Min(ids.Length, corners.Length);

                if (count == 0) return;

                using var rvecs = new CVMATRIX(); // rotation vector
                using var tvecs = new CVMATRIX(); // translation vector

                result._ScreenWidth = src.Width;
                result._CameraMirror = _HorizontalMirror;

                if (_CameraTransform != null && _CameraDistortion != null)
                {
                    ARUCO.CvAruco.EstimatePoseSingleMarkers(corners, _MarkersLength, _CameraTransform, _CameraDistortion, rvecs, tvecs);

                    var x = _CameraTransform.AsSpanTensor2<Double>().Span;

                    result._CameraTransform = new Matrix4x4
                        (
                        (float)x[0], (float)x[3], (float)x[6], 0,
                        (float)x[1], (float)x[4], (float)x[7], 0,
                        (float)x[2], (float)x[5], (float)x[8], 0,
                        0, 0, 0, 1
                        );

                    var dist = _CameraDistortion
                        .AsSpanTensor2<double>()
                        .Span;

                    if (result._CameraDistortion == null) result._CameraDistortion = new float[dist.Length];

                    for (int i = 0; i < result._CameraDistortion.Length; ++i) result._CameraDistortion[i] = (float)dist[i];
                }

                for (int i = 0; i < count; ++i)
                {
                    var c = corners[i];

                    var marker = new Marker
                    {
                        Id = ids[i],
                        A = c[0].ToPoint2(),
                        B = c[1].ToPoint2(),
                        C = c[2].ToPoint2(),
                        D = c[3].ToPoint2(),
                    };

                    if (_CameraTransform != null)
                    {
                        var r = rvecs.AsSpanTensor3<Double>()[i][0].Span;
                        var t = tvecs.AsSpanTensor3<Double>()[i][0].Span;

                        marker.Rotation = new Vector3((float)r[0], (float)r[1], (float)r[2]);
                        marker.Translation = new Vector3((float)t[0], (float)t[1], (float)t[2]);
                    }

                    result._Detected.Add(marker);
                }
            }

            #endregion

            #region default CAM calibration

            private const string _DEFCAM =
                "PD94bWwgdmVyc2lvbj0iMS4wIj8 + CjxvcGVuY3Zfc3RvcmFnZT4KPGNhbGlicmF0aW9uRGF0ZT4i" +
                "RnJpIEphbiAxOCAxMTo0MjoyMiAyMDE5IjwvY2FsaWJyYXRpb25EYXRlPgo8ZnJhbWVzQ291bnQ+" +
                "MTc8L2ZyYW1lc0NvdW50Pgo8Y2FtZXJhUmVzb2x1dGlvbj4KICAxMjgwIDcyMDwvY2FtZXJhUmVz" +
                "b2x1dGlvbj4KPGNhbWVyYU1hdHJpeCB0eXBlX2lkPSJvcGVuY3YtbWF0cml4Ij4KICA8cm93cz4z" +
                "PC9yb3dzPgogIDxjb2xzPjM8L2NvbHM+CiAgPGR0PmQ8L2R0PgogIDxkYXRhPgogICAgMS4wMDk3" +
                "ODg1MjgxODkxMDU1ZSswMyAwLiA2LjQyNTE0NDczNDc5NzM1NTZlKzAyIDAuCiAgICAxLjAwOTc4" +
                "ODUyODE4OTEwNTVlKzAzIDMuNTk3MzM4OTkzNjA3MDg2MmUrMDIgMC4gMC4gMS48L2RhdGE+PC9j" +
                "YW1lcmFNYXRyaXg+CjxjYW1lcmFNYXRyaXhfc3RkX2RldiB0eXBlX2lkPSJvcGVuY3YtbWF0cml4" +
                "Ij4KICA8cm93cz40PC9yb3dzPgogIDxjb2xzPjE8L2NvbHM+CiAgPGR0PmQ8L2R0PgogIDxkYXRh" +
                "PgogICAgMC4gOC42MTAwMjcxMzY4MTAzMjAxZSswMCAyLjMxNzcwNDU4ODIwNzczMjdlKzAwCiAg" +
                "ICAxLjc3MzkyNDczNzIxMzUyMTZlKzAwPC9kYXRhPjwvY2FtZXJhTWF0cml4X3N0ZF9kZXY+Cjxk" +
                "aXN0X2NvZWZmcyB0eXBlX2lkPSJvcGVuY3YtbWF0cml4Ij4KICA8cm93cz4xPC9yb3dzPgogIDxj" +
                "b2xzPjU8L2NvbHM+CiAgPGR0PmQ8L2R0PgogIDxkYXRhPgogICAgMi41NzQwMjYxNDE3OTEyNDE5" +
                "ZS0wMSAtNy4yOTQxNzg0MjM3OTA3MTcwZS0wMSAwLiAwLgogICAgNy41OTc3MzUwODM2NjM3MTU5" +
                "ZS0wMTwvZGF0YT48L2Rpc3RfY29lZmZzPgo8ZGlzdF9jb2VmZnNfc3RkX2RldiB0eXBlX2lkPSJv" +
                "cGVuY3YtbWF0cml4Ij4KICA8cm93cz41PC9yb3dzPgogIDxjb2xzPjE8L2NvbHM+CiAgPGR0PmQ8" +
                "L2R0PgogIDxkYXRhPgogICAgMS41MzYwMjE4Mzc2MDg0NzIxZS0wMiA2Ljc2NDM0MDEzMTEzMzc2" +
                "MDBlLTAyIDAuIDAuCiAgICAxLjA4NDc0ODQ3OTMyNjI4NDZlLTAxPC9kYXRhPjwvZGlzdF9jb2Vm" +
                "ZnNfc3RkX2Rldj4KPGF2Z19yZXByb2plY3Rpb25fZXJyb3I+NC4yMTM4NzIxMTgwMDQzNjkyZS0w" +
                "MTwvYXZnX3JlcHJvamVjdGlvbl9lcnJvcj4KPC9vcGVuY3Zfc3RvcmFnZT4 =";

            #endregion
        }
    }
}
