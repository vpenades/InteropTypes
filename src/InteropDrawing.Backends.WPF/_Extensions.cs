using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using COLOR = System.Drawing.Color;

namespace InteropDrawing
{
    using Backends;

    static class _PrivateExtensions
    {
        public static System.Windows.Media.PenLineCap ToDeviceCapStyle(this LineCapStyle style)
        {
            switch(style)
            {
                case LineCapStyle.Flat: return System.Windows.Media.PenLineCap.Flat;
                case LineCapStyle.Round: return System.Windows.Media.PenLineCap.Round;
                case LineCapStyle.Square: return System.Windows.Media.PenLineCap.Square;
                case LineCapStyle.Triangle: return System.Windows.Media.PenLineCap.Triangle;
                default: throw new NotImplementedException();
            }
        }

        public static System.Windows.Media.Color ToDeviceColor(this UInt32 color)
        {
            byte a = (byte)((color >> 24) & 255);
            byte r = (byte)((color >> 16) & 255);
            byte g = (byte)((color >> 08) & 255);
            byte b = (byte)((color >> 00) & 255);

            return System.Windows.Media.Color.FromArgb(a, r, g, b);            
        }

        public static System.Windows.Media.SolidColorBrush ToDeviceBrush(this UInt32 color)
        {
            return new System.Windows.Media.SolidColorBrush(color.ToDeviceColor());
        }

        public static System.Windows.Media.Color ToDeviceColor(this COLOR color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static System.Windows.Media.SolidColorBrush ToDeviceBrush(this COLOR color)
        {
            return new System.Windows.Media.SolidColorBrush(color.ToDeviceColor());
        }



        public static System.Windows.Point ToDevicePoint(this System.Numerics.Vector2 p)
        {
            return new System.Windows.Point(p.X, p.Y);
        }

        public static System.Windows.Point ToDevicePoint(this Point2 p)
        {
            return new System.Windows.Point(p.X, p.Y);
        }
    }

    public static class WPFExtensions
    {
        [Obsolete("Use PerspectiveTransform instead.")]
        public static void Draw(this System.Windows.Media.DrawingContext dc, Model3D scene, CameraProjection3D camera)
        {
            throw new NotImplementedException();

            // scene.DrawTo(new WPFDrawingContext2D(dc), camera);
        }
    }
        

}
