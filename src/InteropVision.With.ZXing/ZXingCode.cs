using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using InteropTypes.Graphics.Drawing;

namespace InteropVision.With
{
    [System.Diagnostics.DebuggerDisplay("{ToString()}")]
    public partial class ZXingCode
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

        public override string ToString()
        {
            return Results.FirstOrDefault()?.Text ?? string.Empty;
        }

        #endregion
    }
}
