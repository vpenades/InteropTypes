using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using DRAWABLE = InteropTypes.Graphics.Drawing.IDrawingBrush<InteropTypes.Graphics.Drawing.IScene3D>;

namespace InteropTypes.Graphics.Backends.WPF
{
    public partial class Scene3DPanel : ContentControl
    {
        #region lifecycle

        static Scene3DPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Scene3DPanel),
                new FrameworkPropertyMetadata(typeof(Scene3DPanel)));
        }

        public Scene3DPanel() { }

        #endregion

        #region API

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            // We need to "bubble up" a notification that the scene layout and bounds have changed.

            _Update(newContent as DRAWABLE);
        }
        
        private void _Update(DRAWABLE content)
        {
            // the template overrides the default rendering of the ContentControl.

            if (!(this.GetTemplateChild("PART_Viewport3D") is Primitives.Scene3DCanvas canvas)) return;            

            if (content == null) { canvas.Scene = null; return; }

            var record = new Drawing.Record3D();

            content.DrawTo(record);

            canvas.Scene = record;
        }

        #endregion
    }
}
