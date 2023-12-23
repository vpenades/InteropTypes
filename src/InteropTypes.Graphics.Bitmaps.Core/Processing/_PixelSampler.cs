using System;
using System.Collections.Generic;
using System.Text;

using TRANSFORM = System.Numerics.Matrix3x2;

namespace InteropTypes.Graphics.Bitmaps.Processing
{
    internal ref partial struct _PixelSampler<TPixel>
            where TPixel : unmanaged
    {
        #region factory

        public readonly ref struct Factory
        {
            public Factory(in TRANSFORM xform, SpanBitmap<TPixel> src)
            {
                TRANSFORM.Invert(xform, out _Transform);

                _Bytes = src.ReadableBytes;
                _ByteStride = src.Info.StepByteSize;
                _Width = src.Info.Width;
                _Height = src.Info.Height;
            }

            internal readonly TRANSFORM _Transform;

            internal readonly ReadOnlySpan<byte> _Bytes;
            internal readonly int _ByteStride;
            internal readonly int _Width;
            internal readonly int _Height;

            public readonly void UpdateIterator(float x, float y, out _PixelSampler<TPixel> iterator)
            {                
                iterator = new _PixelSampler<TPixel>(new System.Numerics.Vector2(x, y), this);
            }

            public readonly void UpdateIterator(in System.Numerics.Vector2 dst, out _PixelSampler<TPixel> iterator)
            {
                iterator = new _PixelSampler<TPixel>(dst, this);
            }
        }

        /// <summary>
        /// creates a new interator for a given row.
        /// </summary>
        /// <param name="dst">The destination point.</param>
        /// <param name="srcXform">the transform to apply.</param>            
        private _PixelSampler(System.Numerics.Vector2 dst, Factory factory)
        {
            _Bytes = factory._Bytes;
            _ByteStride = factory._ByteStride;
            _Width = factory._Width;
            _Height = factory._Height;

            var origin = System.Numerics.Vector2.Transform(dst, factory._Transform);
            var delta = System.Numerics.Vector2.TransformNormal(System.Numerics.Vector2.UnitX, factory._Transform);

            origin *= 1 << BITSHIFT;
            delta *= 1 << BITSHIFT;

            _X = (int)origin.X;
            _Y = (int)origin.Y;
            _Dx = (int)delta.X;
            _Dy = (int)delta.Y;

            _X += 1 << (BITSHIFT - 1);
            _Y += 1 << (BITSHIFT - 1);
        }

        #endregion

        #region data - row interator

        const int BITSHIFT = 14; // 16384
        const int BITMASK = (1 << BITSHIFT) - 1;

        private int _X;
        private int _Y;

        private readonly int _Dx;
        private readonly int _Dy;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void MoveNext()
        {
            _X += _Dx;
            _Y += _Dy;
        }

        #endregion

        #region data - source image

        private readonly ReadOnlySpan<byte> _Bytes;
        private readonly int _ByteStride;
        private readonly int _Width;
        private readonly int _Height;

        private static readonly TPixel _Default = default(TPixel);

        #endregion

        #region API

        private readonly ref readonly TPixel _GetPixel(int x, int y)
        {
            if (x < 0) return ref _Default;
            else if (x >= _Width) return ref _Default;
            if (y < 0) return ref _Default;
            else if (y >= _Height) return ref _Default;

            y *= _ByteStride;

            return ref System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(_Bytes.Slice(y))[x];
        }

        private TPixel2 _GetPixel<TPixel2>(int x, int y)
            where TPixel2 : unmanaged
        {
            if (x < 0) return default;
            else if (x >= _Width) return default;
            if (y < 0) return default;
            else if (y >= _Height) return default;

            y *= _ByteStride;

            return System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel2>(_Bytes.Slice(y))[x];
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public TPixel GetPixel()
        {
            var x = _X >> BITSHIFT;
            var y = _Y >> BITSHIFT;
            return _GetPixel(x, y);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public TPixel GetSample()
        {
            if (typeof(TPixel) == typeof(Pixel.BGRP32))
            {
                Pixel.BGRP32 r = _GetSampleBGRP32();
                return System.Runtime.CompilerServices.Unsafe.As<Pixel.BGRP32, TPixel>(ref r);
            }

            throw new NotImplementedException();
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void GetSample<TPixel2>(ref TPixel2 dstPixel)
            where TPixel2: unmanaged
        {
            if (typeof(TPixel) == typeof(Pixel.BGRP32)) { _GetSampleBGRP32().CopyTo(ref dstPixel); return; }

            throw new NotImplementedException();
        }


        

        

        #endregion
    }
}
