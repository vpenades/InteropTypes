using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Drawing;

namespace InteropTypes
{
    public class AppMVVM : IDrawingBrush<IScene3D>
    {
        public AppMVVM()
        {
            

            MemoryBitmapTypeless.AsSpanBitmap().SetPixels(System.Drawing.Color.Green);
            MemoryBitmapBGRA32.AsSpanBitmap().AsTypeless().SetPixels(System.Drawing.Color.Red);            

            BindableBitmap.Bitmap = MemoryBitmapBGRA32;
       }
        
        public void DrawTo(IScene3D context)
        {
            context.DrawSphere((0,0,0), 2, ColorStyle.Red);            
        }       

        public ICommand OnPaintMemoryBitmapCmd { get; protected set; }

        public Sphere Item1 => new Sphere();

        public Cube Item2 => new Cube();

        public SentLand Sentinel => SentLand.GenerateLandscape(0);

        public IDrawingBrush<IScene3D> Combined => DrawingToolkit.Combine(Item1, Item2);

        public ThunderbirdRocket Rocket => new ThunderbirdRocket(System.Numerics.Matrix4x4.CreateTranslation(20,0,0));

        public IDrawingBrush<IScene3D> Rockets =>
            DrawingToolkit.Combine
            (
                new ThunderbirdRocket(System.Numerics.Matrix4x4.CreateTranslation(20, 0, 0)).Tinted(ColorStyle.Green),
                new ThunderbirdRocket(System.Numerics.Matrix4x4.CreateTranslation(0, 20, 0)).Tinted(ColorStyle.Red)
            );        

        public BindableBitmap BindableBitmap { get; } = new BindableBitmap();

        public BindableBitmap BindableBitmapUndefined { get; } = new BindableBitmap();

        public MemoryBitmap MemoryBitmapTypeless { get; } = new MemoryBitmap(512, 512, Pixel.BGRA32.Format);

        public MemoryBitmap<Pixel.BGRA32> MemoryBitmapBGRA32 { get; } = new MemoryBitmap<Pixel.BGRA32>(512, 512, Pixel.BGRA32.Format);


        public IDrawingBrush<ICanvas2D> Canvas1 { get; } = new BasicScene2D();

        public IDrawingBrush<ICanvas2D> Canvas2 { get; } = new AdvancedScene2D();

        public BindableCanvas2D Canvas3 { get; protected set; }       
    }


    public class Sphere : IDrawingBrush<IScene3D>
    {
        public void DrawTo(IScene3D context)
        {
            context.DrawSegment((0, 0, 0), (0, 2, 0), 0.5f, ColorStyle.Red);

            context.DrawSegment((0, 0, 0), (2, 0, 0), 0.5f, ColorStyle.Green);

            context.DrawPivot(System.Numerics.Matrix4x4.CreateTranslation(10, 0, 0), 0.1f);

            // context.DrawSphere((0, 0, 0), 2, ColorStyle.Red);
        }
    }

    public class BasicScene2D : IDrawingBrush<ICanvas2D>
    {
        // private static readonly ImageSource _Offset0 = ImageSource.CreateFromBitmap("Assets\\SpriteOffset.png", (192, 192), (40, 108), false).WithScale(0.45f);

        private static readonly ImageSource _Offset0 = ImageSource.Create("Assets\\SpriteOffset.png", (17,83), (50, 50), (25, 25));

        public void DrawTo(ICanvas2D context)
        {
            context.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(160,160), _Offset0);

            context.DrawCircle((160, 160), 10, System.Drawing.Color.Green);            
        }
    }

    public class AdvancedScene2D : IDrawingBrush<ICanvas2D>
    {
        public AdvancedScene2D()
        {
            sprites.RunDynamicsThread();

            GroupBox.ArrangeAtlas(800, vectors, sprites);
        }

        private readonly _Scene2D vectors = new _Scene2D();
        private readonly _Sprites2D sprites = new _Sprites2D();

        public void DrawTo(ICanvas2D context)
        {
            sprites.DrawTo(context);
            vectors.DrawTo(context);
        }
    }

    

    public class Cube : IDrawingBrush<IScene3D>
    {
        public void DrawTo(IScene3D context)
        {
            context.DrawCube(System.Numerics.Matrix4x4.CreateScale(1,2, 3) * System.Numerics.Matrix4x4.CreateTranslation(5, 0, 0), ColorStyle.Blue);
        }
    }
}
