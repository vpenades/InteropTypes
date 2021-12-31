using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    using PEF = PixelFormat.ElementID;

    /// <summary>
    /// Predefined pixel formats encoded as const UInt32 values,
    /// which makes them suitable for switch blocks.
    /// </summary>
    static class _PackedPixelCodes
    {
        private const uint SHIFT0 = 1;
        private const uint SHIFT1 = 256;
        private const uint SHIFT2 = 256 * 256;
        private const uint SHIFT3 = 256 * 256 * 256;

        public const uint Empty = (uint)PEF.Empty;

        public const uint Luminance8 = SHIFT0 * (uint)PEF.Luminance8;
        public const uint Alpha8 = SHIFT0 * (uint)PEF.Alpha8;

        public const uint Luminance16 = SHIFT0 * (uint)PEF.Luminance16;
        public const uint BGR565 = SHIFT0 * (uint)PEF.Blue5 | SHIFT1 * (uint)PEF.Green6 | SHIFT2 * (uint)PEF.Red5;
        public const uint BGRA4444 = SHIFT0 * (uint)PEF.Blue4 | SHIFT1 * (uint)PEF.Green4 | SHIFT2 * (uint)PEF.Red4 | SHIFT3 * (uint)PEF.Alpha4;
        public const uint BGRA5551 = SHIFT0 * (uint)PEF.Blue5 | SHIFT1 * (uint)PEF.Green5 | SHIFT2 * (uint)PEF.Red5 | SHIFT3 * (uint)PEF.Alpha1;
        public const uint Millimeter16 = SHIFT0 * (uint)PEF.Millimeter16;

        public const uint RGB24 = SHIFT0 * (uint)PEF.Red8 | SHIFT1 * (uint)PEF.Green8 | SHIFT2 * (uint)PEF.Blue8;
        public const uint BGR24 = SHIFT0 * (uint)PEF.Blue8 | SHIFT1 * (uint)PEF.Green8 | SHIFT2 * (uint)PEF.Red8;

        public const uint RGBA32 = SHIFT0 * (uint)PEF.Red8 | SHIFT1 * (uint)PEF.Green8 | SHIFT2 * (uint)PEF.Blue8 | SHIFT3 * (uint)PEF.Alpha8;
        public const uint BGRA32 = SHIFT0 * (uint)PEF.Blue8 | SHIFT1 * (uint)PEF.Green8 | SHIFT2 * (uint)PEF.Red8 | SHIFT3 * (uint)PEF.Alpha8;
        public const uint ARGB32 = SHIFT0 * (uint)PEF.Alpha8 | SHIFT1 * (uint)PEF.Red8 | SHIFT2 * (uint)PEF.Green8 | SHIFT3 * (uint)PEF.Blue8;

        public const uint RGBP32 = SHIFT0 * (uint)PEF.Red8 | SHIFT1 * (uint)PEF.Green8 | SHIFT2 * (uint)PEF.Blue8 | SHIFT3 * (uint)PEF.Premul8;
        public const uint BGRP32 = SHIFT0 * (uint)PEF.Blue8 | SHIFT1 * (uint)PEF.Green8 | SHIFT2 * (uint)PEF.Red8 | SHIFT3 * (uint)PEF.Premul8;
        public const uint PRGB32 = SHIFT0 * (uint)PEF.Premul8 | SHIFT1 * (uint)PEF.Red8 | SHIFT2 * (uint)PEF.Green8 | SHIFT3 * (uint)PEF.Blue8;

        public const uint Luminance32F = SHIFT0 * (uint)PEF.Luminance32F;

        public const uint RGB96F = SHIFT0 * (uint)PEF.Red32F | SHIFT1 * (uint)PEF.Green32F | SHIFT2 * (uint)PEF.Blue32F;
        public const uint BGR96F = SHIFT0 * (uint)PEF.Blue32F | SHIFT1 * (uint)PEF.Green32F | SHIFT2 * (uint)PEF.Red32F;

        public const uint RGBA128F = SHIFT0 * (uint)PEF.Red32F | SHIFT1 * (uint)PEF.Green32F | SHIFT2 * (uint)PEF.Blue32F | SHIFT3 * (uint)PEF.Alpha32F;
        public const uint RGBP128F = SHIFT0 * (uint)PEF.Red32F | SHIFT1 * (uint)PEF.Green32F | SHIFT2 * (uint)PEF.Blue32F | SHIFT3 * (uint)PEF.Premul32F;

        public const uint BGRA128F = SHIFT0 * (uint)PEF.Blue32F | SHIFT1 * (uint)PEF.Green32F | SHIFT2 * (uint)PEF.Red32F | SHIFT3 * (uint)PEF.Alpha32F;
        public const uint BGRP128F = SHIFT0 * (uint)PEF.Blue32F | SHIFT1 * (uint)PEF.Green32F | SHIFT2 * (uint)PEF.Red32F | SHIFT3 * (uint)PEF.Premul32F;
    }
}
