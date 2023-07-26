using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Platform;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Drawing;

using VENDOR = Avalonia;


namespace InteropTypes.Graphics.Backends
{
    
    

    internal partial class _AvaloniaResourceCache
    {
        #region data        

        private VENDOR.Media.Pen _TransparentPen;

        private VENDOR.Media.ISolidColorBrush _SolidWhite = VENDOR.Media.Brushes.White;

        private readonly Dictionary<UInt32, VENDOR.Media.SolidColorBrush> _BrushesCache = new Dictionary<UInt32, VENDOR.Media.SolidColorBrush>();

        #endregion

        #region API        

        public VENDOR.Media.Pen UseTransparentPen()
        {
            if (_TransparentPen == null) _TransparentPen = new VENDOR.Media.Pen(VENDOR.Media.Brushes.Transparent, 1);
            return _TransparentPen;
        }

        public VENDOR.Media.ISolidColorBrush UseBrush(ColorStyle color)
        {
            if (color.Packed == uint.MaxValue) return _SolidWhite;

            if (!color.IsVisible) return null;

            if (_BrushesCache.TryGetValue(color.Packed, out VENDOR.Media.SolidColorBrush brush)) return brush;

            brush = color.ToGDI().ToDeviceBrush();

            _BrushesCache[color.Packed] = brush;

            return brush;
        }

        public VENDOR.Media.Pen UsePen(float thickness, LineStyle style)
        {
            if (!style.IsVisible) return null;
            if (thickness <= 0) return null;

            var fill = UseBrush(style.FillColor); if (fill == null) return null;

            var pen = new VENDOR.Media.Pen(fill, thickness, null, style.StartCap.ToDeviceCapStyle());

            return pen;
        }

        

        #endregion
    }
}
