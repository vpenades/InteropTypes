using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics;
using InteropTypes.Graphics.Backends.WPF;
using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Drawing;

using InteropTypes;
using System.Windows.Input;

namespace WPFDemo
{
    internal class AppMVVM : IDrawingBrush<IScene3D>
    {
        public AppMVVM()
        {
            System.Threading.Tasks.Task.Run(_AsyncUpdateBitmap);

            MemoryBitmapTypeless.AsSpanBitmap().SetPixels(System.Drawing.Color.Green);
            MemoryBitmapBGRA32.AsSpanBitmap().AsTypeless().SetPixels(System.Drawing.Color.Red);

            OnPaintMemoryBitmapCmd = new Prism.Commands.DelegateCommand(_PaintOnBitmapAsync);

            BindableBitmap.Bitmap = MemoryBitmapBGRA32;

       }
        
        public void DrawTo(IScene3D context)
        {
            context.DrawSphere((0,0,0), 2, ColorStyle.Red);            
        }       

        public ICommand OnPaintMemoryBitmapCmd { get; }

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

        public WPFClientBitmap ClientBitmap { get; } = new WPFClientBitmap();

        public BindableBitmap BindableBitmap { get; } = new BindableBitmap();

        public BindableBitmap BindableBitmapUndefined { get; } = new BindableBitmap();

        public MemoryBitmap MemoryBitmapTypeless { get; } = new MemoryBitmap(512, 512, Pixel.BGRA32.Format);

        public MemoryBitmap<Pixel.BGRA32> MemoryBitmapBGRA32 { get; } = new MemoryBitmap<Pixel.BGRA32>(512, 512, Pixel.BGRA32.Format);


        public IDrawingBrush<ICanvas2D> Canvas1 { get; } = new BasicScene2D();

        public IDrawingBrush<ICanvas2D> Canvas2 { get; } = new AdvancedScene2D();

        public BindableCanvas2D Canvas3 { get; } = new AsyncScene2D(System.Windows.Threading.Dispatcher.CurrentDispatcher).Bindable;

        private void _PaintOnBitmapAsync()
        {
            Task.Run(_PaintOnBitmap);
        }

        private void _PaintOnBitmap()
        {
            var other = new MemoryBitmap<Pixel.BGRA32>(256, 256);
            other.SetPixels(System.Drawing.Color.Yellow);

            var rnd = new Random();
            var pix = default(Pixel.BGRA32);

            foreach (var p in other.EnumeratePixels())
            {               
                pix.SetRandom(rnd, 255);
                other.SetPixel(p.Location, pix);
            }

            var dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;

            dispatcher.Invoke( this.BindableBitmap.Enqueue(other) );            
        }

        private void _AsyncUpdateBitmap()
        {
            var bmp = new MemoryBitmap(256, 256, Pixel.BGR24.Format);
            var rnd = new Random();

            while(true)
            {
                System.Threading.Thread.Sleep(1000 / 100);

                if (bmp.TryGetBuffer(out var buffer))
                {
                    var data = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, int>(buffer.Array);                   

                    for(int i=0; i < data.Length; ++i)
                    {
                        data[i] = rnd.Next();
                    }
                }

                ClientBitmap.Update(bmp);
            }
        }
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
        private _Sprites2D sprites = new _Sprites2D();
        private ImageSource _noiseTexture = new BindableNoiseTexture().Sprite;

        public void DrawTo(ICanvas2D context)
        {
            sprites.DrawTo(context);

            context.DrawImage(System.Numerics.Matrix3x2.Identity, _noiseTexture);
        }
    }

    public class AsyncScene2D
    {
        public AsyncScene2D(System.Windows.Threading.Dispatcher dispatcher)
        {
            var rnd = new Random();

            void _DrawRandomLines(ICanvas2D canvas)
            {
                for (int i = 0; i < 100; ++i)
                {
                    var p1 = new Point2(rnd) * 200;
                    var p2 = new Point2(rnd) * 200;

                    canvas.DrawLine(p1, p2, 1f, System.Drawing.Color.Red);
                }
            }

            void _UpdateBindableAsync()
            {
                while (true)
                {
                    var dispacherAction = Bindable.Enqueue(_DrawRandomLines);
                    if (dispacherAction != null)
                    {
                        try { dispatcher.Invoke(dispacherAction); }
                        catch (TaskCanceledException) { }
                    }

                    Task.Delay(1000 / 30);
                }
            }

            Task.Run(_UpdateBindableAsync);
        }

        public BindableCanvas2D Bindable { get; } = new BindableCanvas2D();

        
    }

    public class Cube : IDrawingBrush<IScene3D>
    {
        public void DrawTo(IScene3D context)
        {
            context.DrawCube(System.Numerics.Matrix4x4.CreateScale(1,2, 3) * System.Numerics.Matrix4x4.CreateTranslation(5, 0, 0), ColorStyle.Blue);
        }
    }
}
