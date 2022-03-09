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
    using CAMERATEMPLATE = ControlTemplate;

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

            _SetCameraPanel();
        }

        #endregion

        #region dependency properties

        private static readonly PropertyFactory<Scene3DView> _PropFactory = new PropertyFactory<Scene3DView>();

        static readonly StaticProperty<DRAWABLE> SceneProperty = _PropFactory.RegisterCallback<DRAWABLE>(nameof(Scene), null);

        static readonly StaticProperty<CAMERATEMPLATE> PanelTemplateProperty = _PropFactory.RegisterCallback(nameof(PanelTemplate), OrbitCamera3DPanel.CreateDefaultTemplate());

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
        public CAMERATEMPLATE PanelTemplate
        {
            get => PanelTemplateProperty.GetValue(this);
            set => PanelTemplateProperty.SetValue(this, value);
        }

        #endregion

        #region data

        private Primitives.Scene3DPanel _Panel;

        #endregion

        #region callbacks        

        void PropertyFactory<Scene3DView>.IPropertyChanged.OnPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            // https://github.com/dotnet/wpf/blob/a30c4edea55a95ec9d7c2d29d79b2d4fb12ed973/src/Microsoft.DotNet.Wpf/src/PresentationFramework/System/Windows/Controls/ItemsControl.cs#L190

            if (args.Property == SceneProperty.Property)
            {
                if (_Panel != null) _Panel.Scene = this.Scene;
            }

            if (args.Property == PanelTemplateProperty.Property)
            {
                _SetCameraPanel();
            }
        }
        private void _SetCameraPanel()
        {
            var presenter = this.GetTemplateChild("Presenter") as ContentPresenter;
            if (presenter == null) return;
            var panelTmpl = this.PanelTemplate;
            if (panelTmpl == null) return;
            var panelCtrl = panelTmpl.LoadContent();
            if (panelCtrl == null) return;
            _Panel = panelCtrl as Primitives.Scene3DPanel;
            if (_Panel == null) return;

            presenter.Content = _Panel;

            _Panel.Scene = this.Scene;
        }

        #endregion        
    }
}
