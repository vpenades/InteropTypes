using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;

using SCORE = InteropModels.Score;

namespace InteropModels
{
    [System.Diagnostics.DebuggerDisplay("{Name} {Score}")]
    public partial struct DetectedObject
    {
        #region data

        internal int ParentIndex;

        public String Name;
        public SCORE Score;
        public RectangleF Rect;
        
        public float Area => Rect.Width * Rect.Height;

        public bool Exists => Name != null;

        public Vector2 Center => new Vector2(Rect.X, Rect.Y) + new Vector2(Rect.Width, Rect.Height) * 0.5f;

        #endregion

        public DetectedObject Scaled(Vector2 scale)
        {
            var clone = this;

            var rf = clone.Rect;

            rf.X *= scale.X;
            rf.Y *= scale.Y;
            rf.Width *= scale.X;
            rf.Height *= scale.Y;

            clone.Rect = rf;

            return clone;
        }

        public DetectedObject Mirror(float width)
        {
            var clone = this;

            var rf = clone.Rect;
            rf.X = width - rf.X - rf.Width;
            clone.Rect = rf;

            return clone;
        }
    }
}
