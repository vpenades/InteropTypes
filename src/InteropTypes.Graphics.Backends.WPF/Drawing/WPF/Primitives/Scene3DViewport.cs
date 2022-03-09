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

        #endregion

        #region Dependency properties

        private static readonly PropertyFactory<Scene3DViewport> _PropFactory = new PropertyFactory<Scene3DViewport>();

        static readonly StaticProperty<DRAWABLE> SceneProperty = _PropFactory.RegisterCallback<DRAWABLE>(nameof(Scene), null);

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

        #endregion               

        #region API

        public virtual bool OnPropertyChangedEx(DependencyPropertyChangedEventArgs args)
        {
            if (args.Property == SceneProperty.Property)
            {
                OnSceneChanged(Scene);
                this.InvalidateVisual();
                return true;
            }

            return false;
        }
        
        protected virtual void OnSceneChanged(DRAWABLE scene) { }

        public abstract CAMERAVIEW GetCameraTransform3D();        

        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            base.OnRender(dc); // draw background

            this.ClipToBounds = true;

            var currCam = this.GetCameraTransform3D();
            if (!currCam.IsValid) return;

            var rsize = this.RenderSize;
            if (double.IsNaN(rsize.Width) || double.IsNaN(rsize.Height)) return;

            var content = this.Scene;
            if (content == null) return;

            // we require the scene to be stored in a Record3D
            // so we can use Painter's algorythm to sort the primitives.

            if (!(content is Record3D record))
            {
                if (_SceneRecordCache == null) _SceneRecordCache= new Record3D();
                record = _SceneRecordCache;
                record.Clear();
                content.DrawTo(record);
            }

            if (record.IsEmpty) return;

            _Context2D.SetContext(dc);
            record.DrawTo((_Context2D, (float)rsize.Width, (float)rsize.Height), currCam);
            // _Context2D.DrawScene(rsize, currCam, content);
            _Context2D.SetContext(null);
        }

        #endregion        
    }
}
