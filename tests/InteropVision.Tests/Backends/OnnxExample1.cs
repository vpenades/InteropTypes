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
        [TestCase("Resources\\dog.jpeg")]
        [TestCase("Resources\\shannon.jpg")]
        public void TestResnet50(string imagePath)
        {
            // https://onnxruntime.ai/docs/tutorials/resnet50_csharp.html

            var srcImage = InteropBitmaps.MemoryBitmap.Load(imagePath, InteropBitmaps.Codecs.GDICodec.Default);
            
            var model = InteropWith.OnnxModel.FromFile("Models\\resnet50-v2-7.onnx");
            var mean = new System.Numerics.Vector3(0.485f, 0.456f, 0.406f);
            var stddev = new System.Numerics.Vector3(0.229f, 0.224f, 0.225f);

            using (var session = model.CreateSession())
            {
                var workingTensor = session.GetInputTensor<float>(0)
                    .AsSpanTensor4()
                    .GetSubTensor(0);

                // copy image to tensors

                var h = workingTensor.Dimensions[1];
                var w = workingTensor.Dimensions[2];
                
                var tmpTensor = new SpanTensor2<System.Numerics.Vector3>(h, w);
                SpanTensor.FitBitmap(tmpTensor, srcImage);                

                var channelR = workingTensor.GetSubTensor(0);
                var channelG = workingTensor.GetSubTensor(1);
                var channelB = workingTensor.GetSubTensor(2);
                SpanTensor.Copy(tmpTensor, channelB, channelG, channelR, -mean, System.Numerics.Vector3.One / stddev);

                // run

                session.Inference();

                // get results

                var result = session
                    .GetOutputTensor<float>(0)
                    .AsSpanTensor2()
                    .GetSubTensor(0)
                    .Span
                    .ToArray();

                float sum = result.Sum(x => (float)Math.Exp(x));
                var softmax = result.Select(x => (float)Math.Exp(x) / sum);

                var pairs = Labels.resnet50_v2_7_Labels
                    .Zip(softmax, (Label,Score) =>(Label,Score))
                    .OrderByDescending(item => item.Score)
                    .ToList();

                foreach(var pair in pairs)
                {
                    TestContext.WriteLine($"{pair.Label} = {pair.Score}");
                }

            }
        }


        [TestCase("Resources\\dog.jpeg")]
        [TestCase("Resources\\shannon.jpg")]        
        public void TestRealESRGAN(string imagePath)
        {
            // https://onnxruntime.ai/docs/tutorials/resnet50_csharp.html

            var srcImage = InteropBitmaps.MemoryBitmap.Load(imagePath, InteropBitmaps.Codecs.GDICodec.Default);

            // select model based on input size
            var srcSize = Math.Min(srcImage.Width, srcImage.Height);
            var modelPath = "Models\\realesrgan_256x256.onnx";
            if (srcSize <= 128) modelPath = "Models\\realesrgan_128x128.onnx";
            if (srcSize <= 64) modelPath = "Models\\realesrgan_64x64.onnx";
            if (srcSize <= 32) modelPath = "Models\\realesrgan_32x32.onnx";
            if (srcSize <= 16) modelPath = "Models\\realesrgan_16x16.onnx";            

            var model = InteropWith.OnnxModel.FromFile(modelPath);            

            using (var session = model.CreateSession())
            {
                var workingTensor = session.GetInputTensor<float>(0)
                    .AsSpanTensor4()
                    .GetSubTensor(0);

                // copy image to tensors

                var h = workingTensor.Dimensions[1];
                var w = workingTensor.Dimensions[2];
                var tmpTensor = new SpanTensor2<System.Numerics.Vector3>(h, w);

                SpanTensor.FitBitmap(tmpTensor, srcImage);

                var channelR = workingTensor.GetSubTensor(0);
                var channelG = workingTensor.GetSubTensor(1);
                var channelB = workingTensor.GetSubTensor(2);
                SpanTensor.CopyTo(tmpTensor, channelB, channelG, channelR);

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

        private static SpanBitmap<System.Numerics.Vector3> ToBitmap(SpanTensor3<float> tensor)
        {
            var span = tensor.Span;

            for (int i = 0; i < span.Length; ++i)
            {
                span[i] = Math.Min(1, Math.Max(0, span[i]));

            }
            if (tensor.Dimensions[2] == 3)
            {
                return tensor.UpCast<System.Numerics.Vector3>().AsSpanBitmap();
            }

            if (tensor.Dimensions[0] == 3)
            {
                var channelR = tensor.GetSubTensor(0);
                var channelG = tensor.GetSubTensor(1);
                var channelB = tensor.GetSubTensor(2);

                var h = tensor.Dimensions[1];
                var w = tensor.Dimensions[2];
                var tmpTensor = new SpanTensor2<System.Numerics.Vector3>(h, w);
                SpanTensor.Copy(channelB, channelG, channelR, System.Numerics.Vector3.One, System.Numerics.Vector3.Zero, tmpTensor);

                return tmpTensor.AsSpanBitmap();
            }

            throw new NotImplementedException();
        }


    }
}
