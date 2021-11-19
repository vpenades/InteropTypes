using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTensors;
using InteropBitmaps;

using NUnit.Framework;

namespace InteropVision.Backends
{
    public class OnnxExample1
    {
        [SetUp]
        public void Setup()
        {
            InteropWith.OnnxModel.DeviceID = -1;
        }        

        [TestCase("Resources\\dog.jpeg")]
        [TestCase("Resources\\shannon.jpg")]
        public void TestResnet50(string imagePath)
        {
            // https://onnxruntime.ai/docs/tutorials/resnet50_csharp.html            

            var srcImage = MemoryBitmap.Load(imagePath, InteropBitmaps.Codecs.GDICodec.Default);

            var model = InteropWith.OnnxModel.FromFile("Models\\resnet50-v2-7.onnx");

            // image normalization
            // https://github.com/onnx/models/tree/master/vision/classification/resnet#preprocessing            
            var modelXform = Mad3
                .CreateMul(0.229f, 0.224f, 0.225f)
                .ConcatAdd(0.485f, 0.456f, 0.406f)
                .GetTransposedZYX()
                .GetInverse();

            var imagePreprocessor = new ImageProcessor<Pixel.VectorRGB>(modelXform);            

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
            }
        }

        [TestCase("Resources\\shannon.jpg")]
        public void TestMcnnFace(string imagePath)
        {
            var srcImage = MemoryBitmap.Load(imagePath, InteropBitmaps.Codecs.GDICodec.Default);

            var model = InteropWith.OnnxModel.FromFile("Models\\MCNN\\pnet.onnx");
            var imagePreprocessor = new ImageProcessor<Pixel.VectorRGB>();

            using (var session = model.CreateSession())
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
                    .UpCast<System.Numerics.Vector2>();

                var cv2d = conv2d
                    .AsSpanTensor4()
                    .GetSubTensor(0)
                    .UpCast<System.Numerics.Vector4>();
            }
        }


        [TestCase("Resources\\dog.jpeg")]
        [TestCase("Resources\\shannon.jpg")]
        public void TestArcFace(string imagePath)
        {
            // https://github.com/onnx/models/tree/master/vision/body_analysis/arcface
            // https://github.com/openvinotoolkit/open_model_zoo/tree/2021.4.1/models/public/mtcnn
            // https://github.com/linxiaohui/mtcnn-opencv/tree/main/mtcnn_cv2

            var srcImage = MemoryBitmap.Load(imagePath, InteropBitmaps.Codecs.GDICodec.Default);

            var model = InteropWith.OnnxModel.FromFile("Models\\arcfaceresnet100-8.onnx");

            var imagePreprocessor = new ImageProcessor<Pixel.VectorRGB>();

            using (var session = model.CreateSession())
            {
                var workingTensor = session.GetInputTensor<float>(0)
                    .AsSpanTensor4()
                    .GetSubTensor(0)
                    .SetImage(srcImage, imagePreprocessor);                

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

            var srcImage = MemoryBitmap.Load(imagePath, InteropBitmaps.Codecs.GDICodec.Default);

            var modelFactory = new MultiresModels(InteropWith.OnnxModel.FromFile);
            modelFactory.Register(16, 16, "Models\\realesrgan_16x16.onnx");
            modelFactory.Register(32, 32, "Models\\realesrgan_32x32.onnx");
            modelFactory.Register(64, 64, "Models\\realesrgan_64x64.onnx");
            modelFactory.Register(128, 128, "Models\\realesrgan_128x128.onnx");
            modelFactory.Register(256, 256, "Models\\realesrgan_256x256.onnx");
            modelFactory.Register(320, 240, "Models\\realesrgan_240x320.onnx");
            modelFactory.Register(320, 320, "Models\\realesrgan_320x320.onnx");
            modelFactory.Register(640, 480, "Models\\realesrgan_480x640.onnx");

            var imagePreprocessor = new ImageProcessor<Pixel.VectorBGR>();

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

                var resultBitmap = ToBitmap(result);

                var path = TestContext.CurrentContext.GetTestResultPath(imagePath);
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                resultBitmap.Save(path, InteropBitmaps.Codecs.GDICodec.Default);
                TestContext.AddTestAttachment(path);
            }
        }

        private static SpanBitmap<System.Numerics.Vector3> ToBitmap(SpanTensor3<float> srcTensor)
        {
            var span = srcTensor.Span;

            for (int i = 0; i < span.Length; ++i)
            {
                span[i] = Math.Min(1, Math.Max(0, span[i]));
            }

            if (srcTensor.Dimensions[2] == 3)
            {
                return srcTensor.UpCast<System.Numerics.Vector3>().AsSpanBitmap();
            }

            if (srcTensor.Dimensions[0] == 3)
            {
                var h = srcTensor.Dimensions[1];
                var w = srcTensor.Dimensions[2];
                var tmpTensor = new SpanTensor2<System.Numerics.Vector3>(h, w);
                SpanTensor.Copy(srcTensor, tmpTensor, Mad3.Identity);

                return tmpTensor.AsSpanBitmap();
            }

            throw new NotImplementedException();
        }
    }
}
