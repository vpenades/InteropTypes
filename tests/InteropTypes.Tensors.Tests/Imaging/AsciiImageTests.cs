using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using BGR24 = InteropTypes.Graphics.Bitmaps.Pixel.BGR24;

namespace InteropTypes.Tensors.Imaging
{
    internal class AsciiImageTests
    {
        [Test]
        public void TestMonospaceChars()
        {
            var rows = new List<string>();

            rows.Add("----------------------------------------------");

            for(int i=0; i <32; i++)
            {
                var c = (char)(0x2580 + i);

                var cc = c.ToString();                

                rows.Add(string.Empty + cc + cc + cc + cc + cc + "| " + ((int)c).ToString("x"));
            }

            foreach (var cc in new string[] { " ", "  ", "   ", "    " })
            {
                rows.Add(string.Empty + cc + cc + cc + cc + cc + "|");
            }


            rows.Add("▙ |");
            rows.Add("▒|");


            foreach (var row in rows)
            {
                TestContext.Out.WriteLine(row);
            }

        }

        
        [Test]
        public void DisplayAsciiImage()
        {
            var imgPath = ResourceInfo.From("cat.png");

            var img = Graphics.Bitmaps.MemoryBitmap<BGR24>.Load(imgPath);

            if (!img.AsSpanBitmap().TryGetAsSpanTensor(out var src)) throw new InvalidOperationException();

            foreach(var row in src.DebugBitmap.Rows)
            {
                TestContext.Out.WriteLine(row);
            }           
        }

    }
}
