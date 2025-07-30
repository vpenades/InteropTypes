using Foundation;

using Microsoft.Maui.Controls;

using QuickLookThumbnailing;

using System.IO;
using System.Threading.Tasks;

using UIKit;

namespace InteropTypes.Platforms.IOS
{
    internal class IOSThumbnailService
    {
        public static async partial Task<System.IO.Stream?> GetStreamThumbnailAsync(string filePath, int width, int height)
        {
            var fileUrl = NSUrl.FromFilename(filePath);

            var request = new QLThumbnailGeneratorRequest(
                fileUrl,
                new CoreGraphics.CGSize(width, height),
                QLThumbnailGeneratorRequestRepresentationTypes.Icon);

            var sem = new System.Threading.SemaphoreSlim(0, 1);
            QLThumbnailRepresentation? repr = null;

            QLThumbnailGenerator.SharedGenerator.GenerateBestRepresentation(
                request,
                (thumbnail, error) =>
                {
                    repr = thumbnail;
                    sem.Release();
                });

            await sem.WaitAsync();

            if (repr?.Image is UIImage img)
            {
                var data = img.AsPNG();
                return new MemoryStream(data.ToArray());
            }

            return null;
        }
    }
}
