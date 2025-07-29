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
            // TestReferenceGetIcon1();
            // TestReferenceGetIcon2();
            TestReferenceGetIcon3();
            TestFilePreview();
        }        

        public void TestFilePreview()
        {
            var finfo = new System.IO.FileInfo(Environment.ProcessPath);

            var bmp = InteropTypes.IO.FilePreviewFactory.GetPreviewOrDefault(finfo);

            var bmpPath = System.IO.Path.ChangeExtension(Environment.ProcessPath, ".bmp");

            bmp.Save(bmpPath);
        }

        public void TestReferenceGetIcon1()
        {
            var srcPath = System.IO.Path.Combine(AppContext.BaseDirectory, "shannon.jpg");
            var dstPath = System.IO.Path.Combine(AppContext.BaseDirectory, "shannon.ico");

            using var icon = ExtractIcon1.GetIcon(srcPath, false);

            using (var s = System.IO.File.Create(dstPath))
            {
                icon.Save(s);
            }            
        }

        public void TestReferenceGetIcon2()
        {
            var srcPath = System.IO.Path.Combine(AppContext.BaseDirectory, "shannon.jpg");
            var dstPath = System.IO.Path.Combine(AppContext.BaseDirectory, "shannon.bmp");

            using var sntb = new ShellThumbnail();

            using var bmp = sntb.GetThumbnail(srcPath);

            bmp.Save(dstPath);
        }

        public void TestReferenceGetIcon3()
        {
            var srcPath = System.IO.Path.Combine(AppContext.BaseDirectory, "shannon.jpg");
            var dstPath = System.IO.Path.Combine(AppContext.BaseDirectory, "shannon2.bmp");

            using var bmp = ShellThumbnail2.GetThumbnail(srcPath);

            bmp.Save(dstPath);
        }


    }
}
