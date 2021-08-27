using System.Numerics;

// using ShaderGen;

// [assembly: ShaderSet("SolidShader", "InteropWith.VeldridShaders.SolidShader.VS", "InteropWith.VeldridShaders.SolidShader.FS_Solid")]

namespace InteropWith.VeldridShaders
{
    public class SolidShader
    {
        public static Veldrid.ShaderSetDescription CreateShader(Veldrid.ResourceFactory factory)
        {
            var vertexShader = VeldridHelper.LoadShader(factory, "SolidShader", Veldrid.ShaderStages.Vertex, "VS");
            var fragmentShader = VeldridHelper.LoadShader(factory, "SolidShader", Veldrid.ShaderStages.Fragment, "FS_Solid");

            return new Veldrid.ShaderSetDescription(
                new[] { Vertex3D.GetDescription() },
                new[] { vertexShader, fragmentShader });
        }
    }
}