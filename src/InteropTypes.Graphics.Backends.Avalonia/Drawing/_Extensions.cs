using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Drawing;

using COLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Backends
{
    static class _PrivateExtensions
    {
        public static Avalonia.Matrix ToDeviceMatrix(this in System.Numerics.Matrix3x2 xform)
        {
            return new Avalonia.Matrix(xform.M11, xform.M12, xform.M21, xform.M22, xform.M31, xform.M32);
        }

        public static Avalonia.Rect ToDeviceRect(this System.Drawing.RectangleF rect)
        {
            return new Avalonia.Rect(rect.X,rect.Y,rect.Width,rect.Height);
        }

        public static Avalonia.Media.PenLineCap ToDeviceCapStyle(this LineCapStyle style)
        {
            switch(style)
            {
                case LineCapStyle.Flat: return Avalonia.Media.PenLineCap.Flat;
                case LineCapStyle.Round: return Avalonia.Media.PenLineCap.Round;
                case LineCapStyle.Square: return Avalonia.Media.PenLineCap.Square;
                case LineCapStyle.Triangle: return Avalonia.Media.PenLineCap.Round;
                default: throw new NotImplementedException();
            }
        }

        public static Avalonia.Media.Color ToDeviceColor(this UInt32 color)
        {
            byte a = (byte)((color >> 24) & 255);
            byte r = (byte)((color >> 16) & 255);
            byte g = (byte)((color >> 08) & 255);
            byte b = (byte)((color >> 00) & 255);

            return Avalonia.Media.Color.FromArgb(a, r, g, b);            
        }

        public static Avalonia.Media.SolidColorBrush ToDeviceBrush(this UInt32 color)
        {
            return new Avalonia.Media.SolidColorBrush(color.ToDeviceColor());
        }

        public static Avalonia.Media.Color ToDeviceColor(this COLOR color)
        {
            return Avalonia.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static Avalonia.Media.SolidColorBrush ToDeviceBrush(this COLOR color)
        {
            return new Avalonia.Media.SolidColorBrush(color.ToDeviceColor());
        }



        public static Avalonia.Point ToDevicePoint(this System.Numerics.Vector2 p)
        {
            return new Avalonia.Point(p.X, p.Y);
        }

        public static Avalonia.Point ToDevicePoint(this Point2 p)
        {
            return new Avalonia.Point(p.X, p.Y);
        }
    }
}
