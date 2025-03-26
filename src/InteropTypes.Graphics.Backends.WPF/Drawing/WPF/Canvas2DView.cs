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
    using DRAWABLE = Drawing.IDrawingBrush<Drawing.ICanvas2D>;
    using CAMERATEMPLATE = Canvas2DViewportTemplate;

    [TemplatePart(Name = "PART_Presenter", Type = typeof(ContentPresenter))]
    [System.ComponentModel.DefaultProperty(nameof(Canvas))]
    [System.Windows.Markup.ContentProperty(nameof(Canvas))]
    public class Canvas2DView : Control, PropertyFactory<Canvas2DView>.IPropertyChanged
    {
        #region lifecycle
        static Canvas2DView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Canvas2DView), new FrameworkPropertyMetadata(typeof(Canvas2DView)));            
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (ViewportTemplate == null) ViewportTemplate = AutoCanvas2DViewport.CreateDefaultTemplate();

            if (this.Viewport == null && ViewportTemplate != null)
            {
                this.Viewport = ViewportTemplate?.GetViewport();
            }

            InitializeViewport(null, this.Viewport);
        }

        #endregion

        #region dependency properties

        private static readonly PropertyFactory<Canvas2DView> _PropFactory;        

        static readonly StaticProperty<CAMERATEMPLATE> ViewportTemplateProperty = _PropFactory.RegisterCallback<CAMERATEMPLATE>(nameof(ViewportTemplate), null);

        static readonly StaticProperty<Primitives.Canvas2DViewport> ViewportProperty = _PropFactory.RegisterCallback<Primitives.Canvas2DViewport>(nameof(Viewport), null);

        static readonly StaticProperty<DRAWABLE> CanvasProperty = _PropFactory.RegisterCallback<DRAWABLE>(nameof(Canvas), null);

        #endregion

        #region Properties

        public DRAWABLE Canvas
        {
            get => CanvasProperty.GetValue(this);
            set => CanvasProperty.SetValue(this, value);
        }

        /// <summary>
        ///     Camera2DPanel is the panel that controls the point of view of the scene.
        ///     (More precisely, the panel that controls the view is created
        ///     from the template given by Camera2DPanel.)
        /// </summary>
        [System.ComponentModel.Bindable(false)]
        public CAMERATEMPLATE ViewportTemplate
        {
            get => ViewportTemplateProperty.GetValue(this);
            set => ViewportTemplateProperty.SetValue(this, value);
        }

        public Primitives.Canvas2DViewport Viewport
        {
            get => ViewportProperty.GetValue(this);
            private set => ViewportProperty.SetValue(this, value);
        }

        #endregion        

        #region callbacks        

        public virtual bool OnPropertyChangedEx(DependencyPropertyChangedEventArgs args)
        {
            // https://github.com/dotnet/wpf/blob/a30c4edea55a95ec9d7c2d29d79b2d4fb12ed973/src/Microsoft.DotNet.Wpf/src/PresentationFramework/System/Windows/Controls/ItemsControl.cs#L190

            if (args.Property == CanvasProperty.Property)
            {
                if (Viewport != null) Viewport.Canvas = this.Canvas;
                return true;
            }

            if (args.Property == ViewportTemplateProperty.Property)
            {
                this.Viewport = ViewportTemplate?.GetViewport();
                return true;
            }

            if (args.Property == ViewportProperty.Property)
            {
                InitializeViewport(args.OldValue as Primitives.Canvas2DViewport, args.NewValue as Primitives.Canvas2DViewport);

                return true;
            }

            return false;
        }

        private void InitializeViewport(Primitives.Canvas2DViewport oldViewport, Primitives.Canvas2DViewport newViewport)
        {
            // notice that this will not be null until after initial bindings.
            var presenter = this.GetTemplateChild("PART_Presenter") as ContentPresenter;

            if (oldViewport != null)
            {
                oldViewport.Canvas = null;
            }
            if (newViewport != null)
            {
                newViewport.Canvas = this.Canvas;
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
