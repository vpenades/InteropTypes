using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Backends
{
    partial class MeshBuilder : IRenderTargetInfo, IServiceProvider
    {
        #region data

        /// <summary>
        /// This is the "virtual to physical" matrix which includes the axis direction
        /// </summary>
        private Matrix3x2 _ScreenMatrix = Matrix3x2.Identity;

        private IRenderTargetInfo _ParentInfo;

        #endregion

        #region properties

        public int PixelsWidth => _ParentInfo?.PixelsWidth ?? 0;
        public int PixelsHeight => _ParentInfo?.PixelsHeight ?? 0;
        public float DotsPerInchX => _ParentInfo?.DotsPerInchX ?? 0;
        public float DotsPerInchY => _ParentInfo?.DotsPerInchY ?? 0;

        #endregion

        #region API

        public void SetRenderTargetInfo(IRenderTargetInfo info, in Matrix3x2 screenMatrix)
        {
            _ParentInfo = info;
            _ScreenMatrix = screenMatrix;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(Matrix3x2)) return _ScreenMatrix;
            if (serviceType == typeof(IRenderTargetInfo)) return this;
            return null;
        }

        #endregion
    }
}
