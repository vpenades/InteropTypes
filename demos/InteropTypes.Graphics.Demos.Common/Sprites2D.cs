using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Drawing;

using COLOR = System.Drawing.Color;
using XFORM = System.Numerics.Matrix3x2;
using XY = System.Numerics.Vector2;

namespace InteropTypes
{
    public class _Sprites2D : GroupBox.Collection
    {
        #region lifecycle
        protected override void Initialize()
        {
            Register(new _OpacityCatBox());
            Register(new _RGBCatBox());
            Register(new _VectorialAndBitmapLinesBox());
            Register(new _SpriteAlignmentBox());
            Register(new _MapRotationBox());
            Render(210, 120, DrawPunks);
            Render(64, 64, DrawDynamicTexture);
            Render(64, 64, DrawEmbedded);
            Render(120, 64, DrawHalfPixel);
            Render(50, 50, DrawMeshPrimitive);

            Render(150,50, _DrawBitmapText);
        }

        public async Task RunDynamicsAsync()
        {
            await _NoiseBitmap.RunAsync().ConfigureAwait(false);
            await _NoiseBitmap2.RunAsync().ConfigureAwait(false);
        }

        public void RunDynamicsThread()
        {
            _NoiseBitmap.RunTask();
            _NoiseBitmap2.RunTask();
        }

        #endregion

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

        private static readonly ImageSource _Hieroglyph = ImageSource.CreateFromBitmap(_GetImageReference("hieroglyph_sprites_by_asalga.png"), (192, 192), (96, 96)).WithScale(3);

        private static readonly ImageSource _TinyCat = ImageSource.CreateFromBitmap(_GetImageReference("Tiny\\cat.png"), (32, 35), (16, 20)).WithScale(3);

        private InteropTypes.BindableNoiseTexture _NoiseBitmap = new BindableNoiseTexture(64,64);
        private InteropTypes.BindableNoiseTexture _NoiseBitmap2 = new BindableNoiseTexture(32, 32);

        public static FontStyle NumbersFont => _NumbersFont._Default.ToStyle();

        private static readonly ImageSource[] Numbers = ImageSource.CreateGrid(_GetImageReference("Numbers.png"), 10, 10, (64, 64), (32, 32))
            .Select(item => item.WithMirror(false, false))
            .ToArray();

        #endregion

        #region API

        public void __DrawTo(ICanvas2D dc)
        {
            /*
            var x =
                XFORM.CreateScale(0.5f)
                * XFORM.CreateRotation(_Time)
                   * XFORM.CreateTranslation(400, 300);            

            dc.DrawImage(x, _Hieroglyph);            

            var idx = (int)(_Time * 25);
            */

            /*
            dc.DrawImage(XFORM.CreateTranslation(10, 20), _Tiles[1]);
            dc.DrawImage(XFORM.CreateTranslation(10 + 16, 20), _Tiles[2]);

            dc.DrawImage(XFORM.CreateTranslation(10, 250), (_TinyCat, true, false));
            */            

            // rect.Bounds.DrawTo(_Drawing2D, (Color.Red, 1));
        }


        void DrawPunks(ICanvas2D dc)
        {
            var idx = (int)(ElapsedTime * 20);

            var s = XFORM.CreateScale(0.5f);

            dc.DrawImage(s*XFORM.CreateTranslation(40, 40), _Punk[idx % _Punk.Length]);
            dc.DrawImage(s*XFORM.CreateTranslation(160, 40), (_Punk[idx % _Punk.Length], COLOR.Red.WithAlpha(128), true, false));
        }        

        void DrawDynamicTexture(ICanvas2D dc)
        {
            if (_NoiseBitmap != null) dc.DrawImage(XFORM.CreateTranslation(0, 0), _NoiseBitmap.Sprite);
            if (_NoiseBitmap2 != null) dc.DrawImage(XFORM.CreateTranslation(0, 0), _NoiseBitmap2.Sprite);
        }

        void DrawEmbedded(ICanvas2D dc)
        {
            var qrhead = ImageSource.Create((typeof(_Sprites2D).Assembly, "InteropTypes.Embedded.qrhead.jpg"), (0, 0), (255, 255), (128, 128));
            dc.DrawImage(XFORM.CreateScale(0.25f) * XFORM.CreateTranslation(32, 32), qrhead);
        }

        void DrawHalfPixel(ICanvas2D dc)
        {
            // draw tiles with half pixel

            var tile1 = ImageSource.Create("Assets\\Tiles.png", (16, 64), (16, 16), (5, 5)).WithScale(4);
            var tile2 = ImageSource.Create("Assets\\Tiles.png", (16, 64), (16, 16), (5, 5)).WithScale(4).WithExpandedSource(-3.5f);

            dc.DrawImage(XFORM.CreateTranslation(16+0, 16), tile1);
            dc.DrawImage(XFORM.CreateTranslation(16 + 65, 16), tile2);
        }

        void DrawMeshPrimitive(ICanvas2D dc)
        {
            // Draw with IMeshCanvas2D extended API (Monogame Only)

            var vertices = new Vertex2[4];
            vertices[0] = new Vertex2((0, 0), (0, 0));
            vertices[1] = new Vertex2((50, 0), (1, 0));
            vertices[2] = new Vertex2((50, 50), (1, 1));
            vertices[3] = new Vertex2((0, 50), (0, 1));

            var svc = dc.CreateTransformed2D(XFORM.CreateTranslation(0, 0)) as IServiceProvider;

            var xformed = svc.GetService(typeof(IMeshCanvas2D)) as IMeshCanvas2D;

            xformed?.DrawMeshPrimitive(vertices, new int[] { 0, 1, 2, 0, 2, 3 }, "Assets\\Tiles.png");
        }

        private void _DrawBitmapText(ICanvas2D dc)
        {
            dc.DrawTextLine((5, 5), "0123456789", 5, NumbersFont);

            dc.DrawTextLine(XFORM.CreateScale(2) * XFORM.CreateTranslation(5, 15), "0123456789", 5, NumbersFont);
        }

        #endregion

        #region nested types

        class _MapRotationBox : GroupBox
        {
            public _MapRotationBox() : base(280, 280) { }

            protected override void DrawContentTo(ICanvas2D dc)
            {
                var x =
                XFORM.CreateScale(1.4224f)
                * XFORM.CreateRotation(ElapsedTime)
                   * XFORM.CreateTranslation(140, 140);

                _Map1.DrawTo(dc, x);
            }
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

                for (int i = 0; i < 3; ++i)
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

        public class _NumbersFont : InteropTypes.Graphics.Drawing.Fonts.IFont
        {
            public static InteropTypes.Graphics.Drawing.Fonts.IFont _Default { get; } = new _NumbersFont();

            public RectangleF MeasureTextLine(string text)
            {
                float x = 0;

                foreach (var c in text)
                {
                    if (!char.IsNumber(c)) continue;
                    var idx = (int)c - (int)'0';

                    x += _Width;
                }

                return new RectangleF(-16, -15, x, Height);
            }

            public void DrawTextLineTo(ICoreCanvas2D target, Matrix3x2 transform, string text, ColorStyle tintColor)
            {
                _PreviewRect(target, transform, text);

                var delta = new XY(_Width, 0);
                delta = XY.TransformNormal(delta, transform);

                foreach (var c in text)
                {
                    if (!char.IsNumber(c)) continue;
                    var idx = (int)c - (int)'0';

                    target.DrawImage(transform, new ImageStyle(Numbers[idx], tintColor));

                    transform.Translation = transform.Translation += delta;
                }
            }

            [Conditional("DEBUG")]
            private void _PreviewRect(ICoreCanvas2D target, Matrix3x2 transform, string text)
            {
                var rect = MeasureTextLine(text);
                var rectPoints = new Point2[5];
                InteropTypes.Graphics.Drawing.Point2.FromRect(rectPoints, rect, true);
                Point2.Transform(rectPoints, transform);
                target.DrawLines(rectPoints, 2, System.Drawing.Color.Red);
            }

            private const float _Width = 36;

            public int Height => 30;
        }

        #endregion
    }
}
