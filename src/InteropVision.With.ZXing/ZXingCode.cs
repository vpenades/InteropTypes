using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using InteropDrawing;

namespace InteropVision.With
{
    [System.Diagnostics.DebuggerDisplay("{ToString()}")]
    public partial class ZXingCode : IDrawable2D
    {
        #region data

        public string   CaptureDevice { get; internal set; }
        public DateTime CaptureTime { get; internal set; }

        private Rectangle? _ResultRect;
        private readonly List<ZXing.Result> _Results = new List<ZXing.Result>();

        #endregion

        #region properties

        public Point2 Offset => _ResultRect?.Location ?? Point.Empty;

        public IReadOnlyList<ZXing.Result> Results => _Results;

        #endregion

        #region API

        public void DrawTo(IDrawing2D dc)
        {
            if (this.Results == null) return;

            var o = Offset;

            foreach (var r in this.Results) DrawTo(dc, r, o);
        }

        private static void DrawTo(IDrawing2D dc, ZXing.Result result, Point2 offset)
        {
            if (result == null) return;
            if (result.ResultPoints == null || result.ResultPoints.Length == 0) return;

            /* TODO: the ResultPoints interpretation depends on BarcodeFormat
            switch (Result.BarcodeFormat)
            {
                case ZXing.BarcodeFormat.AZTEC:                    
            }*/

            var points = result.ResultPoints
                .Select(item => new Point2(item.X, item.Y) + offset)
                .ToArray();

            dc.DrawPolygon((Color.Red, 4), points);

            var center = Point2.Centroid(points);

            dc.DrawFont(center, 0.4f, result?.Text ?? string.Empty, Color.Red);
        }

        public override string ToString()
        {
            return Results.FirstOrDefault()?.Text ?? string.Empty;
        }

        #endregion
    }
}
