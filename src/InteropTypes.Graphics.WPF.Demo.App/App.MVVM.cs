﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics
{
    internal class AppMVVM : Drawing.IDrawingBrush<Drawing.IScene3D>
    {
        public void DrawTo(IScene3D context)
        {
            context.DrawSphere((0,0,0), 2, ColorStyle.Red);
        }

        public Sphere Item1 => new Sphere();

        public Cube Item2 => new Cube();
    }


    public class Sphere : Drawing.IDrawingBrush<Drawing.IScene3D>
    {
        public void DrawTo(IScene3D context)
        {
            context.DrawSegment((0, 0, 0), (0, 2, 0), 0.5f, ColorStyle.Red);

            context.DrawSegment((0, 0, 0), (2, 0, 0), 0.5f, ColorStyle.Green);

            context.DrawPivot(System.Numerics.Matrix4x4.CreateTranslation(10, 0, 0), 0.1f);

            // context.DrawSphere((0, 0, 0), 2, ColorStyle.Red);
        }
    }

    public class Cube : Drawing.IDrawingBrush<Drawing.IScene3D>
    {
        public void DrawTo(IScene3D context)
        {
            context.DrawCube(System.Numerics.Matrix4x4.CreateScale(1,2, 3) * System.Numerics.Matrix4x4.CreateTranslation(5, 0, 0), ColorStyle.Blue);
        }
    }
}
