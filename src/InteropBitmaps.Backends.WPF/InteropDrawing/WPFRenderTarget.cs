using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Backends
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

        private bool _IsOpen;

        #endregion

        #region API        

        public IDisposableCanvas2D OpenDrawingContext()
        {
            if (_IsOpen) throw new InvalidOperationException("already open.");

            return new _DrawingContext(this, _RenderTarget);
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

            using (var dc = renderTarget.OpenDrawingContext())
            {
                scene.DrawTo(dc);
            }

            renderTarget.SaveToPNG(filePath);
        }

        #endregion

        #region nested types

        sealed class _DrawingContext :
            WPF.WPFDrawingContext2D,
            IDisposableCanvas2D,
            IBackendViewportInfo

        {
            #region lifecycle

            public _DrawingContext(WPFRenderTarget owner, System.Windows.Media.Imaging.RenderTargetBitmap rt)
            {
                owner._IsOpen = true;

                _Owner = owner;
                _RenderTarget = rt;
                _Visual = new System.Windows.Media.DrawingVisual();
                _Context = _Visual.RenderOpen();

                SetContext(_Context);
            }

            public void Dispose()
            {
                _Context.Close();
                _RenderTarget.Render(_Visual);

                _Owner._IsOpen = false;
            }

            #endregion

            #region API

            private WPFRenderTarget _Owner;

            private System.Windows.Media.Imaging.RenderTargetBitmap _RenderTarget;
            private System.Windows.Media.DrawingVisual _Visual;
            private System.Windows.Media.DrawingContext _Context;

            private Action _OnDispose;

            #endregion

            #region properties

            public int PixelsWidth => _RenderTarget.PixelWidth;
            public int PixelsHeight => _RenderTarget.PixelHeight;
            public float DotsPerInchX => (float)_RenderTarget.DpiX;
            public float DotsPerInchY => (float)_RenderTarget.DpiY;

            #endregion
        }

        #endregion
    }
}
