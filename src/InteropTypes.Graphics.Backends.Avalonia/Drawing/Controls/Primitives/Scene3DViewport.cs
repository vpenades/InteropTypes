using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;
using Avalonia.Metadata;

using InteropTypes.Graphics.Drawing;

using CAMERAVIEW = InteropTypes.Graphics.Drawing.CameraTransform3D;
using DRAWABLE = InteropTypes.Graphics.Drawing.IDrawingBrush<InteropTypes.Graphics.Drawing.IScene3D>;

namespace InteropTypes.Graphics.Backends.Controls.Primitives
{


    /// <summary>
    /// Represents a 3D scene that can be rendered over a 2D panel using the given camera.
    /// </summary>
    /// <remarks>    
    /// <para>This panel is part of <see cref="Scene3DView"/> architecture.</para>     
    /// <para>Derived classes: <see cref="OrbitCamera3DViewport"/>, <see cref="AutoCamera3DViewport"/></para>
    /// <para>Which are usually declared inside a <see cref="Scene3DViewportTemplate"/> at <see cref="Scene3DView.ViewportTemplate"/> in axaml.</para>    
    /// </remarks>
    public abstract partial class Scene3DViewport : Control, CAMERAVIEW.ISource
    {
        #region lifecycle

        static Scene3DViewport()
        {
            AffectsRender<Scene3DViewport>(UpDirectionIsZProperty);
        }

        #endregion

        #region data        

        private readonly Canvas2DFactory _Context2D = new Canvas2DFactory();

        private Record3D _SceneRecordCache;

        private _AnimatedRenderDispatcher _Runner;

        private DRAWABLE _Scene;
        private float _FrameRate;

        #endregion

        #region Dependency properties        

        public static readonly DirectProperty<Scene3DViewport, DRAWABLE> SceneProperty
            = AvaloniaProperty.RegisterDirect<Scene3DViewport, DRAWABLE>(nameof(Scene), c => c.Scene, (c, v) => c.Scene = v);

        public static readonly DirectProperty<Scene3DViewport, float> FrameRateProperty
            = AvaloniaProperty.RegisterDirect<Scene3DViewport, float>(nameof(FrameRate), c => c.FrameRate, (c, v) => c.FrameRate = v);

        public static readonly StyledProperty<bool> EnableSceneRedrawProperty
            = AvaloniaProperty.Register<Scene3DViewport, bool>(nameof(EnableSceneRedraw));

        public static readonly StyledProperty<bool> UpDirectionIsZProperty
            = AvaloniaProperty.Register<Scene3DViewport, bool>(nameof(UpDirectionIsZ));

        #endregion

        #region properties

        /// <summary>
        /// Represents a drawable scene
        /// </summary>
        public DRAWABLE Scene
        {
            get => _Scene;
            set
            {
                if (!SetAndRaise(SceneProperty, ref _Scene, value)) return;

                OnSceneChanged(_Scene);
                _DrawSceneToCache();
                this.InvalidateVisual();
            }
        }        

        public float FrameRate
        {
            get => _FrameRate;
            set
            {
                if (!SetAndRaise(FrameRateProperty, ref _FrameRate, value)) return;

                _Runner.Release();                

                if (_FrameRate > 0)
                {
                    EnableSceneRedraw = true;
                    _Runner = new _AnimatedRenderDispatcher(this, TimeSpan.FromSeconds(1f / _FrameRate));
                }
            }
        }

        /// <summary>
        /// Forces the source scene to be re-evaluated before each render call
        /// </summary>
        public bool EnableSceneRedraw
        {
            get => GetValue(EnableSceneRedrawProperty);
            set => SetValue(EnableSceneRedrawProperty, value);
        }

        public bool UpDirectionIsZ
        {
            get => GetValue(UpDirectionIsZProperty);
            set => SetValue(UpDirectionIsZProperty, value);
        }

        #endregion

        #region API        

        protected virtual void OnSceneChanged(DRAWABLE scene) { }

        public abstract CAMERAVIEW GetCameraTransform3D();

        public override void Render(DrawingContext dc)
        {
            base.Render(dc); // draw background            

            var renderSize = this.Bounds;

            // do we have an area to render?
            Point2 portSize = (renderSize.Width, renderSize.Height);
            if (!Point2.IsFinite(portSize)) return;

            // prepare the scene
            if (EnableSceneRedraw) _DrawSceneToCache();
            if (_SceneRecordCache == null || _SceneRecordCache.IsEmpty) return;

            // acquire the camera AFTER the scene has been prepared (so we can get the latest scene bounds, required by orbit camera)
            var currCam = this.GetCameraTransform3D();
            if (!currCam.IsValid) return; // TODO: check is debug mode and render: "camera is invalid"                 

            // this.ClipToBounds = true;

            _Context2D.SetContext(dc);
            using (var canvas2D = _Context2D.UsingCanvas2D(portSize.X, portSize.Y))
            {
                _SceneRecordCache.DrawTo((canvas2D, portSize.X, portSize.Y), currCam);
            }
            _Context2D.SetContext(null);
        }

        private bool _DrawSceneToCache()
        {
            // we require the scene to be stored in a Record3D
            // so we can use Painter's algorythm to sort the primitives.

            if (_SceneRecordCache == null) _SceneRecordCache = new Record3D();

            _SceneRecordCache.Clear();

            var currScene = this.Scene;
            if (currScene == null) return false;

            currScene.DrawTo(_SceneRecordCache);
            OnPrepareScene(_SceneRecordCache); // let derived classes modify the scene to be rendered.

            return !_SceneRecordCache.IsEmpty;
        }

        /// <summary>
        /// Allows derived classes to modify the scene before rendering.
        /// </summary>
        /// <param name="scene">the scene to be modified.</param>
        protected virtual void OnPrepareScene(Record3D scene) { }

        #endregion        
    }
}
