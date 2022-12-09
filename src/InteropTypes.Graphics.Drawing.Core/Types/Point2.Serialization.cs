using System;
using System.Collections.Generic;
using System.Text;

// https://docs.microsoft.com/es-es/dotnet/api/system.runtime.serialization.iserializable?view=net-6.0

namespace InteropTypes.Graphics.Drawing
{
    partial struct Point2
    {
        /// <inheritdoc/>
        public override string ToString() { return XY.ToString(); }

        /// <inheritdoc/>
        public string ToString(string format, IFormatProvider formatProvider) { return XY.ToString(format, formatProvider); }
    }
}
