using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace InteropTypes.Tensors
{
    partial struct SpanTensor2<T>
    {
        internal unsafe Imaging.AsciiImage DebugBitmap
        {
            get
            {
                if (sizeof(T) == 3)
                {
                    var span = System.Runtime.InteropServices.MemoryMarshal.Cast<T, _PixelXYZ24>(this.Span);
                    return Imaging.AsciiImage.Create(14, span, p => p.Gray, this.Dimensions[1], this.Dimensions[1], this.Dimensions[1]);
                }

                if (typeof(T) == typeof(byte))
                {
                    var span = System.Runtime.InteropServices.MemoryMarshal.Cast<T, byte>(this.Span);
                    return Imaging.AsciiImage.Create(14, span, p => p, this.Dimensions[1], this.Dimensions[1], this.Dimensions[1]);
                }

                if (typeof(T) == typeof(float))
                {
                    var span = System.Runtime.InteropServices.MemoryMarshal.Cast<T, float>(this.Span);
                    var min = System.Numerics.Tensors.TensorPrimitives.Min(span);
                    var max = System.Numerics.Tensors.TensorPrimitives.Max(span);
                    var mul = 255f / (max - min);

                    byte grayEval(float l)
                    {
                        l -= min;
                        l *= mul;
                        return (Byte)Math.Clamp(l, 0, 255);
                    }

                    return Imaging.AsciiImage.Create(14, span, grayEval, this.Dimensions[1], this.Dimensions[1], this.Dimensions[1]);
                }

                if (typeof(T) == typeof(System.Numerics.Vector3))
                {
                    var span = System.Runtime.InteropServices.MemoryMarshal.Cast<T, System.Numerics.Vector3>(this.Span);
                    var singles = System.Runtime.InteropServices.MemoryMarshal.Cast<T, float>(this.Span);
                    var min = System.Numerics.Tensors.TensorPrimitives.Min(singles);
                    var max = System.Numerics.Tensors.TensorPrimitives.Max(singles);
                    var mul = 255f / (max - min);

                    byte grayEval(System.Numerics.Vector3 v)
                    {
                        var l = (v.X + v.Y + v.Z) / 3;
                        l -= min;
                        l *= mul;
                        return (Byte)Math.Clamp(l, 0, 255);
                    }

                    return Imaging.AsciiImage.Create(14, span, grayEval, this.Dimensions[1], this.Dimensions[1], this.Dimensions[1]);
                }

                return default;
            }
        }
    }

    namespace Imaging
    {

        /// <summary>
        /// helper structure used to convert a pixels image into an ascii image that can be used for debugging
        /// </summary>
        internal readonly struct AsciiImage
        {
            #region constructor API

            public static AsciiImage Create<T>(int maxRows, Span<T> bitmap, Func<T, Byte> evalGray, int rowStep, int width, int height, Func<(Byte m11, Byte m12, Byte m21, Byte m22), char> charFunc = null)
            {
                var maxCols = width * maxRows / height;

                maxCols *= 2; // characters are twice as high than wide

                var ascii = new AsciiImage(maxCols * 2, maxRows * 2, charFunc);

                ascii.FitBitmap(bitmap, evalGray, rowStep, width, height);

                return ascii;
            }

            #endregion

            #region lifecycle

            private AsciiImage(int w, int h, Func<(Byte m11, Byte m12, Byte m21, Byte m22), char> pixEval)
            {
                _Width = w;
                _Pixels = new byte[w * h];
                _PixEvalFunc = pixEval ?? HighDefEvaluator;
            }

            #endregion

            #region data

            private readonly Byte[] _Pixels;
            private readonly int _Width;
            private readonly Func<(Byte m11, Byte m12, Byte m21, Byte m22), char> _PixEvalFunc;

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public readonly string[] Rows => Enumerable.Range(0, Height / 2).Select(EvalRow).ToArray();

            #endregion

            #region properties

            public int Width => _Width;

            public int Height => _Pixels.Length / _Width;

            #endregion

            #region API

            public void FitBitmap<T>(Span<T> srcBitmap, Func<T, byte> evalGray, int srcStep, int srcWidth, int srcHeight)
            {
                var dstHeight = this.Height;

                for (int dstY = 0; dstY < dstHeight; ++dstY)
                {
                    var srcY = dstY * srcHeight / dstHeight;
                    var srcRow = srcBitmap.Slice(srcY * srcStep, srcWidth);

                    for (int dstX = 0; dstX < _Width; ++dstX)
                    {
                        var srcX = dstX * srcWidth / _Width;
                        var val = evalGray(srcRow[srcX]);
                        SetPixel(dstX, dstY, val);
                    }
                }
            }

            public void SetPixel(int x, int y, byte value)
            {
                if (x < 0 || x >= Width) return;
                y *= _Width;
                if (y < 0 || y >= _Pixels.Length) return;
                _Pixels[y + x] = value;
            }

            private string EvalRow(int rowIdx)
            {
                rowIdx *= 2;

                var y0 = (rowIdx + 0) * _Width;
                var y1 = y0 + _Width;

                var srcRow0 = _Pixels.AsSpan().Slice(y0, _Width);
                var srcRow1 = _Pixels.AsSpan().Slice(y1, _Width);

                var row = string.Empty;

                for (int x = 0; x < srcRow0.Length / 2; ++x)
                {
                    var a = srcRow0[x * 2 + 0];
                    var b = srcRow0[x * 2 + 1];
                    var c = srcRow1[x * 2 + 0];
                    var d = srcRow1[x * 2 + 1];

                    var cc = _PixEvalFunc((a, b, c, d));

                    row += cc;
                }

                return row;
            }

            #endregion

            #region pixel evaluators

            private const char _BlockWhiteSpaceChar = ' ';

            public static char HighDefEvaluator((Byte m11, Byte m12, Byte m21, Byte m22) block)
            {
                static bool? _isBlackOrWhite(byte value, byte min = 128, byte max = 128)
                {
                    if (value < min) return false;
                    if (value >= max) return true;
                    return null;
                }

                bool? m11_BW = _isBlackOrWhite(block.m11);
                bool? m12_BW = _isBlackOrWhite(block.m12);
                bool? m21_BW = _isBlackOrWhite(block.m21);
                bool? m22_BW = _isBlackOrWhite(block.m22);

                var count = 0;
                count += m11_BW.HasValue ? 1 : 0;
                count += m12_BW.HasValue ? 1 : 0;
                count += m21_BW.HasValue ? 1 : 0;
                count += m22_BW.HasValue ? 1 : 0;

                if (count >= 3 && false)
                {
                    m11_BW ??= _isBlackOrWhite(block.m11, 128, 128);
                    m12_BW ??= _isBlackOrWhite(block.m12, 128, 128);
                    m21_BW ??= _isBlackOrWhite(block.m21, 128, 128);
                    m22_BW ??= _isBlackOrWhite(block.m22, 128, 128);

                    return GetQuadBlockChar(!m11_BW.Value, !m12_BW.Value, !m21_BW.Value, !m22_BW.Value);
                }

                var average = (int)block.m11;
                average += (int)block.m12;
                average += (int)block.m21;
                average += (int)block.m22;
                average /= 4;

                if (average < 51 * 1) return '█';
                if (average < 51 * 2) return '▓';
                if (average < 51 * 3) return '▒';
                if (average < 51 * 4) return '░';
                return _BlockWhiteSpaceChar;
            }

            public static char GetQuadBlockChar(bool m11, bool m12, bool m21, bool m22)
            {
                var index = 0;
                index |= m11 ? 1 : 0;
                index |= m12 ? 2 : 0;
                index |= m21 ? 4 : 0;
                index |= m22 ? 8 : 0;

                var c = GetQuadBlockChar(index);

                System.Diagnostics.Debug.Assert(c < 0x258c && c > 0x2589, "these characters are not monospace on visual studio fonts");

                return c;
            }

            public static char GetQuadBlockChar(int index)
            {
                switch (index)
                {
                    case 0: return (char)0x2581; // should be 258c but it seems it's not monospace
                    case 1: return '▘';
                    case 2: return '▝';
                    case 3: return '▀';
                    case 4: return '▖';
                    case 5: return (char)0x2581; // should be 258c but it seems it's not monospace
                    case 6: return '▞';
                    case 7: return '▛';
                    case 8: return '▗';
                    case 9: return '▚';
                    case 10: return '▐';
                    case 11: return '▜';
                    case 12: return '▄';
                    case 13: return '▙';
                    case 14: return '▟';
                    case 15: return '█';
                    default: throw new NotImplementedException();
                }
            }

            #endregion
        }
    }

}
