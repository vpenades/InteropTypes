using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InteropTypes.Graphics.Backends.WPF
{
    using DRAWABLE = Drawing.IDrawingBrush<Drawing.IScene3D>;
    using CAMERATEMPLATE = Scene3DViewportTemplate;

    [TemplatePart(Name = "PART_Presenter", Type = typeof(ContentPresenter))]
    [System.ComponentModel.DefaultProperty(nameof(Scene))]
    [System.Windows.Markup.ContentProperty(nameof(Scene))]
    public class Scene3DView : Control, PropertyFactory<Scene3DView>.IPropertyChanged
    {
        #region lifecycle

        static Scene3DView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Scene3DView),new FrameworkPropertyMetadata(typeof(Scene3DView)));
        }        

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.Viewport == null && ViewportTemplate != null)
            {
                this.Viewport = ViewportTemplate?.GetViewport();
            }

            InitializeViewport(null, this.Viewport);
        }

        #endregion

        #region dependency properties

        private static readonly PropertyFactory<Scene3DView> _PropFactory = new PropertyFactory<Scene3DView>();

        static readonly StaticProperty<DRAWABLE> SceneProperty = _PropFactory.RegisterCallback<DRAWABLE>(nameof(Scene), null);

        static readonly StaticProperty<CAMERATEMPLATE> ViewportTemplateProperty = _PropFactory.RegisterCallback(nameof(ViewportTemplate), OrbitCamera3DViewport.CreateDefaultTemplate());

        static readonly StaticProperty<Primitives.Scene3DViewport> ViewportProperty = _PropFactory.RegisterCallback<Primitives.Scene3DViewport>(nameof(Viewport), null);

        #endregion

        #region Properties

        public DRAWABLE Scene
        {
            get => SceneProperty.GetValue(this);
            set => SceneProperty.SetValue(this, value);
        }

        /// <summary>
        ///     Camera3DPanel is the panel that controls the point of view of the scene.
        ///     (More precisely, the panel that controls the view is created
        ///     from the template given by Camera3DPanel.)
        /// </summary>
        [System.ComponentModel.Bindable(false)]
        public CAMERATEMPLATE ViewportTemplate
        {
            get => ViewportTemplateProperty.GetValue(this);
            set => ViewportTemplateProperty.SetValue(this, value);
        }

        public Primitives.Scene3DViewport Viewport
        {
            get => ViewportProperty.GetValue(this);
            private set => ViewportProperty.SetValue(this, value);
        }

        #endregion        

        #region callbacks        

        public virtual bool OnPropertyChangedEx(DependencyPropertyChangedEventArgs args)
        {
            // https://github.com/dotnet/wpf/blob/a30c4edea55a95ec9d7c2d29d79b2d4fb12ed973/src/Microsoft.DotNet.Wpf/src/PresentationFramework/System/Windows/Controls/ItemsControl.cs#L190

            if (args.Property == SceneProperty.Property)
            {
                if (Viewport != null) Viewport.Scene = this.Scene;
                return true;
            }

            if (args.Property == ViewportTemplateProperty.Property)
            {
                this.Viewport = ViewportTemplate?.GetViewport();
                return true;
            }

            if (args.Property == ViewportProperty.Property)
            {
                InitializeViewport(args.OldValue as Primitives.Scene3DViewport, args.NewValue as Primitives.Scene3DViewport);

                return true;
            }

            return false;
        }

        private void InitializeViewport(Primitives.Scene3DViewport oldViewport, Primitives.Scene3DViewport newViewport)
        {
            // notice that this will not be null until after initial bindings.
            var presenter = this.GetTemplateChild("PART_Presenter") as ContentPresenter;

            if (oldViewport != null)
            {
                oldViewport.Scene = null;
            }
            if (newViewport != null)
            {
                newViewport.Scene = this.Scene;
                if (presenter != null) presenter.Content = newViewport;
            }
            else if (presenter != null)
            {
                presenter.Content = null;
            }
        }

        #endregion
    }
}
