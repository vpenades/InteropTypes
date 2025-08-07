using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Backends
{
    public interface IMonoGameCanvas2D :
        IRenderTargetInfo,
        GlobalStyle.ISource,
        IDisposableCanvas2D,
        IMeshCanvas2D,
        ITransformer2D,
        IServiceProvider
    {
        void Begin(int virtualWidth, int virtualHeight, bool keepAspect);
        void SetCamera(System.Numerics.Matrix3x2 camera);
        void End();
    }

    public interface IMonoGameScene3D :
        IRenderTargetInfo,
        GlobalStyle.ISource,
        IDisposableScene3D,
        // IMeshScene3D,
        IServiceProvider
    {
        void Clear();
        void SetCamera(CameraTransform3D camera);
        void Render();
    }
}
