using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Headless;


using NUnit.Framework;
using Avalonia.Headless.NUnit;

[assembly: AvaloniaTestApplication(typeof(AvaloniaAppBuilder))]

public class AvaloniaAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
        .UseHeadless(new AvaloniaHeadlessPlatformOptions());

    public class App : Application
    {
        public override void Initialize()
        {
            // AvaloniaXamlLoader.Load(this);
        }
    }
}

namespace InteropTypes.Graphics.Bitmaps.Backends
{
    internal class AvaloniaTests
    {
        [AvaloniaTest]
        [TestCase("shannon.jpg")] // not working, it displays a white image
        [TestCase("diagram.jpg")]
        [TestCase("white.png")]
        public void LoadImage(string filePath)
        {
            filePath = ResourceInfo.From(filePath);

            var bitmap = MemoryBitmap.Load(filePath, InteropTypes.Codecs.GDICodec.Default);
            new AttachmentInfo("Original.png").WriteObject(f => bitmap.Save(f));

            // convert to avalonia
            var avbmp = InteropTypes.Graphics.Backends.AvaloniaToolkit.ToAvaloniaBitmap(bitmap);

            // convert back to memorybitmap
            var converted = InteropTypes.Graphics.Backends.AvaloniaToolkit.ToMemoryBitmap(avbmp);            

            new AttachmentInfo("Result.png").WriteObject( f=> converted.Save(f) );
        }
    }
}
