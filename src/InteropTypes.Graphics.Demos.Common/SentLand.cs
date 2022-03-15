using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InteropTypes.Graphics.Drawing;

namespace InteropTypes
{
    /// <summary>
    /// The Sentinel map generator.
    /// </summary>
    /// <remarks>
    /// <see href="https://github.com/simonowen/sentland">Originally created by Simon Owen.</see>
    /// </remarks>
    public class SentLand : IDrawingBrush<IScene3D>
    {
        #region random

        public static ulong ull = 0;

        // Seed RNG using landscape number
        public static void seed(int landscape_bcd)
        {
            ull = (ulong)(1 << 16 | landscape_bcd);
        }

        // Pull next 8-bit value from random number generator
        public static int rng()
        {
            for (int i = 0; i < 8; ++i)
            {
                ull <<= 1;
                ull |= ((ull >> 20) ^ (ull >> 33)) & 1;
            }
            return (int)((ull >> 32) & 0xff);
        }

        // Random number in range 0 to 0x16
        public static int rng_00_16()
        {
            var r = rng();
            return (r & 7) + (r >> 3 & 0xf);
        }

        #endregion

        #region data

        private readonly int[] _Cells = new int[1024];

        public int this[int y, int x]
        {
            get => _Cells[_WrappedIndex(y, x)];
            set => _Cells[_WrappedIndex(y, x)] = value;
        }

        private static int _WrappedIndex(int y, int x)
        {
            var yy = ((uint)y) & 0x1f;
            var xx = ((uint)x) & 0x1f;
            return (int)(yy * 0x20 + xx);
        }

        public Point3 GetPoint(int y, int x)
        {
            var h = this[y, x] >> 4;

            return new Point3(y-16, h*2, x-16);
        }

        #endregion

        #region core

        private void _SetRow(int y, IEnumerable<int> xxx)
        {
            int x = 0;
            foreach (var v in xxx) this[y, x++] = v;
        }

        private void _SetColumn(int x, IEnumerable<int> yyy)
        {
            int y = 0;
            foreach (var v in yyy) this[y++, x] = v;
        }


        private IEnumerable<int> _SliceWrappedRow(int y, int count = 0x23)
        {
            return Enumerable.Range(0, count).Select(x => this[y, x & 0x1f]);
        }

        private IEnumerable<int> _SliceWrappedColumn(int x, int count = 0x23)
        {
            return Enumerable.Range(0, count).Select(y => this[y & 0x1f, x]);
        }
        

        // Convert linear game map data offset to x and z coordinates
        private static (int,int) _GetXY(int offset)
        {
            var x = (offset & 0x300) >> 8 | (offset & 0xe0) >> 3;
            var z = offset & 0x1f;
            return (x, z);
        }

        // Return map entry at given original game map offset
        private int _AtOffset(int offset)
        {
            var _tup_1 = _GetXY(offset);
            var x = _tup_1.Item1;
            var z = _tup_1.Item2;
            return this[z,x];
        }        

        // Smooth the map by averaging groups across the given axis
        private void _SmoothMap(string axis)
        {
            if (axis == "z")
            {
                foreach (var z in Enumerable.Range(0, 0x20))
                {
                    var slice = _SliceWrappedRow(z).ToArray();
                    slice = _SmoothSlice(slice).ToArray();

                    _SetRow(z, slice);
                }
            }
            else
            {
                foreach (var x in Enumerable.Range(0, 0x20))
                {
                    var slice = _SliceWrappedColumn(x).ToArray();
                    slice = _SmoothSlice(slice).ToArray();
                    _SetColumn(x, slice);
                }
            }
        }

        // De-spike the map in slices across the given axis
        private void _DespikeMap(string axis)
        {
            if (axis == "z")
            {
                foreach (var z in Enumerable.Range(0, 0x20))
                {
                    _SetRow(z, _DespikeSlice(_SliceWrappedRow(z).ToArray()));
                }

            }
            else
            {
                foreach (var x in Enumerable.Range(0, 0x20))
                {
                    _SetColumn(x, _DespikeSlice(_SliceWrappedColumn(x).ToArray()));
                }
            }
        }

        // Smooth a map slice by averaging neighbouring groups of values
        private static IEnumerable<int> _SmoothSlice(int[] arr)
        {
            var group_size = arr.Length - 0x1f;

            return Enumerable
                .Range(0, arr.Length - group_size + 1)
                .Select(x => arr.Skip(x).Take(group_size).Sum() / group_size);
        }

        // Return lowest neighbour if peak or trough, else middle value
        private static int _DespikeMidval(int a, int b, int c)
        {
            var isTrough = a > b && b < c;
            var isPeak = a < b && b > c;
            return isTrough || isPeak
                ? Math.Min(a, c)
                : b;
        }

        // Smooth a slice by flattening single vertex peaks and troughs
        private static IEnumerable<int> _DespikeSlice(int[] arr)
        {
            var arr_copy = new int[arr.Length];
            arr.CopyTo(arr_copy, 0);

            foreach (var x in Enumerable.Range(1, arr.Length - 1 - 1).Reverse())
            {
                arr_copy[x] = _DespikeMidval(arr_copy[x - 1], arr_copy[x + 0], arr_copy[x + 1]);
            }

            return arr_copy.Take(32);
        }

        

        // Scale and offset values to generate vertex heights
        private static int _ScaleAndOffset(int val, int scale = 0x18)
        {
            var mag = val - 0x80;
            mag = (int)Math.Floor((float)mag * (float)scale / 256f);
            mag = Math.Max(mag + 6, 0);
            mag = Math.Min(mag + 1, 11);
            return mag;
        }

        // Determine tile shape code from 4 vertex heights
        private static int _TileShape(int fl, int bl, int br, int fr)
        {
            int shape;
            if (fl == fr)
            {
                if (fl == bl)
                {
                    if (fl == br) { shape = 0; }
                    else if (fl < br) { shape = 0xa; }
                    else { shape = 0x3; }
                }
                else if (br == bl)
                {
                    if (br < fr) { shape = 0x1; }
                    else { shape = 0x9; }
                }
                else if (br == fr)
                {
                    if (br < bl) { shape = 0x6; }
                    else { shape = 0xf; }
                }
                else { shape = 0xc; }
            }
            else if (fl == bl)
            {
                if (br == fr)
                {
                    if (br < bl) { shape = 0x5; }
                    else { shape = 0xd; }
                }
                else if (br == bl)
                {
                    if (br < fr) { shape = 0xe; }
                    else { shape = 0x7; }
                }
                else
                {
                    shape = 0x4;
                }
            }
            else if (br == fr)
            {
                if (br == bl)
                {
                    if (br < fl) { shape = 0xb; }
                    else { shape = 0x2; }
                }
                else { shape = 0x4; }
            }
            else
            {
                shape = 0xc;
            }

            return shape;
        }

        // Add tile shape code to upper 4 bits of each tile
        private static SentLand add_tile_shapes(SentLand maparr)
        {
            var new_maparr = new SentLand();

            foreach (var z in Enumerable.Range(0, 0x1f).Reverse())
            {
                foreach (var x in Enumerable.Range(0, 0x1f).Reverse())
                {
                    var fl = maparr[z + 0,x + 0] & 0xf;
                    var bl = maparr[z + 1,x + 0] & 0xf;
                    var br = maparr[z + 1,x + 1] & 0xf;
                    var fr = maparr[z + 0,x + 1] & 0xf;
                    var shape = _TileShape(fl, bl, br, fr);
                    new_maparr[z,x] = shape << 4 | maparr[z,x] & 0xf;
                }
            }
            return new_maparr;
        }

        // Convert array data to in-memory format used by game
        private Byte[] _ToBytes()
        {
            return Enumerable.Range(0, 1024)
                .Select(x => _AtOffset(x))
                .Select(item => (byte)item)
                .ToArray();
        }        

        private void _Verify(int code, string name)
        {
            if (code != 0 && code != 0x9999) return;

            var rbytes = System.IO.File.ReadAllBytes($"golden/{code:X4}_{name}.bin");
            var lbytes = this._ToBytes();

            for (int i=0; i< 1024; ++i)
            {
                if (rbytes[i] != lbytes[i]) throw new InvalidOperationException();
            }            
        }

        #endregion

        #region API

        public static uint num_landscapes = 0xe000;

        public static SentLand GenerateLandscape(int landscape_bcd)
        {
            // Seed RNG using landscape number in BCD.
            seed(landscape_bcd);

            // Read 81 values to warm the RNG.
            for (int i = 0; i < 0x51; ++i) rng();

            // Random height scaling (but fixed value for landscape 0000!).
            var height_scale = landscape_bcd != 0
                ? rng_00_16() + 0x0e
                : 0x18;

            // Fill the map with random values (z from back to front, x from right to left).
            var land = new SentLand();

            foreach(var z in Enumerable.Range(0, 0x20).Reverse())
            {
                foreach(var x in Enumerable.Range(0, 0x20).Reverse())
                {
                    land[z, x] = rng();
                }
            }

            land._Verify(landscape_bcd, "random");

            // 2 passes of smoothing, each across z-axis then x-axis.
            foreach (var _ in Enumerable.Range(0, 2))
            {
                land._SmoothMap("z");
                land._SmoothMap("x");
            }

            land._Verify(landscape_bcd, "smooth3");

            // Scale and offset values to give vertex heights in range 1 to 11.

            for(int i=0; i < land._Cells.Length; ++i)
            {
                land._Cells[i] = _ScaleAndOffset(land._Cells[i], height_scale);
            }

            land._Verify(landscape_bcd, "scaled");

            // Two de-spike passes, each across z-axis then x-axis.
            foreach (var _ in Enumerable.Range(0, 2))
            {
                land._DespikeMap("z");
                land._DespikeMap("x");
            }

            land._Verify(landscape_bcd, "despike3");

            // Add shape codes for each tile, to simplify examining the landscape.
            // land = add_tile_shapes(land);

            // maparr.verify(landscape_bcd, "shape");

            // Finally, swap the high and low nibbles in each byte for the final format.
            for (int i = 0; i < land._Cells.Length; i++)
            {
                land._Cells[i] = (land._Cells[i] & 0xf) << 4 | land._Cells[i] >> 4;
            }

            // verify(maparr, landscape_bcd, "swap");

            return land;
        }

        // Crude viewing of generated landscape data
        public void DrawTo(IScene3D context)
        {
            for(int z=0; z <32; ++z)
            {
                for (int x = 0; x <32; ++x)
                {
                    var v1 = GetPoint(z + 0, x + 0);
                    var v2 = GetPoint(z + 1, x + 0);
                    var v3 = GetPoint(z + 1, x + 1);
                    var v4 = GetPoint(z + 0, x + 1);

                    var light = 0 != ((x & 1) ^ (z & 1));
                    var slope = !(v1.Y == v2.Y && v1.Y == v3.Y && v1.Y == v4.Y);

                    var color = slope
                        ? light ? System.Drawing.Color.Gray : System.Drawing.Color.DarkGray
                        : light ? System.Drawing.Color.Green : System.Drawing.Color.DarkGreen;                                   

                    context.DrawSurface(color, v1, v2, v3, v4);
                }
            }
        }

        #endregion
    }
}
