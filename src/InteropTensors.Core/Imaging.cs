using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTensors
{
    public struct TensorImageSettings
    {
        #region constructors

        public static TensorImageSettings PassThrough
        {
            get
            {
                return new TensorImageSettings
                {
                    DepthMean = 0,
                    DepthScale = 1,
                    ColorScale = new Vector3(104.0f, 177.0f, 123.0f)
                };
            }
        }

        public static TensorImageSettings Normalized
        {
            get
            {
                return new TensorImageSettings
                {
                    DepthMean = 0,
                    DepthScale = 1.0f / 255.0f,
                    ColorScale = new Vector3(104.0f, 177.0f, 123.0f)
                };
            }
        }

        public TensorImageSettings WithInputSize(int w, int h)
        {
            var clone = this;
            clone.InputSize = new System.Drawing.Size(w, h);
            return clone;
        }

        #endregion

        #region data

        public float DepthMean;
        public float DepthScale;
        public Vector3 ColorScale;

        public System.Drawing.Size InputSize;

        #endregion        
    }
}
