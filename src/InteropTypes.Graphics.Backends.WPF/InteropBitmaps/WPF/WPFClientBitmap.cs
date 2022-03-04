﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Backends.WPF
{

    /// <summary>
    /// Defines a WPF <see cref="System.Windows.Media.Imaging.BitmapSource"/> based bitmap that can be updated from any thread.
    /// </summary>
    public class WPFClientBitmap : DispatcherObject, System.ComponentModel.INotifyPropertyChanged
    {
        // TODO: this must be a visual collection so we can add/remove
        // layers so we can draw a bitmap and a drawing.

        #region lifecycle
        
        public WPFClientBitmap() { }

        #endregion

        #region data        

        private InterlockedBitmap _Interlocked = new InterlockedBitmap(3);        

        private System.Windows.Media.Imaging.WriteableBitmap _FrontBuffer;        

        private static readonly System.ComponentModel.PropertyChangedEventArgs _FrontBufferChanged = new System.ComponentModel.PropertyChangedEventArgs(nameof(FrontBuffer));

        #endregion

        #region properties        

        /// <summary>
        /// This is the bitmap that is displayed by WPF
        /// </summary>
        public System.Windows.Media.Imaging.BitmapSource FrontBuffer => _FrontBuffer;
        
        /// <summary>
        /// Called when <see cref="FrontBuffer"/> is updated.
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region API        

        /// <summary>
        /// Called from any thread to update the display bitmap
        /// </summary>
        /// <param name="src"></param>
        public void Update(SpanBitmap src)
        {
            if (!this.CheckAccess())
            {
                // Create an internal copy
                if (!_Interlocked.TryEnqueue(src)) return;

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

            void _OnBmpRead(MemoryBitmap bmp)
            {
                var src = bmp.AsSpanBitmap();

                var oldBuff = _FrontBuffer;

                src.WithWPF().CopyTo(ref _FrontBuffer);                

                if (ReferenceEquals(oldBuff, _FrontBuffer)) PropertyChanged?.Invoke(this, _FrontBufferChanged);
            }

            _Interlocked.TryDropAndDequeueLast(_OnBmpRead);
        }

        #endregion
    }    
}
