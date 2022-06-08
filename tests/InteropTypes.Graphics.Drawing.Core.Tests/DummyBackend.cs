using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace InteropTypes.Graphics.Drawing
{
    internal class DummyBackend2D : Record2D, IRenderTargetInfo, IServiceProvider
    {
        public DummyBackend2D(int w, int h)
        {
            PixelsWidth = w;
            PixelsHeight = h;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IRenderTargetInfo)) return this;
            return null;
        }

        public int PixelsWidth { get; set; } = 100;

        public int PixelsHeight { get; set; } = 100;

        public float DotsPerInchX { get; set; } = 76;

        public float DotsPerInchY { get; set; } = 76;

        
    }

    internal class DummyBackend3D : Record3D
    {
        public DummyBackend3D()
        {
            
        }
    }
}
