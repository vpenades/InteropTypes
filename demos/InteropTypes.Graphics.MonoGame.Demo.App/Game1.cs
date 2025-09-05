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

            _Sprites.RunDynamicsThread();
        }        

        #endregion

        #region data

        private readonly GraphicsDeviceManager _Graphics;
        private SpriteFont _Arial64;

        private SpriteBatch _SpriteBatch;
        private IMonoGameCanvas2D _Drawing2D;
        private IMonoGameScene3D _Drawing3D;

        private _Sprites2D _Sprites = new _Sprites2D();
        private _Scene2D _Vectors = new _Scene2D();        

        #endregion

        #region content loading

        protected override void LoadContent()
        {
            _SpriteBatch = new SpriteBatch(this._Graphics.GraphicsDevice);
            _Drawing2D = MonoGameToolkit.CreateCanvas2D(this.GraphicsDevice);
            _Drawing3D = MonoGameToolkit.CreateScene3D(this.GraphicsDevice);

            _Arial64 = this.Content.Load<SpriteFont>("Arial64");
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

            base.Draw(gameTime);
            
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;            

            _Drawing2D.Begin(800, _UseQuadrant1 ? - 600 : 600, true);
            System.Diagnostics.Debug.Assert(_Graphics.GraphicsDevice.BlendState == BlendState.AlphaBlend);
            _DrawCanvas2D();            
            _Drawing2D.End();

            System.Diagnostics.Debug.Assert(_Graphics.GraphicsDevice.BlendState == BlendState.Opaque);

            _Drawing3D.Clear();
            _DrawScene3D();
            _Drawing3D.Render();            

            _SpriteBatch.Begin();
            _SpriteBatch.DrawString(_Arial64, "Native Text", new System.Numerics.Vector2(5, 5), Microsoft.Xna.Framework.Color.Yellow);
            _SpriteBatch.End();
        }        

        private void _DrawCanvas2D()
        {
            var mouseState = Mouse.GetState(this.Window);

            var vp = _Drawing2D.TransformInverse(new Point2(mouseState.Position.X, mouseState.Position.Y));

            GroupBox.ArrangeAtlas(800, _Sprites, _Vectors);

            _Drawing2D.DrawLine((0, 0), (800, 600), 2, COLOR.Red);
            _Drawing2D.DrawAsset(System.Numerics.Matrix3x2.Identity, _Sprites);
            _Drawing2D.DrawAsset(System.Numerics.Matrix3x2.Identity, _Vectors);
            _Drawing2D.DrawLine((800, 0), (0, 600), 2, COLOR.Red);

            if (_Drawing2D.TryGetBackendViewportBounds(out var viewportBounds))
            {
                viewportBounds.Inflate(-10, -10);
                _Drawing2D.DrawRectangle(viewportBounds, (COLOR.Yellow, 2));
            }

            if (_Drawing2D.TryGetQuadrant(out var quadrant))
            {
                _Drawing2D.DrawTextLine((10, 70), $"{quadrant}", 15, COLOR.White);
            }

            _Drawing2D.DrawTextLine((10, 20), $"{(int)vp.X} {(int)vp.Y}", 15, COLOR.White);            
        }

        private void _DrawScene3D()
        {
            var camera = CameraTransform3D.CreatePerspective(1.1f);
            camera.SetOrbitWorldMatrix((0, 0, 0), 30, 0, 0, 0);
            _Drawing3D.SetCamera(camera);

            var yaw = 10f * (float)(DateTime.Now - DateTime.Today).TotalMinutes;

            ThunderbirdRocket.DrawTo(System.Numerics.Matrix4x4.CreateRotationY(yaw), _Drawing3D);
        }

        #endregion
    }


    
}
