using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    public class Canvas2DArgs : EventArgs
    {
        public Canvas2DArgs(ICanvas2D canvas)
        {
            Canvas = canvas;
        }

        public ICanvas2D Canvas { get; }
    }

    public class Scene3DArgs : EventArgs
    {
        public Scene3DArgs(IScene3D scene)
        {
            Scene = scene;
        }

        public IScene3D Scene { get; }
    }
}
