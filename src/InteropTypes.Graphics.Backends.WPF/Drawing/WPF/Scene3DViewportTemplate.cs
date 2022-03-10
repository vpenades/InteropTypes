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

            var p = panelCtrl as Primitives.Scene3DViewport;
            if (p == null) return null;

            _CachedViewport = p;

            return _CachedViewport;
        }        
    }
}
