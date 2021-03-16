using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using XY = System.Numerics.Vector2;
using COLOR = System.Drawing.Color;

namespace InteropDrawing
{
    public class Game1 : Game
    {
        #region lifecycle

        public Game1()
        {
            _Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            this.Window.Title = "MonoGame + InteropDrawing Demo";
            this.Window.AllowUserResizing = true;
            this.Window.AllowAltF4 = true;

            base.Initialize();
        }

        #endregion

        #region data

        private readonly GraphicsDeviceManager _Graphics;

        private Backends.MonoGameDrawing2D _Drawing2D;
        // private Backends.MonoGameDrawing3D _Drawing3D;


        // these are the actual hardware resources that represent every model's geometry.
                
        private static readonly SpriteAsset[] _Punk = SpriteAsset.CreateGrid("Assets\\PunkRun.png", (256, 256), (128, 128), 8, 8).ToArray();
        private static readonly SpriteAsset[] _Tiles = SpriteAsset.CreateGrid("Assets\\Tiles.png", (16, 16), XY.Zero, 63, 9).ToArray();

        private static readonly BitmapGrid _Map1 = new BitmapGrid(4, 4, _Tiles);

        private static readonly SpriteAsset Beam1 = SpriteAsset.CreateFromBitmap("Assets\\beam1.png", (256, 32), (16, 16));

        #endregion

        #region content loading

        protected override void LoadContent()
        {
            _Drawing2D = new Backends.MonoGameDrawing2D(this.GraphicsDevice);            
        }

        #endregion

        #region loop

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
            
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;

            var t = (float)gameTime.TotalGameTime.TotalSeconds;

            _Drawing2D.Begin(800, -600, true);

            _Drawing2D.DrawLine((0, 0), (800, 600), 2, COLOR.Red);

            _DrawSprites2D(t);
            _DrawScene2D(t);

            _Drawing2D.DrawLine((800, 0), (0, 600), 2, COLOR.Red);

            _Drawing2D.End();
        }

        private void _DrawScene2D(float t)
        {
            _Drawing2D.DrawCircle((0, 0), 50, COLOR.Red);
            _Drawing2D.DrawCircle((200, 200), 50, COLOR.White);
            _Drawing2D.DrawRectangle((175, 175), (50, 50), (COLOR.Transparent, COLOR.Red, 4));

            _Drawing2D.DrawRectangle((480, 200), (130, 130), (COLOR.Transparent, COLOR.Red, 4), 12);

            _Drawing2D.DrawRectangle((10, 10), (200, 200), (COLOR.Yellow, 2));

            // DrawFlower(_Drawing2D, new XY(450, 450), 4);

            _Drawing2D.DrawFont((100, 100), 0.75f, "Hello World!", (COLOR.White, 2));

            // var bee = _CreateBeeModel2D(COLOR.Yellow);
            // _Drawing2D.DrawAsset(System.Numerics.Matrix3x2.CreateRotation(t) * System.Numerics.Matrix3x2.CreateTranslation(600, 350), bee, Color.White);
        }

        private void _DrawSprites2D(float t)
        {
            var kk =
                System.Numerics.Matrix3x2.CreateScale(0.5f)
                * System.Numerics.Matrix3x2.CreateRotation(t)
                   * System.Numerics.Matrix3x2.CreateTranslation(400, 300);

            var image = SpriteAsset.CreateFromBitmap("Assets\\hieroglyph_sprites_by_asalga.png", (192, 192), (96, 96)).WithScale(3);

            _Drawing2D.DrawSprite(kk, image);

            // rect.DrawSprite(kk, image);

            var idx = (int)(t * 25);

            _Drawing2D.DrawSprite(System.Numerics.Matrix3x2.CreateTranslation(400, 300), _Punk[idx % _Punk.Length]);
            _Drawing2D.DrawSprite(System.Numerics.Matrix3x2.CreateTranslation(200, 300), (_Punk[idx % _Punk.Length], COLOR.Red.WithAlpha(128), true, false));

            _Drawing2D.DrawSprite(System.Numerics.Matrix3x2.CreateTranslation(50, 300), _Punk[idx % _Punk.Length]);

            _Drawing2D.DrawSprite(System.Numerics.Matrix3x2.CreateTranslation(10, 20), _Tiles[1]);
            _Drawing2D.DrawSprite(System.Numerics.Matrix3x2.CreateTranslation(10 + 16, 20), _Tiles[2]);

            kk =
                System.Numerics.Matrix3x2.CreateScale(1.4224f)
                * System.Numerics.Matrix3x2.CreateRotation(t)
                   * System.Numerics.Matrix3x2.CreateTranslation(400, 500);

            _Map1.DrawTo(_Drawing2D, kk);


            _Drawing2D.DrawLine((20, 100), (300, 150), 30, Beam1);
            _Drawing2D.DrawLine((20, 100), (25, 150), 30, Beam1);

            _Drawing2D.DrawLine((20, 100), (300, 150), 1, COLOR.Black);
            _Drawing2D.DrawLine((20, 100), (25, 150), 1, COLOR.Black);

            // rect.Bounds.DrawTo(_Drawing2D, (Color.Red, 1));
        }

        #endregion
    }


    class BitmapGrid
    {
        #region lifecycle

        public BitmapGrid(int width, int height, SpriteAsset[] templates)
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

        private readonly SpriteAsset[] _Sprites;

        private readonly int _Width;
        private readonly int _Height;

        private readonly int[] _Tiles;

        #endregion

        #region API

        public void DrawTo(IDrawing2D target, System.Numerics.Matrix3x2 transform)
        {
            var tmp = new SpriteAsset();

            for (int y = 0; y < _Height; ++y)
            {
                for (int x = 0; x < _Width; ++x)
                {
                    var offset = new XY(x * 16, y * 16);

                    var idx = _Tiles[y * _Width + x];

                    _Sprites[idx].CopyTo(tmp, -offset);

                    target.DrawSprite(transform, tmp);
                }
            }
        }

        #endregion
    }
}
