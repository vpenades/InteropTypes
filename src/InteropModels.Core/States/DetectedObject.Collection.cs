using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;

using XY = System.Numerics.Vector2;

using SCORE = InteropModels.Score;
using RECTI = System.Drawing.Rectangle;
using RECTF = System.Drawing.RectangleF;
using COLOR = System.Drawing.Color;

namespace InteropModels
{
    partial struct DetectedObject
    {
        private sealed class DebugViewProxy
        {
            public DebugViewProxy(View v) { _View = v; }

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
            private View _View;

            public String Name => _View.Current.Name;
            public Score Score => _View.Current.Score;

            public RECTF Rectangle => _View.Current.Rect;            

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public View[] Z_Children => _View.Children.ToArray();
        }

        [System.Diagnostics.DebuggerDisplay("{Current.Name} {Current.Score} {Current.Rect}")]
        [System.Diagnostics.DebuggerTypeProxy(typeof(DebugViewProxy))]
        public readonly struct View
        {
            internal View(Collection c, int i) { _Source = c; _Index = i; }

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
            private readonly Collection _Source;

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
            internal readonly int _Index;


            public DetectedObject Current => _Source.Objects[_Index];

            public IEnumerable<View> Children
            {
                get
                {
                    for (int i = 0; i < _Source.Objects.Count; ++i)
                    {
                        var o = _Source.Objects[i];
                        if (o.ParentIndex == _Index) yield return new View(_Source, i);
                    }
                }
            }
        }

        public class Collection : DisplayPrimitive.ISource
        {
            #region lifecycle

            public Collection() { }

            #endregion

            #region data

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
            private readonly List<DetectedObject> _Objects = new List<DetectedObject>();

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
            private readonly List<(int, int)> _DisplayLineIndices = new List<(int, int)>();

            public void Overwrite(Collection other)
            {
                _Objects.Clear();
                _DisplayLineIndices.Clear();

                _Objects.AddRange(other._Objects);
                _DisplayLineIndices.AddRange(other._DisplayLineIndices);
            }

            #endregion

            #region properties

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

            #region API

            public void Clear()
            {
                _Objects.Clear();
                _DisplayLineIndices.Clear();
            }

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



            public int AddPoint(DisplayVector2 point, String name, int idx = -1)
            {
                return AddPoint(point, SCORE.Ok, name, idx);
            }

            public int AddPoint(DisplayVector2 point, SCORE confidence, String name, int idx = -1)
            {
                var rect = point.ToCenteredRectangle(0);
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


            public void AddDisplayLine(int indexA, int indexB)
            {
                _DisplayLineIndices.Add((indexA, indexB));
            }

            public void ChangeResolution(XY from, XY to)
            {
                var scale = to / from;

                for(int i=0; i < _Objects.Count; ++i)
                {
                    _Objects[i] = _Objects[i].Scaled(scale);
                }
            }

            public IEnumerable<DisplayPrimitive> GetDisplayPrimitives()
            {
                foreach (var r in _Objects)
                {
                    var rr = r.Rect;

                    var color = _GetColor(r.Name);

                    yield return DisplayPrimitive.Rect(color, rr.Location, rr.Size);

                    if (r.Area < 8 * 8) continue;
                    if (!r.Score.IsValid) continue;
                    if (r.Score.Value <= 0) continue;
                    if (r.Score.Value > 1) continue;

                    float x = rr.Location.X - 6;
                    float y = rr.Location.Y + rr.Height;
                    float s = r.Score.Value * (float)rr.Height;
                    y -= s;

                    yield return DisplayPrimitive.Rect(COLOR.Green, x, y, 2, s);
                }

                foreach(var l in _DisplayLineIndices)
                {
                    var a = _Objects[l.Item1].Rect;
                    var b = _Objects[l.Item2].Rect;

                    yield return DisplayPrimitive.Line(COLOR.Green, a, b);
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
