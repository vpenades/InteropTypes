using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Backends.WPF
{

    /// <summary>
    /// Represents an object that exposes a <see cref="WriteableBitmap"/> which can be safely updated from any thread.
    /// </summary>
    public class WPFClientBitmap : DispatcherObject, System.ComponentModel.INotifyPropertyChanged
    {
        // TODO: this must be a visual collection so we can add/remove
        // layers so we can draw a bitmap and a drawing.

        #region lifecycle
        
        public WPFClientBitmap() { }

        #endregion

        #region data        

        /// <summary>
        /// This is the backbuffer that receives the frames asyncronously.
        /// </summary>
        protected InterlockedBitmap BackBuffer { get; } = new InterlockedBitmap(3);

        /// <summary>
        /// This is the bitmap that is displayed by WPF
        /// </summary>
        private WriteableBitmap _FrontBuffer;

        #endregion

        #region properties

        private static readonly System.ComponentModel.PropertyChangedEventArgs _FrontBufferChanged = new System.ComponentModel.PropertyChangedEventArgs(nameof(FrontBuffer));

        public WriteableBitmap FrontBuffer => _FrontBuffer;

        #endregion

        #region bindable base

        /// <summary>
        /// Called when <see cref="_FrontBuffer"/> is updated.
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="System.Runtime.CompilerServices.CallerMemberNameAttribute"/>.</param>
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="args">The PropertyChangedEventArgs</param>
        protected virtual void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        #endregion

        #region API

        /// <summary>
        /// Called from any thread to update the display bitmap
        /// </summary>
        /// <param name="src"></param>
        public void Update(SpanBitmap src)
        {
            if (this.CheckAccess()) { _UpdateDirect(src); return; }
            
            // Create an internal copy
            if (!BackBuffer.TryEnqueue(src)) return;

            // And invoke the update in the UI thread.
            try { this.Dispatcher.Invoke(_UpdateFromBackBuffer); }

            // When the app closes and the Dispatcher has shut down any of these DispatcherOperations
            // still in the queue are aborted and throw the TaskCanceledException.
            catch (System.Threading.Tasks.TaskCanceledException) { }
        }

        private void _UpdateDirect(SpanBitmap src)
        {
            VerifyAccess();

            if (_Copy(src, ref _FrontBuffer))
            {
                OnPropertyChanged(_FrontBufferChanged);
            }
        }

        private void _UpdateFromBackBuffer()
        {
            VerifyAccess();

            void _OnBmpRead(MemoryBitmap bmp)
            {
                OnPrepareFrameForDisplay(bmp);
                var src = bmp.AsSpanBitmap();
                var frontBufferChanged = _Copy(src, ref _FrontBuffer);
                if (frontBufferChanged) RaiseFrontBufferChanged();
            }

            BackBuffer.TryDropAndDequeueLast(_OnBmpRead);
        }

        private static bool _Copy(SpanBitmap src, ref WriteableBitmap dst)
        {
            if (src.IsEmpty && dst != null)
            {
                dst.ModifyAsPointerBitmap(ptr => ptr.AsSpanBitmap().SetPixels(System.Drawing.Color.Transparent));
                return false;
            }

            return src.WithWPF().CopyTo(ref dst);
        }

        protected virtual void OnPrepareFrameForDisplay(MemoryBitmap bmp)
        {
            this.VerifyAccess();
        }

        /// <summary>
        /// Called when the front buffer has been rebuilt.
        /// </summary>
        protected virtual void RaiseFrontBufferChanged()
        {
            PropertyChanged?.Invoke(this, _FrontBufferChanged);
        }

        #endregion
    }    
}
