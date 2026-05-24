using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.IO
{
    internal class GetThumbnailTests
    {
        [Test]
        [Arguments("note.txt")]
        [Arguments("doc.json")]
        public async Task GetThumbnail(string name)
        {
            var finfo = new System.IO.FileInfo($"Assets/{name}");

            var bmp = FilePreviewFactory.GetPreviewOrDefault(finfo);

            await Assert.That(bmp).IsNotNull();

            bmp.Save($"{name}.bmp");
        }
    }
}
