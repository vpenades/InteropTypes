using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Drawing;

using TRACES = Plotly.Box<Plotly.Types.ITracesProperty>;

namespace InteropDrawing.Backends
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
        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle style)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void DrawConvexPolygon(ReadOnlySpan<Point2> points, ColorStyle color)
        {
            if (color.IsEmpty) return;

            _Traces.Add(Plotly.TracesFactory.Polygon(points, color.Color));
        }

        /// <inheritdoc/>
        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, in LineStyle style)
        {
            if (!style.IsVisible) return;

            var lstyle = Plotly.LineProperties.Create(style.Style.FillColor, diameter, style.Style.OutlineColor, style.Style.OutlineWidth);

            _Traces.Add(Plotly.TracesFactory.Lines(points, lstyle));
        }

        /// <inheritdoc/>
        public void DrawEllipse(Point2 center, float width, float height, in OutlineFillStyle style)
        {
            if (!style.IsVisible) return;

            _Markers.Add((center, (width + height) * 0.25f, style.FillColor));
        }

        /// <inheritdoc/>
        public void DrawImage(in Matrix3x2 transform, in ImageStyle style)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public void DrawPolygon(ReadOnlySpan<Point2> points, in PolygonStyle style)
        {
            if (!style.IsVisible) return;

            if (style.HasOutline)
            {
                var ls = Plotly.LineProperties.Create(style.OutlineColor, style.OutlineWidth);

                _Traces.Add(Plotly.TracesFactory.Polygon(points, style.FillColor, ls));
            }
            else
            {
                if (style.HasFill)
                {
                    _Traces.Add(Plotly.TracesFactory.Polygon(points, style.FillColor));
                }
            }
        }        

        #endregion
    }
}
