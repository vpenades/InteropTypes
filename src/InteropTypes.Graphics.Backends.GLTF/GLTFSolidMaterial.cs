using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using COLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Backends
{
    [System.Diagnostics.DebuggerDisplay("{Color} {DoubleSided}")]
    struct GltfSolidMaterial : IEquatable<GltfSolidMaterial>
    {
        public GltfSolidMaterial(COLOR color, bool doubleSided)
        {
            Color = new Vector4(color.R, color.G, color.B, color.A) / 255f;
            DoubleSided = doubleSided;
        }

        public Vector4 Color;
        public bool DoubleSided;

        public bool Equals(GltfSolidMaterial other)
        {
            if (Color != other.Color) return false;
            if (DoubleSided != other.DoubleSided) return false;
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
                .WithChannelParam(SharpGLTF.Materials.KnownChannel.BaseColor,SharpGLTF.Materials.KnownProperty.RGBA, Color)
                .WithDoubleSide(DoubleSided)
                .WithAlpha(Color.W == 1 ? SharpGLTF.Materials.AlphaMode.OPAQUE : SharpGLTF.Materials.AlphaMode.BLEND);
        }
    }
}
