using System;
using System.Collections.Generic;
using System.Text;

using LINEPROPS = Plotly.Box<Plotly.Types.ILineProperty>;
using COLOR = System.Drawing.Color;
using System.Linq;

namespace Plotly
{
    struct LineProperties
    {
        public static implicit operator LINEPROPS[](LineProperties props) { return props._GetProperties().ToArray(); }

        public static LineProperties Create(COLOR color)
        {
            var ls = new LineProperties();
            ls.FillColor = color;
            return ls;
        }

        public static LineProperties Create(COLOR color, float width)
        {
            var ls = new LineProperties();
            ls.FillColor = color;
            ls.FillWidth = width;
            return ls;
        }

        public static LineProperties Create(COLOR fc, float fw, COLOR oc, float ow)
        {
            var ls = new LineProperties();
            ls.FillColor = fc;
            ls.FillWidth = fw;
            ls.OutlierColor = oc;
            ls.OutlierWidth = ow;
            return ls;
        }

        public COLOR? FillColor;
        public float? FillWidth;
        public COLOR? OutlierColor;
        public float? OutlierWidth;
        public bool? ReversesScale;

        private static bool _IsVisible(COLOR? color, float? width)
        {
            if (color.HasValue && color.Value.A == 0) return false;
            if (width.HasValue && width.Value == 0) return false;
            return true;
        }

        public bool IsVisible
        {
            get
            {
                return _IsVisible(FillColor, FillWidth) | _IsVisible(OutlierColor, OutlierWidth);
            }
        }

        

        private IEnumerable<LINEPROPS> _GetProperties()
        {
            if (FillWidth.HasValue) yield return Line.width(FillWidth.Value);
            if (FillColor.HasValue) yield return Line.color(FillColor.Value.ToPlotlyStringRGBA());

            if (OutlierWidth.HasValue) yield return Line.outlierwidth(OutlierWidth.Value);
            if (OutlierColor.HasValue) yield return Line.outliercolor(OutlierColor.Value.ToPlotlyStringRGBA());

            if (ReversesScale.HasValue) yield return Line.reversescale(ReversesScale.Value);
        }
    }
}
