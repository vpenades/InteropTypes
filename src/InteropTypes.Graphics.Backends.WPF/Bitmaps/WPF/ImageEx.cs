using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using SOURCEBITMAP = InteropTypes.Graphics.Bitmaps.SpanBitmap.ISource;
using TARGETBITMAP = System.Windows.Media.Imaging.WriteableBitmap;

namespace InteropTypes.Graphics.Backends.WPF
{
    internal class ImageEx : System.Windows.Controls.Image
    {
        static readonly DependencyProperty BitmapSourceProperty = DependencyProperty
            .Register(nameof(BitmapSource), typeof(SOURCEBITMAP), typeof(ImageEx), new PropertyMetadata(null, _Changed));

        public SOURCEBITMAP BitmapSource
        {
            get => GetValue(BitmapSourceProperty) as SOURCEBITMAP;
            set => SetValue(BitmapSourceProperty, value);
        }

        private static void _Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageEx image)
            {
                var dst = image.Source as TARGETBITMAP;

                image.BitmapSource                    
                    .AsSpanBitmap()
                    .WithWPF()
                    .CopyTo(ref dst);

                image.Source = dst;
            }
        }
    }
}
