using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;

using InteropBitmaps;

namespace InteropTypes.Graphics.Backends
{

    /// <summary>
    /// Defines a WPF <see cref="System.Windows.Media.Imaging.BitmapSource"/> based bitmap that can be updated from any thread.
    /// </summary>
    public class WPFClientBitmap : DispatcherObject, System.ComponentModel.INotifyPropertyChanged
    {
        // TODO: this must be a visual collection so we can add/remove layers

        #region lifecycle
        
        public WPFClientBitmap() { }

        #endregion

        #region data        

        private readonly object _BackBufferLock = new object();
        private MemoryBitmap _BackBuffer;

        private System.Windows.Media.Imaging.WriteableBitmap _FrontBuffer;        

        private static readonly System.ComponentModel.PropertyChangedEventArgs _FrontBufferChanged = new System.ComponentModel.PropertyChangedEventArgs(nameof(FrontBuffer));

        #endregion

        #region properties

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public System.Windows.Media.Imaging.BitmapSource FrontBuffer => _FrontBuffer;

        #endregion

        #region API        

        public void Update(SpanBitmap src)
        {
            if (!this.CheckAccess())
            {
                // Create an internal copy
                lock (_BackBufferLock) { src.CopyTo(ref _BackBuffer); }

                // And invoke the update in the UI thread.
                try { this.Dispatcher.Invoke(_UpdateFromBackBuffer); }

                // When the app closes and the Dispatcher has shut down any of these DispatcherOperations
                // still in the queue are aborted and throw the TaskCanceledException.
                catch (System.Threading.Tasks.TaskCanceledException) { }

                return;
            }            

            _UpdateDirect(src);
        }

        private void _UpdateDirect(SpanBitmap src)
        {
            VerifyAccess();

            if (src.WithWPF().CopyTo(ref _FrontBuffer))
            {
                PropertyChanged?.Invoke(this, _FrontBufferChanged);
            }
        }

        private void _UpdateFromBackBuffer()
        {
            VerifyAccess();

            bool recycled = false;

            lock (_BackBufferLock)
            {
                var src = _BackBuffer.AsSpanBitmap();
                recycled = src.WithWPF().CopyTo(ref _FrontBuffer);                
            }

            if (recycled) PropertyChanged?.Invoke(this, _FrontBufferChanged);
        }

        #endregion
    }    
}
