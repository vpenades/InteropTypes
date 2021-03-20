using System;
using System.Collections.Generic;
using System.Numerics;

using XYZ = System.Numerics.Vector3;
using BBOX = System.Numerics.Matrix3x2; // Column 0 is Min, Column 1 is Max


namespace InteropDrawing
{

    /// <summary>
    /// Represents a collection of drawing commands that can be replayed against an <see cref="IDrawing3D"/> target.
    /// </summary>
    [System.Diagnostics.DebuggerTypeProxy(typeof(_Model3DProxy))]
    public class Model3D : IDrawing3D, IPseudoImmutable
    {
        #region data

        internal readonly _CommandStream3D _Commands = new _CommandStream3D();

        private Model3DVersionKey _ImmutableKey;

        #endregion

        #region properties

        internal IEnumerable<int> Commands => _Commands.GetCommands();
        

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
                if (_ImmutableKey == null) _ImmutableKey = new Model3DVersionKey(this);
                return _ImmutableKey;
            }
        }


        public BBOX BoundingMatrix => BoundingBox.MinMaxToMatrix3x2();

        public (XYZ Min, XYZ Max) BoundingBox
        {
            get
            {
                var key = (Model3DVersionKey)ImmutableKey;
                return key.BoundingBox;
            }
        }

        
        public (XYZ Center,Single Radius) BoundingSphere
        {
            get
            {
                var key = (Model3DVersionKey)ImmutableKey;
                return key.BoundingSphere;
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

        public void DrawAsset((Quaternion R, Vector3 T) transform, object asset, ColorStyle brush)
        {
            var xform = Matrix4x4.CreateFromQuaternion(transform.R);
            xform.Translation = transform.T;

            DrawAsset(xform, asset, brush);
        }

        public void DrawAsset(in Matrix4x4 transform, object asset, ColorStyle brush)
        {
            _ImmutableKey = null;
            _Commands.DrawAsset(transform, asset, brush);
        }

        public void DrawSegment(Point3 a, Point3 b, Single diameter, LineStyle brush)
        {
            _ImmutableKey = null;
            _Commands.DrawSegment(a, b, diameter, brush);
        }

        public void DrawSphere(Point3 center, Single diameter, ColorStyle brush)
        {
            _ImmutableKey = null;
            _Commands.DrawSphere(center, diameter, brush);
        }

        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle brush)
        {
            _ImmutableKey = null;
            _Commands.DrawSurface(vertices, brush);
        }

        public void DrawTo(IDrawing3D dc, bool collapse = false)
        {
            int offset = 0;

            while (offset < _Commands.Count)
            {
                offset += _Commands.DrawTo(offset, dc, collapse);
                offset += 4;
            }
        }        

        public void DrawTo(IDrawing2D target, float width, float height, XYZ cameraPosition, bool perspective = true)
        {
            var bounds = this.BoundingMatrix;
            var dimensions = bounds.ColumnY() - bounds.ColumnX();

            var avgsize = (dimensions.X + dimensions.Y + dimensions.Z) / 3;

            var cameraWTF = CreateWorldMatrix(cameraPosition, bounds.MinMaxCenter(), avgsize * 2);

            var context = Transforms.PerspectiveTransform.CreatePerspective((target, width, height), 1.2f, cameraWTF);

            context.DrawScene(this);            
        }

        public void DrawTo((IDrawing2D target, float width, float height) renderTarget, Matrix4x4 camera, Matrix4x4 projection)
        {
            var context = Transforms.PerspectiveTransform.Create(renderTarget, projection, camera);

            context.DrawScene(this);
        }

        private static Matrix4x4 CreateWorldMatrix(XYZ fromPosition, XYZ targetPosition, float minDist)
        {
            var d = targetPosition - fromPosition;

            if (d.Length() < minDist)
            {
                d = d.WithLength(minDist);
                fromPosition = targetPosition - d;
            }

            return Matrix4x4.CreateWorld(fromPosition, d, XYZ.UnitY);
        }

        public void CopyTo(Model3D other)
        {
            other._Commands.Set(this._Commands);
            other._ImmutableKey = null;
        }

        #endregion
    }

    sealed class _Model3DProxy : _CommandStream3D_DebuggerProxy
    {
        public _Model3DProxy(Model3D src) : base(src._Commands) { ImmutableKey = src.ImmutableKey; }

        public object ImmutableKey { get; }
    }

    /// <summary>
    /// Represents the version key of a <see cref="Model3D"/>.
    /// </summary>
    /// <remarks>
    /// Stores computationally expensive resources of a given version of a <see cref="Model3D"/>
    /// and also serves as <see cref="Model3D.ImmutableKey"/> underlaying object.
    /// </remarks>
    sealed class Model3DVersionKey
    {
        #region constructor

        public Model3DVersionKey(Model3D model)
        {
            _Source = new WeakReference<Model3D>(model);
        }

        #endregion

        #region data

        private readonly WeakReference<Model3D> _Source;

        private int? _ContentHash;

        private (XYZ Min, XYZ Max)? _BoundingBox;
        private (XYZ Center, Single Radius)? _BoundingSphere;

        private (int, int, int, System.Drawing.Color)[] _Triangles;

        // convex hull

        #endregion

        #region properties

        public int ContentHashCode
        {
            get
            {
                if (_ContentHash.HasValue) return _ContentHash.Value;

                if (!_Source.TryGetTarget(out Model3D model)) return default;

                // create a hash from model _Commands, _Vectors and _References.

                int h = 0;
                /*
                h ^= model.Commands.Count;
                h ^= model._Vectors.Count;
                h ^= model._References.Count;

                foreach (var v in model._Vectors)
                {
                    h ^= v.GetHashCode();
                    h *= 17;
                }

                _ContentHash = h;
                */

                return h;
            }
        }
        
        public (XYZ Min, XYZ Max) BoundingBox
        {
            get
            {
                if (_BoundingBox.HasValue) return _BoundingBox.Value;

                if (!_Source.TryGetTarget(out Model3D model)) return default;

                _UpdateBounds(model);

                return _BoundingBox.Value;
            }
        }       

        public (XYZ Center, Single Radius) BoundingSphere
        {
            get
            {
                if (_BoundingSphere.HasValue) return _BoundingSphere.Value;

                if (!_Source.TryGetTarget(out Model3D model)) return default;

                _UpdateBounds(model);

                return _BoundingSphere.Value;
            }
        }

        private void _UpdateBounds(Model3D model)
        {
            var (min, max, center, radius) = model._Commands.GetBounds();
            _BoundingBox = (min, max);
            _BoundingSphere = (center, radius);
        }

        #endregion
    }    
}
