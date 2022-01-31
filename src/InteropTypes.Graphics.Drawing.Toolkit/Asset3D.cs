using System;
using System.Collections.Generic;
using System.Text;

using COLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Drawing
{
    public abstract class Asset3D
    {
        public object PrimaryAsset { get; protected set; }

        public object FallbackAsset { get; protected set; }

        protected abstract void DrawAsSurfaces(IScene3D target);

        protected abstract void DrawAsPrimitives(IScene3D target);

        internal void _DrawAsSurfaces(IScene3D target)
        {
            if (PrimaryAsset is Record3D md3d)
            {
                target.DrawAssetAsSurfaces(PrimaryAsset, COLOR.White);
                return;
            }

            if (FallbackAsset != null)
            {
                target.DrawAssetAsSurfaces(FallbackAsset, COLOR.White);
                return;
            }

            DrawAsSurfaces(target);
        }

        internal void _DrawAsPrimitives(IScene3D target)
        {
            if (PrimaryAsset is Record3D md3d)
            {
                target.DrawAssetAsPrimitives(PrimaryAsset, COLOR.White);
                return;
            }

            if (FallbackAsset != null)
            {
                target.DrawAssetAsPrimitives(FallbackAsset, COLOR.White);
                return;
            }

            DrawAsPrimitives(target);
        }
    }
}
