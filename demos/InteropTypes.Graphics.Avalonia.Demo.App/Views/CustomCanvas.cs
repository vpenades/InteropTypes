﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

namespace InteropTypes.Graphics.Avalonia.Demo.App.Views
{
    internal class CustomCanvas : UserControl
    {
        public CustomCanvas()
        {
            _timer.Tick += _timer_Tick;
            
            this.Loaded += (s,e) => _timer.Start();
            this.Unloaded += (s, e) => _timer.Stop();
        }

        private void _timer_Tick(object? sender, EventArgs e)
        {
            this.InvalidateVisual();
        }

        private InteropTypes._Scene2D _Scene = new _Scene2D();
        private InteropTypes._Sprites2D _Sprites = new _Sprites2D();

        private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromSeconds(1f / 30) };

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            context.DrawEllipse(Brushes.Red, null, new Point(50, 50), 20, 20);

            
            var factory = new Backends.Drawing.Canvas2DFactory(context);

            using (var dc = factory.UsingCanvas2D(800, 600))
            {
                _Scene.DrawTo(dc);
                _Sprites.DrawTo(dc);
            }
        }

    }

}
