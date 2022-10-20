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

    public class Cube : IDrawingBrush<IScene3D>
    {
        public void DrawTo(IScene3D context)
        {
            context.DrawCube(System.Numerics.Matrix4x4.CreateScale(1,2, 3) * System.Numerics.Matrix4x4.CreateTranslation(5, 0, 0), ColorStyle.Blue);
        }
    }
}
