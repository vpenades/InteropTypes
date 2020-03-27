## InteropBitmaps

An experimental, proof of concept, low level library to exchange bitmaps
between imaging libraries.

There's already a good number of great imaging libraries around, some
of them are general purpose, while others solve specific problems,
but certainly, there's no imaging library that fits all needs, so in
many cases, developers are forced to use multiple imaging libraries
concurrently.

This is specially true when dealing with image acquisition devices,
GPU textures, or client bitmaps like GDI or WIC,
that typically use unmanaged memory to store bitmaps, and forces
developers to write cumbersome and sometimes dangerous code.

Exchanging bitmaps between libraries is a challenge, and __InteropBitmaps__
is an attempt to solve this issue by providing a number of low level
bitmap types that can be instantiated over existing memory:

|Memory type|Bitmap type|
|-|-|
|`IntPtr`|`PointerBitmap`|
|`Span<Byte>`|`SpanBitmap`, `SpanBitmap<TPixel>`|
|`Memory<Byte>`|`MemoryBitmap`, `MemoryBitmap<TPixel>`|

These types simply wrap previously existing memory, and provide mechanisms
to access bitmap data in a safer way.

### Backends

In order to interoperate with commonly used libraries, a number of
backend extensions are provided.

Notice that using these backends is entirely optional.

|Target Library|Package|
|-|-|
|System.Drawing.Common|InteropBitmaps.Backends.GDI|
|System.Windows.Media|InteropBitmaps.Backends.WPF|
|SixLabors.ImageSharp|InteropBitmaps.Backends.ImageSharp|
|SkiaSharp|InteropBitmaps.Backends.SkiaSharp|
|StbImageLib|InteropBitmaps.Backends.STB|

Additionally, I'm experimenting with other image based
libraries:

|Target Library|Package|
|-|-|
|OpenCVSharp4|InteropBitmaps.Backends.OpenCVSharp4|
|ZXing|InteropBitmaps.Backends.ZXing|
|FaceRecognitionDotNet|InteropBitmaps.Backends.Takeuchi.FaceRecognition|

Other libraries I am considering are: ImageMagick, SharpDX,
and some sensor libraries like Kinectv2, Orbbec and Nuitrack. 

### Example

Here's an example of using different backends:

```c#

// Use SkiaSharp to load a WEBP image:
var bmp = MemoryBitmap.Load("shannon.webp", Codecs.SkiaCodec.Default);

// Use OpenCV to resize the image:
bmp = bmp
    .WithOpenCv()
    .ToResizedMemoryBitmap(55, 55, OpenCvSharp.InterpolationFlags.Lanczos4)

// Use GDI to draw a triangle:
var a = new System.Drawing.Point(5, 5);
var b = new System.Drawing.Point(50, 50);
var c = new System.Drawing.Point(5, 50);        
bmp
    .WithGDI()
    .Draw(dc => dc.DrawPolygon(System.Drawing.Pens.Red, new[] { a, b, c }));

// Use Imagesharp to save to PNG
bmp.Save("shannon.png", Codecs.ImageSharpCodec.Default);

```








