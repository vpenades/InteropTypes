using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Data;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Backends.WPF
{
    /// <summary>
    /// Converts a <see cref="Record2D"/> scene to a <see cref="System.Windows.Media.ImageSource"/>
    /// that can be bound to <see cref="System.Windows.Controls.Image.Source"/>
    /// </summary>
    public class WPFSceneConverter : IValueConverter
    {
        #region data

        private WPFSceneRender _Renderer = new WPFSceneRender();

        public Size Viewport { get; set; } = Size.Empty;        

        public SceneView2D View2D { get; set; } = null;
        public SceneView3D View3D { get; set; } = null;

        #endregion

        #region API

        private Size? _GetViewport(object parameter)
        {
            Size? r = null;
            if (!Viewport.IsEmpty) r = Viewport;            
            if (parameter is Size size) r = size;
            return r;
        }

        private SceneView2D _GetView2D(object parameter)
        {
            if (parameter is Matrix3x2 xform) return xform;
            if (parameter is SceneView2D view) return view;
            return this.View2D;
        }

        private SceneView3D _GetView3D(object parameter)
        {
            if (parameter is Matrix4x4 xform) return xform;
            if (parameter is SceneView3D view) return view;
            return this.View3D;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(System.Windows.Media.ImageSource))
            {
                if (value is Record2D scene2D)
                {
                    var r = _GetViewport(parameter);
                    var v = _GetView2D(parameter);
                    _Renderer.Update(r, v, scene2D);
                    return _Renderer.VectorImage;
                }

                if (value is ValueTuple<Matrix3x2, Record2D> scene2Dx)
                {
                    var r = _GetViewport(parameter);
                    _Renderer.Update(r, scene2Dx.Item1, scene2Dx.Item2);
                    return _Renderer.VectorImage;
                }

                if (value is ValueTuple<Size, Matrix3x2, Record2D> scene2Drx)
                {
                    _Renderer.Update(scene2Drx.Item1, scene2Drx.Item2, scene2Drx.Item3);
                    return _Renderer.VectorImage;
                }                

                if (value is Record3D scene3D)
                {
                    var r = _GetViewport(parameter);
                    var v = _GetView3D(parameter);
                    _Renderer.Update(r, v, scene3D);
                    return _Renderer.VectorImage;
                }

                if (value is ValueTuple<Matrix4x4, Record3D> scene3Dx)
                {
                    var r = _GetViewport(parameter);                    
                    _Renderer.Update(r, scene3Dx.Item1, scene3Dx.Item2);
                    return _Renderer.VectorImage;
                }
            }

            return null;
        }        

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
