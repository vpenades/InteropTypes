﻿using System;
using System.Collections.Generic;
using System.Numerics;

using XY = System.Numerics.Vector2;
using RECT = System.Drawing.RectangleF;

namespace InteropTypes.Graphics.Drawing
{
    [System.Diagnostics.DebuggerTypeProxy(typeof(_Model2DProxy))]
    public class Record2D
        : ICloneable
        , ICanvas2D
        , IPseudoImmutable
        , IDrawingBrush<ICanvas2D>
        , GlobalStyle.ISource        
    {
        #region lifecycle

        Object ICloneable.Clone() { return new Record2D(this); }

        public Record2D Clone() { return new Record2D(this); }

        public Record2D() { }

        protected Record2D(Record2D other)
        {
            _Commands.Set(other._Commands);
            _GlobalStyle = other._GlobalStyle?.Clone();

            // whenever the original or the clone changes,
            // this will be refreshed, so it's safe to share.
            _ImmutableKey = other._ImmutableKey;
        }

        #endregion

        #region data

        internal readonly _CommandStream2D _Commands = new _CommandStream2D();

        private GlobalStyle _GlobalStyle;

        private Model2DVersionKey _ImmutableKey;

        #endregion

        #region properties        

        /// <summary>
        /// This value does not change as long as the content of <see cref="Record3D"/> doesn't change either.
        /// </summary>
        /// <remarks>
        /// <see cref="ImmutableKey"/> can be used as a Dictionary Key so a client backend can create an optimized
        /// renderable resource to be used in place of this <see cref="Record3D"/>
        /// </remarks>
        public object ImmutableKey
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


        public (XY Center, float Radius) BoundingCircle
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
            _Commands.Clear();
            _GlobalStyle = null;
            _ImmutableKey = null;
        }

        public bool TryGetGlobalProperty<T>(string name, out T value)
        {
            return GlobalStyle.TryGetGlobalProperty(_GlobalStyle, name, out value);
        }

        public bool TrySetGlobalProperty<T>(string name, T value)
        {
            return GlobalStyle.TrySetGlobalProperty(ref _GlobalStyle, name, value);
        }

        /// <inheritdoc/>
        public void DrawAsset(in Matrix3x2 transform, object asset)
        {
            _ImmutableKey = null;
            _Commands.DrawAsset(transform, asset);
        }

        /// <inheritdoc/>
        public void DrawConvexPolygon(ReadOnlySpan<Point2> points, ColorStyle color)
        {
            _ImmutableKey = null;
            _Commands.DrawConvexPolygon(points, color);
        }

        /// <inheritdoc/>
        public void DrawLines(ReadOnlySpan<Point2> points, float width, LineStyle brush)
        {
            _ImmutableKey = null;
            _Commands.DrawLines(points, width, brush);
        }

        /// <inheritdoc/>
        public void DrawEllipse(Point2 center, float w, float h, OutlineFillStyle brush)
        {
            _ImmutableKey = null;
            _Commands.DrawEllipse(center, w, h, brush);
        }

        /// <inheritdoc/>
        public void DrawPolygon(ReadOnlySpan<Point2> points, PolygonStyle brush)
        {
            _ImmutableKey = null;
            _Commands.DrawPolygon(points, brush);
        }

        /// <inheritdoc/>
        public void DrawImage(in Matrix3x2 transform, ImageStyle style)
        {
            _ImmutableKey = null;
            _Commands.DrawImage(transform, style);
        }

        /// <inheritdoc/>
        public void DrawTextLine(in Matrix3x2 transform, string text, float size, FontStyle font)
        {
            _ImmutableKey = null;
            _Commands.DrawTextLine(transform, text, size, font);
        }

        /// <inheritdoc/>
        public void DrawTo(ICanvas2D dc)
        {
            foreach (var offset in _Commands.GetCommands())
            {
                _Commands.DrawTo(offset, dc, false);
            }
        }

        public IEnumerable<string> ToLog() { return new _Model2DProxy(this).Primitives; }

        public void DrawTo(IScene3D dc, Matrix4x4 xform) { DrawTo(dc.CreateTransformed2D(xform)); }        

        public void CopyTo(Record2D other)
        {
            other._Commands.Set(_Commands);            
            other._ImmutableKey = null;            
        }        

        #endregion
    }

    sealed class _Model2DProxy : _CommandStream2D_DebuggerProxy
    {
        public _Model2DProxy(Record2D src) : base(src._Commands) { ImmutableKey = src.ImmutableKey; }

        public object ImmutableKey { get; }
    }

    /// <summary>
    /// Represents the version key of a <see cref="Record2D"/>.
    /// </summary>
    /// <remarks>
    /// Stores computationally expensive resources of a given version of a <see cref="Record3D"/>
    /// and also serves as <see cref="Record3D.ImmutableKey"/> underlaying object.
    /// </remarks>
    sealed class Model2DVersionKey
    {
        #region constructor

        public Model2DVersionKey(Record2D model)
        {
            _Source = new WeakReference<Record2D>(model);
        }

        #endregion

        #region data

        private readonly WeakReference<Record2D> _Source;

        private int? _ContentHash;

        private (XY Min, XY Max)? _BoundingBox;
        private (XY Center, float Radius)? _BoundingCircle;

        #endregion

        #region properties

        public int ContentHashCode
        {
            get
            {
                if (_ContentHash.HasValue) return _ContentHash.Value;

                if (!_Source.TryGetTarget(out Record2D model)) return default;

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

                if (!_Source.TryGetTarget(out Record2D model)) return (new XY(float.MaxValue), new XY(float.MinValue));

                _UpdateBounds(model);

                return _BoundingBox.Value;
            }
        }

        public (XY Center, float Radius) BoundingCircle
        {
            get
            {
                if (_BoundingCircle.HasValue) return _BoundingCircle.Value;

                if (!_Source.TryGetTarget(out Record2D model)) return (XY.Zero, -1);

                _UpdateBounds(model);

                return _BoundingCircle.Value;
            }
        }

        private void _UpdateBounds(Record2D model)
        {
            var (min, max, center, radius) = model._Commands.GetBounds();
            _BoundingBox = (min, max);
            _BoundingCircle = (center, radius);
        }

        #endregion
    }
}
