using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {
        public partial struct RGB24
        {
            public readonly struct MulAdd : IApplyTo<RGB24>
            {
                #region lifecycle

                public static implicit operator MulAdd((float m, float a) op) { return new MulAdd(new RGB96F(op.m, op.m, op.m), new RGB96F(op.a, op.a, op.a)); }

                public static implicit operator MulAdd((RGB96F m, RGB96F a) op) { return new MulAdd(op.m, op.a); }

                public MulAdd(RGB96F.MulAdd other) : this(new RGB96F(other.Multiply), new RGB96F(other.Addition)) { }

                public MulAdd(RGB96F mul, RGB96F add)
                {
                    MulR = (int)(65536f * mul.R);
                    MulG = (int)(65536f * mul.G);
                    MulB = (int)(65536f * mul.B);

                    AddR = (int)(255 * add.R);
                    AddG = (int)(255 * add.G);
                    AddB = (int)(255 * add.B);
                }

                #endregion

                #region data

                public readonly int MulR;
                public readonly int MulG;
                public readonly int MulB;

                public readonly int AddR;
                public readonly int AddG;
                public readonly int AddB;

                #endregion

                #region API

                public bool IsIdentity
                {
                    [MethodImpl(_PrivateConstants.Fastest)]
                    get
                    {
                        if (MulR != 65536) return false;
                        if (MulG != 65536) return false;
                        if (MulB != 65536) return false;
                        if (AddR != 0) return false;
                        if (AddG != 0) return false;
                        if (AddB != 0) return false;
                        return true;
                    }
                }

                [MethodImpl(_PrivateConstants.Fastest)]
                public readonly void ApplyTo(ref RGB24 target)
                {
                    int r = target.R;
                    int g = target.G;
                    int b = target.B;

                    r *= MulR;
                    g *= MulG;
                    b *= MulB;

                    r >>= 16;
                    g >>= 16;
                    b >>= 16;

                    r += AddR;
                    g += AddG;
                    b += AddB;

                    #if NETSTANDARD2_0
                    if (r > 255) r = 255; else if (r < 0) r = 0;
                    if (g > 255) g = 255; else if (g < 0) g = 0;
                    if (b > 255) b = 255; else if (b < 0) b = 0;
                    #else
                    r = Math.Clamp(r, 0, 255);
                    g = Math.Clamp(g, 0, 255);
                    b = Math.Clamp(b, 0, 255);
                    #endif

                    target.R = (Byte)r;
                    target.G = (Byte)g;
                    target.B = (Byte)b;
                }                

                #endregion
            }
        }

        public partial struct BGR24
        {
            public readonly struct MulAdd : IApplyTo<BGR24>
            {
                #region lifecycle

                public static implicit operator MulAdd((float m, float a) op) { return new MulAdd(new RGB96F(op.m, op.m, op.m), new RGB96F(op.a, op.a, op.a)); }

                public static implicit operator MulAdd((RGB96F m, RGB96F a) op) { return new MulAdd(op.m, op.a); }

                public MulAdd(RGB96F.MulAdd other) : this(new RGB96F(other.Multiply), new RGB96F(other.Addition)) { }

                public MulAdd(RGB96F mul, RGB96F add)
                {
                    MulR = (int)(65536f * mul.R);
                    MulG = (int)(65536f * mul.G);
                    MulB = (int)(65536f * mul.B);

                    AddR = (int)(255 * add.R);
                    AddG = (int)(255 * add.G);
                    AddB = (int)(255 * add.B);
                }

                #endregion

                #region data

                public readonly int MulR;
                public readonly int MulG;
                public readonly int MulB;

                public readonly int AddR;
                public readonly int AddG;
                public readonly int AddB;

                #endregion

                #region API

                public bool IsIdentity
                {
                    [MethodImpl(_PrivateConstants.Fastest)]
                    get
                    {
                        if (MulR != 65536) return false;
                        if (MulG != 65536) return false;
                        if (MulB != 65536) return false;
                        if (AddR != 0) return false;
                        if (AddG != 0) return false;
                        if (AddB != 0) return false;
                        return true;
                    }
                }

                [MethodImpl(_PrivateConstants.Fastest)]
                public readonly void ApplyTo(ref BGR24 target)
                {
                    int r = target.R;
                    int g = target.G;
                    int b = target.B;

                    r *= MulR;
                    g *= MulG;
                    b *= MulB;

                    r >>= 16;
                    g >>= 16;
                    b >>= 16;

                    r += AddR;
                    g += AddG;
                    b += AddB;

                    #if NETSTANDARD2_0
                    if (r > 255) r = 255; else if (r < 0) r = 0;
                    if (g > 255) g = 255; else if (g < 0) g = 0;
                    if (b > 255) b = 255; else if (b < 0) b = 0;
                    #else
                    r = Math.Clamp(r, 0, 255);
                    g = Math.Clamp(g, 0, 255);
                    b = Math.Clamp(b, 0, 255);
                    #endif

                    target.R = (Byte)r;
                    target.G = (Byte)g;
                    target.B = (Byte)b;
                }

                #endregion
            }
        }

        public partial struct RGB96F
        {            
            public readonly struct MulAdd : IApplyTo<RGB96F>
            {
                public static implicit operator MulAdd((float m, float a) op) { return new MulAdd(new RGB96F(op.m,op.m,op.m), new RGB96F(op.a, op.a, op.a)); }

                public static implicit operator MulAdd((RGB96F m, RGB96F a) op) { return new MulAdd(op.m, op.a); }

                public MulAdd(RGB96F.MulAdd other)
                {
                    Multiply = other.Multiply;
                    Addition = other.Addition;
                }

                public MulAdd(RGB96F mul, RGB96F add)
                {
                    Multiply = mul.RGB;
                    Addition = add.RGB;
                }

                public readonly System.Numerics.Vector3 Multiply;
                public readonly System.Numerics.Vector3 Addition;

                public bool IsOpacity => Multiply.X == Multiply.Y && Multiply.X == Multiply.Z && Addition == System.Numerics.Vector3.Zero;

                public float Opacity => Multiply.X;

                public bool IsIdentity => Multiply == System.Numerics.Vector3.One && Addition == System.Numerics.Vector3.Zero;

                [MethodImpl(_PrivateConstants.Fastest)]
                public readonly void ApplyTo(ref RGB96F target)
                {
                    target.RGB *= Multiply;
                    target.RGB += Addition;
                }                
            }
        }

        public partial struct BGR96F
        {
            public readonly struct MulAdd : IApplyTo<BGR96F>
            {
                public static implicit operator MulAdd((float m, float a) op) { return new MulAdd(new BGR96F(op.m, op.m, op.m), new BGR96F(op.a, op.a, op.a)); }

                public static implicit operator MulAdd((BGR96F m, BGR96F a) op) { return new MulAdd(op.m, op.a); }

                public MulAdd(RGB96F.MulAdd other)
                {
                    Multiply = new RGB96F(other.Multiply).To<BGR96F>().BGR;
                    Addition = new RGB96F(other.Addition).To<BGR96F>().BGR;
                }

                public MulAdd(BGR96F mul, BGR96F add)
                {
                    Multiply = mul.BGR;
                    Addition = add.BGR;
                }

                public readonly System.Numerics.Vector3 Multiply;
                public readonly System.Numerics.Vector3 Addition;

                public bool IsIdentity => Multiply == System.Numerics.Vector3.One && Addition == System.Numerics.Vector3.Zero;

                [MethodImpl(_PrivateConstants.Fastest)]
                public readonly void ApplyTo(ref BGR96F target)
                {
                    target.BGR *= Multiply;
                    target.BGR += Addition;
                }
            }
        }
    }
}
