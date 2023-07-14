using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace InteropTypes.Graphics.Avalonia.Demo.App.Views
{
    internal class CustomCanvas : Control
    {
        private InteropTypes._Scene2D _Scene = new _Scene2D();
        private InteropTypes._Sprites2D _Sprites = new _Sprites2D();

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            // context.DrawEllipse(Brushes.Red, null, new Point(50, 50), 20, 20);

            var factory = new Backends.Drawing.Canvas2DFactory(context);

            using (var dc = factory.UsingCanvas2D(800, 600))
            {
                _Scene.DrawTo(dc);
                _Sprites.DrawTo(dc);
            }
        }

    }

}
