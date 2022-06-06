using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Vision.Backends
{
    partial class ZXingCode : IDrawingBrush<ICanvas2D>
    {
        #region API

        public void DrawTo(ICanvas2D dc)
        {
            if (this.Results == null) return;

            var o = Offset;

            foreach (var r in this.Results) DrawTo(dc, r, o);
        }

        private static void DrawTo(ICanvas2D dc, ZXing.Result result, Point2 offset)
        {
            if (result == null) return;
            if (result.ResultPoints == null || result.ResultPoints.Length == 0) return;

            /* TODO: the ResultPoints interpretation depends on BarcodeFormat
            switch (Result.BarcodeFormat)
            {
                case ZXing.BarcodeFormat.AZTEC:                    
            }*/

            var points = result.ResultPoints
                .Select(item => (Point2)(new Point2(item.X, item.Y) + offset))
                .ToArray();

            dc.DrawPolygon((Color.Red, 4), points);

            var center = Point2.Centroid(points);

            dc.DrawTextLine(center, result.Text, 20, Color.Red);
        }        

        #endregion
    }
}
