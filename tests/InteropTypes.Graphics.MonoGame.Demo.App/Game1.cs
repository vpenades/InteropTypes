using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using COLOR = System.Drawing.Color;

using InteropTypes.Graphics.Drawing;
using InteropTypes.Graphics.Backends;
using InteropTypes;

namespace MonoGameDemo
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

        private IMonoGameCanvas2D _Drawing2D;        

        private _Sprites2D _Sprites = new _Sprites2D();

        private _DynamicTexture _DynTex;

        #endregion

        #region content loading

        protected override void LoadContent()
        {
            _Drawing2D = MonoGameToolkit.CreateCanvas2D(this.GraphicsDevice);            

            _DynTex = new _DynamicTexture(this.GraphicsDevice);
        }

        #endregion

        #region loop

        private KeyboardState _LastKey;

        private bool _UseQuadrant1 = false;
        private bool _FlipSprites = false;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            var kcurr = Keyboard.GetState();
            var klast = _LastKey;
            _LastKey = kcurr;


            if (kcurr.IsKeyDown(Keys.D1) && klast.IsKeyUp(Keys.D1)) _UseQuadrant1 = !_UseQuadrant1;
            if (kcurr.IsKeyDown(Keys.D2) && klast.IsKeyUp(Keys.D2)) _FlipSprites = !_FlipSprites;

            // TODO: Add your update logic here

            // var sdlHandle = this.Window.Handle;
            // var win32Handle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;

            // var wnd = System.Windows.Forms.NativeWindow.FromHandle(win32Handle);
            // if (wnd != null) wnd.Cursor = System.Windows.Forms.Cursors.Cross;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            var mouseState = Mouse.GetState(this.Window);


            base.Draw(gameTime);
            
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;            

            _Drawing2D.Begin(800, _UseQuadrant1 ? - 600 : 600, true);                     

            var vp = _Drawing2D.TransformInverse(new Point2(mouseState.Position.X, mouseState.Position.Y));

            _Drawing2D.DrawLine((0, 0), (800, 600), 2, COLOR.Red);
            _Drawing2D.DrawAsset(System.Numerics.Matrix3x2.Identity, _Sprites);
            _Drawing2D.DrawAsset(System.Numerics.Matrix3x2.Identity, new _Scene2D());
            _Drawing2D.DrawAsset(System.Numerics.Matrix3x2.Identity, _DynTex);
            _Drawing2D.DrawLine((800, 0), (0, 600), 2, COLOR.Red);

            if (_Drawing2D.TryGetBackendViewportBounds(out var viewportBounds))
            {
                viewportBounds.Inflate(-10, -10);
                _Drawing2D.DrawRectangle(viewportBounds, (COLOR.Yellow,2) );
            }

            if (_Drawing2D.TryGetQuadrant(out var quadrant))
            {
                _Drawing2D.DrawTextLine((10, 70), $"{quadrant}", 15, COLOR.White);
            }            

            _Drawing2D.DrawTextLine((10, 20), $"{(int)vp.X} {(int)vp.Y}", 15, COLOR.White);

            // Draw with IMeshCanvas2D extended API

            var vertices = new Vertex2[4];
            vertices[0] = new Vertex2((0, 0), (0, 0));
            vertices[1] = new Vertex2((50, 0), (1, 0));
            vertices[2] = new Vertex2((50, 50), (1, 1));
            vertices[3] = new Vertex2((0, 50), (0, 1));

            var svc = _Drawing2D.CreateTransformed2D(System.Numerics.Matrix3x2.CreateTranslation(600, 300)) as IServiceProvider;

            var xformed = svc.GetService(typeof(IMeshCanvas2D)) as IMeshCanvas2D;

            xformed?.DrawMeshPrimitive(vertices, new int[] { 0, 1, 2, 0, 2, 3 },"Assets\\Tiles.png");

            // draw tiles with half pixel

            var tile1 = ImageSource.Create("Assets\\Tiles.png", (16, 64), (16, 16), (5, 5)).WithScale(4);
            var tile2 = ImageSource.Create("Assets\\Tiles.png", (16, 64), (16, 16), (5, 5)).WithScale(4).WithExpandedSource(-3.5f);            

            _Drawing2D.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(600, 500), tile1);
            _Drawing2D.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(600 + 65, 500), tile2);

            var qrhead = ImageSource.Create((typeof(Game1).Assembly, "MonoGameDemo.Assets.qrhead.jpg"),(0,0),(255,255),(128,128));
            _Drawing2D.DrawImage(System.Numerics.Matrix3x2.CreateScale(0.25f)*System.Numerics.Matrix3x2.CreateTranslation(125, 50), qrhead);

            _Drawing2D.DrawCircle((600, 100), 50, (COLOR.Yellow,2));

            _Drawing2D.End();
        }        

        #endregion
    }    
}
