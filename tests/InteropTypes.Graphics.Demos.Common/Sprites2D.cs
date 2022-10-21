using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using InteropTypes.Graphics.Drawing;

using XY = System.Numerics.Vector2;
using COLOR = System.Drawing.Color;

namespace InteropTypes
{
    

    public class _Sprites2D : IDrawingBrush<ICanvas2D>
    {
        private static readonly ImageSource[] _Punk = ImageSource.CreateGrid("Assets\\PunkRun.png", 8, 8, (256, 256), (128, 128)).ToArray();
        private static readonly ImageSource[] _Tiles = ImageSource.CreateGrid("Assets\\Tiles.png", 63, 9, (16, 16), XY.Zero).ToArray();

        private static readonly ImageSource _Offset0 = ImageSource.CreateFromBitmap("Assets\\SpriteOffset.png", (192, 192), (40, 108), false).WithScale(0.45f);
        private static readonly ImageSource _Offset1 = ImageSource.CreateFromBitmap("Assets\\SpriteOffset.png", (192, 192), (40, 108), true).WithScale(0.45f);

        private static readonly BitmapGrid _Map1 = new BitmapGrid(4, 4, _Tiles);

        private static readonly ImageSource Beam1 = ImageSource.CreateFromBitmap("Assets\\beam1.png", (256, 32), (16, 16));

        private static readonly System.Diagnostics.Stopwatch _Timer = System.Diagnostics.Stopwatch.StartNew();

        private float _Time => (float)_Timer.Elapsed.TotalSeconds;

        private static readonly ImageSource _Asalga = ImageSource.CreateFromBitmap("Assets\\hieroglyph_sprites_by_asalga.png", (192, 192), (96, 96)).WithScale(3);

        public void DrawTo(ICanvas2D dc)
        {
            var x =
                System.Numerics.Matrix3x2.CreateScale(0.5f)
                * System.Numerics.Matrix3x2.CreateRotation(_Time)
                   * System.Numerics.Matrix3x2.CreateTranslation(400, 300);            

            dc.DrawImage(x, _Asalga);            

            var idx = (int)(_Time * 25);

            dc.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(400, 300), _Punk[idx % _Punk.Length]);
            dc.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(200, 300), (_Punk[idx % _Punk.Length], COLOR.Red.WithAlpha(128), true, false));

            dc.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(50, 300), _Punk[idx % _Punk.Length]);

            dc.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(10, 20), _Tiles[1]);
            dc.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(10 + 16, 20), _Tiles[2]);

            x =
                System.Numerics.Matrix3x2.CreateScale(1.4224f)
                * System.Numerics.Matrix3x2.CreateRotation(_Time)
                   * System.Numerics.Matrix3x2.CreateTranslation(400, 500);

            _Map1.DrawTo(dc, x);


            dc.DrawLine((20, 100), (300, 150), 30, Beam1);
            dc.DrawLine((20, 100), (25, 150), 30, Beam1);

            dc.DrawLine((20, 100), (300, 150), 1, COLOR.Black);
            dc.DrawLine((20, 100), (25, 150), 1, COLOR.Black);


            dc.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(700, 40), _Offset0);

            dc.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(700, 80), (_Offset0, true, false));

            dc.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(700, 140), _Offset1);

            dc.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(700, 180), (_Offset1, true, false));

            dc.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(700, 220), (_Offset1, true, true));

            dc.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(700, 260), (_Offset1, false, true));

            // rect.Bounds.DrawTo(_Drawing2D, (Color.Red, 1));
        }
    }
}
