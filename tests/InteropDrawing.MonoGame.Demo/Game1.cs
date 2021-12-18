using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        private _Sprites2D _Sprites = new _Sprites2D();

        private _DynamicTexture _DynTex;


        #endregion

        #region content loading

        protected override void LoadContent()
        {
            _Drawing2D = new Backends.MonoGameDrawing2D(this.GraphicsDevice);

            _DynTex = new _DynamicTexture(this.GraphicsDevice);
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

            base.Draw(gameTime);
            
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;            

            _Drawing2D.Begin(800, 600, true);

            _Drawing2D.DrawLine((0, 0), (800, 600), 2, COLOR.Red);
            _Drawing2D.DrawAsset(System.Numerics.Matrix3x2.Identity, _Sprites);
            _Drawing2D.DrawAsset(System.Numerics.Matrix3x2.Identity, new _Scene2D());
            _Drawing2D.DrawAsset(System.Numerics.Matrix3x2.Identity, _DynTex);
            _Drawing2D.DrawLine((800, 0), (0, 600), 2, COLOR.Red);                     

            _Drawing2D.End();
        }        

        #endregion
    }    
}
