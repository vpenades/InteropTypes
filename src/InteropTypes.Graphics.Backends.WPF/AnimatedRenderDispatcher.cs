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
    struct _AnimatedRenderDispatcher
    {
        public _AnimatedRenderDispatcher(System.Windows.FrameworkElement element, TimeSpan interval)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(element))
            {
                this = default;
                return;
            }

            if (element == null) throw new ArgumentNullException(nameof(element));
            if (interval <= TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(interval));

            _Timer = null;
            _Parent = element;
            _Interval = interval;

            element.Loaded += _Loaded;
            element.Unloaded -= _Unloaded;
        }

        public void Release()
        {
            _Timer?.Stop();
            _Timer = null;
        }

        private System.Windows.FrameworkElement _Parent;
        private TimeSpan _Interval;
        private System.Windows.Threading.DispatcherTimer _Timer;

        private void _Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _Timer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal, _Parent.Dispatcher);
            _Timer.Interval = this._Interval;
            _Timer.Tick += _Timer_Tick;
            _Timer.Start();
        }

        private void _Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Release();
        }

        private void _Timer_Tick(object sender, EventArgs e)
        {
            _Parent?.InvalidateVisual();
        }
    }
}
