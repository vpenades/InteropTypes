using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InteropTypes.Graphics.Backends.WPF
{
    public class Canvas2DViewportTemplate : ControlTemplate
    {
        public Canvas2DViewportTemplate() : base(typeof(Canvas2DView)) { }

        private Primitives.Canvas2DViewport _CachedViewport;

        public Primitives.Canvas2DViewport GetViewport()
        {
            if (_CachedViewport != null) return _CachedViewport;

            var panelCtrl = this.LoadContent();
            if (panelCtrl == null) return null;

            if (System.Diagnostics.Debugger.IsAttached)
            {
                if (panelCtrl.GetType().IsNotPublic) throw new InvalidOperationException($"{this.GetType().FullName} must be a public class so it can be used with {nameof(Canvas2DViewportTemplate)}");
            }

            if (panelCtrl is Primitives.Canvas2DViewport viewport)
            {
                _CachedViewport = viewport;
                return _CachedViewport;
            }

            return null;
        }        
    }
}
