using System;
using System.Collections.Generic;
using System.Text;

namespace InteropDrawing
{
    static class _Extensions
    {
        public static string UseFilePath(this NUnit.Framework.TestContext context, string fileName)
        {
            if (System.IO.Path.IsPathRooted(fileName)) return fileName;

            var dir = System.IO.Path.Combine(context.TestDirectory, "TestResults");

            System.IO.Directory.CreateDirectory(dir);

            return System.IO.Path.Combine(dir, $"{context.Test.ID}_{fileName}");
        }

        public static void Attach<T>(this NUnit.Framework.TestContext context, string fileName, T value, Action<string,T> saveCallback)
        {
            fileName = context.UseFilePath(fileName);
            saveCallback(fileName, value);
            NUnit.Framework.TestContext.AddTestAttachment(fileName);
        }

        public static void AttachToCurrentTest(this Record2D batch, string filePath)
        {
            filePath = NUnit.Framework.TestContext.CurrentContext.UseFilePath(filePath);

            if (filePath.ToLower().EndsWith(".svg"))
            {
                Backends.SVGSceneDrawing2D.SaveToSVG(filePath, batch);
                NUnit.Framework.TestContext.AddTestAttachment(filePath);
                return;
            }

            if (filePath.ToLower().EndsWith(".png") || filePath.ToLower().EndsWith(".jpg") || filePath.ToLower().EndsWith(".gif"))
            {
                
                Backends.WPFDrawingContext2D.SaveToBitmap(filePath, 1024, 1024, null, batch);
                NUnit.Framework.TestContext.AddTestAttachment(filePath);
                return;
            }
        }

        [Obsolete]
        public static void AttachToCurrentTestAsPlot(this Record2D batch, string filePath)
        {
            throw new NotImplementedException("Switch to Plotly");

            // filePath = NUnit.Framework.TestContext.CurrentContext.GetFilePath(filePath);

            // Backends.PLPlotToolkit.SaveToFile(filePath, batch);
            // NUnit.Framework.TestContext.AddTestAttachment(filePath);
            return;

        }

        
        public static void AttachToCurrentTest(this Record3D batch, string filePath)
        {
            filePath = NUnit.Framework.TestContext.CurrentContext.UseFilePath(filePath);

            if (filePath.ToLower().EndsWith(".stl"))
            {
                throw new NotImplementedException();

                /*
                var mb = new Backends.STLMeshBuilder();
                batch.DrawTo(mb);
                var wfcontent = mb.ToAsciiSTL();
                System.IO.File.WriteAllText(filePath, wfcontent);
                NUnit.Framework.TestContext.AddTestAttachment(filePath);
                return;
                */
            }

            if (filePath.ToLower().EndsWith(".html"))
            {
                var html = Backends.PlotlyDocumentBuilder.ConvertToHtml(batch);
                System.IO.File.WriteAllText(filePath, html);
                NUnit.Framework.TestContext.AddTestAttachment(filePath);
            }

            if (filePath.ToLower().EndsWith(".gltf") || filePath.ToLower().EndsWith(".glb") || filePath.ToLower().EndsWith(".obj"))
            {
                Backends.GltfSceneBuilder
                    .Convert(batch)
                    .ToGltf2()
                    .Save(filePath);
                
                NUnit.Framework.TestContext.AddTestAttachment(filePath);
                return;
            }
        }

        /*
        public static void Attach(this NUnit.Framework.TestContext context, string fileName, Backends.STLMeshBuilder mesh)
        {
            var filePath = context.GetFilePath(fileName);

            var stl = mesh.ToAsciiSTL();

            System.IO.File.WriteAllText(filePath, stl);

            NUnit.Framework.TestContext.AddTestAttachment(filePath);
        }*/

        public static void AttachShowDirLink(this NUnit.Framework.TestContext context)
        {
            var dirPath = System.IO.Path.GetDirectoryName(context.UseFilePath("hello.txt"));

            context.AttachFileLink("📂 Show Directory", dirPath);
        }

        public static void AttachFileLink(this NUnit.Framework.TestContext context, string linkPath, string targetPath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("[InternetShortcut]");
            sb.AppendLine("URL=file:///" + targetPath);
            sb.AppendLine("IconIndex=0");
            string icon = targetPath.Replace('\\', '/');
            sb.AppendLine("IconFile=" + icon);

            linkPath = System.IO.Path.ChangeExtension(linkPath, ".url");
            linkPath = context.UseFilePath(linkPath);

            System.IO.File.WriteAllText(linkPath, sb.ToString());

            NUnit.Framework.TestContext.AddTestAttachment(linkPath);
        }
    }
}
