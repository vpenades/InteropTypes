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
            // this.Initialize();            
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
