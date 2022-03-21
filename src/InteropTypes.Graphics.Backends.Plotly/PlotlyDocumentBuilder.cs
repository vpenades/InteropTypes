using System;
using System.Collections.Generic;
using System.Numerics;

using Plotly;
using Plotly.Types;

using InteropTypes.Graphics.Drawing;

using TRACES = Plotly.Box<Plotly.Types.ITracesProperty>;
using SHAPES = Plotly.Box<Plotly.Types.IShapesProperty>;

namespace InteropTypes.Graphics.Backends
{
    public class PlotlyDocumentBuilder
    {
        #region lifecycle

        public static Retyped.dom.HTMLElement ConvertToHtml(Record3D srcModel)
        {
            return ConvertToPlot(srcModel).Render();
        }

        public static Plot ConvertToPlot(Record3D srcModel)
        {
            var dst = new PlotlyDocumentBuilder();

            using (var dc = dst.CreateScene3DContext())
            {
                srcModel.DrawTo(dc);
            }

            return dst.ToPlot();
        }

        #endregion

        #region data

        private readonly List<TRACES> _Traces = new List<TRACES>();
        private readonly List<SHAPES> _Shapes = new List<SHAPES>();

        #endregion

        #region API

        public PlotlyDocumentBuilder Draw(params IDrawingBrush<IScene3D>[] drawables)
        {
            using (var dc = CreateScene3DContext())
            {
                foreach (var d in drawables) d.DrawTo(dc);
            }

            return this;
        }

        public PlotlyDocumentBuilder Draw(Matrix4x4 xform, params IDrawingBrush<IScene3D>[] drawables)
        {
            using (var dc = CreateScene3DContext())
            {
                var dcx = dc.CreateTransformed3D(xform);
                foreach (var d in drawables) d.DrawTo(dcx);
            }

            return this;
        }

        /// <summary>
        /// Creates a new <see cref="IDisposableCanvas2D"/> context optimized for data sets.
        /// </summary>
        /// <returns></returns>
        public IDisposableCanvas2D CreateTraces2DContext() { return new _PlotlyDrawing2DTracesContext(this); }

        // <summary>
        /// Creates a new <see cref="IDisposableCanvas2D"/> context optimized for vector graphics.
        /// </summary>
        /// <returns></returns>
        public IDisposableCanvas2D CreateShapes2DContext() { return new _PlotlyDrawing2DShapesContext(this); }

        /// <summary>
        /// Creates a new <see cref="IDisposableScene3D"/> context optimized for 3D Scenes.
        /// </summary>
        /// <returns></returns>
        public IDisposableScene3D CreateScene3DContext() { return new _PlotlyDrawing3DContext(this); }

        public Box<IPlotProperty> ToPlotProperties()
        {
            return Plot.traces(_Traces.ToArray());
        }
        
        public Plot ToPlot(Box<IPlotProperty> layout = null)
        {
            var plots = new List<Box<IPlotProperty>>();

            plots.Add(ToPlotProperties());            

            if (layout == null) layout = _CreateLayoutProperties();            

            plots.Add(layout);

            var document = new Plot(plots.ToArray());
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

        internal void AppendShape(SHAPES shape)
        {
            if (shape == null) return;            
            _Shapes.Add(shape);
        }

        private Box<IPlotProperty> _CreateLayoutProperties()
        {
            var xaxis = Scene.xaxis(Xaxis.color("red"));
            var yaxis = Scene.yaxis(Yaxis.color("green"));
            var zaxis = Scene.zaxis(Zaxis.color("blue"));
            var camera = Camera.up(Up.x(0), Up.y(1), Up.z(0));
            var scene = Layout.scene(Scene.Aspectmode.data(), Scene.camera(camera), xaxis, yaxis, zaxis);
            var shapes = Layout.shapes(_Shapes.ToArray());

            

            return Plot.layout
                (Layout.autosize(true)
                // , Layout.width(0)
                , Layout.height(920)
                // , Layout.margin(Margin.autoexpand(true))
                // , Layout.margin(Margin.pad(5))                    
                // , Layout.margin(Margin.t(5), Margin.b(5))                    
                , scene
                , shapes                
                );            
        }        

        #endregion
    }        
}
