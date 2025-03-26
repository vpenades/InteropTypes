using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropTypes.Graphics.Backends
{
    /// <summary>
    /// helper class used to force a render update on a <see cref="System.Windows.FrameworkElement"/>
    /// </summary>
    /// <remarks>
    /// Maybe this is achieved better using Avalonia Behaviors
    /// </remarks>
    struct _AnimatedRenderDispatcher
    {
        #region lifecycle
        public _AnimatedRenderDispatcher(Control element, TimeSpan interval)
        {
            if (interval <= TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(interval));

            _Timer = null;
            _Parent = element ?? throw new ArgumentNullException(nameof(element));
            _Interval = interval;

            element.Loaded += _Loaded;
            element.Unloaded -= _Unloaded;
        }

        private void _Loaded(object sender, RoutedEventArgs e)
        {
            _Timer = new Avalonia.Threading.DispatcherTimer(_Interval, Avalonia.Threading.DispatcherPriority.Normal, _Timer_Tick);
            _Timer.Start();
        }

        private void _Unloaded(object sender, RoutedEventArgs e)
        {
            Release();
        }

        public void Release()
        {
            _Timer?.Stop();
            _Timer = null;
        }

        #endregion

        #region data

        private Visual _Parent;
        private TimeSpan _Interval;
        private Avalonia.Threading.DispatcherTimer _Timer;

        #endregion

        #region loop

        private void _Timer_Tick(object sender, EventArgs e)
        {
            _Parent?.InvalidateVisual();
        }

        #endregion
    }
}
