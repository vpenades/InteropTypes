using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using InteropTypes.Graphics.Drawing;

using CAMERAVIEW = InteropTypes.Graphics.Drawing.CameraTransform3D;
using DRAWABLE = InteropTypes.Graphics.Drawing.IDrawingBrush<InteropTypes.Graphics.Drawing.IScene3D>;


using PROPERTYFLAGS = System.Windows.FrameworkPropertyMetadataOptions;
namespace InteropTypes.Graphics.Backends.WPF.Primitives
{
    /// <summary>
    /// Represents a 3D scene that can be rendered over a 2D panel using the given camera.
    /// </summary>
    /// <remarks>
    /// <para>The camera is the "bridge" between the 3D scene and the screen's viewport.</para>
    /// <para>Derived classes: <see cref="OrbitCamera3DViewport"/>, <see cref="AutoCamera3DViewport"/></para>
    /// <para>This panel is part of <see cref="Scene3DView"/> architecture.</para>
    /// </remarks>
    public abstract partial class Scene3DViewport :
        FrameworkElement,
        CAMERAVIEW.ISource,
        PropertyFactory<Scene3DViewport>.IPropertyChanged
    {
        #region data        

        private readonly DrawingContext2D _Context2D = new DrawingContext2D();

        private Record3D _SceneRecordCache;

        private _AnimatedRenderDispatcher _Runner;

        #endregion

        #region Dependency properties

        private static readonly PropertyFactory<Scene3DViewport> _PropFactory = new PropertyFactory<Scene3DViewport>();

        static readonly StaticProperty<DRAWABLE> SceneProperty = _PropFactory.RegisterCallback<DRAWABLE>(nameof(Scene), null);

        static readonly StaticProperty<bool> EnableSceneRedrawProperty = _PropFactory.RegisterCallback(nameof(EnableSceneRedraw), false);

        static readonly StaticProperty<float> FrameRateProperty = _PropFactory.RegisterCallback<float>(nameof(FrameRate), 0);

        static readonly StaticProperty<bool> UpDirectionIsZProperty = _PropFactory.Register<bool>(nameof(UpDirectionIsZ), false, PROPERTYFLAGS.AffectsRender);

        #endregion

        #region properties

        /// <summary>
        /// Represents a drawable scene
        /// </summary>
        public DRAWABLE Scene
        {
            get => SceneProperty.GetValue(this);
            set => SceneProperty.SetValue(this, value);
        }

        public bool EnableSceneRedraw
        {
            get => EnableSceneRedrawProperty.GetValue(this);
            set => EnableSceneRedrawProperty.SetValue(this, value);
        }

        public float FrameRate
        {
            get => FrameRateProperty.GetValue(this);
            set => FrameRateProperty.SetValue(this, value);
        }

        public bool UpDirectionIsZ
        {
            get => UpDirectionIsZProperty.GetValue(this);
            set => UpDirectionIsZProperty.SetValue(this, value);
        }

        #endregion

        #region API        

        public virtual bool OnPropertyChangedEx(DependencyPropertyChangedEventArgs args)
        {
            if (args.Property == SceneProperty.Property)
            {
                OnSceneChanged(Scene);

                _DrawSceneToCache();

                this.InvalidateVisual();
                return true;
            }

            if (args.Property == FrameRateProperty.Property)
            {
                _Runner.Release();

                var newVal = (float)args.NewValue;

                if ( newVal > 0)
                {
                    EnableSceneRedraw = true;
                    _Runner = new _AnimatedRenderDispatcher(this, TimeSpan.FromSeconds(1f / newVal));
                }
            }

            return false;
        }
        
        protected virtual void OnSceneChanged(DRAWABLE scene) { }

        public abstract CAMERAVIEW GetCameraTransform3D();        

        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            base.OnRender(dc); // draw background

            // do we have an area to render?
            var portSize = this.RenderSize;
            if (double.IsNaN(portSize.Width) || double.IsNaN(portSize.Height)) return;                        

            // prepare the scene
            if (EnableSceneRedraw) _DrawSceneToCache();
            if (_SceneRecordCache == null || _SceneRecordCache.IsEmpty) return;

            // acquire the camera AFTER the scene has been prepared (so we can get the latest scene bounds, required by orbit camera)
            var currCam = this.GetCameraTransform3D();
            if (!currCam.IsValid) return; // TODO: check is debug mode and render: "camera is invalid"                 

            this.ClipToBounds = true;

            _Context2D.SetContext(dc);
            _SceneRecordCache.DrawTo((_Context2D, (float)portSize.Width, (float)portSize.Height), currCam);            
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
