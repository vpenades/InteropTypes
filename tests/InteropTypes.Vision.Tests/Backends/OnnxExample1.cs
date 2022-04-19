using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Tensors;
using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Codecs;

using NUnit.Framework;

namespace InteropTypes.Vision.Backends
{
    public class OnnxExample1
    {
        [SetUp]
        public void Setup()
        {
            OnnxModel.DeviceID = 0;
        }        

        [TestCase("Resources\\dog.jpeg", "Golden Retriever")]
        [TestCase("Resources\\shannon.jpg", "cowboy hat")]
        public void TestResnet50(string imagePath, string expectedResult)
        {
            // https://onnxruntime.ai/docs/tutorials/resnet50_csharp.html            

            var srcImage = MemoryBitmap.Load(imagePath);

            var model = OnnxModel.FromFile("Models\\resnet50-v2-7.onnx");

            // image normalization
            // https://github.com/onnx/models/tree/master/vision/classification/resnet#preprocessing            
            var modelXform = MultiplyAdd
                .CreateMul(0.229f, 0.224f, 0.225f)
                .ConcatAdd(0.485f, 0.456f, 0.406f)                
                // .GetTransposedZYXW()
                .GetInverse();

            var imagePreprocessor = new ImageProcessor<Pixel.RGB96F>(modelXform);            

            using (var session = model.CreateSession())
            {
                var workingTensor = session
                    .GetInputTensor<float>(0)
                    .AsSpanTensor4()
                    .GetSubTensor(0)
                    .SetImage(srcImage, imagePreprocessor);

                // run

                session.Inference();

                // get results

                var result = session
                    .GetOutputTensor<float>(0)
                    .AsSpanTensor2()
                    .GetSubTensor(0);                

                result.ApplySoftMax();

                var scores = result.ToArray();                             

                var pairs = Labels.resnet50_v2_7_Labels
                    .Zip(scores, (Label,Score) =>(Label,Score))
                    .OrderByDescending(item => item.Score)
                    .ToList();                

                foreach(var pair in pairs)
                {
                    TestContext.WriteLine($"{pair.Label} = {pair.Score}");
                }

                Assert.AreEqual(expectedResult, pairs[0].Label);
            }
        }

        [TestCase("Resources\\shannon.jpg")]
        public void TestMcnnFace(string imagePath)
        {
            // https://github.com/linxiaohui/mtcnn-opencv/tree/main/mtcnn_cv2

            var srcImage = MemoryBitmap.Load(imagePath, GDICodec.Default);

            var pmodel = OnnxModel.FromFile("Models\\MCNN\\pnet.onnx");
            var imagePreprocessor = new ImageProcessor<Pixel.RGB96F>();

            using (var session = pmodel.CreateSession())
            {
                var workingTensor = session.UseInputTensor<float>(0, 1, srcImage.Height, srcImage.Width, 3)
                    .VerifyName(n=> n == "input_1")
                    .AsSpanTensor4()
                    .GetSubTensor(0)
                    .SetImage(srcImage, imagePreprocessor);                

                var conv2d = session.UseOutputTensor<float>(0, 1, 251, 251, 4).VerifyName(n => n == "conv2d_4");
                var softmax = session.UseOutputTensor<float>(1, 1, 251, 251, 2).VerifyName(n => n == "softmax");

                // run

                session.Inference();

                // get results:

                var score = softmax
                    .AsSpanTensor4()
                    .GetSubTensor(0)
                    .UpCast<System.Numerics.Vector2>()
                    .Span
                    .ToArray();

                var cv2d = conv2d
                    .AsSpanTensor4()
                    .GetSubTensor(0)
                    .UpCast<System.Numerics.Vector4>()                    
                    .Span
                    .ToArray();

                var pairs = score
                    .Select(item => item.Y)
                    .Zip(cv2d.Select(xyxy => xyxy * new System.Numerics.Vector4(srcImage.Height, srcImage.Width, srcImage.Height, srcImage.Width)))
                    .OrderByDescending(item => item.First)
                    .ToArray();
            }
        }


        struct YuNetEntry
        {
            public System.Numerics.Vector2 Min;
            public System.Numerics.Vector2 Max;
            public System.Numerics.Vector2 L0;
            public System.Numerics.Vector2 L1;
            public System.Numerics.Vector2 L2;
            public System.Numerics.Vector2 L3;
            public System.Numerics.Vector2 L4;


            // https://github.com/Kazuhito00/YuNet-ONNX-TFLite-Sample/blob/main/yunet/yunet_onnx.py

            // MIN_SIZES = [[10, 16, 24], [32, 48], [64, 96], [128, 192, 256]]
            // STEPS = [8, 16, 32, 64]
            // VARIANCE = [0.1, 0.2]



            public static SpanTensor2<System.Numerics.Vector4> priorbox(
                int feature_width, int feature_height,
                int img_width, int img_height,
                int step, int num_sizes,
                float[] pWinSizes)
            {
                var outputData = new SpanTensor2<System.Numerics.Vector4>(feature_height, feature_width);

                for (int r = 0; r < outputData.Dimensions[1]; r++)
                {
                    for (int c = 0; c < outputData.Dimensions[0]; c++)
                    {                        
                        //priorbox
                        for (int s = 0; s < num_sizes; s++)
                        {
                            float min_size_ = pWinSizes[s];

                            float center_x = (c + 0.5f) * step;
                            float center_y = (r + 0.5f) * step;                            
                            var xmin = (center_x - min_size_ / 2f) / img_width;                            
                            var ymin = (center_y - min_size_ / 2f) / img_height;                            
                            var xmax = (center_x + min_size_ / 2f) / img_width;                            
                            var ymax = (center_y + min_size_ / 2f) / img_height;

                            outputData[c,r] = new System.Numerics.Vector4(xmin,ymin,xmax,ymax);
                        }
                    }
                }

                return outputData;
            }
        }

        [TestCase("Resources\\shannon.jpg")]
        public void TestYuNet(string imagePath)
        {
        // https://github.com/opencv/opencv_zoo/tree/master/models/face_detection_yunet
        // https://github.com/ShiqiYu/libfacedetection
        // https://github.com/Kazuhito00/YuNet-ONNX-TFLite-Sample/blob/main/yunet/yunet_onnx.py

            var srcImage = MemoryBitmap.Load(imagePath, GDICodec.Default);

            var pmodel = OnnxModel.FromFile("Models\\face_detection_yunet_2021sep.onnx");
            var imagePreprocessor = new ImageProcessor<Pixel.RGB96F>();

            using (var session = pmodel.CreateSession())
            {
                var workingTensor = session.UseInputTensor<float>(0, 1, 3, srcImage.Height, srcImage.Width)
                    .VerifyName(n => n == "input")
                    .AsSpanTensor4()
                    .GetSubTensor(0)
                    .SetImage(srcImage, imagePreprocessor);

                const int COUNT = 15040;

                var loc = session.UseOutputTensor<float>(0, COUNT, 14).VerifyName(n => n == "loc");
                var conf = session.UseOutputTensor<float>(1, COUNT, 2).VerifyName(n => n == "conf");                
                var iou = session.UseOutputTensor<float>(2, COUNT, 1).VerifyName(n => n == "iou");

                // run

                session.Inference();

                // https://github.com/ShiqiYu/libfacedetection/blob/master/src/facedetectcnn.cpp#L731

                var r_loc = loc.AsSpanTensor2().UpCast<YuNetEntry>().Span.ToArray();
                var r_conf = conf.AsSpanTensor2().UpCast<System.Numerics.Vector2>().Span.ToArray();
                var r_iou = iou.AsSpanTensor2().UpCast<float>().Span;

                // softmax1vector2class(mbox_conf);
                // clamp1vector(mbox_iou);

                // SpanTensor.ApplySoftMax(r_conf);

                var sorted = r_conf
                    .Select(item => item.Y)
                    .Zip(r_loc)
                    .OrderByDescending(item => item.First)
                    .ToArray();

                var prior_variance = new [] { 0.1f, 0.1f, 0.2f, 0.2f };
            }
        }

        [Ignore("https://github.com/microsoft/onnxruntime/issues/2175")]
        [TestCase("Resources\\dog.jpeg")]
        [TestCase("Resources\\shannon.jpg")]
        public void TestArcFace(string imagePath)
        {
            // https://github.com/onnx/models/tree/master/vision/body_analysis/arcface
            // https://github.com/openvinotoolkit/open_model_zoo/tree/2021.4.1/models/public/mtcnn
            

            var srcImage = MemoryBitmap.Load(imagePath, GDICodec.Default);

            var srcImagePreprocessor = new ImageProcessor<Pixel.RGB96F>();

            var model = OnnxModel.FromFile("Models\\arcfaceresnet100-8.onnx");            

            using (var session = model.CreateSession())
            {
                var workingTensor = session.GetInputTensor<float>(0)
                    .AsSpanTensor4()
                    .GetSubTensor(0)
                    .SetImage(srcImage, srcImagePreprocessor);                

                // run

                session.Inference();

                // get results

                var result = session
                    .GetOutputTensor<float>(0)
                    .AsSpanTensor2()
                    .GetSubTensor(0)
                    .ToArray();

                var resultv4 = System.Runtime.InteropServices.MemoryMarshal.Cast<float, System.Numerics.Vector4>(result);

                for(int i=0; i < resultv4.Length; ++i)
                {
                    TestContext.WriteLine(resultv4[i]);
                }
            }
        }

        [TestCase("Resources\\dog.jpeg")]
        [TestCase("Resources\\shannon.jpg")]        
        [TestCase("Resources\\pixel-art.png")]        
        public void TestRealESRGAN(string imagePath)
        {
            // https://onnxruntime.ai/docs/tutorials/resnet50_csharp.html

            var srcImage = MemoryBitmap.Load(imagePath, GDICodec.Default);

            using var modelFactory = new MultiresModels(OnnxModel.FromFile);
            modelFactory.Register(16, 16, "Models\\realesrgan_16x16.onnx");
            modelFactory.Register(32, 32, "Models\\realesrgan_32x32.onnx");
            modelFactory.Register(64, 64, "Models\\realesrgan_64x64.onnx");
            modelFactory.Register(128, 128, "Models\\realesrgan_128x128.onnx");
            modelFactory.Register(256, 256, "Models\\realesrgan_256x256.onnx");
            modelFactory.Register(320, 240, "Models\\realesrgan_240x320.onnx");
            modelFactory.Register(320, 320, "Models\\realesrgan_320x320.onnx");
            modelFactory.Register(640, 480, "Models\\realesrgan_480x640.onnx");

            var imagePreprocessor = new ImageProcessor<Pixel.BGR96F>();

            var model = modelFactory.UseModel(srcImage.Width, srcImage.Height);
            var modelOptions = (model as IServiceProvider).GetService(typeof(Microsoft.ML.OnnxRuntime.SessionOptions)) as Microsoft.ML.OnnxRuntime.SessionOptions;
            modelOptions.GraphOptimizationLevel = Microsoft.ML.OnnxRuntime.GraphOptimizationLevel.ORT_DISABLE_ALL;

            using (var session = model.CreateSession())
            {
                var workingTensor = session.GetInputTensor<float>(0)
                    .AsSpanTensor4()
                    .GetSubTensor(0)
                    .SetImage(srcImage,imagePreprocessor);

                // run

                session.Inference();

                // get results

                var result = session.GetOutputTensor<float>(0)
                    .AsSpanTensor4()
                    .GetSubTensor(0);

                MemoryBitmap<Pixel.BGR24> resultBitmap = default;

                result
                    .AsTensorBitmap(Tensors.Imaging.ColorEncoding.BGR)
                    .CopyTo(ref resultBitmap);                

                var path = TestContext.CurrentContext.GetTestResultPath(imagePath);
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                resultBitmap.Save(path, GDICodec.Default);
                TestContext.AddTestAttachment(path);
            }
        }

        [TestCase("Resources\\dog.jpeg")]
        [TestCase("Resources\\shannon.jpg")]
        [TestCase("Resources\\pixel-art.png")]
        // [TestCase("Resources\\yukikaze.jpg")]
        public void TestAnime2Sketch(string imagePath)
        {
            var srcImage = MemoryBitmap.Load(imagePath, GDICodec.Default);
            MemoryBitmap<Pixel.Luminance8> dstImage = default;

            using(var filter = new Anime2SketchFilter())
            {
                filter.Filter(srcImage, ref dstImage);
            }

            var path = TestContext.CurrentContext.GetTestResultPath(imagePath);
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            dstImage.Save(path);
            TestContext.AddTestAttachment(path);
        }

        
        [TestCase("Resources\\shannon.jpg")]        
        public void TestFullStackFaceDetector(string imagePath)
        {
            var srcImage = MemoryBitmap.Load(imagePath, GDICodec.Default);            

            using (var filter = new FullStackFaceInfo.Detector())
            {
                var result = filter.Process(srcImage).ToArray();

                for(int i=0; i < result.Length; i++)
                {
                    var path = TestContext.CurrentContext.GetTestResultPath($"detected image {i}.jpg");
                    result[0].AlignedImage.Save(path);
                    TestContext.AddTestAttachment(path);
                }                
            }
        }
    }   


    /// <summary>
    /// Takes an image with colorized anime style content and strips the colors, leaving the ink lineart.
    /// </summary>
    class Anime2SketchFilter : IDisposable
    {
        public Anime2SketchFilter()
        {
            var model = OnnxModel.FromFile("Models\\anime2sketch_512x512.onnx");
            
            var modelOptions = (model as IServiceProvider).GetService(typeof(Microsoft.ML.OnnxRuntime.SessionOptions)) as Microsoft.ML.OnnxRuntime.SessionOptions;
            modelOptions.GraphOptimizationLevel = Microsoft.ML.OnnxRuntime.GraphOptimizationLevel.ORT_DISABLE_ALL;

            _Session = model.CreateSession();
        }

        public void Dispose()
        {
            System.Threading.Interlocked.Exchange(ref _Session, null)?.Dispose();
        }

        private IModelSession _Session;

        public void Filter(SpanBitmap srcImage, ref MemoryBitmap<Pixel.Luminance8> dstImage)
        {
            var srcTensor = new SpanTensor2<Pixel.RGB24>(srcImage.Height, srcImage.Width);
            srcTensor.AsSpanBitmap().AsTypeless().SetPixels(0, 0, srcImage);

            var stats = srcTensor.Statistics as Statistics.Vector3;

            var dstTensor = new SpanTensor2<Pixel.Luminance8>(srcImage.Height, srcImage.Width);

            // use a fixed 512x512 kernel to iterate through the image, processing small chunks
            srcTensor.CopyTo(dstTensor, new TensorSize2(512, 512), _kernel, new TensorIndices2(32,32));

            // copy the tensor data back to the dst image
            dstTensor.AsTensorBitmap(Tensors.Imaging.ColorEncoding.L).CopyTo(ref dstImage);
        }

        void _kernel(SpanTensor2<Pixel.RGB24> src, SpanTensor2<Pixel.Luminance8> dst)
        {
            // src & dst dimensions must be 512x512

            var workingTensor = _Session.GetInputTensor<float>(0)
            .AsSpanTensor4()
            .GetSubTensor(0)
            .VerifyDimensions(3, 512, 512);

            SpanTensor.Copy(src.DownCast<Byte>(), workingTensor[0], workingTensor[1], workingTensor[2], MultiplyAdd.CreateMul(255).GetInverse());

            // increase image contrast (optional)
            var imagePreprocessor = new ImageProcessor<Pixel.RGB96F>();
            imagePreprocessor.ColorTransform = MultiplyAdd.CreateAdd(-0.5f).ConcatMul(1.7f);

            imagePreprocessor.ColorTransform.ApplyTransformTo(workingTensor.Span);

            _Session.Inference();

            var result = _Session.GetOutputTensor<float>(0)
                .AsSpanTensor4()
                .GetSubTensor(0)
                .VerifyDimensions(1, 512, 512)
                .GetSubTensor(0);

            SpanTensor.Copy(result, dst.Cast<Byte>(), MultiplyAdd.CreateMul(255));
        }
    }



    struct FullStackFaceInfo
    {
        // https://github.com/atksh/onnx-facial-lmk-detector

        public float Score { get; set; }
        public System.Drawing.Rectangle BoundingBox { get; set; }
        public MemoryBitmap<Pixel.BGR24> AlignedImage { get; set; }
        public System.Numerics.Vector2[] Landmarks { get; set; }

        public class Detector : IDisposable
        {
            #region lifecycle

            public Detector()
            {
                var model = OnnxModel.FromFile("Models\\facial-lmk-detector.onnx");

                var modelOptions = (model as IServiceProvider).GetService(typeof(Microsoft.ML.OnnxRuntime.SessionOptions)) as Microsoft.ML.OnnxRuntime.SessionOptions;
                modelOptions.GraphOptimizationLevel = Microsoft.ML.OnnxRuntime.GraphOptimizationLevel.ORT_DISABLE_ALL;

                _Session = model.CreateSession();
            }

            public void Dispose()
            {
                System.Threading.Interlocked.Exchange(ref _Session, null)?.Dispose();
            }

            #endregion

            #region data

            private IModelSession _Session;

            #endregion

            #region API

            public FullStackFaceInfo[] Process(SpanBitmap srcImage)
            {
                var workingTensor = _Session.UseInputTensor<Byte>(0, srcImage.Height,srcImage.Width, 3)
                        .AsSpanTensor3()
                        .UpCast<Pixel.RGB24>();

                workingTensor.AsSpanBitmap().AsTypeless().SetPixels(0, 0, srcImage);

                _Session.Inference();

                var scores = _Session.GetOutputTensor<float>(0).VerifyName(n => n == "scores").AsSpanTensor1();
                var bboxes = _Session.GetOutputTensor<Int64>(1).VerifyName(n => n == "bboxes").AsSpanTensor2();
                var kpss = _Session.GetOutputTensor<Int64>(2).VerifyName(n => n == "kpss").AsSpanTensor3();
                var align_imgs = _Session.GetOutputTensor<byte>(3).VerifyName(n => n == "align_imgs").AsSpanTensor4().UpCast<Pixel.BGR24>();
                var lmks = _Session.GetOutputTensor<Int64>(4).VerifyName(n => n == "lmks").AsSpanTensor3();
                var M = _Session.GetOutputTensor<float>(5).VerifyName(n => n == "M").AsSpanTensor3().UpCast<System.Numerics.Vector3>();

                var result = new FullStackFaceInfo[scores.Dimensions[0]];

                for(int i=0; i < scores.Dimensions[0]; ++i)
                {
                    result[i].Score = scores[i];

                    var bbox = bboxes[i];
                    result[i].BoundingBox = new System.Drawing.Rectangle((int)bbox[0], (int)bbox[1], (int)bbox[2], (int)bbox[3]);

                    MemoryBitmap<Pixel.BGR24> bmp = default;
                    align_imgs[i].AsTensorBitmap(Tensors.Imaging.ColorEncoding.RGB).CopyTo(ref bmp);
                    result[i].AlignedImage = bmp;

                    var lmarks = lmks[i].ToArray();
                }

                return result;
            }

            #endregion
        }
    }
}
