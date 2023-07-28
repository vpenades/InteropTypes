using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using InteropTypes.Graphics.Drawing;

using XFORM = System.Numerics.Matrix3x2;
using XY = System.Numerics.Vector2;
using COLOR = System.Drawing.Color;


namespace InteropTypes
{
    public class _Sprites2D : IDrawingBrush<ICanvas2D>
    {
        #region data

        private static Object _GetImageReference(string name)
        {
            name ="Assets\\" + name;

            #if ANDROID
            name = name.Replace("\\",".");
            name = name.Replace("/",".");
            return (typeof(_Scene2D).Assembly, name);
            #endif

            return name;
        }

        private static readonly ImageSource[] _Punk = ImageSource.CreateGrid(_GetImageReference("PunkRun.png"), 8, 8, (256, 256), (128, 128)).ToArray();
        private static readonly ImageSource[] _Tiles = ImageSource.CreateGrid(_GetImageReference("Tiles.png"), 63, 9, (16, 16), XY.Zero).ToArray();

        

        private static readonly BitmapGrid _Map1 = new BitmapGrid(4, 4, _Tiles);

        

        private static readonly System.Diagnostics.Stopwatch _Timer = System.Diagnostics.Stopwatch.StartNew();

        private float _Time => (float)_Timer.Elapsed.TotalSeconds;

        private static readonly ImageSource _Hieroglyph = ImageSource.CreateFromBitmap(_GetImageReference("hieroglyph_sprites_by_asalga.png"), (192, 192), (96, 96)).WithScale(3);

        private static readonly ImageSource _TinyCat = ImageSource.CreateFromBitmap(_GetImageReference("Tiny\\cat.png"), (32, 35), (16, 20)).WithScale(3);


        private GroupBox _OpacityCat = new _OpacityCatBox();
        private GroupBox _RgbCats = new _RGBCatBox();
        private GroupBox _Lines = new _VectorialAndBitmapLinesBox();
        private GroupBox _SpriteAlignment = new _SpriteAlignmentBox();


        #endregion

        #region API

        public void DrawTo(ICanvas2D dc)
        {
            var x =
                XFORM.CreateScale(0.5f)
                * XFORM.CreateRotation(_Time)
                   * XFORM.CreateTranslation(400, 300);            

            dc.DrawImage(x, _Hieroglyph);            

            var idx = (int)(_Time * 25);

            dc.DrawImage(XFORM.CreateTranslation(400, 300), _Punk[idx % _Punk.Length]);
            dc.DrawImage(XFORM.CreateTranslation(200, 300), (_Punk[idx % _Punk.Length], COLOR.Red.WithAlpha(128), true, false));

            dc.DrawImage(XFORM.CreateTranslation(50, 300), _Punk[idx % _Punk.Length]);

            dc.DrawImage(XFORM.CreateTranslation(10, 20), _Tiles[1]);
            dc.DrawImage(XFORM.CreateTranslation(10 + 16, 20), _Tiles[2]);

            dc.DrawImage(XFORM.CreateTranslation(10, 250), (_TinyCat, true, false));

            // map rotation
            var gb = CreateGroupBox(dc, 300, 300, 280, 280);
            x =
                XFORM.CreateScale(1.4224f)
                * XFORM.CreateRotation(_Time)
                   * XFORM.CreateTranslation(140, 140);
            _Map1.DrawTo(gb, x);

            float gx = 250;

            // RGB check
            _RgbCats.DrawTo(dc, gx, 15);
            gx += _RgbCats.Width + 5;

            // line drawing Vectorial vs bitmap
            _Lines.DrawTo(dc, gx, 15);
            gx += _Lines.Width + 5;

            // opacity cat
            _OpacityCat.DrawTo(dc, gx, 15);
            gx += _OpacityCat.Width + 5;

            // sprite alignment
            _SpriteAlignment.DrawTo(dc, gx, 15);

            // rect.Bounds.DrawTo(_Drawing2D, (Color.Red, 1));
        }

        private static ICanvas2D CreateGroupBox(ICanvas2D dc, float x, float y, float w, float h)
        {
            var gb = dc.CreateTransformed2D(XFORM.CreateTranslation(x, y));
            gb.DrawRectangle((0, 0), (w, h), (COLOR.Yellow, 3), 6);
            return gb.CreateTransformed2D(XFORM.CreateTranslation(10, 10));
        }

        class _OpacityCatBox : GroupBox
        {
            public _OpacityCatBox() : base(110, 110) { }

            protected override void DrawContentTo(ICanvas2D dc)
            {
                var opacity = (float)Math.Sin((DateTime.Now - DateTime.Today).TotalSeconds);
                dc.DrawImage(XFORM.CreateTranslation(50, 50), new ImageStyle(_TinyCat).WithOpacity(opacity));
            }
        }

        class _RGBCatBox : GroupBox
        {
            public _RGBCatBox() : base(120, 50) { }

            protected override void DrawContentTo(ICanvas2D dc)
            {
                var rgbScale = XFORM.CreateScale(0.25f);

                for(int i=0; i < 3; ++i)
                {
                    var c = COLOR.FromArgb(i == 0 ? 255 : 0, i == 1 ? 255 : 0, i == 2 ? 255 : 0);

                    dc.DrawCircle((10 + i * 40, 15), 30, (COLOR.Black, c, 2));
                    dc.DrawImage(rgbScale * XFORM.CreateTranslation(10 + i * 40, 15), (_TinyCat, c));                    
                }
            }
        }

        class _VectorialAndBitmapLinesBox : GroupBox
        {
            public _VectorialAndBitmapLinesBox() : base(170, 70) { }

            private static readonly ImageSource Beam1 = ImageSource.CreateFromBitmap(_GetImageReference("beam1.png"), (256, 32), (16, 16));

            protected override void DrawContentTo(ICanvas2D dc)
            {
                dc.DrawLine((5, 5), (150, 50), 30, Beam1);
                dc.DrawLine((5, 5), (25, 50), 30, Beam1);
                dc.DrawLine((5, 5), (150, 50), 1, COLOR.Black);
                dc.DrawLine((5, 5), (25, 50), 1, COLOR.Black);
            }
        }

        class _SpriteAlignmentBox : GroupBox
        {
            private static readonly ImageSource _Offset0 = ImageSource.CreateFromBitmap(_GetImageReference("SpriteOffset.png"), (192, 192), (40, 108), false).WithScale(0.45f);
            private static readonly ImageSource _Offset1 = ImageSource.CreateFromBitmap(_GetImageReference("SpriteOffset.png"), (192, 192), (40, 108), true).WithScale(0.45f);

            public _SpriteAlignmentBox() : base(150, 320) { }

            protected override void DrawContentTo(ICanvas2D dc)
            {
                dc.DrawImage(XFORM.CreateTranslation(60, 40), _Offset0);
                dc.DrawImage(XFORM.CreateTranslation(60, 80), (_Offset0, true, false));
                dc.DrawImage(XFORM.CreateTranslation(60, 140), _Offset1);
                dc.DrawImage(XFORM.CreateTranslation(60, 180), (_Offset1, true, false));
                dc.DrawImage(XFORM.CreateTranslation(60, 220), (_Offset1, true, true));
                dc.DrawImage(XFORM.CreateTranslation(60, 260), (_Offset1, false, true));
            }
        }

        #endregion
    }
}
