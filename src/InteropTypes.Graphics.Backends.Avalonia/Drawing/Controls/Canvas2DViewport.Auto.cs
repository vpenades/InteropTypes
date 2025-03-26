using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Backends.Controls
{

    using CAMERATEMPLATE = Canvas2DViewportTemplate;

    public class AutoCanvas2DViewport : Primitives.Canvas2DViewport
    {
        public AutoCanvas2DViewport()
        {
            FrameRate = 30;
            EnableCanvasRedraw = true;
        }        

        protected override void OnCanvasChanged(IDrawingBrush<ICanvas2D> canvas)
        {
            
        }
    }
}
