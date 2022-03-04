using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Backends
{
    public static class MauiExtensions
    {
        public static Microsoft.Maui.Graphics.Color ToMaui(this System.Drawing.Color color)
        {
            return Microsoft.Maui.Graphics.Color.FromUint((uint)color.ToArgb());
        }

    }
}
