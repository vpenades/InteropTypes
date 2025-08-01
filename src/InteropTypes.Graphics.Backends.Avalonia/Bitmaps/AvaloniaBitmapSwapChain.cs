using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using InteropTypes.Graphics.Adapters;
using InteropTypes.Graphics.Bitmaps;

using AVLWBITMAP = Avalonia.Media.Imaging.WriteableBitmap;

namespace InteropTypes.Graphics.Backends.Bitmaps
{

    /// <summary>
    /// Represents an object that exposes a <see cref="AVLWBITMAP"/> which can be safely updated from any thread,
    /// and ensures any image bound to it will automatically refresh.
    /// </summary>
    public class AvaloniaBitmapSwapChain : INotifyPropertyChanged
    {
        #region lifecycle
        
        public AvaloniaBitmapSwapChain() { }

        #endregion

        #region data        

        /// <summary>
        /// This is the backbuffer that receives the frames asyncronously.
        /// </summary>
        protected InterlockedBitmap BackBuffer { get; } = new InterlockedBitmap(3);

        // https://github.com/AvaloniaUI/Avalonia/issues/9835
        // Apparently, Avalonia does not auto-invalidate the buffers when they're overwritten
        // so what we do is to keep two buffers that are alternatively swapped, which triggers
        // Image to update the view at each update.

        private bool _Swap;
        private AVLWBITMAP _BackBuffer1;
        private AVLWBITMAP _BackBuffer2;
        private AVLWBITMAP _FrontBuffer;        

        #endregion

        #region properties
        public AVLWBITMAP FrontBuffer => _FrontBuffer;

        #endregion        

        #region bindable base

        private static readonly PropertyChangedEventArgs _FrontBufferChanged = new PropertyChangedEventArgs(nameof(FrontBuffer));

        /// <summary>
        /// Called when the front buffer has been rebuilt.
        /// </summary>
        protected virtual void RaiseFrontBufferChanged()
        {
            OnPropertyChanged(_FrontBufferChanged);
        }

        /// <summary>
        /// Called when <see cref="_FrontBuffer"/> is updated.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="System.Runtime.CompilerServices.CallerMemberNameAttribute"/>.</param>
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="args">The PropertyChangedEventArgs</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
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
            if (Avalonia.Threading.Dispatcher.UIThread.CheckAccess())
            {
                _UpdateDirect(src);
                return;
            }
            
            // Create an internal copy
            if (!BackBuffer.TryEnqueue(src)) return;

            // And invoke the update in the UI thread.
            try { Avalonia.Threading.Dispatcher.UIThread.Invoke(_UpdateFromBackBuffer); }

            // When the app closes and the Dispatcher has shut down any of these DispatcherOperations
            // still in the queue are aborted and throw the TaskCanceledException.
            catch (System.Threading.Tasks.TaskCanceledException) { }            
        }

        private void _UpdateFromBackBuffer()
        {
            Avalonia.Threading.Dispatcher.UIThread.VerifyAccess();

            void _OnBmpRead(MemoryBitmap bmp)
            {                
                var src = bmp.AsSpanBitmap();

                _UpdateDirect(src);
            }

            BackBuffer.TryDropAndDequeueLast(_OnBmpRead);
        }

        private void _UpdateDirect(SpanBitmap src)
        {
            Avalonia.Threading.Dispatcher.UIThread.VerifyAccess();            

            _Swap = !_Swap;

            if (_Swap)
            {
                _Copy(src, ref _BackBuffer1);
                _FrontBuffer = _BackBuffer1;
            }
            else
            {
                _Copy(src, ref _BackBuffer2);
                _FrontBuffer = _BackBuffer2;
            }            

            RaiseFrontBufferChanged();
        }

        private bool _Copy(SpanBitmap src, ref AVLWBITMAP dst)
        {
            if (src.IsEmpty && dst != null)
            {
                using(var l = dst.Lock())
                {
                    var dstSpan = _Implementation.AsPointerBitmap(l).AsSpanBitmap();

                    dstSpan.SetPixels(System.Drawing.Color.Transparent);

                    OnPrepareFrameForDisplay(dstSpan);
                }                

                return false;
            }

            return new AVLAdapter(src).CopyTo(ref dst);
        }

        protected virtual void OnPrepareFrameForDisplay(SpanBitmap bmp)
        {
            
        }        

        #endregion
    }    
}
