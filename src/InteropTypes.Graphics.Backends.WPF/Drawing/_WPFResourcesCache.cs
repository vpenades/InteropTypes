using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Backends
{
    internal class _WPFResourcesCache
    {
        #region data        

        private System.Windows.Media.Pen _TransparentPen;

        private readonly Dictionary<UInt32, System.Windows.Media.SolidColorBrush> _BrushesCache = new Dictionary<UInt32, System.Windows.Media.SolidColorBrush>();

        private readonly Dictionary<Object, System.Windows.Media.Imaging.BitmapSource> _ImagesCache = new Dictionary<Object, System.Windows.Media.Imaging.BitmapSource>();

        #endregion

        #region API

        public System.Windows.Media.Pen UseTransparentPen()
        {
            if (_TransparentPen == null) _TransparentPen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Transparent, 1);
            return _TransparentPen;
        }

        public System.Windows.Media.SolidColorBrush UseBrush(ColorStyle color)
        {
            if (!color.IsVisible) return null;

            if (_BrushesCache.TryGetValue(color.Packed, out System.Windows.Media.SolidColorBrush brush)) return brush;

            brush = color.ToGDI().ToDeviceBrush();

            _BrushesCache[color.Packed] = brush;

            return brush;
        }

        public System.Windows.Media.Pen UsePen(float thickness, LineStyle style)
        {
            if (!style.IsVisible) return null;
            if (thickness <= 0) return null;

            var fill = UseBrush(style.FillColor); if (fill == null) return null;

            var pen = new System.Windows.Media.Pen(fill, thickness)
            {
                StartLineCap = style.StartCap.ToDeviceCapStyle(),
                EndLineCap = style.EndCap.ToDeviceCapStyle()
            };

            return pen;
        }

        public System.Windows.Media.Imaging.BitmapSource UseImage(object imageKey)
        {
            if (_ImagesCache.TryGetValue(imageKey, out System.Windows.Media.Imaging.BitmapSource image)) return image;

            var imagePath = imageKey as String;

            using (var s = System.IO.File.OpenRead(imagePath))
            {
                if (true)
                {
                    var img = new System.Windows.Media.Imaging.BitmapImage();

                    img.BeginInit();
                    img.StreamSource = s;
                    img.EndInit();

                    image = img;
                }
                else
                {
                    image = System.Windows.Media.Imaging.BitmapFrame.Create(s, System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache, System.Windows.Media.Imaging.BitmapCacheOption.None);
                }
            }

            image.Freeze();

            _ImagesCache[imageKey] = image;

            return image;
        }

        #endregion
    }
}
