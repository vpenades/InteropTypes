using System;
using System.Collections.Generic;
using System.Text;

using InteropDrawing;

using XY = System.Numerics.Vector2;
using POINT = InteropDrawing.Point2;

using SCORE = InteropVision.Score;
using RECTI = System.Drawing.Rectangle;
using RECTF = System.Drawing.RectangleF;
using COLOR = System.Drawing.Color;

namespace InteropVision
{
    partial struct DetectedObject
    {
        public partial class Collection : IDrawable2D
        {
            public virtual void DrawTo(IDrawing2D dc)
            {
                // draw rectangles

                foreach (var r in _Objects)
                {
                    var rr = r.Rect;                    

                    dc.DrawRectangle(rr, _GetColor(r.Name));

                    // draw score

                    if (r.Area < 8 * 8) continue;
                    if (!r.Score.IsValid) continue;
                    if (r.Score.Value <= 0) continue;
                    if (r.Score.Value > 1) continue;                    

                    float x = rr.Location.X - 6;
                    float y = rr.Location.Y + rr.Height;
                    float s = r.Score.Value * (float)rr.Height;
                    y -= s;

                    dc.DrawLine((x, y), (x, y + s), 0.01f, COLOR.Green);                    
                }

                // draw lines

                foreach (var l in _DisplayLineIndices)
                {
                    var a = _Objects[l.Item1].Rect;
                    var b = _Objects[l.Item2].Rect;

                    dc.DrawLine(POINT.Center(a), POINT.Center(b), 0.01f, COLOR.Green);
                }

                // draw text

                int row = 0;

                foreach (var item in this.Roots)
                {
                    var s = item.Current.Score;
                    if (s.Equals(SCORE.Ok)) continue;

                    // dc.DrawFont()
                    // yield return DisplayPrimitive.Write(s.IsValid ? COLOR.Green : COLOR.Red, (5, row), s.Value.ToString(), 10);

                    row += 20;
                }
            }

            private static COLOR _GetColor(string name)
            {
                if (name == null) name = string.Empty;
                var h = 0;
                foreach (var c in name) { h ^= c.GetHashCode(); h *= 17; }

                var r = (h & 4) != 0 ? 255 : 0;
                var g = (h & 2) != 0 ? 255 : 0;
                var b = (h & 1) != 0 ? 255 : 0;

                return COLOR.FromArgb(r, g, b);
            }
        }
    }
}
