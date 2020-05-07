using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{

    /// <summary>
    /// Defines a WPF <see cref="System.Windows.Media.Imaging.BitmapSource"/> based bitmap that can be updated from any thread.
    /// </summary>
    public class WPFClientBitmap : System.ComponentModel.INotifyPropertyChanged
    {
        // TODO: this should be used as a WPF control too! as an Image control

        // TODO: this must be a visual collection so we can add/remove layers

        #region lifecycle
        
        public WPFClientBitmap(System.Windows.Threading.Dispatcher dispatcher = null)
        {
            _Dispatcher = dispatcher;
        }

        #endregion

        #region data

        private System.Windows.Threading.Dispatcher _Dispatcher;

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

        protected bool CheckAccess() { return (_Dispatcher ?? System.Windows.Threading.Dispatcher.CurrentDispatcher).CheckAccess(); }

        protected void VerifyAccess() { (_Dispatcher ?? System.Windows.Threading.Dispatcher.CurrentDispatcher).VerifyAccess(); }

        public void Update(SpanBitmap src)
        {
            var dsp = _Dispatcher ?? System.Windows.Threading.Dispatcher.CurrentDispatcher;
            
            if (!dsp.CheckAccess())
            {
                // Create an internal copy
                lock (_BackBufferLock) { MemoryBitmap.CreateOrUpdate(ref _BackBuffer, src); }

                // And invoke the update in the UI thread.
                try { dsp.Invoke(_UpdateFromBackBuffer); }
                // When the app closes and the Dispatcher is shutdown any of these DispatcherOperations
                // still in the queue are aborted and throw the TaskCanceledException.
                catch (System.Threading.Tasks.TaskCanceledException) { }

                return;
            }            

            _UpdateDirect(src);
        }

        private void _UpdateDirect(SpanBitmap src)
        {
            VerifyAccess();

            if (src.WithWPF().CreateOrUpdate(ref _FrontBuffer))
            {
                PropertyChanged?.Invoke(this, _FrontBufferChanged);
            }
        }

        private void _UpdateFromBackBuffer()
        {
            VerifyAccess();

            bool notify = false;

            lock (_BackBufferLock)
            {
                var src = _BackBuffer.AsSpanBitmap();
                notify = src.WithWPF().CreateOrUpdate(ref _FrontBuffer);                
            }

            if (notify) PropertyChanged?.Invoke(this, _FrontBufferChanged);
        }

        #endregion
    }
}
