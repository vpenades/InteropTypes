﻿using System;
using System.Collections.Generic;
using System.Text;

using SIZE = System.Drawing.Size;

namespace InteropTypes.Graphics.Bitmaps
{
    /// <summary>
    /// Represents a Bitmap wrapped around a <see cref="Memory{Byte}"/>
    /// </summary>    
    [System.Diagnostics.DebuggerDisplay("{Info.ToDebuggerDisplayString(),nq}")]
    // [System.Diagnostics.DebuggerTypeProxy(typeof(Debug.SpanBitmapProxy))]
    public readonly partial struct MemoryBitmap        
        : IEquatable<MemoryBitmap>
    {
        #region lifecycle        
        public MemoryBitmap(in BitmapInfo info)
        {
            _Info = info;
            _Data = info.BitmapByteSize == 0
                ? Array.Empty<Byte>()
                : new Byte[info.BitmapByteSize];
        }

        public MemoryBitmap(Byte[] array, in BitmapInfo info)
        {
            _Info = info;            
            _Data = array.AsMemory().Slice(0, _Info.BitmapByteSize);
        }

        public MemoryBitmap(Memory<Byte> data, in BitmapInfo info)
        {
            _Info = info;
            _Data = data.Slice(0, _Info.BitmapByteSize);
        }

        public MemoryBitmap(int width, int height, PixelFormat pixelFormat, int stepByteSize = 0)
        {
            _Info = new BitmapInfo(width, height, pixelFormat, stepByteSize);
            var bytes = new byte[_Info.BitmapByteSize];            
            _Data = bytes;
        }        

        public MemoryBitmap(Memory<Byte> data, int width, int height, PixelFormat pixelFormat, int stepByteSize = 0)
        {
            _Info = new BitmapInfo(width, height, pixelFormat, stepByteSize);
            _Data = data.Slice(0, _Info.BitmapByteSize);
        }

        #endregion

        #region data

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly BitmapInfo _Info;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly Memory<Byte> _Data;

        /// <inheritdoc/>
        public override int GetHashCode() { return _Data.GetHashCode() ^ _Info.GetHashCode(); }

        /// <inheritdoc/>
        public override bool Equals(object obj) { return obj is MemoryBitmap other && AreEqual(this, other); }

        /// <inheritdoc/>
        public bool Equals(MemoryBitmap other) { return AreEqual(this, other); }

        /// <inheritdoc/>
        public static bool operator ==(in MemoryBitmap a, in MemoryBitmap b) { return AreEqual(a, b); }

        /// <inheritdoc/>
        public static bool operator !=(in MemoryBitmap a, in MemoryBitmap b) { return !AreEqual(a, b); }

        /// <summary>
        /// Indicates whether both instances are equal.
        /// </summary>
        /// <param name="a">The first instance.</param>
        /// <param name="b">The first instance.</param>
        /// <returns>true if both instances represent the same value.</returns>
        public static bool AreEqual(in MemoryBitmap a, in MemoryBitmap b)
        {
            if (a.Info != b.Info) return false;
            if (a._Data.Equals(b._Data)) return false;
            return true;
        }

        #endregion

        #region properties

        public Memory<Byte> Memory => _Data;

        public bool IsEmpty => _Info.IsEmpty || _Data.IsEmpty;

        #endregion

        #region properties - Info

        /// <inheritdoc/>
        public BitmapInfo Info => _Info;

        /// <inheritdoc/>
        public PixelFormat PixelFormat => _Info.PixelFormat;

        /// <inheritdoc/>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public SIZE Size => _Info.Size;

        /// <inheritdoc/>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int Width => _Info.Width;

        /// <inheritdoc/>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int Height => _Info.Height;

        /// <summary>
        /// Gets the size of a single pixel, in bytes.
        /// </summary>
        public int PixelByteSize => _Info.PixelByteSize;

        /// <summary>
        /// Gets the number of bytes required to jump from one row to the next, in bytes. This is also known as the ByteStride.
        /// </summary>
        public int StepByteSize => _Info.StepByteSize;

        /// <summary>
        /// Gets the bounds of the bitmap, in pixels.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public BitmapBounds Bounds => _Info.Bounds;

        #endregion        

        #region API - Buffers

        [System.Diagnostics.DebuggerStepThrough]
        public Memory<byte> UseScanlineBytes(int y) { return _Info.UseScanlineBytes(_Data, y); }

        [System.Diagnostics.DebuggerStepThrough]
        public ReadOnlyMemory<byte> GetScanlineBytes(int y) { return _Info.GetScanlineBytes(_Data, y); }

        /// <summary>
        /// Gets the underlaying buffer as a <typeparamref name="TPixel"/> memory buffer.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format</typeparam>
        /// <returns>A memory buffer</returns>
        public unsafe Memory<TPixel> GetPixelMemory<TPixel>() where TPixel : unmanaged
        {
            if ((this.Info.StepByteSize % sizeof(TPixel)) != 0) throw new ArgumentException("The bitmap stride is not a multiple of TPixel size.", nameof(TPixel));

            return new MemoryManagers.CastMemoryManager<Byte, TPixel>(_Data).Memory;
        }
        
        /// <summary>
        /// Gets the underlaying byte array, as long as the backing <see cref="Memory"/> was constructed from an <see cref="Array"/>.
        /// </summary>
        /// <param name="segment">The underlaying memory <see cref="Array"/></param>
        /// <returns>true if the operation succeeded.</returns>
        public bool TryGetBuffer(out ArraySegment<Byte> segment)
        {
            return System.Runtime.InteropServices.MemoryMarshal.TryGetArray(_Data, out segment);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public Byte[] ToByteArray()
        {
            if (TryGetBuffer(out ArraySegment<Byte> array) && array.Offset == 0 && array.Array.Length == _Info.BitmapByteSize) return array.Array;

            return _Data.ToArray();
        }

        [System.Diagnostics.DebuggerStepThrough]
        public MemoryBitmap Slice(in BitmapBounds rect)
        {
            var (offset, info) = _Info.Slice(rect);

            if (info.BitmapByteSize == 0) return new MemoryBitmap(info);

            var memory = this._Data.Slice(offset, info.BitmapByteSize);
            return new MemoryBitmap(memory, info);
        }

        #endregion

        #region API - Cast

        public static implicit operator SpanBitmap(MemoryBitmap bmp) { return new SpanBitmap(bmp._Data.Span, bmp._Info); }        
        
        
        public unsafe MemoryBitmap<TPixel> OfType<TPixel>()
            where TPixel : unmanaged
        { return new MemoryBitmap<TPixel>(_Data, _Info); }

        #endregion

        #region API - Pixel Ops
        
        public void SetPixels(int dstX, int dstY, SpanBitmap src) { AsSpanBitmap().SetPixels(dstX, dstY, src); }        

        

        /// <summary>
        /// Reshapes <paramref name="bmp"/> if it doesn't match <paramref name="fmt"/>.
        /// </summary>
        /// <param name="bmp">The bitmap to reshape.</param>
        /// <param name="fmt">The shape to apply.</param>
        /// <param name="compareStep">True if we want to take <see cref="BitmapInfo.StepByteSize"/> into account.</param>
        /// <param name="discardPixels">True if we don't want to copy the pixels when reshaping.</param>
        /// <returns></returns>
        public static bool Reshape(ref MemoryBitmap bmp, BitmapInfo fmt, bool compareStep = false, bool discardPixels = false)
        {
            if (BitmapInfo.AreEqual(bmp.Info, fmt, compareStep)) return false;

            var newBmp = new MemoryBitmap(fmt);
            if (!discardPixels) newBmp.SetPixels(0, 0, bmp);
            bmp = newBmp;
            return true;
        }

        public MemoryBitmap ToMemoryBitmap(PixelFormat? fmtOverride = null)
        {
            return this.AsSpanBitmap().ToMemoryBitmap(fmtOverride);
        }

        public bool TrySetPixelsFormatRGBX(out MemoryBitmap newBitmap)
        {
            if (this.AsSpanBitmap().TrySetPixelsFormatRGBX(out var newSpan))
            {
                newBitmap = new MemoryBitmap(this._Data, newSpan.Info);
                return true;
            }
            else
            {
                newBitmap = default;
                return false;
            }
        }

        public bool TrySetPixelsFormatBGRX(out MemoryBitmap newBitmap)
        {
            if (this.AsSpanBitmap().TryConvertPixelsFormatBGRX(out var newSpan))
            {
                newBitmap = new MemoryBitmap(this._Data, newSpan.Info);
                return true;
            }
            else
            {
                newBitmap = default;
                return false;
            }
        }

        public bool TrySetPixelsFormat(PixelFormat newFormat, out MemoryBitmap newBitmap)
        {
            if (this.AsSpanBitmap().TryConvertPixelsFormat(newFormat, out var newSpan))
            {
                newBitmap = new MemoryBitmap(this._Data, newSpan.Info);
                return true;
            }
            else
            {
                newBitmap = default;
                return false;
            }
        }


        public MemoryBitmap<TPixel> CloneAs<TPixel>()
            where TPixel : unmanaged
        {
            MemoryBitmap<TPixel> newBitmap = default;
            CopyTo(ref newBitmap);
            return newBitmap;
        }

        public bool CopyTo<TPixel>(ref MemoryBitmap<TPixel> dst)
            where TPixel : unmanaged
        {
            return this.AsSpanBitmap().CopyTo(ref dst);
        }

        public bool CopyTo(ref MemoryBitmap dst, PixelFormat format)
        {
            return this.AsSpanBitmap().CopyTo(ref dst, format);
        }

        #endregion

        #region API - IO

        public static MemoryBitmap Load(string filePath, params Codecs.IBitmapDecoder[] factory)
        {
            using (var s = System.IO.File.OpenRead(filePath))
            {
                return Codecs.BitmapCodecFactory.Read(s, factory, (int)s.Length);
            }
        }

        public static MemoryBitmap Load(Func<System.IO.Stream> usingStream, params Codecs.IBitmapDecoder[] factory)
        {
            using(var s = usingStream?.Invoke())
            {
                return Read(s, factory);
            }
        }

        public static MemoryBitmap Read(System.IO.Stream s, params Codecs.IBitmapDecoder[] factory)
        {
            return Codecs.BitmapCodecFactory.Read(s, factory);
        }
        

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public void Save(Action<Action<System.IO.FileInfo>> saveCallback, params Codecs.IBitmapEncoder[] factory)
        {
            var image = this;
            saveCallback(finfo => image.Save(finfo.FullName, factory));
        }

        public void Save(string filePath, params Codecs.IBitmapEncoder[] factory)
        {
            this.AsSpanBitmap().Save(filePath, factory);
        }

        public void Write(System.IO.Stream stream, Codecs.CodecFormat format, params Codecs.IBitmapEncoder[] factory)
        {
            this.AsSpanBitmap().Write(stream, format, factory);
        }

        public static void WritePlanes(System.IO.Stream stream, MemoryBitmap[] planes)
        {
            var settings = new Codecs._InBuiltCodec();
            settings.EnableCompression = true;

            Codecs._InBuiltCodec.WriteRaw(stream, planes, settings);
        }

        public static IEnumerable<MemoryBitmap> ReadPlanes(System.IO.Stream stream)
        {
            return Codecs._InBuiltCodec.ReadRaw(stream);
        }

        #endregion
    }
}
