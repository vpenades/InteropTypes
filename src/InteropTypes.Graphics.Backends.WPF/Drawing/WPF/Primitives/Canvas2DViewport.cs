using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using InteropTypes.Graphics.Drawing;

using DRAWABLE = InteropTypes.Graphics.Drawing.IDrawingBrush<InteropTypes.Graphics.Drawing.ICanvas2D>;
using PROPERTYFLAGS = System.Windows.FrameworkPropertyMetadataOptions;

namespace InteropTypes.Graphics.Backends.WPF.Primitives
{
    /// <summary>
    /// Represents a 2D drawing that can be rendered over a 2D panel using the given camera.
    /// </summary>
    /// <remarks>    
    /// <para>This panel is part of <see cref="Canvas2DView"/> architecture.</para>
    /// </remarks>
    public abstract partial class Canvas2DViewport :
        FrameworkElement,        
        PropertyFactory<Canvas2DViewport>.IPropertyChanged
    {
        #region data        

        private readonly Canvas2DFactory _Context2D = new Canvas2DFactory();

        private Record2D _CanvasRecordCache;

        private _AnimatedRenderDispatcher _Runner;

        #endregion

        #region Dependency properties

        private static readonly PropertyFactory<Canvas2DViewport> _PropFactory;

        static readonly StaticProperty<DRAWABLE> CanvasProperty = _PropFactory.RegisterCallback<DRAWABLE>(nameof(Canvas), null);

        static readonly StaticProperty<bool> EnableCanvasRedrawProperty = _PropFactory.RegisterCallback(nameof(EnableCanvasRedraw), false);

        static readonly StaticProperty<float> FrameRateProperty = _PropFactory.RegisterCallback<float>(nameof(FrameRate), 0);        

        #endregion

        #region properties

        /// <summary>
        /// Represents a drawable scene
        /// </summary>
        public DRAWABLE Canvas
        {
            get => CanvasProperty.GetValue(this);
            set => CanvasProperty.SetValue(this, value);
        }

        public bool EnableCanvasRedraw
        {
            get => EnableCanvasRedrawProperty.GetValue(this);
            set => EnableCanvasRedrawProperty.SetValue(this, value);
        }

        public float FrameRate
        {
            get => FrameRateProperty.GetValue(this);
            set => FrameRateProperty.SetValue(this, value);
        }

        #endregion

        #region API        

        public virtual bool OnPropertyChangedEx(DependencyPropertyChangedEventArgs args)
        {
            if (args.Property == CanvasProperty.Property)
            {
                OnCanvasChanged(Canvas);

                _DrawCanvasToCache();

                this.InvalidateVisual();
                return true;
            }

            if (args.Property == FrameRateProperty.Property)
            {
                _Runner.Release();

                var newVal = (float)args.NewValue;

                if (newVal > 0)
                {
                    EnableCanvasRedraw = true;
                    _Runner = new _AnimatedRenderDispatcher(this, TimeSpan.FromSeconds(1f / newVal));
                }
            }

            return false;
        }

        protected virtual void OnCanvasChanged(DRAWABLE scene) { }        

        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            base.OnRender(dc); // draw background

            // do we have an area to render?
            Point2 portSize = (this.RenderSize.Width, this.RenderSize.Height);
            if (!Point2.IsFinite(portSize)) return;

            // prepare the scene
            if (EnableCanvasRedraw) _DrawCanvasToCache();
            if (_CanvasRecordCache == null || _CanvasRecordCache.IsEmpty) return;            

            this.ClipToBounds = true;

            _Context2D.SetContext(dc);
            using (var canvas2D = _Context2D.UsingCanvas2D(portSize.X, portSize.Y))
            {
                _CanvasRecordCache.DrawTo(canvas2D);
            }
            _Context2D.SetContext(null);
        }

        private bool _DrawCanvasToCache()
        {
            // we require the scene to be stored in a Record2D
            // so we can use Painter's algorythm to sort the primitives.

            if (_CanvasRecordCache == null) _CanvasRecordCache = new Record2D();

            _CanvasRecordCache.Clear();

            var currCanvas = this.Canvas;
            if (currCanvas == null) return false;

            currCanvas.DrawTo(_CanvasRecordCache);
            OnPrepareCanvas(_CanvasRecordCache); // let derived classes modify the scene to be rendered.

            return !_CanvasRecordCache.IsEmpty;
        }

        /// <summary>
        /// Allows derived classes to modify the scene before rendering.
        /// </summary>
        /// <param name="scene">the scene to be modified.</param>
        protected virtual void OnPrepareCanvas(Record2D scene) { }

        #endregion        
    }
}
