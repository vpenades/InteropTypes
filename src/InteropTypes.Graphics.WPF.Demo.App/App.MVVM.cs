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
        }


        public void DrawTo(IScene3D context)
        {
            context.DrawSphere((0,0,0), 2, ColorStyle.Red);            
        }       

        public Sphere Item1 => new Sphere();

        public Cube Item2 => new Cube();

        public IDrawingBrush<IScene3D> Combined => Toolkit.Combine(Item1, Item2);

        public ThunderbirdRocket Rocket => new ThunderbirdRocket(System.Numerics.Matrix4x4.CreateTranslation(20,0,0));

        public WPFClientBitmap ClientBitmap { get; } = new WPFClientBitmap();


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
