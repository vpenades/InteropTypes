using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropTypes
{
    internal class FilePreviewTests
    {
        public static void RunTests()
        {
            var instance = new FilePreviewTests();
            instance._Run();
        }

        private void _Run()
        {
            TestFilePreview();
        }        

        public void TestFilePreview()
        {
            var finfo = new System.IO.FileInfo(Environment.ProcessPath);

            var bmp = InteropTypes.IO.FilePreviewFactory.GetPreviewOrDefault(finfo);

            var bmpPath = System.IO.Path.ChangeExtension(Environment.ProcessPath, ".bmp");

            bmp.Save(bmpPath);
        }


    }
}
