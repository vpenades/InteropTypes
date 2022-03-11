using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Backends
{
    public class WPFRenderTarget
    {
        #region lifecycle

        public WPFRenderTarget(int width, int height) : this(width, height, PixelFormats.Default) { }

        public WPFRenderTarget(int width, int height, PixelFormat format)
        {
            _RenderTarget = new System.Windows.Media.Imaging.RenderTargetBitmap(width, height, 96, 96, format);
        }

        #endregion

        #region data

        private readonly System.Windows.Media.Imaging.RenderTargetBitmap _RenderTarget;

        #endregion

        #region API

        public void Draw(IDrawingBrush<ICanvas2D> drawable)
        {
            Draw(dc => drawable.DrawTo(dc));
        }

        public void Draw(IDrawingBrush<IScene3D> drawable, CameraTransform3D camera)
        {
            if (!(drawable is Record3D record))
            {
                record = new Record3D();
                drawable.DrawTo(record);
            }            

            Draw((dc,size) => record.DrawTo( (dc,size.X,size.Y), camera));
        }

        public void Draw(Action<ICanvas2D,Point2> action)
        {
            var visual = new DrawingVisual();
            var context = visual.RenderOpen();
            var canvas2DFactory = new Canvas2DFactory(context);

            using (var dc = canvas2DFactory.UsingCanvas2D(_RenderTarget.PixelWidth, _RenderTarget.PixelHeight))
            {
                action(dc,(_RenderTarget.PixelWidth, _RenderTarget.PixelHeight));
            }

            context.Close();
            _RenderTarget.Render(visual);
        }

        public  void Draw(Action<ICanvas2D> action)
        {
            var visual = new DrawingVisual();
            var context = visual.RenderOpen();
            var canvas2DFactory = new Canvas2DFactory(context);
            
            using(var dc = canvas2DFactory.UsingCanvas2D(_RenderTarget.PixelWidth,_RenderTarget.PixelHeight))
            {
                action(dc);
            }

            context.Close();
            _RenderTarget.Render(visual);
        }

        public void SaveToPNG(string filePath)
        {
            using (var s = System.IO.File.Create(filePath))
            {
                WritePNG(s);
            }
        }            

        public void WritePNG(System.IO.Stream writer)
        {
            // Save the image to a location on the disk.
            var encoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(_RenderTarget));
            encoder.Save(writer);
        }

        public void WriteJPEG(System.IO.Stream writer)
        {
            // Save the image to a location on the disk.
            var encoder = new System.Windows.Media.Imaging.JpegBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(_RenderTarget));
            encoder.Save(writer);
        }

        #endregion        

        #region static API

        public static void SaveToBitmap(string filePath, int width, int height, IDrawingBrush<ICanvas2D> scene)
        {
            var renderTarget = new WPFRenderTarget(width, height);

            renderTarget.Draw(scene);

            renderTarget.SaveToPNG(filePath);
        }

        #endregion        
    }
}
