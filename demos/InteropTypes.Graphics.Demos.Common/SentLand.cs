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

        // Calculate random map axis coordinate
        public static int random_coord()
        {
            while (true)
            {
                var r = rng() & 0x1F;
                if (r < 0x1F)
                {
                    return r;
                }
            }
        }

        #endregion

        #region data

        public enum ObjectType
        {
            NONE = -1,
            ROBOT = 0,
            SENTRY = 1,
            TREE = 2,
            BOULDER = 3,
            MEANIE = 4,
            SENTINEL = 5,
            PEDESTAL = 6,
        }

        public class SentObject
        {
            public int X;
            public int Y;
            public int Z;
            public int Rot;
            public int Step;
            public int Timer;
            public ObjectType Type;
        }

        private readonly int[] _Cells = new int[1024];

        private readonly List<SentObject> _Objects = new List<SentObject>();

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

        public int shape_at(int x, int z)
        {
            return _Cells[_WrappedIndex(z,x)] & 0xF;
        }

        // Return map height at given location
        public int height_at(int x, int z)
        {
            return _Cells[_WrappedIndex(z, x)] >> 4;
        }

        // Return True if the map location is a flat tile
        public bool is_flat(int x, int z)
        {
            return shape_at(x, z) == 0;
        }

        // Return a list of objects stacked at map location
        public IEnumerable<SentObject> objects_at(int x, int z)
        {
            return _Objects.Where(item => item.X == x && item.Z == z);
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

        // Smooth 3 map vertices, returning a new central vertex height
        private static int _DespikeMidval(int a, int b, int c)
        {
            if (b == c) return b;

            if (b > c)
            {
                if (b <= a) return b;
                else if (a < c) return c;
                else return a;
            }
            
            if (b >= a) return b;
            else if (c < a) return c;
            else return a;
        }

        // Smooth a slice by flattening single vertex peaks and troughs
        private static IEnumerable<int> _DespikeSlice(int[] arr)
        {
            var arr_copy = new int[arr.Length];
            arr.CopyTo(arr_copy, 0);

            foreach (var x in Enumerable.Range(0, 0x20).Reverse())
            {
                arr_copy[x + 1] = _DespikeMidval(arr_copy[x + 0], arr_copy[x + 1], arr_copy[x + 2]);
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
            return; // test files not available

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

            // generate objects

            return land;
        }




        // Determine number of sentries on landscape
        private static int calc_num_sentries(int landscape_bcd)
        {
            // Only ever the Sentinel on the first landscape.
            if (landscape_bcd == 0x0000) return 1;            

            // Base count uses landscape BCD thousands digit, offset by 2.
            var base_sentries = ((landscape_bcd & 0xF000) >> 12) + 2;

            var num_sentries = 0;

            while (true)
            {
                var r = rng();

                // count leading zeros on b6-0 for adjustment size
                var adjust = (format(r & 0x7F, "07b") + "1").IndexOf('1');

                // b7 determines adjustment sign (note: 1s complement)
                if ((r & 0x80) != 0)
                {
                    adjust = ~adjust;
                }

                num_sentries = base_sentries + adjust;
                if (0 <= num_sentries && num_sentries <= 7)
                {
                    break;
                }
            }

            // Levels under 100 use tens digit to limit number of sentries.
            var max_sentries = (landscape_bcd & 0x00F0) >> 4;
            if (landscape_bcd >= 0x0100 || max_sentries > 7)
            {
                max_sentries = 7;
            }
            // Include Sentinel in sentry count.
            return 1 + Math.Min(num_sentries, max_sentries);
        }

        // Find the highest placement positions in 4x4 regions on the map
        private IEnumerable<(int height, int x, int z)> highest_positions()
        {            
            // Scan the map as 64 regions of 4x4 (less one on right/back edges)
            // in z order from front to back and x from left to right.

            foreach (var i in Enumerable.Range(0, 0x40))
            {
                var gridx = (i & 7) << 2;
                var gridz = (i & 0x38) >> 1;
                var max_height = 0;
                var max_x = -1;
                var max_z = -1;

                // Scan each 4x4 region, z from front to back, x from left to right.
                foreach (var j in Enumerable.Range(0, 0x10))
                {
                    var x = gridx + (j & 3);
                    var z = gridz + (j >> 2);

                    // The back and right edges are missing a tile, so skip.
                    if (x == 0x1F || z == 0x1F) continue;

                    var height = height_at(x,z);
                    if (is_flat(x, z) && height >= max_height)
                    {
                        max_height = height;
                        max_x = x;
                        max_z = z;
                    }
                }

                yield return (max_height, max_x, max_z);
            }            
        }

        // Place object at given position but with random rotation
        private SentObject object_at(ObjectType type, int x, int y, int z)
        {
            var obj = new SentObject();
            obj.Type = type;
            obj.X = x;
            obj.Y = y;
            obj.Z = z;

            // Random rotation, limited to 32 steps, biased by +135 degrees.
            obj.Rot = (rng() & 0xF8) + 0x60 & 0xFF;
            return obj;
        }

        

        // Generate given object at a random unused position below the given height
        private SentObject object_random(ObjectType type, int max_height)
        {
            while (true)
            {
                foreach (var attempt in Enumerable.Range(0, 0xFF))
                {
                    var x = random_coord();
                    var z = random_coord();
                    var y = height_at(x, z);

                    if (!is_flat(x, z)) continue;
                    if (objects_at(x, z).Any()) continue;
                    if (y >= max_height) continue;
                    
                    return object_at(type, x, y, z);
                }

                max_height += 1;

                if (max_height >= 0xC) return null;
            }
        }

        static string format(int value, string fmt)
        {
            throw new NotImplementedException();
        }

        // Place Sentinel and appropriate sentry count for given landscape
        public int place_sentries(int landscape_bcd)
        {
            int[] height_indices;
            var objects = new List<object>();
            var highest = highest_positions().ToArray();
            var max_height = highest.Select(item => item.height).Max();
            var num_sentries = calc_num_sentries(landscape_bcd);

            foreach (var _ in Enumerable.Range(0, num_sentries))
            {
                while (true)
                {
                    // Filter for high positions at the current height limit.
                    height_indices = highest
                        .Select((tuple, i) => (i, tuple))
                        .Where(tuple => tuple.tuple.height == max_height)
                        .Select(item => item.i)
                        .ToArray();

                    if (height_indices.Length == 0) break;

                    // No locations so try 1 level down, stopping at zero.
                    max_height -= 1;
                    if (max_height == 0) return max_height;
                }

                // Results are in reverse order due to backwards 6502 iteration loop.
                height_indices.Reverse();

                // Mask above number of entries to limit random scope.
                int idx_mask = 0xFF >> format(height_indices.Length, "08b").IndexOf('1');
                int idx = 0;
                while (true)
                {
                    idx = rng() & idx_mask;
                    if (idx < height_indices.Length)
                    {
                        break;
                    }
                }
                var idx_grid = height_indices[idx];
                var _tup_2 = highest[idx_grid];
                var y = _tup_2.height;
                var x = _tup_2.x;
                var z = _tup_2.z;

                // Invalidate the selected and surrounding locations by setting zero height.
                foreach (var offset in new int[] { -9, -8, -7, -1, 0, 1, 7, 8, 9 })
                {
                    var idx_clear = idx_grid + offset;
                    if (idx_clear >= 0 && idx_clear < highest.Length)
                    {
                        highest[idx_clear].height = 0;
                    }
                }
                if (_Objects.Count == 0)
                {
                    var pedestal = object_at(ObjectType.PEDESTAL, x, y, z);
                    pedestal.Rot = 0;
                    objects.Add(pedestal);
                    objects.Add(object_at(ObjectType.SENTINEL, x, y + 1, z));
                }
                else
                {
                    objects.Add(object_at(ObjectType.SENTRY, x, y, z));
                }

                // Generate rotation step/direction and timer delay from RNG.
                var r = rng();
                _Objects[-1].Step = (r & 1) != 0 ? -20 : +20;
                _Objects[-1].Timer = r >> 1 & 0x1F | 5;
            }
            return max_height;
        }

        // Place player robot on the landscape
        public int place_player(int landscape_bcd, int max_height)
        {
            SentObject player;

            // The player position is fixed on landscape 0000.
            if (landscape_bcd == 0)
            {
                var x = 0x08;
                var z = 0x11;
                player = object_at(ObjectType.ROBOT, x, height_at(x, z), z);
            }
            else
            {
                // Player is never placed above height 6.
                var max_player_height = Math.Min(max_height, 6);
                player = object_random(ObjectType.ROBOT, max_player_height);
            }
            _Objects.Add(player);
            return max_height;
        }

        // Place the appropriate number of trees for the sentry count
        public int place_trees(int max_height)
        {
            // Count the placed Sentinel and sentries.
            var num_sents = _Objects
                .Where(item => item.Type == ObjectType.SENTINEL || item.Type == ObjectType.SENTRY)
                .Count();

            var r = rng();
            var max_trees = 48 - 3 * num_sents;
            var num_trees = (r & 7) + (r >> 3 & 0xF) + 10;
            num_trees = Math.Min(num_trees, max_trees);

            foreach (var _ in Enumerable.Range(0, num_trees))
            {
                var tree = object_random(ObjectType.TREE, max_height);

                _Objects.Add(tree);
            }

            return max_height;
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
