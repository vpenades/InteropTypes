using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Drawing;

using TRACES = Plotly.Box<Plotly.Types.ITracesProperty>;

namespace InteropTypes.Graphics.Backends
{
    class _PlotlyDrawing2DTracesContext : IDisposableCanvas2D
    {
        #region lifecycle
        public _PlotlyDrawing2DTracesContext(PlotlyDocumentBuilder owner)
        {
            _Owner = owner;
            _Traces = new List<TRACES>();
            _Markers = new List<(Point2, float, System.Drawing.Color)>();
        }
        public void Dispose()
        {
            if (_Traces != null)
            {
                if (_Markers != null && _Markers.Count > 0)
                {
                    _Traces.Add(Plotly.TracesFactory.Markers(_Markers));
                }

               foreach(var t in _Traces) _Owner.AppendTrace(t);
            }
            
            _Traces = null;
        }        

        #endregion

        #region data

        private readonly PlotlyDocumentBuilder _Owner;
        private List<TRACES> _Traces;
        private List<(Point2, float, System.Drawing.Color)> _Markers;

        #endregion

        #region API

        /// <inheritdoc/>
        public void DrawAsset(in Matrix3x2 transform, object asset)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void DrawConvexPolygon(ReadOnlySpan<Point2> points, ColorStyle color)
        {
            if (color.IsEmpty) return;

            _Traces.Add(Plotly.TracesFactory.Polygon(points, color.ToGDI()));
        }

        /// <inheritdoc/>
        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle style)
        {
            if (!style.IsVisible) return;

            var lstyle = Plotly.LineProperties.Create(style.Style.FillColor.ToGDI(), diameter, style.Style.OutlineColor.ToGDI(), style.Style.OutlineWidth);

            _Traces.Add(Plotly.TracesFactory.Lines(points, lstyle));
        }

        /// <inheritdoc/>
        public void DrawEllipse(Point2 center, float width, float height, OutlineFillStyle style)
        {
            if (!style.IsVisible) return;

            _Markers.Add((center, (width + height) * 0.25f, style.FillColor.ToGDI()));
        }

        /// <inheritdoc/>
        public void DrawImage(in Matrix3x2 transform, ImageStyle style)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public void DrawPolygon(ReadOnlySpan<Point2> points, PolygonStyle style)
        {
            if (!style.IsVisible) return;

            if (style.HasOutline)
            {
                var ls = Plotly.LineProperties.Create(style.OutlineColor.ToGDI(), style.OutlineWidth);

                _Traces.Add(Plotly.TracesFactory.Polygon(points, style.FillColor.ToGDI(), ls));
            }
            else
            {
                if (style.HasFill)
                {
                    _Traces.Add(Plotly.TracesFactory.Polygon(points, style.FillColor.ToGDI()));
                }
            }
        }

        public void DrawTextLine(in Matrix3x2 transform, string text, float size, FontStyle font)
        {
            font.DrawDecomposedTo(this, transform, text, size);
        }

        #endregion
    }
}
