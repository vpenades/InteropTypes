using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using System.Windows.Input;

namespace InteropTypes.Graphics.Backends.Controls
{
    public class Canvas2DView : Control
    {
        public Canvas2DView()
        {
            this.Loaded += Canvas2DView_Loaded;
            this.Unloaded += Canvas2DView_Unloaded;
        }

        private void Canvas2DView_Unloaded(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (_Timer != null) _Timer.IsEnabled = false;
        }

        private void Canvas2DView_Loaded(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (_Timer != null && _Timer.Interval.TotalSeconds > 0) _Timer.IsEnabled = true;
        }

        private DispatcherTimer _Timer;

        public static readonly DirectProperty<Canvas2DView, float> RenderFrameRateProperty
            = AvaloniaProperty.RegisterDirect<Canvas2DView, float>(nameof(RenderFrameRate), o => o.RenderFrameRate, (o, v) => o.RenderFrameRate = v);

        private float _RenderFrameRate;

        public float RenderFrameRate
        {
            get { return _RenderFrameRate; }
            set
            {
                if (SetAndRaise(RenderFrameRateProperty, ref _RenderFrameRate, value))
                {
                    _UpdateTimer(_RenderFrameRate);
                }
            }
        }        

        public static readonly DirectProperty<Canvas2DView, ICommand> OnRenderCommandProperty
            = AvaloniaProperty.RegisterDirect<Canvas2DView, ICommand>(nameof(OnRenderCommand), o => o.OnRenderCommand, (o, v) => o.OnRenderCommand = v);

        private ICommand _OnRenderCmd;

        public ICommand OnRenderCommand
        {
            get { return _OnRenderCmd; }
            set { SetAndRaise(OnRenderCommandProperty, ref _OnRenderCmd, value); }
        }

        public event EventHandler<InteropTypes.Graphics.Drawing.Canvas2DArgs> OnRender;

        private void _UpdateTimer(float frameRate)
        {
            if (_Timer == null)
            {
                _Timer = new DispatcherTimer();
                _Timer.Tick += (s,e) => this.InvalidateVisual();
            }

            _Timer.Stop();
            if (frameRate > 0) _Timer.Interval = TimeSpan.FromSeconds(1f / frameRate);
            if (_Timer.Interval.TotalSeconds > 0) _Timer.Start();
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            try
            {
                var factory = new Drawing.Canvas2DFactory(context);

                using (var clip = context.PushGeometryClip(new RectangleGeometry(this.Bounds)))
                {
                    using (var dc = factory.UsingCanvas2D((float)this.Bounds.Width, (float)this.Bounds.Height))
                    {
                        OnRenderCommand?.Execute(dc);

                        OnRender?.Invoke(this,new Graphics.Drawing.Canvas2DArgs(dc));
                    }
                }
            }
            catch { throw; }
        }
    }
}
