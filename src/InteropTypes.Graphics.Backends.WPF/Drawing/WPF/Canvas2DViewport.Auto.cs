using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Drawing;



namespace InteropTypes.Graphics.Backends.WPF
{

    using CAMERATEMPLATE = Canvas2DViewportTemplate;

    public class AutoCanvas2DViewport : Primitives.Canvas2DViewport
    {
        public AutoCanvas2DViewport()
        {
            FrameRate = 30;
            EnableCanvasRedraw = true;
        }

        #region template

        internal static CAMERATEMPLATE CreateDefaultTemplate()
        {
            var template = new CAMERATEMPLATE();
            template.VisualTree = new System.Windows.FrameworkElementFactory(typeof(AutoCanvas2DViewport));
            template.Seal();
            return template;
        }

        #endregion      

        protected override void OnCanvasChanged(IDrawingBrush<ICanvas2D> canvas)
        {
            
        }
    }
}
