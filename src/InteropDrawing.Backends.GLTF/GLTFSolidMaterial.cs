using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using COLOR = System.Drawing.Color;

namespace InteropDrawing.Backends
{
    [System.Diagnostics.DebuggerDisplay("{Color} {DoubleSided}")]
    struct GLTFSolidMaterial : IEquatable<GLTFSolidMaterial>
    {
        public GLTFSolidMaterial(COLOR color, bool doubleSided)
        {
            Color = new Vector4(color.R, color.G, color.B, color.A) / 255f;
            DoubleSided = doubleSided;
        }

        public Vector4 Color;
        public Boolean DoubleSided;

        public bool Equals(GLTFSolidMaterial other)
        {
            if (this.Color != other.Color) return false;
            if (this.DoubleSided != other.DoubleSided) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Color.GetHashCode() ^ DoubleSided.GetHashCode();
        }

        public SharpGLTF.Materials.MaterialBuilder CreateMaterial()
        {
            return new SharpGLTF.Materials.MaterialBuilder()
                .WithMetallicRoughnessShader()
                .WithChannelParam("BaseColor", Color)
                .WithDoubleSide(DoubleSided)
                .WithAlpha(Color.W == 1 ? SharpGLTF.Materials.AlphaMode.OPAQUE : SharpGLTF.Materials.AlphaMode.BLEND);
        }        
    }
}
