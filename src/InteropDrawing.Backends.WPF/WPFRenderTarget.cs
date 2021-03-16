using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace InteropDrawing.Backends
{
    public class WPFRenderTarget
    {
        #region lifecycle

        public WPFRenderTarget(int width, int height) : this(width,height,System.Windows.Media.PixelFormats.Default) { }

        public WPFRenderTarget(int width, int height, System.Windows.Media.PixelFormat format)
        {
            _RenderTarget = new System.Windows.Media.Imaging.RenderTargetBitmap(width, height, 96, 96, format);
        }

        #endregion

        #region data

        private readonly System.Windows.Media.Imaging.RenderTargetBitmap _RenderTarget;

        #endregion

        #region API        

        public ICanvasDrawingContext2D OpenDrawingContext()
        {
            return new _DrawingContext(_RenderTarget);
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

        #region nested types

        class _DrawingContext : WPFDrawingContext2D, ICanvasDrawingContext2D
        {
            #region lifecycle

            public _DrawingContext(System.Windows.Media.Imaging.RenderTargetBitmap rt)
            {
                _RenderTarget = rt;
                _Visual = new System.Windows.Media.DrawingVisual();
                _Context = _Visual.RenderOpen();

                SetContext(_Context);
            }

            public void Dispose()
            {
                _Context.Close();
                _RenderTarget.Render(_Visual);
            }

            #endregion

            #region API

            private System.Windows.Media.Imaging.RenderTargetBitmap _RenderTarget;
            private System.Windows.Media.DrawingVisual _Visual;
            private System.Windows.Media.DrawingContext _Context;

            #endregion
        }

        #endregion

        #region static API

        public static void SaveToBitmap(string filePath, int width, int height, Model2D scene)
        {
            var renderTarget = new WPFRenderTarget(width, height);

            using (var dc = renderTarget.OpenDrawingContext())
            {
                scene.DrawTo(dc, true);
            }

            renderTarget.SaveToPNG(filePath);
        }

        #endregion
    }
}
