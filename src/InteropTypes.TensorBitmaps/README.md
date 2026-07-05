
This project contains some bare bones classes to wrap System.Numerics.Tensors with a Bitmap API facade.

### Bitmap core types

|Tensor type|Bitmap type|
|-|-|
|`Tensor<TElement>`|`TensorBitmap<TElement,TPixel>`|
|`TensorSpan<TElement>`|`TensorSpanBitmap<TElement,TPixel>`|
|`ReadOnlyTensorSpan<TElement>`|`ReadOnlyTensorSpanBitmap<TElement,TPixel>`|

TElement is the type of the underlaying tensor as long as a component of the TPixel, so for example we can do:

`TensorBitmap<byte,Rgb24>`   for a pixel format that uses 3 bytes

or

`TensorBitmap<float,Vector3>` for a float based vector that uses 3 floats

but it is also possible to do

`TensorBitmap<byte,Vector3>` for a bye based vector that uses 12 bytes casted to 3 floats

The three types have the same exact API:

```c#
class TensorBitmap<TElement,TPixel>
{
    public int Width { get; }
    public int Height { get; }
    public Span<TPixel> GetRowPixelsSpan(y);
    TensorBitmap<TElement,TPixel> GetCropped(System.Drawing.Rectangle cropRect);
}
```

`GetCropped` returns a clipped area of the original surface without allocating new memory

### pixel formats

Bitmaps usually require declaring some kind of pixel format.
Tensors do require a lot of flexibility to define its pixel format,
and at the same time it's important to keep things simple.

Declaring a pixel type is done using two types:

- `record TensorPixelFormat`
- `record TensorPixelComponent`

Where `TensorPixelComponent` is just a collection of `TensorPixelComponent` plus a few predefined formats like:

- `TensorPixelFormat.Rgb24`
- `TensorPixelFormat.Rgb96f`
- `TensorPixelFormat.Rgba32`
- etc

But defining a custom format is extremely simple:

```c#
var infrared = new TensorPixelComponent("Infrared",false,0,1);
var depth = new TensorPixelComponent("Depth",false,0,500);
var customFormat = new TensorPixelFormat(infrared, depth);
```

The component class also defines a minimum and maximum value,
which can be useful to automate the conversion between tensors
that require aplying a standard-deviation pattern for each pixels, for example:

```c#
var red = new TensorPixelComponent("Red",false, -0.823, +0.7432);
var green = new TensorPixelComponent("Green",false, -0.923, +0.9432);
var blue = new TensorPixelComponent("Blue",false, -0.623, +0.5432);
var tensorFormat = new TensorPixelFormat(red,green,blue);
```

This pixel format design also has the advantage to seamlessly translate
to CHW tensors that store each component per plane, where we would have
a bitmap per plane, and each bitmap defining a single pixel component.

For example:

```c#
var tensor = Tensor.Create<float>(3,256,256); // CHW tensor

TensorBitmap<float,Vector3>.CreatePlanes(
    tensor,
    TensorPixelFormat.Rgb96f,
    out TensorBitmap<float,float> redPlane,
    out TensorBitmap<float,float> greenPlane,
    out TensorBitmap<float,float> bluePlane);
```

where redPlane, greenPlane and bluePlane represents each plane, and has captured each component of Rgb96f separately










