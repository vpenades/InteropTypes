using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Drawing;

using XFORM = System.Numerics.Matrix3x2;
using COLOR = System.Drawing.Color;

namespace InteropTypes
{
    internal abstract class GroupBox : IDrawingBrush<ICanvas2D>
    {
        protected GroupBox(float width, float height)
        {
            Width= width; Height = height;
        }

        public float Width { get; }
        public float Height { get; }

        public void DrawTo(ICanvas2D dc, float x, float y)
        {
            dc = dc.CreateTransformed2D(XFORM.CreateTranslation(x, y));
            DrawTo(dc);
        }

        public void DrawTo(ICanvas2D dc)
        {            
            dc.DrawRectangle((0, 0), (Width, Height), (COLOR.Yellow, 3), 6);
            dc = dc.CreateTransformed2D(XFORM.CreateTranslation(10, 10));
            DrawContentTo(dc);
        }

        protected abstract void DrawContentTo(ICanvas2D dc);
    }
}
