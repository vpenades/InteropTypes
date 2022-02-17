using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Backends.WPF
{
    public class WPFSceneLayer2D : DependencyObject , ISceneViewport2D
    {
        #region properties        

        private static PropertyFactory<WPFSceneLayer2D> _PropFactory = new PropertyFactory<WPFSceneLayer2D>();

        private static readonly StaticProperty<Record2D> _SceneProperty = _PropFactory.Register<Record2D>(nameof(Scene), null);
        public Record2D Scene
        {
            get => _SceneProperty.GetValue(this);
            set => _SceneProperty.SetValue(this, value);
        }

        private static readonly StaticProperty<float> _WidthProperty = _PropFactory.Register<float>(nameof(Width), 100);
        public float Width
        {
            get => _WidthProperty.GetValue(this);
            set => _WidthProperty.SetValue(this, value);
        }

        private static readonly StaticProperty<float> _HeightProperty = _PropFactory.Register<float>(nameof(Height), 100);
        public float Height
        {
            get => _HeightProperty.GetValue(this);
            set => _HeightProperty.SetValue(this, value);
        }

        #endregion

        #region API

        public (Matrix3x2 Camera, Matrix3x2 Projection) GetMatrices(float renderWidth, float renderHeight)
        {
            var c = Matrix3x2.Identity;
            var p = GetProjectionMatrix(renderWidth, renderHeight);

            return (c, p);
        }

        public Matrix3x2 GetProjectionMatrix(float renderWidth, float renderHeight)
        {
            var www = renderWidth / Width;
            var hhh = renderHeight / Height;

            return CreateOrthographic2D(www, hhh);
        }

        public static Matrix3x2 CreateOrthographic2D(float width, float height)
        {
            Matrix3x2 result;

            result.M11 = width;
            result.M12 = 0;

            result.M22 = height;
            result.M21 = 0;

            result.M31 = 0;
            result.M32 = 0;

            return result;
        }

        #endregion
    }

    class WPFSceneLayer2DPanel : Panel
    {
        private readonly WPFDrawingContext2D _Context2D = new WPFDrawingContext2D();

        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            base.OnRender(dc); // draw background

            if (!(this.DataContext is WPFSceneLayer2D data)) return;

            var model = data.Scene;
            if (model == null || model.IsEmpty) return;
            
            _Context2D.DrawScene(dc, this.RenderSize, data, model);
        }
    }
}
