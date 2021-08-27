﻿using System.Numerics;

using ShaderGen;

[assembly: ShaderSet("SpriteShaderSolid", "InteropWith.VeldridShaders.SpriteShader.VS", "InteropWith.VeldridShaders.SpriteShader.FS_Solid")]
[assembly: ShaderSet("SpriteShaderTextured", "InteropWith.VeldridShaders.SpriteShader.VS", "InteropWith.VeldridShaders.SpriteShader.FS_Textured")]

namespace InteropWith.VeldridShaders
{
    public class SpriteShader
    {
        public Matrix4x4 WorldViewProjection;
        public Texture2DResource InputTexture;
        public SamplerResource InputSampler;

        [VertexShader]
        public FragmentInput VS(VertexInput input)
        {
            FragmentInput output;
            output.Position = ShaderBuiltins.Mul(WorldViewProjection, new Vector4(input.Position, 1));
            output.Color = input.Color;
            output.TextureCoordinates = input.TextureCoordinates;
            return output;
        }

        [FragmentShader]
        public Vector4 FS_Solid(FragmentInput input)
        {            
            return input.Color;
        }

        [FragmentShader]
        public Vector4 FS_Textured(FragmentInput input)
        {
            Vector2 texCoords = input.TextureCoordinates;
            Vector4 inputColor = ShaderBuiltins.Sample(InputTexture, InputSampler, texCoords);
            return inputColor * input.Color;
        }

        public struct VertexInput
        {
            [PositionSemantic]
            public Vector3 Position;
            [ColorSemantic]
            public Vector4 Color;
            [TextureCoordinateSemantic]
            public Vector2 TextureCoordinates;
        }

        public struct FragmentInput
        {
            [SystemPositionSemantic]
            public Vector4 Position;
            [ColorSemantic]
            public Vector4 Color;
            [TextureCoordinateSemantic]
            public Vector2 TextureCoordinates;
        }        
    }
}