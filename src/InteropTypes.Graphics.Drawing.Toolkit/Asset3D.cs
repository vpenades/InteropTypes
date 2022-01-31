using System;
using System.Collections.Generic;
using System.Text;

using COLOR = System.Drawing.Color;

namespace InteropDrawing
{
    public abstract class Asset3D
    {
        public Object PrimaryAsset { get; protected set; }

        public Object FallbackAsset { get; protected set; }

        protected abstract void DrawAsSurfaces(IScene3D target);

        protected abstract void DrawAsPrimitives(IScene3D target);

        internal void _DrawAsSurfaces(IScene3D target)
        {
            if (this.PrimaryAsset is Record3D md3d)
            {
                target.DrawAssetAsSurfaces(this.PrimaryAsset, COLOR.White);
                return;
            }

            if (this.FallbackAsset != null)
            {
                target.DrawAssetAsSurfaces(this.FallbackAsset, COLOR.White);
                return;
            }

            this.DrawAsSurfaces(target);
        }

        internal void _DrawAsPrimitives(IScene3D target)
        {
            if (this.PrimaryAsset is Record3D md3d)
            {
                target.DrawAssetAsPrimitives(this.PrimaryAsset, COLOR.White);
                return;
            }

            if (this.FallbackAsset != null)
            {
                target.DrawAssetAsPrimitives(this.FallbackAsset, COLOR.White);
                return;
            }

            this.DrawAsPrimitives(target);
        }
    }
}
