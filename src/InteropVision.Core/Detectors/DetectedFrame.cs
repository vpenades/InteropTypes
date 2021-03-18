using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

using RECTI = System.Drawing.Rectangle;
using RECTF = System.Drawing.RectangleF;

namespace InteropVision
{
    [System.Diagnostics.DebuggerDisplay("T:{Time} S:{Score} Area:{Region.Width*Region.Height}")]
    public struct DetectedFrame
    {
        #region constructors

        public static implicit operator DetectedFrame(RECTI rect)
        {
            return new DetectedFrame(rect, FrameTime.Now, Score.Ok);
        }

        public DetectedFrame(RECTF r, FrameTime t, Score s)
        {
            Region = r;
            Time = t;
            Score = s;
        }

        public DetectedFrame(RECTF r, float rs, FrameTime t, Score s)
        {
            Region = r;
            Time = t;
            Score = s;

            Region.Inflate(rs, rs);
        }

        #endregion

        #region data

        public RECTF Region;
        public FrameTime Time;
        public Score Score;        

        #endregion

        #region properties

        public Vector2 Size => new Vector2(Region.Width, Region.Height);

        public float MaxSize => Math.Max(Region.Width, Region.Height);

        public float AvgSize => (Region.Width + Region.Height) * 0.5f;

        public Vector2 Center => new Vector2(Region.X, Region.Y) + Size * 0.5f;

        #endregion

        #region API

        public static Vector2 GetVelocity(in DetectedFrame t0, in DetectedFrame t1)
        {
            var dt = t1.Time.RelativeTime - t0.Time.RelativeTime;
            var cc = t1.Center - t0.Center;

            return cc / (float)dt.TotalSeconds;
        }

        public static RECTF GetNextDetectionWindow(DetectedFrame t, float scaleFactor)
        {
            var c = t.Center;
            var ss = new Vector2(t.MaxSize * scaleFactor);

            c -= ss * 0.5f;

            return new RECTF(c.ToPoint(), ss.ToSize());
        }

        public static RECTF GetNextDetectionWindow(DetectedFrame t0, DetectedFrame t1, float scaleFactor)
        {
            var ctr0 = t1.Center;
            var siz0 = t1.MaxSize;

            const float sizeSmoothing = 0.7f;
            const float positionSmoothing = 0.6f;

            var ctr1 = t0.Center;
            var siz1 = t0.MaxSize;

            var ccc = Vector2.Lerp(ctr1, ctr0, (1 - positionSmoothing));
            var sss = new Vector2(siz1 * sizeSmoothing + siz0 * (1 - sizeSmoothing));

            // predict the direction of face movement
            ccc += GetVelocity(t0, t1) / 60;

            sss *= scaleFactor;
            ccc -= sss * 0.5f;
            return new RECTF(ccc.ToPoint(), sss.ToSize());
        }

        public static void RemoveOverlapping(IList<DetectedFrame> frames)
        {
            for (int i = frames.Count - 1; i >= 0; --i)
            {
                for (int j = i - 1; j >= 0; --j)
                {
                    var ii = frames[i];
                    var jj = frames[j];

                    var overlap = RECTF.Intersect(ii.Region, jj.Region);
                    if (overlap.IsEmpty) continue;

                    if (ii.Score < jj.Score) { frames.RemoveAt(i); break; }
                    if (ii.Score > jj.Score) { frames.RemoveAt(j); break; }
                }
            }
        }

        #endregion
    }
}
