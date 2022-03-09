using System;
using System.Collections.Generic;
using System.Numerics;

using XYZ = System.Numerics.Vector3;
using BBOX = System.Numerics.Matrix3x2; // Column 0 is Min, Column 1 is Max

namespace InteropTypes.Graphics.Drawing
{

    /// <summary>
    /// Represents a collection of drawing commands that can be replayed against an <see cref="IScene3D"/> target.
    /// </summary>
    [System.Diagnostics.DebuggerTypeProxy(typeof(_Model3DProxy))]
    public class Record3D : IScene3D, IDrawingBrush<IScene3D>, IPseudoImmutable, BoundingSphere.ISource
    {
        #region data

        internal readonly _CommandStream3D _Commands = new _CommandStream3D();

        private Model3DVersionKey _ImmutableKey;

        #endregion

        #region properties

        internal IEnumerable<int> Commands => _Commands.GetCommands();


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


        public BoundingSphere BoundingSphere
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

        public BoundingSphere GetBoundingSphere()
        {
            var key = (Model3DVersionKey)ImmutableKey;
            return key.BoundingSphere;
        }

        public void Clear()
        {
            _ImmutableKey = null;

            _Commands.Clear();
        }

        /// <inheritdoc/>        
        public void DrawAsset((Quaternion R, XYZ T) transform, object asset)
        {
            var xform = Matrix4x4.CreateFromQuaternion(transform.R);
            xform.Translation = transform.T;

            DrawAsset(xform, asset);
        }

        /// <inheritdoc/>
        public void DrawAsset(in Matrix4x4 transform, object asset)
        {
            _ImmutableKey = null;
            _Commands.DrawAsset(transform, asset);
        }        

        /// <inheritdoc/>
        public void DrawSegments(ReadOnlySpan<Point3> vertices, float diameter, LineStyle style)
        {
            _ImmutableKey = null;
            _Commands.DrawSegments(vertices, diameter, style);
        }

        /// <inheritdoc/>
        public void DrawSphere(Point3 center, float diameter, OutlineFillStyle style)
        {
            _ImmutableKey = null;
            _Commands.DrawSphere(center, diameter, style);
        }

        /// <inheritdoc/>
        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle style)
        {
            _ImmutableKey = null;
            _Commands.DrawSurface(vertices, style);
        }

        /// <inheritdoc/>
        public void DrawConvexSurface(ReadOnlySpan<Point3> vertices, ColorStyle style)
        {
            _ImmutableKey = null;
            _Commands.DrawConvexSurface(vertices, style);
        }

        /// <inheritdoc/>
        public void DrawTo(IScene3D dc)
        {
            foreach (var offset in _Commands.GetCommands())
            {
                _Commands.DrawTo(offset, dc, false);
            }
        }

        public void DrawTo(ICanvas2D target, float width, float height, XYZ cameraPosition, bool perspective = true)
        {
            var bounds = BoundingMatrix;
            var dimensions = bounds.ColumnY() - bounds.ColumnX();

            var avgsize = (dimensions.X + dimensions.Y + dimensions.Z) / 3;

            var cameraWTF = CreateWorldMatrix(cameraPosition, bounds.MinMaxCenter(), avgsize * 2);


            /* Unmerged change from project 'InteropTypes.Graphics.Drawing.Toolkit (netstandard2.1)'
            Before:
                        var context = Transforms.PerspectiveTransform.CreatePerspective((target, width, height), 1.2f, cameraWTF);
            After:
                        var context = PerspectiveTransform.CreatePerspective((target, width, height), 1.2f, cameraWTF);
            */
            var context = Transforms.PerspectiveTransform.CreatePerspective((target, width, height), 1.2f, cameraWTF);

            context.DrawScene(this);
        }

        public void DrawTo((ICanvas2D target, float width, float height) renderTarget, CameraTransform3D camera)
        {
            var context = Transforms.PerspectiveTransform.Create(renderTarget, camera);

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

        public void CopyTo(Record3D other)
        {
            other._Commands.Set(_Commands);
            other._ImmutableKey = null;
        }

        #endregion
    }

    sealed class _Model3DProxy : _CommandStream3D_DebuggerProxy
    {
        public _Model3DProxy(Record3D src) : base(src._Commands) { ImmutableKey = src.ImmutableKey; }

        public object ImmutableKey { get; }
    }

    /// <summary>
    /// Represents the version key of a <see cref="Record3D"/>.
    /// </summary>
    /// <remarks>
    /// Stores computationally expensive resources of a given version of a <see cref="Record3D"/>
    /// and also serves as <see cref="Record3D.ImmutableKey"/> underlaying object.
    /// </remarks>
    sealed class Model3DVersionKey
    {
        #region constructor

        public Model3DVersionKey(Record3D model)
        {
            _Source = new WeakReference<Record3D>(model);
        }

        #endregion

        #region data

        private readonly WeakReference<Record3D> _Source;

        private int? _ContentHash;

        private (XYZ Min, XYZ Max)? _BoundingBox;
        private BoundingSphere? _BoundingSphere;

        private (int, int, int, System.Drawing.Color)[] _Triangles;

        // convex hull

        #endregion

        #region properties

        public int ContentHashCode
        {
            get
            {
                if (_ContentHash.HasValue) return _ContentHash.Value;

                if (!_Source.TryGetTarget(out Record3D model)) return default;

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

                if (!_Source.TryGetTarget(out Record3D model)) return default;

                _UpdateBounds(model);

                return _BoundingBox.Value;
            }
        }

        public BoundingSphere BoundingSphere
        {
            get
            {
                if (_BoundingSphere.HasValue) return _BoundingSphere.Value;

                if (!_Source.TryGetTarget(out Record3D model)) return default;

                _UpdateBounds(model);

                return _BoundingSphere.Value;
            }
        }

        private void _UpdateBounds(Record3D model)
        {
            var (min, max, sphere) = model._Commands.GetBounds();
            _BoundingBox = (min, max);
            _BoundingSphere = sphere;
        }

        #endregion
    }
}
