# InteropBitmaps

This is not "Yet Another imaging library", but a collection of very
low level image types that simplify interacting with bitmaps at low level.

As a comparison with the lowest level memory types in C#:

|C# memory type|Bitmap type|
|-|-|
|IntPtr|PointerBitmap|
|Span&lt;T&gt;|SpanBitmap and SpanBitmap&lt;T&gt;|
|Memory&lt;T&gt;|MemoryBitmap and MemoryBitmap&lt;T&gt;|

Unlike most imaging libraries around, InteropBitmaps does not provide a
memory allocation strategy and simply wraps the memory already allocated
by others.

This feature makes InteropBitmaps a very convenient library to interop with
image adquisition APIs, or exchanging bitmap objects between imaging libraries
avoiding memory allocations or expensive bitmap conversions.








