using Avalonia.Styling;
using Avalonia;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropTypes.Graphics.Backends.Controls
{
    internal static class _AvaloniaUtils
    {
        public static bool TryApplyTo<TControl>(this ControlTheme theme) where TControl : StyledElement
        {
            // defval is usually null the first time a theme is set, afterwards
            // trying to set it again will throw an exception
            var defval = StyledElement.ThemeProperty.GetDefaultValue(typeof(TControl));
            if (defval != null) return false;

            StyledElement.ThemeProperty.OverrideDefaultValue<TControl>(theme);
            return true;
        }
    }
}
