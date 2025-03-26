using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;

namespace InteropTypes.Graphics.Backends.Controls
{
    using DRAWABLE = InteropTypes.Graphics.Drawing.IDrawingBrush<InteropTypes.Graphics.Drawing.IScene3D>;
    using CAMERATEMPLATE = Scene3DViewportTemplate;

    class Scene3DViewDefaultTheme : ControlTheme { }

    /// <summary>
    /// Represents a 3D Scene display control
    /// </summary>
    /// <remarks>
    /// The rendering is actually performed by <see cref="Primitives.Scene3DViewport"/>
    /// </remarks>
    /// <example>
    /// 
    /// <drawing:Scene3DView Scene="{Binding Rockets}" >
    ///   <drawing:Scene3DView.ViewportTemplate>
    ///     <drawing:Scene3DViewportTemplate>
    ///       <demo:AutoCamera3DViewport UpDirectionIsZ="true" FrameRate="30" />
    ///     </drawing:Scene3DViewportTemplate>
    ///   </drawing:Scene3DView.ViewportTemplate>
    /// </drawing:Scene3DView>
    /// 
    /// </example>
    [TemplatePart(Name = "PART_Presenter", Type = typeof(ContentPresenter))]
    [System.ComponentModel.DefaultProperty(nameof(Scene))]
    // [System.Windows.Markup.ContentProperty(nameof(Canvas))]
    public class Scene3DView : TemplatedControl
    {
        #region theme
        static Scene3DView()
        {
            new Scene3DViewDefaultTheme().TryApplyTo<Scene3DView>();
        }

        #endregion

        #region lifecycle

        public Scene3DView()
        {
            ViewportTemplate = null;
        }            

        #endregion

        #region data

        private CAMERATEMPLATE _ViewportTemplate;
        private Primitives.Scene3DViewport _Viewport;
        private DRAWABLE _Scene;

        #endregion

        #region properties        

        public static readonly DirectProperty<Scene3DView, CAMERATEMPLATE> ViewportTemplateProperty
            = AvaloniaProperty.RegisterDirect<Scene3DView, CAMERATEMPLATE>(nameof(ViewportTemplate), c => c.ViewportTemplate, (c, v) => c.ViewportTemplate = v);        

        public static readonly DirectProperty<Scene3DView, Primitives.Scene3DViewport> ViewportProperty
            = AvaloniaProperty.RegisterDirect<Scene3DView, Primitives.Scene3DViewport>(nameof(Viewport), c => c.Viewport, (c, v) => c.Viewport = v);

        public static readonly DirectProperty<Scene3DView, DRAWABLE> SceneProperty
            = AvaloniaProperty.RegisterDirect<Scene3DView, DRAWABLE>(nameof(Scene), c => c.Scene, (c, v) => c.Scene = v);

        private static readonly CAMERATEMPLATE _ViewportTemplateDefault = new CAMERATEMPLATE(() => new OrbitCamera3DViewport());


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
        public Primitives.Scene3DViewport Viewport
        {
            get => _Viewport;
            private set
            {
                var oldViewport = _Viewport;

                if (!SetAndRaise(ViewportProperty, ref _Viewport, value)) return;

                if (oldViewport != null) oldViewport.Scene = null;
                if (_Viewport != null) _Viewport.Scene = this.Scene;

                // notice that this will not be null until after initial bindings.
                /*
                var presenter = this.GetTemplateChild("PART_Presenter") as ContentPresenter;
                if (presenter != null) presenter.Content = _Viewport;
                else if (presenter != null) presenter.Content = null; }
                */
            }
        }

        /// <summary>
        /// The current scene being drawn
        /// </summary>
        public DRAWABLE Scene
        {
            get => _Scene;
            set
            {
                if (!SetAndRaise(SceneProperty, ref _Scene, value)) return;

                if (Viewport != null) Viewport.Scene = _Scene;
            }
        }

        #endregion
    }
}