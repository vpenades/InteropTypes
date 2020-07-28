using System;
using System.Collections.Generic;
using System.Text;



namespace InteropBitmaps
{
    using PEF = Pixel.ElementID;

    public static partial class Pixel
    {
        /// <summary>
        /// Predefined pixel formats encoded as const UInt32 values,
        /// which makes them suitable for switch blocks.
        /// </summary>
        public static class Packed
        {
            private const uint SHIFT0 = 1;
            private const uint SHIFT1 = 256;
            private const uint SHIFT2 = 256 * 256;
            private const uint SHIFT3 = 256 * 256 * 256;

            public const uint Empty = (uint)PEF.Empty;

            public const uint Gray8 = SHIFT0 * (uint)PEF.Gray8;
            public const uint Alpha8 = SHIFT0 * (uint)PEF.Alpha8;

            public const uint Gray16 = SHIFT0 * (uint)PEF.Gray16;
            public const uint BGR565 = SHIFT0 * (uint)PEF.Blue5 | SHIFT1 * (uint)PEF.Green6 | SHIFT2 * (uint)PEF.Red5;
            public const uint BGRA4444 = SHIFT0 * (uint)PEF.Blue4 | SHIFT1 * (uint)PEF.Green4 | SHIFT2 * (uint)PEF.Red4 | SHIFT3 * (uint)PEF.Alpha4;
            public const uint BGRA5551 = SHIFT0 * (uint)PEF.Blue5 | SHIFT1 * (uint)PEF.Green5 | SHIFT2 * (uint)PEF.Red5 | SHIFT3 * (uint)PEF.Alpha1;
            public const uint DepthMM16 = SHIFT0 * (uint)PEF.DepthMM16;

            public const uint RGB24 = SHIFT0 * (uint)PEF.Red8 | SHIFT1 * (uint)PEF.Green8 | SHIFT2 * (uint)PEF.Blue8;
            public const uint BGR24 = SHIFT0 * (uint)PEF.Blue8 | SHIFT1 * (uint)PEF.Green8 | SHIFT2 * (uint)PEF.Red8;

            public const uint RGBA32 = SHIFT0 * (uint)PEF.Red8 | SHIFT1 * (uint)PEF.Green8 | SHIFT2 * (uint)PEF.Blue8 | SHIFT3 * (uint)PEF.Alpha8;
            public const uint BGRA32 = SHIFT0 * (uint)PEF.Blue8 | SHIFT1 * (uint)PEF.Green8 | SHIFT2 * (uint)PEF.Red8 | SHIFT3 * (uint)PEF.Alpha8;
            public const uint ARGB32 = SHIFT0 * (uint)PEF.Alpha8 | SHIFT1 * (uint)PEF.Red8 | SHIFT2 * (uint)PEF.Green8 | SHIFT3 * (uint)PEF.Blue8;

            public const uint Gray32F = SHIFT0 * (uint)PEF.Gray32F;

            public const uint BGR96F = SHIFT0 * (uint)PEF.Blue32F | SHIFT1 * (uint)PEF.Green32F | SHIFT2 * (uint)PEF.Red32F;

            public const uint RGBA128F = SHIFT0 * (uint)PEF.Red32F | SHIFT1 * (uint)PEF.Green32F | SHIFT2 * (uint)PEF.Blue32F | SHIFT3 * (uint)PEF.Alpha32F;

            public const uint BGRA128F = SHIFT0 * (uint)PEF.Blue32F | SHIFT1 * (uint)PEF.Green32F | SHIFT2 * (uint)PEF.Red32F | SHIFT3 * (uint)PEF.Alpha32F;
        }

        /// <summary>
        /// Predefined pixel formats, mirroring <see cref="Packed"/> definitions.
        /// </summary>
        public static class Standard
        {
            public static readonly Format Empty = new Format(Packed.Empty);

            public static readonly Format Gray8 = new Format(Packed.Gray8);
            public static readonly Format Alpha8 = new Format(Packed.Alpha8);

            public static readonly Format Gray16 = new Format(Packed.Gray16);
            public static readonly Format BGR565 = new Format(Packed.BGR565);
            public static readonly Format BGRA4444 = new Format(Packed.BGRA4444);
            public static readonly Format BGRA5551 = new Format(Packed.BGRA5551);
            public static readonly Format DepthMM16 = new Format(Packed.DepthMM16);

            public static readonly Format RGB24 = new Format(Packed.RGB24);
            public static readonly Format BGR24 = new Format(Packed.BGR24);

            public static readonly Format RGBA32 = new Format(Packed.RGBA32);
            public static readonly Format BGRA32 = new Format(Packed.BGRA32);
            public static readonly Format ARGB32 = new Format(Packed.ARGB32);

            public static readonly Format Gray32F = new Format(Packed.Gray32F);

            public static readonly Format BGR96F = new Format(Packed.BGR96F);

            public static readonly Format RGBA128F = new Format(Packed.RGBA128F);

            public static readonly Format BGRA128F = new Format(Packed.BGRA128F);
        }
    }
}
