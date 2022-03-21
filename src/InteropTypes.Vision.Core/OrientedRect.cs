using System;
using System.Collections.Generic;
using System.Text;

using XY = System.Numerics.Vector2;
using MATRIX = System.Numerics.Matrix3x2;

namespace InteropTypes.Vision
{
    /// <summary>
    /// Represents an oriented rectangle, backed by a <see cref="MATRIX"/>
    /// </summary>
    /// <remarks>
    /// - The <see cref="MATRIX.Translation"/> represents the top left corner of the rectangle.
    /// - The X and Y axes represent the orientation and size of the rectangle.
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("X:{X} Y:{Y} W:{Width} H:{Height}")]
    public readonly struct OrientedRect
    {
        #region constructor

        public OrientedRect(MATRIX matrix)
        {
            Matrix = matrix;
        }

        public OrientedRect(System.Drawing.PointF location, System.Drawing.SizeF size)
        {
            Matrix = new MATRIX(size.Width, 0, 0, size.Height, location.X, location.Y);
        }

        public OrientedRect(float x, float y, float w, float h)
        {
            Matrix = new MATRIX(w, 0, 0, h, x, y);
        }

        public OrientedRect(System.Drawing.RectangleF rect)
        {
            Matrix = new MATRIX(rect.Width, 0, 0, rect.Height, rect.X, rect.Y);
        }

        public static OrientedRect FromCenter(float centerX, float centerY, float width, float height, float radians = 0)
        {
            return FromCenter(new XY(centerX, centerY), new XY(width, height), radians);
        }

        public static OrientedRect FromCenter(XY center, XY size, float radians = 0)
        {
            var xform = MATRIX.CreateScale(size) * MATRIX.CreateRotation(radians);
            xform.Translation = center - XY.TransformNormal(XY.One * 0.5f, xform);
            return new OrientedRect(xform);
        }

        /// <summary>
        /// Creates a new <see cref="OrientedRect"/>
        /// </summary>
        /// <param name="location">The pivot location.</param>
        /// <param name="offset">
        /// The offset of the pivot relative the a unit box
        /// - (0,0) means <paramref name="location"/> represents the TopLeft corner of the rectangle.
        /// - (0.5,0.5) means <paramref name="location"/> represents the center of the rectangle.
        /// - (1,1) means <paramref name="location"/> represents the BottomRight corner of the rectangle.
        /// </param>
        /// <param name="size">The width and height of the rectangle.</param>
        /// <param name="radians">the rotation around <see cref="Location"/>.</param>
        /// <returns>A new <see cref="OrientedRect"/>.</returns>
        public static OrientedRect FromLocation(XY location, XY offset, XY size, float radians = 0)
        {
            var xform = MATRIX.CreateScale(size) * MATRIX.CreateRotation(radians);
            xform.Translation = location - XY.TransformNormal(offset, xform);
            return new OrientedRect(xform);
        }

        #endregion

        #region data

        public readonly MATRIX Matrix;

        public override int GetHashCode() { return Matrix.GetHashCode(); }

        #endregion

        #region properties

        public bool IsEmpty => Matrix == default;

        public bool IsAligned => Matrix.M12 == 0 && Matrix.M21 == 0;

        public float X => Matrix.M31;
        public float Y => Matrix.M32;        
        public float Width => new XY(Matrix.M11 , Matrix.M21).Length(); // XY.TransformNormal(XY.UnitX, Matrix).Length();
        public float Height => new XY(Matrix.M12 , Matrix.M22).Length(); //  XY.TransformNormal(XY.UnitY, Matrix).Length();

        public XY Location => Matrix.Translation;
        public XY TopLeft => Matrix.Translation; // XY.Transform(XY.Zero, Matrix);
        public XY TopRight => XY.Transform(XY.UnitX, Matrix);
        public XY BottomRight => XY.Transform(XY.One, Matrix);
        public XY BottomLeft => XY.Transform(XY.UnitY, Matrix);

        #endregion

        #region API

        public OrientedRect Inverted() { return MATRIX.Invert(Matrix, out MATRIX ix) ? new OrientedRect(ix) : default; }

        public static bool Invert(OrientedRect rect, out OrientedRect inverted)
        {
            var r = MATRIX.Invert(rect.Matrix, out MATRIX ix);
            inverted = r ? new OrientedRect(ix) : default;
            return r;
        }

        public static OrientedRect Transform(OrientedRect rect, MATRIX transform)
        {
            return new OrientedRect(rect.Matrix * transform);
        }        

        #endregion
    }
}
