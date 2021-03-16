using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

using XY = System.Numerics.Vector2;
using COLOR = System.Drawing.Color;
using RECT = System.Drawing.RectangleF;

namespace InteropDrawing
{
    [System.Diagnostics.DebuggerTypeProxy(typeof(_Model2DProxy))]
    public class Model2D : IDrawing2D, IPseudoImmutable
    {
        #region data

        internal readonly _CommandStream2D _Commands = new _CommandStream2D();        

        private Model2DVersionKey _ImmutableKey;

        #endregion

        #region properties        

        /// <summary>
        /// This value does not change as long as the content of <see cref="Model3D"/> doesn't change either.
        /// </summary>
        /// <remarks>
        /// <see cref="ImmutableKey"/> can be used as a Dictionary Key so a client backend can create an optimized
        /// renderable resource to be used in place of this <see cref="Model3D"/>
        /// </remarks>
        public Object ImmutableKey
        {
            get
            {
                if (_ImmutableKey == null) _ImmutableKey = new Model2DVersionKey(this);
                return _ImmutableKey;
            }
        }

        public RECT BoundingRect => BoundingBox.MinMaxToRectF();

        public (XY Min, XY Max) BoundingBox
        {
            get
            {
                var key = (Model2DVersionKey)ImmutableKey;
                return key.BoundingBox;
            }
        }

        
        public (XY Center, Single Radius) BoundingCircle
        {
            get
            {
                var key = (Model2DVersionKey)ImmutableKey;
                return key.BoundingCircle;
            }
        }

        public bool IsEmpty => _Commands.Count == 0;

        #endregion

        #region API

        public void Clear()
        {
            _ImmutableKey = null;

            _Commands.Clear();
        }

        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle brush)
        {
            _ImmutableKey = null;
            _Commands.DrawAsset(transform, asset, brush);
        }

        public void DrawLines(ReadOnlySpan<Point2> points, Single width, LineStyle brush)
        {
            _ImmutableKey = null;
            _Commands.DrawLines(points, width, brush);
        }

        public void DrawEllipse(Point2 center, float w, float h, ColorStyle brush)
        {
            _ImmutableKey = null;
            _Commands.DrawEllipse(center, w, h, brush);            
        }

        public void DrawPolygon(ReadOnlySpan<Point2> points, ColorStyle brush)
        {
            _ImmutableKey = null;
            _Commands.DrawPolygon(points, brush);            
        }

        public void DrawSprite(in Matrix3x2 transform, in SpriteStyle style)
        {
            _ImmutableKey = null;
            _Commands.DrawSprite(transform, style);
        }

        public void DrawTo(IDrawing2D dc, bool collapse = false)
        {
            foreach(var offset in _Commands.GetCommands())
            {
                _Commands.DrawTo(offset, dc, collapse);
            }
        }

        public IEnumerable<string> ToLog() { return new _Model2DProxy(this).Primitives; }


        public void DrawTo(IDrawing3D dc, Matrix4x4 xform) { this.DrawTo(dc.CreateTransformed2D(xform)); }

        public void DrawTo((IDrawing2D target, float width, float height) renderTarget, Matrix3x2 projection, Matrix3x2 camera)
        {
            var context = Transforms.Drawing2DTransform.Create(renderTarget, projection, camera);

            this.DrawTo(context);
        }

        public void CopyTo(Model2D other)
        {
            other._Commands.Set(this._Commands);
            other._ImmutableKey = null;
        }        

        #endregion
    }

    sealed class _Model2DProxy : _CommandStream2D_DebuggerProxy
    {
        public _Model2DProxy(Model2D src) : base(src._Commands) { ImmutableKey = src.ImmutableKey; }

        public object ImmutableKey {get;}
    }

    /// <summary>
    /// Stores computationally expensive resources of a given version of a <see cref="Model3D"/>
    /// and also serves as <see cref="Model3D.ImmutableKey"/> underlaying object.
    /// </summary>
    sealed class Model2DVersionKey
    {
        #region constructor

        public Model2DVersionKey(Model2D model)
        {
            _Source = new WeakReference<Model2D>(model);
        }

        #endregion

        #region data

        private readonly WeakReference<Model2D> _Source;

        private int? _ContentHash;

        private (XY Min, XY Max)? _BoundingBox;
        private (XY Center,Single Radius)? _BoundingCircle;

        #endregion

        #region properties

        public int ContentHashCode
        {
            get
            {
                if (_ContentHash.HasValue) return _ContentHash.Value;

                if (!_Source.TryGetTarget(out Model2D model)) return default;

                int h = 0;

                h ^= model._Commands.GetContentHashCode();                
                // h ^= model._References.Count;
                _ContentHash = h;
                return h;
            }
        }        

        public (XY Min, XY Max) BoundingBox
        {
            get
            {
                if (_BoundingBox.HasValue) return _BoundingBox.Value;

                if (!_Source.TryGetTarget(out Model2D model)) return (new XY(float.MaxValue), new XY(float.MinValue));

                _UpdateBounds(model);

                return _BoundingBox.Value;
            }
        }
        
        public (XY Center, Single Radius) BoundingCircle
        {
            get
            {
                if (_BoundingCircle.HasValue) return _BoundingCircle.Value;

                if (!_Source.TryGetTarget(out Model2D model)) return (XY.Zero, -1);

                _UpdateBounds(model);

                return _BoundingCircle.Value;
            }
        }        

        private void _UpdateBounds(Model2D model)
        {
            var (min, max, center, radius) = model._Commands.GetBounds();            
            _BoundingBox = (min, max);
            _BoundingCircle = (center, radius);
        }

        #endregion
    }    
}
