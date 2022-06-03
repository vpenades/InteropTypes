using System;
using System.Collections.Generic;
using System.Text;

using SIZE = System.Drawing.Size;
using POINT = System.Drawing.Point;

namespace InteropTypes.Graphics.Bitmaps
{
    /// <summary>
    /// Represents an interface to a bitmap that allows accessing pixels individually.
    /// </summary>    
    /// <remarks>
    /// This interface is suitable for image types that store pixel data in non contiguous memory,
    /// or if the underlaying memory data is not accessible.
    /// </remarks>
    public interface IBitmap<TPixel>
        : BitmapInfo.IProperties
        where TPixel : unmanaged
    {
        TPixel GetPixel(POINT point);
        void SetPixel(POINT point, TPixel value);
    }   

    partial struct BitmapInfo : BitmapInfo.IProperties
    {
        public interface IProperties
        {
            /// <summary>
            /// Gets the pixel format of the bitmap.
            /// </summary>
            PixelFormat PixelFormat { get; }

            /// <summary>
            /// Gets the size of the bitmap, in pixels.
            /// </summary>
            SIZE Size { get; }

            /// <summary>
            /// Gets the width of the bitmap, in pixels.
            /// </summary>
            int Width { get; }

            /// <summary>
            /// Gets the height of the bitmap, in pixels.
            /// </summary>
            int Height { get; }
        }

        public interface ISource
        {
            /// <summary>
            /// Gets the layout information of the bitmap; Width, Height, PixelFormat, etc.
            /// </summary>
            BitmapInfo Info { get; }            
        }
    }

    partial struct PointerBitmap
        : PointerBitmap.ISource
        , SpanBitmap.ISource
    {
        public interface IDisposableSource : ISource, IDisposable { }

        public interface ISource : BitmapInfo.ISource
        {
            /// <summary>
            /// Gets the <see cref="PointerBitmap"/> owned by this instance.<br/>
            /// If this <see cref="IDisposableSource"/> is disposed, the <see cref="AsPointerBitmap()"/> will no longet be valid.
            /// </summary>
            PointerBitmap AsPointerBitmap();
        }        

        /// <inheritdoc />
        [System.Diagnostics.DebuggerStepThrough]
        PointerBitmap ISource.AsPointerBitmap() { return this; }

        /// <inheritdoc />
        [System.Diagnostics.DebuggerStepThrough]
        public SpanBitmap AsSpanBitmap() { return this; }
    }

    partial struct SpanBitmap
    {
        public interface IDisposableSource : ISource, IDisposable { }
        public interface ISource : BitmapInfo.ISource
        {
            /// <summary>
            /// Casts this bitmap to a <see cref="SpanBitmap"/>
            /// </summary>
            /// <returns>A <see cref="SpanBitmap"/> that shares the pixels with the source bitmap.</returns>
            SpanBitmap AsSpanBitmap();
        }

        public interface IEffect
        {
            bool TryApplyTo(SpanBitmap target);
            bool TryApplyTo<TPixel>(SpanBitmap<TPixel> target) where TPixel : unmanaged;
        }

        public interface ITransfer
        {
            bool TryTransfer<TSrcPixel, TDstPixel>(SpanBitmap<TSrcPixel> source, SpanBitmap<TDstPixel> target) where TSrcPixel : unmanaged where TDstPixel : unmanaged;
            bool TryTransfer<TPixel>(SpanBitmap<TPixel> source, SpanBitmap<TPixel> target) where TPixel : unmanaged;
            bool TryTransfer(SpanBitmap source, SpanBitmap target);
        }

        public interface ITransfer<TSrcPixel,TDstPixel>
            where TSrcPixel : unmanaged
            where TDstPixel : unmanaged
        {
            bool TryTransfer(SpanBitmap<TSrcPixel> source, SpanBitmap<TDstPixel> target);
        }
    }

    partial struct MemoryBitmap
        : MemoryBitmap.ISource
        , SpanBitmap.ISource
    {
        public interface IDisposableSource : ISource, IDisposable { }

        /// <summary>
        /// Represents an object that promises a <see cref="MemoryBitmap"/> and controls its life cycle.
        /// </summary>
        public interface ISource : BitmapInfo.ISource
        {
            /// <summary>
            /// Gets the <see cref="MemoryBitmap"/> owned by this instance.<br/>
            /// If this <see cref="IDisposableSource"/> is disposed, the <see cref="AsMemoryBitmap()"/> will no longet be valid.
            /// </summary>
            MemoryBitmap AsMemoryBitmap();
        }        

        /// <inheritdoc />
        [System.Diagnostics.DebuggerStepThrough]
        public SpanBitmap AsSpanBitmap() { return this; }

        /// <inheritdoc />
        [System.Diagnostics.DebuggerStepThrough]
        MemoryBitmap ISource.AsMemoryBitmap() { return this; }
    }

    partial struct SpanBitmap<TPixel>
    {
        public interface IDisposableSource : ISource, IDisposable { }

        public interface ISource : BitmapInfo.ISource
        {
            /// <summary>
            /// Casts this bitmap to a <see cref="SpanBitmap{TPixel}"/>
            /// </summary>
            /// <returns>A <see cref="SpanBitmap{TPixel}"/> that shares the pixels with the source bitmap.</returns>
            SpanBitmap<TPixel> AsSpanBitmap();
        }
        

        [System.Diagnostics.DebuggerStepThrough]
        public readonly TPixel GetPixel(int x, int y) { return GetScanlinePixels(y)[x]; }

        [System.Diagnostics.DebuggerStepThrough]
        public readonly void SetPixel(int x, int y, TPixel value) { UseScanlinePixels(y)[x] = value; }

        /// <summary>
        /// Returns a pixel typeless <see cref="SpanBitmap"/>.
        /// </summary>
        /// <returns>A <see cref="SpanBitmap"/></returns>
        /// <remarks>
        /// This is the opposite operation of <see cref="SpanBitmap.OfType{TPixel}"/>
        /// </remarks>
        [System.Diagnostics.DebuggerStepThrough]
        public readonly SpanBitmap AsTypeless() { return this; }
    }

    partial struct MemoryBitmap<TPixel>
        : IBitmap<TPixel>
        , SpanBitmap.ISource
        , SpanBitmap<TPixel>.ISource
        , MemoryBitmap.ISource
        , MemoryBitmap<TPixel>.ISource
    {
        public interface IDisposableSource : ISource, IDisposable { }

        /// <summary>
        /// Represents an object that promises a <see cref="MemoryBitmap{TPixel}"/> and controls its life cycle.
        /// </summary>
        public interface ISource : BitmapInfo.ISource
        {
            /// <summary>
            /// Gets the <see cref="MemoryBitmap{TPixel}"/> owned by this instance.<br/>            
            /// </summary>
            MemoryBitmap<TPixel> AsMemoryBitmap();
        }        

        /// <inheritdoc />
        [System.Diagnostics.DebuggerStepThrough]
        public readonly TPixel GetPixel(int x, int y) { return GetScanlinePixels(y)[x]; }

        /// <inheritdoc />
        [System.Diagnostics.DebuggerStepThrough]
        public readonly void SetPixel(int x, int y, TPixel value) { UseScanlinePixels(y)[x] = value; }
        
        [System.Diagnostics.DebuggerStepThrough]
        public readonly TPixel GetPixel(POINT point) { return GetScanlinePixels(point.Y)[point.X]; }
        
        [System.Diagnostics.DebuggerStepThrough]
        public readonly void SetPixel(POINT point, TPixel value) { UseScanlinePixels(point.Y)[point.X] = value; }

        /// <inheritdoc />
        [System.Diagnostics.DebuggerStepThrough]
        public readonly SpanBitmap<TPixel> AsSpanBitmap() { return this; }

        /// <inheritdoc />
        [System.Diagnostics.DebuggerStepThrough]
        readonly SpanBitmap SpanBitmap.ISource.AsSpanBitmap() { return this; }

        /// <inheritdoc />
        [System.Diagnostics.DebuggerStepThrough]
        readonly MemoryBitmap MemoryBitmap.ISource.AsMemoryBitmap() { return this; }

        /// <inheritdoc />
        [System.Diagnostics.DebuggerStepThrough]
        readonly MemoryBitmap<TPixel> ISource.AsMemoryBitmap() { return this; }
        
        [System.Diagnostics.DebuggerStepThrough]
        public readonly MemoryBitmap AsTypeless() { return this; }
    }
}
