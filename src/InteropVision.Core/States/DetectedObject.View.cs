using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using RECTF = System.Drawing.RectangleF;

namespace InteropTypes.Vision
{
    partial struct DetectedObject
    {
        [System.Diagnostics.DebuggerDisplay("{Current.Name} {Current.Score} {Current.Rect}")]
        [System.Diagnostics.DebuggerTypeProxy(typeof(_DebugViewProxy))]
        public readonly struct View
        {
            #region constructor
            internal View(Collection c, int i) { _Source = c; _Index = i; }

            #endregion

            #region data

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
            private readonly Collection _Source;

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
            internal readonly int _Index;

            #endregion

            #region properties

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

            #endregion
        }

        private sealed class _DebugViewProxy
        {
            public _DebugViewProxy(View v) { _View = v; }

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
            private View _View;

            public String Name => _View.Current.Name;
            public Score Score => _View.Current.Score;

            public RECTF Rectangle => _View.Current.Rect;

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public View[] Z_Children => _View.Children.ToArray();
        }
    }
}
