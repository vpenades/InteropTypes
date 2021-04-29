using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows;

namespace InteropDrawing.Backends
{
    public class WPFCamera2D : DependencyObject , ISceneViewport2D
    {
        #region dependency properties

        private static PropertyFactory<WPFCamera2D> _PropFactory = new PropertyFactory<WPFCamera2D>();

        #endregion

        #region properties        

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

            return CreateOrthographic2D(www,hhh);
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
    }
}
