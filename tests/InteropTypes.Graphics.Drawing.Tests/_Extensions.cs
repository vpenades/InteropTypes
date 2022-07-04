using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Backends;

namespace InteropTypes.Graphics.Drawing
{
    static class _Extensions
    {
        public static void AttachToCurrentTest(this Record2D batch, string filePath)
        {
            var ainfo = NUnit.Framework.AttachmentInfo.From(filePath);

            if (filePath.ToLower().EndsWith(".svg"))
            {
                ainfo.WriteObject(f => SVGSceneDrawing2D.SaveToSVG(f.FullName, batch) );
                return;
            }

            if (filePath.ToLower().EndsWith(".png") || filePath.ToLower().EndsWith(".jpg") || filePath.ToLower().EndsWith(".gif"))
            {
                ainfo.WriteObject(f => Canvas2DFactory.SaveToBitmap(f.FullName, 1024, 1024, null, batch) );
                return;
            }
        }       

        
        public static string AttachToCurrentTest(this Record3D batch, string filePath)
        {
            var ainfo = NUnit.Framework.AttachmentInfo.From(filePath);

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
                var html = PlotlyDocumentBuilder.ConvertToHtml(batch);
                ainfo.WriteAllText(html);
            }

            if (filePath.ToLower().EndsWith(".InteropDrawing"))
            {
                
            }

            if (filePath.ToLower().EndsWith(".gltf") || filePath.ToLower().EndsWith(".glb") || filePath.ToLower().EndsWith(".obj"))
            {
                var model = GltfSceneBuilder
                    .Convert(batch)
                    .ToGltf2();

                ainfo.WriteObject(f => model.Save(f.FullName));
            }

            return filePath;
        }           
    }
}
