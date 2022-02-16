

### InteropTypes.Graphics.Drawing.Core

##### Motivation

In many projects, it is often common to require drawing graphics in one way or another.

In the early days of computer graphics the resources where limited, but that also resulted in simple
and straightforward graphics APIs. Over time, graphics technologies have evolved, and also the graphics
APIs, which have become too complex for simple tasks. How many lines of code are required to draw a circle
using the Vulkan API, or the later iterations of DirectX or OpenGL?

The motivation of this library is to "go back" to the early days and provide a simple API for simple graphics.

Example:

```c#
using InteropTypes.Graphics.Drawing;

class Circle : IDrawingBrush<ICanvas2D>
{
    public int X;
    public int Y;
    public int Radius;

    public override ToString()
    {
        return $"{X} {Y} {Radius}";
    }

    public void DrawTo(ICanvas2D canvas)
    {
        canvas.DrawEllipse( (X,Y), Radius, Radius, ColorStyle.RED) );
    }
}

```

The interfaces, specially ICanvas2D and IScene3D are implemented by external backends, allowing the same
object to be "drawable" into many render target contexts and platforms.

##### Design Goals

- Provide a simple, yet powerful drawing API.
- Keep the API as simple as possible to make backends development as easy as possible.
- Support 2D drawing APIs.
- Support 3D drawing APIs.
- Drawing API must be usable for both document creation and real time rendering.
- Cross platform.

##### Non Goals

- Not a fully featured rendering API.
- Focus on simplicity and ease of use, not performance.