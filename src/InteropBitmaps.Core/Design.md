



### Pixel format enum

los formatos de pixel enumerados son largos hasta el infinito, y no muy flexibles.

tal vez seria mejor un formato de elemento:

PixelElementFormat
{
Undefined
QuantizedRed8
QuantizedRed16
QuantizedGreen8
QuantizedBlue16
}

y luego un struct de tipo

readonly struct PixelFormat
{
    private readonly uint _PackedFormat;

    // 8 bytes para cada element

    public PixelElementFormat Element0 => ...
    public PixelElementFormat Element1 => ...
    public PixelElementFormat Element2 => ...
    public PixelElementFormat Element3 => ...
}

la forma de calcular el tamaño: sumar los bits y redondear a 8.


### Tipos

- `MemoryBitmap`
- `MemoryBitmap<TPixel>`
- `SpanBitmap`
- `SpanBitmap<TPixel>`
- `ArrayBitmap` - Realmente un ArraySegment
- `ArrayBitmap<TPixel>`
- `IBitmap`
- `IBitmap<TPixel>`

SpanBitmap puede hacer cast a cualquier tipo, pero MemoryBitmap no.

Las soluciones podrian ser:
- `MemoryBitmap<T>`
- `MemoryBitmap<T,TPixel>`

Pero esta solucion dispara las permutaciones.
Realmente haria falta algun tipo de solucion para interoperar con ImageSharp sin copiar memoria,
pero hay operaciones como Resize que te acaban dando una copia igualmente, así que tampoco te
libras...

### Conversion de tipos

||||
|:-:|:-:|:-:|
|`MemoryBitmap<T>`|🡺Base Class🡺<br>🡸`CloneToMemoryBitmap<T>`🡸|`MemoryBitmap`|
|🡹`AsMemoryBitmap`🡹<br>🡻Implicit🡻|*|🡻Implicit🡻<br>|
|`SpanBitmap<T>`|🡺Implicit🡺<br>🡸`AsSpanBitmap<T>`🡸|`SpanBitmap`|


### backends

|backend|Types|memory model|Disposing|
|-|-|
|System.Drawing.Common|`Image`,`Bitmap`|unmanaged heap|required|
|ImageSharp|`Image`, `Image<TPixel>`|custom memory model|Required|
|OpenCvSharp4|`Map`, `Map<TPixel>`|unmanaged heap|Required|

Other candidates:
- SkiaSharp
- ImageMagick
- Kinectv2, Orbbec, Nuitrack
- Microsoft.ML
- PLPlot
- Xing
- FaceRecognitionDotNet
- http://sharpdx.org/wiki/class-library-api/wic/

https://docs.microsoft.com/es-es/windows/win32/wic/-wic-lh?redirectedfrom=MSDN