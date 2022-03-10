using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InteropTypes.Graphics.Backends.WPF
{
    public class Scene3DViewportTemplate : ControlTemplate
    {
        public Scene3DViewportTemplate() : base(typeof(Scene3DView)) { }

        private Primitives.Scene3DViewport _CachedViewport;

        public Primitives.Scene3DViewport GetViewport()
        {
            if (_CachedViewport != null) return _CachedViewport;

            var panelCtrl = this.LoadContent();
            if (panelCtrl == null) return null;

            if (System.Diagnostics.Debugger.IsAttached)
            {
                if (panelCtrl.GetType().IsNotPublic) throw new InvalidOperationException($"{this.GetType().FullName} must be a public class so it can be used with {nameof(Scene3DViewportTemplate)}");
            }

            if (panelCtrl is Primitives.Scene3DViewport viewport)
            {
                _CachedViewport = viewport;
                return _CachedViewport;
            }

            return null;
        }        
    }
}
