﻿using System.Numerics;

using ShaderGen;

[assembly: ShaderSet("SolidShader", "InteropTypes.Graphics.Backends.VeldridShaders.SolidShader.VS", "InteropTypes.Graphics.Backends.VeldridShaders.SolidShader.FS_Solid")]

namespace InteropTypes.Graphics.Backends.VeldridShaders
{
    public class SolidShader
    {
        public Matrix4x4 XformView;
        public Matrix4x4 XformProj;

        public struct VertexInput
        {
            [PositionSemantic]
            public Vector3 Position;
            [NormalSemantic]
            public Vector3 Normal;
            [ColorSemantic]
            public Vector4 Color;
        }

        [VertexShader]
        public FragmentInput VS(VertexInput input)
        {
            FragmentInput output;

            Vector4 p = ShaderBuiltins.Mul(XformView, new Vector4(input.Position, 1));
            Vector4 n = ShaderBuiltins.Mul(XformView, new Vector4(input.Normal, 0));

            output.RelativePosition = p.XYZ();
            output.RelativeNormal = ShaderBuiltins.Sqrt(n.XYZ());

            output.Projected = ShaderBuiltins.Mul(XformProj, p);            

            output.Color = input.Color;            
            return output;
        }

        public struct FragmentInput
        {
            [SystemPositionSemantic]
            public Vector4 Projected;

            [PositionSemantic]
            public Vector3 RelativePosition;

            [NormalSemantic]
            public Vector3 RelativeNormal;

            [ColorSemantic]
            public Vector4 Color;
        }

        [FragmentShader]
        public Vector4 FS_Solid(FragmentInput input)
        {   
            var n = ShaderBuiltins.Sqrt(input.RelativeNormal);

            return new Vector4(input.Color.XYZ() * n.Z, input.Color.Z);            
        }            
    }
}