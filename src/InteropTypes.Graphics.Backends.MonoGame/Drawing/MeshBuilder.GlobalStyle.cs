using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Backends
{
    partial class MeshBuilder : GlobalStyle.ISource
    {
        private GlobalStyle _GlobalStyle;
        public bool TryGetGlobalProperty<T>(string name, out T value)
        {
            if (_GlobalStyle == null)
            {
                // set defaults
                new FontStyle(Drawing.Fonts.HersheyFont.Default, ColorStyle.White, 0.1f, Drawing.Fonts.FontAlignStyle.FlipAuto)
                    .TrySetDefaultFontTo(ref _GlobalStyle);
            }

            return GlobalStyle.TryGetGlobalProperty<T>(_GlobalStyle, name, out value);
        }

        public bool TrySetGlobalProperty<T>(string name, T value)
        {
            return GlobalStyle.TrySetGlobalProperty(ref _GlobalStyle, name, value);
        }
    }
}
