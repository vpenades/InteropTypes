using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Drawing;

using XY = System.Numerics.Vector2;
using COLOR = System.Drawing.Color;

namespace InteropTypes
{
    public class BitmapGrid
    {
        #region lifecycle

        public BitmapGrid(int width, int height, ImageSource[] templates)
        {
            _Sprites = templates;
            _Width = width;
            _Height = height;
            _Tiles = new int[_Width * _Height];

            _Tiles[2] = 5;
            _Tiles[5] = 7;
        }

        #endregion

        #region data

        private readonly ImageSource[] _Sprites;

        private readonly int _Width;
        private readonly int _Height;

        private readonly int[] _Tiles;

        #endregion

        #region API

        public void DrawTo(ICanvas2D target, System.Numerics.Matrix3x2 transform)
        {
            for (int y = 0; y < _Height; ++y)
            {
                for (int x = 0; x < _Width; ++x)
                {
                    var tmp = new ImageSource();

                    var offset = new XY(x * 16, y * 16);

                    var idx = _Tiles[y * _Width + x];

                    _Sprites[idx].CopyTo(tmp, -offset);

                    target.DrawImage(transform, tmp);
                }
            }
        }

        #endregion
    }
}
