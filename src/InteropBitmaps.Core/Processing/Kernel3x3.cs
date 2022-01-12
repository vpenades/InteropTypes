using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    partial struct SpanBitmap<TPixel>
    {
        public void Apply(Processing.Kernel3x3<TPixel>.KernelEvaluator<TPixel> function)            
        {
            Processing.Kernel3x3<TPixel>.Apply(this, function);
        }

        public void Apply<TKernelPixel>(Processing.Kernel3x3<TKernelPixel>.KernelEvaluator<TPixel> function)
            where TKernelPixel : unmanaged
        {
            Processing.Kernel3x3<TKernelPixel>.Apply(this, function);
        }

        public void SetPixels<TSrcPixel>(SpanBitmap<TSrcPixel> src, Processing.Kernel3x3<TPixel>.KernelEvaluator<TPixel> function)
            where TSrcPixel: unmanaged
        {
            Processing.Kernel3x3<TPixel>.Copy(src,this, function);
        }
    }

    namespace Processing
    {
        public readonly struct Kernel3x3<TPixel>
            where TPixel : unmanaged
        {
            #region delegates

            public delegate TResult KernelEvaluator<TResult>(in Kernel3x3<TPixel> kernel);

            #endregion

            #region constructor        

            internal Kernel3x3(in _RollingRows rows, int idx1, int idx2, int idx3)
            {
                P11 = rows.Row1[idx1];
                P12 = rows.Row1[idx2];
                P13 = rows.Row1[idx3];

                P21 = rows.Row2[idx1];
                P22 = rows.Row2[idx2];
                P23 = rows.Row2[idx3];

                P31 = rows.Row3[idx1];
                P32 = rows.Row3[idx2];
                P33 = rows.Row3[idx3];
            }

            internal Kernel3x3(ReadOnlySpan<TPixel> row1, ReadOnlySpan<TPixel> row2, ReadOnlySpan<TPixel> row3, int idx1, int idx2, int idx3)
            {
                P11 = row1[idx1];
                P12 = row1[idx2];
                P13 = row1[idx3];

                P21 = row2[idx1];
                P22 = row2[idx2];
                P23 = row2[idx3];

                P31 = row3[idx1];
                P32 = row3[idx2];
                P33 = row3[idx3];
            }

            #endregion

            #region data

            // a better data setup would be ReadOnlySpan<TPixel> Row0, Row1, Row2;

            public readonly TPixel P11;
            public readonly TPixel P12;
            public readonly TPixel P13;

            public readonly TPixel P21;
            public readonly TPixel P22;
            public readonly TPixel P23;

            public readonly TPixel P31;
            public readonly TPixel P32;
            public readonly TPixel P33;

            #endregion

            #region API

            public static void Apply(SpanBitmap<TPixel> dstBitmap, KernelEvaluator<TPixel> function)
            {
                var w = dstBitmap.Width;

                var rows = new _RollingRows(w);
                dstBitmap.GetScanlinePixels(0).CopyTo(rows.Row3);
                dstBitmap.GetScanlinePixels(0).CopyTo(rows.Row2);

                --w;

                for (int y = 0; y < dstBitmap.Height; y++)
                {
                    // cycle tmp rows:
                    rows.Roll();
                    dstBitmap.GetScanlinePixels(Math.Min(dstBitmap.Height - 1, y + 1)).CopyTo(rows.Row3);

                    // apply to dst row:

                    var dstRow = dstBitmap.UseScanlinePixels(y);

                    var kernel = new Kernel3x3<TPixel>(rows, 0, 0, 1);
                    dstRow[0] = function(kernel);

                    for (int x = 1; x < w; x++)
                    {
                        kernel = new Kernel3x3<TPixel>(rows, x - 1, x, x + 1);
                        dstRow[x] = function(kernel);
                    }

                    kernel = new Kernel3x3<TPixel>(rows, w - 1, w, w);
                    dstRow[w] = function(kernel);
                }
            }

            public static void Apply<TSrcPixel>(SpanBitmap<TSrcPixel> dstBitmap, KernelEvaluator<TSrcPixel> function)
                where TSrcPixel: unmanaged
            {
                var w = dstBitmap.Width;

                var xrow = new TPixel[w];

                var converter = Pixel.GetPixelCopyConverter<TSrcPixel, TPixel>();
                

                var rows = new _RollingRows(w);
                converter(dstBitmap.GetScanlinePixels(0), rows.Row3);
                converter(dstBitmap.GetScanlinePixels(0), rows.Row2);

                --w;                

                for (int y = 0; y < dstBitmap.Height; y++)
                {
                    // cycle tmp rows:
                    rows.Roll();
                    converter(dstBitmap.GetScanlinePixels(Math.Min(dstBitmap.Height - 1, y + 1)), rows.Row3);

                    // apply to dst row:
                    
                    var dstRow = dstBitmap.UseScanlinePixels(y);

                    var kernel = new Kernel3x3<TPixel>(rows, 0, 0, 1);
                    dstRow[0] = function(kernel);

                    for (int x = 1; x < w; x++)
                    {
                        kernel = new Kernel3x3<TPixel>(rows, x - 1, x, x + 1);
                        dstRow[x] = function(kernel);
                    }

                    kernel = new Kernel3x3<TPixel>(rows, w - 1, w, w);
                    dstRow[w] = function(kernel);                    
                }                
            }

            public static void Process<TSrcPixel>(SpanBitmap<TSrcPixel> dstBitmap, KernelEvaluator<TPixel> kernelFunction, Action<TPixel> outputFunction)
                where TSrcPixel : unmanaged
            {
                var w = dstBitmap.Width;

                var xrow = new TPixel[w];

                var converter = Pixel.GetPixelCopyConverter<TSrcPixel, TPixel>();


                var rows = new _RollingRows(w);
                converter(dstBitmap.GetScanlinePixels(0), rows.Row3);
                converter(dstBitmap.GetScanlinePixels(0), rows.Row2);

                --w;

                for (int y = 0; y < dstBitmap.Height; y++)
                {
                    // cycle tmp rows:
                    rows.Roll();
                    converter(dstBitmap.GetScanlinePixels(Math.Min(dstBitmap.Height - 1, y + 1)), rows.Row3);

                    // apply to dst row:                    

                    var kernel = new Kernel3x3<TPixel>(rows, 0, 0, 1);
                    outputFunction(kernelFunction(kernel));

                    for (int x = 1; x < w; x++)
                    {
                        kernel = new Kernel3x3<TPixel>(rows, x - 1, x, x + 1);
                        outputFunction(kernelFunction(kernel));
                    }

                    kernel = new Kernel3x3<TPixel>(rows, w - 1, w, w);
                    outputFunction(kernelFunction(kernel));
                }
            }

            public static void Copy<TSrcPixel>(SpanBitmap<TSrcPixel> srcBitmap, SpanBitmap<TPixel> dstBitmap, KernelEvaluator<TPixel> function)
                where TSrcPixel : unmanaged
            {
                Guard.AreEqual(nameof(dstBitmap.Width), srcBitmap.Width, dstBitmap.Width);
                Guard.AreEqual(nameof(dstBitmap.Height), srcBitmap.Height, dstBitmap.Height);

                var converter = Pixel.GetPixelCopyConverter<TSrcPixel, TPixel>();

                var w = dstBitmap.Width;

                var rows = new _RollingRows(w);
                converter(srcBitmap.GetScanlinePixels(0), rows.Row3);
                converter(srcBitmap.GetScanlinePixels(0), rows.Row2);

                --w;

                for (int y = 0; y < dstBitmap.Height; y++)
                {
                    // cycle tmp rows:
                    rows.Roll();
                    converter(srcBitmap.GetScanlinePixels(Math.Min(dstBitmap.Height - 1, y + 1)), rows.Row3);

                    // apply to dst row:

                    var dstRow = dstBitmap.UseScanlinePixels(y);

                    var kernel = new Kernel3x3<TPixel>(rows, 0, 0, 1);
                    dstRow[0] = function(kernel);

                    for (int x = 1; x < w; x++)
                    {
                        kernel = new Kernel3x3<TPixel>(rows, x - 1, x, x + 1);
                        dstRow[x] = function(kernel);
                    }

                    kernel = new Kernel3x3<TPixel>(rows, w - 1, w, w);
                    dstRow[w] = function(kernel);
                }
            }

            #endregion

            #region nested types

            internal ref struct _RollingRows
            {
                public _RollingRows(int count)
                {
                    var trows = new TPixel[count * 3];
                    Row1 = trows.AsSpan(count * 0, count);
                    Row2 = trows.AsSpan(count * 1, count);
                    Row3 = trows.AsSpan(count * 2, count);
                }

                public Span<TPixel> Row1;
                public Span<TPixel> Row2;
                public Span<TPixel> Row3;

                public void Roll()
                {
                    var tmp = Row1;
                    Row1 = Row2;
                    Row2 = Row3;
                    Row3 = tmp;
                }
            }

            #endregion
        }
    }
}
