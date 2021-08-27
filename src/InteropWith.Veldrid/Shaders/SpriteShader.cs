using System.Numerics;

// using ShaderGen;

// [assembly: ShaderSet("SpriteShaderSolid", "InteropWith.VeldridShaders.SpriteShader.VS", "InteropWith.VeldridShaders.SpriteShader.FS_Solid")]
// [assembly: ShaderSet("SpriteShaderTextured", "InteropWith.VeldridShaders.SpriteShader.VS", "InteropWith.VeldridShaders.SpriteShader.FS_Textured")]

namespace InteropWith.VeldridShaders
{
    public class SpriteShader
    {
        public static Veldrid.ShaderSetDescription CreateShaderSolid(Veldrid.ResourceFactory factory)
        {
            var vertexShader = VeldridHelper.LoadShader(factory, "SpriteShaderSolid", Veldrid.ShaderStages.Vertex, "VS");
            var fragmentShader = VeldridHelper.LoadShader(factory, "SpriteShaderSolid", Veldrid.ShaderStages.Fragment, "FS_Solid");

            return new Veldrid.ShaderSetDescription(
                new[] { Vertex2D.GetDescription() },
                new[] { vertexShader, fragmentShader });
        }

        public static Veldrid.ShaderSetDescription CreateShaderTextured(Veldrid.ResourceFactory factory)
        {
            var vertexShader = VeldridHelper.LoadShader(factory, "SpriteShaderTextured", Veldrid.ShaderStages.Vertex, "VS");
            var fragmentShader = VeldridHelper.LoadShader(factory, "SpriteShaderTextured", Veldrid.ShaderStages.Fragment, "FS_Textured");

            return new Veldrid.ShaderSetDescription(
                new[] { Vertex2D.GetDescription() },
                new[] { vertexShader, fragmentShader });
        }
    }
}