using System;
using System.Collections.Generic;

using Plotly;
using Plotly.Types;

using TRACES = Plotly.Box<Plotly.Types.ITracesProperty>;


namespace InteropDrawing.Backends
{
    public class PlotlyDocumentBuilder
    {
        #region lifecycle

        public static Retyped.dom.HTMLElement ConvertToHtml(Model3D srcModel)
        {
            return ConvertToPlot(srcModel).Render();
        }

        public static Plot ConvertToPlot(Model3D srcModel)
        {
            var dst = new PlotlyDocumentBuilder();

            using (var dc = dst.CreateDrawing3DContext())
            {
                srcModel.DrawTo(dc);
            }

            return dst.ToPlot();
        }

        #endregion

        #region data

        private readonly List<TRACES> _Traces = new List<TRACES>();        

        #endregion

        #region API

        public IDrawingContext3D CreateDrawing3DContext() { return new _PlotlyDrawing3DContext(this); }

        public Box<IPlotProperty> ToPlotProperties() { return Plot.traces(_Traces.ToArray()); }

        public Plot ToPlot()
        {
            var plot = ToPlotProperties();
            var layout = _CreateLayoutProperties();

            var document = new Plot(plot, layout);
            return document;
        }

        public Retyped.dom.HTMLElement ToHtml()
        {
            Plot document = ToPlot();
            return document.Render();
        }

        public void SaveHtml(string filePath)
        {
            var htmlBody = ToHtml().ToString();
            System.IO.File.WriteAllText(filePath, htmlBody);
        }

        #endregion

        #region core

        internal void AppendTrace(TRACES trace)
        {
            if (trace == null) return;
            _Traces.Add(trace);
        }

        private static Box<IPlotProperty> _CreateLayoutProperties()
        {
            var xaxis = Scene.xaxis(Xaxis.color("red"));
            var yaxis = Scene.yaxis(Yaxis.color("green"));
            var zaxis = Scene.zaxis(Zaxis.color("blue"));
            var camera = Camera.up(Up.x(0), Up.y(1), Up.z(0));
            var scene = Layout.scene(Scene.Aspectmode.data(), Scene.camera(camera), xaxis, yaxis, zaxis);

            return Plot.layout
                (Layout.autosize(true)
                // , Layout.width(0)
                , Layout.height(920)
                // , Layout.margin(Margin.autoexpand(true))
                // , Layout.margin(Margin.pad(5))                    
                // , Layout.margin(Margin.t(5), Margin.b(5))                    
                , scene
                );
        }        

        #endregion
    }        
}
