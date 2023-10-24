using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Diagnostics;

namespace InteropTypes.Graphics.Bitmaps
{
    namespace Processing
    {
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public readonly struct KernelNxM<TPixel>
            where TPixel : unmanaged
        {
            #region constructor

            private static KernelNxM<TPixel> CreateRadial(int w, int h, Func<float,TPixel> weightToPixel)
            {
                var k = new Processing.KernelNxM<TPixel>(w, h);
                float rx = k.Width / 2;
                float ry = k.Height / 2;

                float _getRadius(int x, int y)
                {
                    var xx = (x - rx) / rx;
                    var yy = (y - ry) / ry;
                    return 1 - Math.Clamp(MathF.Sqrt(xx * xx + yy * yy), 0, 1);
                }

                float www = 0;

                for (int y = 0; y < k.Height; ++y)
                {
                    for (int x = 0; x < k.Width; ++x)
                    {
                        www += _getRadius(x, y);
                    }
                }

                for (int y = 0; y < k.Height; ++y)
                {
                    for (int x = 0; x < k.Width; ++x)
                    {
                        var r = _getRadius(x, y) / www;
                        k[x, y] = weightToPixel(r);
                    }
                }

                return k;
            }

            internal KernelNxM(int w, int h)
            {
                _Rows = new TPixel[w * h];
                Width = w;
                Height = h;
            }

            #endregion

            #region data            

            internal readonly TPixel[] _Rows;

            public readonly int Width;
            public readonly int Height;

            public TPixel this[int x, int y]
            {
                get => _Rows[y * Width + x];
                set => _Rows[y * Width + x] = value;
            }

            #endregion

            #region API

            private void Update(in _RollingRows rows, ReadOnlySpan<int> xindices)
            {
                for(int y=0; y < Height; y++)
                {
                    for(int x=0; x < Width; x++)
                    {
                        this[x,y] = rows[y][xindices[x]];
                    }
                }
            }

            public static void Apply(SpanBitmap<TPixel> dstBitmap, (int n, int m) ksize, KernelEvaluator<TPixel> function)
            {
                var rows = new _RollingRows(dstBitmap.Width, ksize.m);
                for(int i=0; i < rows.Height; i++)
                {
                    dstBitmap.GetScanlinePixels(0).CopyTo(rows[i]);
                }                

                var indices = new int[ksize.n];
                var kernel = new KernelNxM<TPixel>(ksize.n, ksize.m);

                for (int y = 0; y < dstBitmap.Height; y++)
                {
                    // cycle tmp rows:
                    rows.Roll();
                    dstBitmap.GetScanlinePixels(Math.Min(dstBitmap.Height - 1, y + 1)).CopyTo(rows.LastRow);

                    // apply to dst row:

                    var dstRow = dstBitmap.UseScanlinePixels(y);

                    for (int x = 0; x < dstBitmap.Width; x++)
                    {
                        for(int i=0; i < indices.Length; i++)
                        {                            
                            indices[i] = Math.Clamp(x + i - ksize.n/2, 0, rows.Width-1);
                        }

                        kernel.Update(rows, indices);
                        dstRow[x] = function(kernel);
                    }                    
                }
            }

            #endregion

            #region nested types

            public delegate TResult KernelEvaluator<TResult>(in KernelNxM<TPixel> kernel);

            internal struct _RollingRows
            {
                public _RollingRows(int width, int rows)
                {
                    _Rows = new TPixel[width * rows];
                    Width = width;
                    Height = rows;
                    _Scroll = 0;
                }

                private readonly TPixel[] _Rows;

                public readonly int Width;
                public readonly int Height;

                private int _Scroll;

                public readonly Span<TPixel> this[int index]
                {
                    get
                    {
                        index += _Scroll;
                        index %= Height;
                        return _Rows.AsSpan(Width * index, Width);
                    }
                }

                public Span<TPixel> LastRow => this[Height - 1];

                public void Roll()
                {
                    ++_Scroll;
                }
            }

            #endregion
        }
    }
}
