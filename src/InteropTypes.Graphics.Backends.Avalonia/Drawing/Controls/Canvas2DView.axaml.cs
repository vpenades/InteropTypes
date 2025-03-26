using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using System.Windows.Input;
using Avalonia.Data.Converters;
using Avalonia.Styling;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;

namespace InteropTypes.Graphics.Backends.Controls
{
    using DRAWABLE = Drawing.IDrawingBrush<Drawing.ICanvas2D>;
    using CAMERATEMPLATE = Canvas2DViewportTemplate;

    class Canvas2DViewDefaultTheme : ControlTheme { }


    /// <summary>
    /// Represents a 2D Canvas display control
    /// </summary>
    /// <remarks>
    /// The rendering is actually performed by <see cref="Primitives.Canvas2DViewport"/>
    /// </remarks>
    /// <example>
    /// 
    /// <drawing:Canvas2DView Canvas="{Binding Sprites}" >
    ///   <drawing:Canvas2DView.ViewportTemplate>
    ///     <drawing:Canvas2DViewportTemplate>
    ///       <demo:AutoCanvas2DViewport FrameRate="30" />
    ///     </drawing:Canvas2DViewportTemplate>
    ///   </drawing:Canvas2DView.ViewportTemplate>
    /// </drawing:Scene3DView>
    /// 
    /// </example>
    [TemplatePart(Name = "PART_Presenter", Type = typeof(ContentPresenter))]
    [System.ComponentModel.DefaultProperty(nameof(Canvas))]    
    public class Canvas2DView : TemplatedControl
    {
        #region theme

        static Canvas2DView()
        {
            new Canvas2DViewDefaultTheme().TryApplyTo<Canvas2DView>();            
        }

        #endregion

        #region lifecycle

        public Canvas2DView()
        {
            ViewportTemplate = null;
        }

        #endregion        

        #region data

        private CAMERATEMPLATE _ViewportTemplate;
        private Primitives.Canvas2DViewport _Viewport;
        private DRAWABLE _Canvas;

        #endregion

        #region properties

        public static readonly DirectProperty<Canvas2DView, CAMERATEMPLATE> ViewportTemplateProperty
            = AvaloniaProperty.RegisterDirect<Canvas2DView, CAMERATEMPLATE>(nameof(ViewportTemplate), c => c.ViewportTemplate, (c, v) => c.ViewportTemplate = v);

        public static readonly DirectProperty<Canvas2DView, Primitives.Canvas2DViewport> ViewportProperty
            = AvaloniaProperty.RegisterDirect<Canvas2DView, Primitives.Canvas2DViewport>(nameof(Viewport), c => c.Viewport, (c, v) => c.Viewport = v);

        public static readonly DirectProperty<Canvas2DView, DRAWABLE> CanvasProperty
            = AvaloniaProperty.RegisterDirect<Canvas2DView, DRAWABLE>(nameof(Canvas), c => c.Canvas, (c, v) => c.Canvas = v);

        private static readonly CAMERATEMPLATE _ViewportTemplateDefault = new CAMERATEMPLATE(() => new AutoCanvas2DViewport());

        /// <summary>
        /// Represents the template used to initialize the viewport camera
        /// </summary>        
        public CAMERATEMPLATE ViewportTemplate
        {
            get => _ViewportTemplate;
            set
            {
                value ??= _ViewportTemplateDefault;

                if (!SetAndRaise(ViewportTemplateProperty, ref _ViewportTemplate, value)) return;                

                this.Viewport = _ViewportTemplate.Build();
            }
        }

        /// <summary>
        /// The current viewport camera (created by the attached <see cref="ViewportTemplate"/>)
        /// </summary>
        public Primitives.Canvas2DViewport Viewport
        {
            get => _Viewport;
            private set
            {
                var oldViewport = _Viewport;

                if (!SetAndRaise(ViewportProperty, ref _Viewport, value)) return;

                if (oldViewport != null) oldViewport.Canvas = null;
                if (_Viewport != null) _Viewport.Canvas = this.Canvas;

                // notice that this will not be null until after initial bindings.
                /*
                var presenter = this.GetTemplateChild("PART_Presenter") as ContentPresenter;
                if (presenter != null) presenter.Content = _Viewport;
                else if (presenter != null) presenter.Content = null; }
                */
            }
        }

        /// <summary>
        /// The current canvas being drawn
        /// </summary>
        public DRAWABLE Canvas
        {
            get => _Canvas;
            set
            {
                if (!SetAndRaise(CanvasProperty, ref _Canvas, value)) return;

                if (Viewport != null) Viewport.Canvas = _Canvas;
            }
        }

        #endregion        
    }
}
