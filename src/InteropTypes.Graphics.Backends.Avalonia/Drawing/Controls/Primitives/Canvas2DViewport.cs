using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

using InteropTypes.Graphics.Drawing;

using DRAWABLE = InteropTypes.Graphics.Drawing.IDrawingBrush<InteropTypes.Graphics.Drawing.ICanvas2D>;


namespace InteropTypes.Graphics.Backends.Controls.Primitives
{
    /// <summary>
    /// Represents a 2D drawing that can be rendered over a 2D panel using the given camera.
    /// </summary>
    /// <remarks>    
    /// <para>This panel is part of <see cref="Canvas2DView"/> architecture.</para>    
    /// <para>
    /// Derived types: <see cref="AutoCanvas2DViewport"/>
    /// </para>
    /// <para>Which are usually declared inside a <see cref="Canvas2DViewportTemplate"/> at <see cref="Canvas2DView.ViewportTemplate"/> in axaml.</para>
    /// </remarks>
    public abstract partial class Canvas2DViewport : Control
    {
        #region data        

        private readonly Canvas2DFactory _Context2D = new Canvas2DFactory();

        private Record2D _CanvasRecordCache;

        private _AnimatedRenderDispatcher _Runner;

        private DRAWABLE _Canvas;
        private float _FrameRate;

        #endregion

        #region Dependency properties        

        public static readonly DirectProperty<Canvas2DViewport, DRAWABLE> CanvasProperty
            = AvaloniaProperty.RegisterDirect<Canvas2DViewport, DRAWABLE>(nameof(Canvas), c => c.Canvas, (c, v) => c.Canvas = v);

        public static readonly DirectProperty<Canvas2DViewport, float> FrameRateProperty
            = AvaloniaProperty.RegisterDirect<Canvas2DViewport, float>(nameof(FrameRate), c => c.FrameRate, (c, v) => c.FrameRate = v);

        public static readonly StyledProperty<bool> EnableCanvasRedrawProperty
            = AvaloniaProperty.Register<Canvas2DViewport, bool>(nameof(EnableCanvasRedraw));

        #endregion

        #region properties

        /// <summary>
        /// Represents a drawable scene
        /// </summary>
        public DRAWABLE Canvas
        {
            get => _Canvas;
            set
            {
                if (!SetAndRaise(CanvasProperty, ref _Canvas, value)) return;

                OnCanvasChanged(Canvas);
                _DrawCanvasToCache();
                this.InvalidateVisual();
            }
        }

        public bool EnableCanvasRedraw
        {
            get => GetValue(EnableCanvasRedrawProperty);
            set => SetValue(EnableCanvasRedrawProperty, value);
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
                    EnableCanvasRedraw = true;
                    _Runner = new _AnimatedRenderDispatcher(this, TimeSpan.FromSeconds(1f / _FrameRate));
                }
            }
        }

        #endregion

        #region API

        protected virtual void OnCanvasChanged(DRAWABLE scene) { }

        public override void Render(DrawingContext dc)
        {
            base.Render(dc); // draw background            

            var renderSize = this.Bounds;

            // do we have an area to render?
            Point2 portSize = (renderSize.Width, renderSize.Height);
            if (!Point2.IsFinite(portSize)) return;

            // prepare the scene
            if (EnableCanvasRedraw) _DrawCanvasToCache();
            if (_CanvasRecordCache == null || _CanvasRecordCache.IsEmpty) return;            

            // this.ClipToBounds = true;

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
