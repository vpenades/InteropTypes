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
    internal class AppMVVM : InteropTypes.AppMVVM
    {
        public AppMVVM()
        {
            System.Threading.Tasks.Task.Run(_AsyncUpdateBitmap);

            OnPaintMemoryBitmapCmd = new Prism.Commands.DelegateCommand(_PaintOnBitmapAsync);

            Canvas3 =  new AsyncScene2D(System.Windows.Threading.Dispatcher.CurrentDispatcher).Bindable;
        }        

        public WPFClientBitmap ClientBitmap { get; } = new WPFClientBitmap();
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
   
}
