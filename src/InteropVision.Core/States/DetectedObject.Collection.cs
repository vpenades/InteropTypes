using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;

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
        public class Collection : DisplayPrimitive.ISource
        {
            #region lifecycle

            public Collection() { }

            public Collection(SizeF frameSize)
            {
                _FrameSize = frameSize;                
            }

            public void CopyTo(ref Collection dst)
            {
                if (dst == null) dst = new Collection();
                dst.Overwrite(this);
            }

            #endregion

            #region data

            private SizeF _FrameSize;            

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
            private readonly List<DetectedObject> _Objects = new List<DetectedObject>();

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
            private readonly List<(int, int)> _DisplayLineIndices = new List<(int, int)>();            

            private void Overwrite(Collection other)
            {
                _FrameSize = other._FrameSize;                

                _Objects.Clear();
                _DisplayLineIndices.Clear();

                _Objects.AddRange(other._Objects);
                _DisplayLineIndices.AddRange(other._DisplayLineIndices);
            }

            #endregion

            #region properties

            public SizeF FrameSize => _FrameSize;
            
            public IReadOnlyList<DetectedObject> Objects => _Objects;

            public IEnumerable<View> Roots
            {
                get
                {
                    for (int i = 0; i < _Objects.Count; ++i)
                    {
                        if (_Objects[i].ParentIndex >= 0) continue;
                        yield return new View(this, i);
                    }
                }
            }

            #endregion

            #region API - management

            public void Clear()
            {
                _Objects.Clear();
                _DisplayLineIndices.Clear();
            }

            public void SetFrameSize(InferenceInput input)
            {
                SetFrameSize(input.Image.Width, input.Image.Height);
            }

            public void SetFrameSize(int width, int height)
            {
                if (_Objects.Count > 0 || _DisplayLineIndices.Count > 0) throw new InvalidOperationException("Frame Can only be set when collection is empty");

                _FrameSize = new SizeF(width, height);
            }

            public void ChangeResolutionTo(XY to)
            {
                ChangeResolution(new XY(_FrameSize.Width, _FrameSize.Height), to);
            }

            public void ChangeResolution(XY from, XY to)
            {
                if (from.X != 0 && from.Y != 0 && from != to)
                {
                    var scale = to / from;

                    for (int i = 0; i < _Objects.Count; ++i)
                    {
                        _Objects[i] = _Objects[i].Scaled(scale);
                    }
                }

                _FrameSize = new SizeF(to.X, to.Y);
            }

            public void Mirror()
            {
                for (int i = 0; i < _Objects.Count; ++i)
                {
                    _Objects[i] = _Objects[i].Mirror(_FrameSize.Width);
                }
            }

            #endregion

            #region API - Add

            public void Add(Collection other, int parentIdx = -1)
            {
                var dict = other._DisplayLineIndices.Count == 0 ? null : new Dictionary<int, int>();

                foreach (var r in other.Roots) { Add(r, dict, parentIdx); }

                if (dict != null)
                {
                    foreach(var l in other._DisplayLineIndices)
                    {
                        var a = dict[l.Item1];
                        var b = dict[l.Item2];

                        this._DisplayLineIndices.Add((a, b));
                    }
                }
            }

            public int Add(View v, int parentIdx = -1) { return Add(v, null, parentIdx); }

            private int Add(View v, Dictionary<int,int> map,  int parentIdx = -1)
            {
                var idx = Add(v.Current, parentIdx);

                if (map != null) map[v._Index] = idx;

                foreach (var vv in v.Children) { Add(vv, map, idx); }
                return idx;
            }

            public int Add(DetectedObject o, int parentIdx = -1)
            {
                o.ParentIndex = parentIdx;

                var idx = this._Objects.Count;
                this._Objects.Add(o);
                return idx;
            }            

            public int AddObject(RECTF rect, SCORE confidence, String name, RECTF? parentRect)
            {
                if (parentRect.HasValue) rect.Offset(parentRect.Value.Location);

                var item = new DetectedObject
                {
                    Rect = rect,
                    Score = confidence,
                    Name = name,
                    ParentIndex = -1
                };

                var idx = _Objects.Count;

                _Objects.Add(item);

                return idx;
            }



            public int AddPoint(POINT point, String name, int idx = -1)
            {
                return AddPoint(point, SCORE.Ok, name, idx);
            }

            public int AddPoint(POINT point, SCORE confidence, String name, int idx = -1)
            {
                var rect = point.ToGDIRectangleOffCenter(0);
                return AddObject(rect, confidence, name, idx);
            }

            public void AddObject(RECTF? rect, SCORE score, String name, int idx = -1)
            {
                if (rect.HasValue) AddObject(rect.Value, score, name, idx);

            }

            public int AddObject(RECTF rect, SCORE confidence, String name, int idx = -1)
            {
                var item = new DetectedObject
                {
                    Rect = rect,
                    Score = confidence,
                    Name = name,
                    ParentIndex = idx
                };

                idx = _Objects.Count;

                _Objects.Add(item);

                return idx;
            }

            public void AddObjects(IEnumerable<RECTI> localRects, string name, RECTF? parentRect)
            {
                _Objects.Clear();

                if (parentRect.HasValue)
                {
                    foreach (var r in localRects)
                    {
                        RECTF rr = r;
                        rr.Offset(parentRect.Value.Location);

                        AddObject(rr, (1, true), name);
                    }
                }
                else
                {
                    foreach (var r in localRects)
                    {
                        AddObject(r, SCORE.Ok, name);
                    }
                }
            }

            public void AddObjects(IEnumerable<RECTF> localRects, string name, RECTF? parentRect)
            {
                _Objects.Clear();

                if (parentRect.HasValue)
                {
                    foreach (var r in localRects)
                    {
                        var rr = r;
                        rr.Offset(parentRect.Value.Location);

                        AddObject(rr, (1,true), name);
                    }
                }
                else
                {
                    foreach (var r in localRects)
                    {
                        AddObject(r, SCORE.Ok, name);
                    }
                }
            }


            public void AddDisplayLine(int indexA, int indexB) { _DisplayLineIndices.Add((indexA, indexB)); }            
                 

            #endregion

            #region API - Display

            public IEnumerable<DisplayPrimitive> GetDisplayPrimitives() { return GetDisplayPrimitives(false); }

            public IEnumerable<DisplayPrimitive> GetDisplayPrimitives(bool mirrored)
            {
                var xform = Matrix3x2.Identity;

                if (mirrored)
                {
                    var w = _FrameSize.Width;
                    // if w == 0 get bounds

                    xform = Matrix3x2.CreateScale(-1, 1) * Matrix3x2.CreateTranslation(w, 0);
                }                

                return GetDisplayPrimitives(xform);
            }

            public IEnumerable<DisplayPrimitive> GetDisplayPrimitives(Matrix3x2 xform)
            {
                foreach (var r in _Objects)
                {
                    var rr = r.Rect;

                    var color = _GetColor(r.Name);

                    foreach (var prim in DisplayPrimitive.Rect(color, rr.Location, rr.Size, xform)) yield return prim;

                    if (r.Area < 8 * 8) continue;
                    if (!r.Score.IsValid) continue;
                    if (r.Score.Value <= 0) continue;
                    if (r.Score.Value > 1) continue;

                    float x = rr.Location.X - 6;
                    float y = rr.Location.Y + rr.Height;
                    float s = r.Score.Value * (float)rr.Height;
                    y -= s;

                    yield return DisplayPrimitive.Line(COLOR.Green, (x, y), (x, y + s));                    
                }

                foreach(var l in _DisplayLineIndices)
                {
                    var a = _Objects[l.Item1].Rect;
                    var b = _Objects[l.Item2].Rect;

                    yield return DisplayPrimitive.Line(COLOR.Green, POINT.Center(a), POINT.Center(b));
                }

                int row = 0;

                foreach(var item in this.Roots)
                {
                    var s = item.Current.Score;
                    if (s.Equals(SCORE.Ok)) continue;

                    yield return DisplayPrimitive.Write(s.IsValid ? COLOR.Green : COLOR.Red, (5, row), s.Value.ToString(), 10);

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

            #endregion
        }
    }
}
