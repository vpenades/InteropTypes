using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {
        partial struct RGB24
        {
            public override string ToString() => $"<{R}, {G}, {B}>";
        }
        partial struct BGR24
        {
            public override string ToString() => $"<{R}, {G}, {B}>";
        }

        partial struct BGRA32
        {
            public override string ToString() => $"<{R}, {G}, {B}, {A}>";
        }

        partial struct RGBA32
        {
            public override string ToString() => $"<{R}, {G}, {B}, {A}>";
        }

        partial struct ARGB32
        {
            public override string ToString() => $"<{R}, {G}, {B}, {A}>";
        }

        partial struct BGRP32
        {
            public override string ToString() => $"<{R}, {G}, {B}, {A}>";
        }

        partial struct RGBP32
        {
            public override string ToString() => $"<{R}, {G}, {B}, {A}>";
        }

    }
}
