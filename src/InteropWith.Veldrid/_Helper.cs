using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Veldrid;

namespace InteropWith
{
    internal static class VeldridHelper
    {
        public static Shader LoadShader(ResourceFactory factory, string set, ShaderStages stage, string entryPoint)
        {
            string name = $"{set}-{stage.ToString().ToLower()}.{GetExtension(factory.BackendType)}";
            return factory.CreateShader(new ShaderDescription(stage, ReadEmbeddedAssetBytes(name), entryPoint));
        }

        public static Stream OpenEmbeddedAssetStream(string name, Type t) => t.Assembly.GetManifestResourceStream(name);

        public static byte[] ReadEmbeddedAssetBytes(string name)
        {
            var path = System.IO.Path.Combine("Shaders", name);
            path = path.Substring(0, path.Length - ".bytes".Length);
            
            if (System.IO.File.Exists(path)) return System.IO.File.ReadAllBytes(path);

            var info = typeof(VeldridHelper).Assembly.GetManifestResourceInfo(name);

            using (MemoryStream ms = new MemoryStream())
            {
                using (var stream = OpenEmbeddedAssetStream(name, typeof(VeldridHelper)))
                {
                    stream.CopyTo(ms);
                }

                return ms.ToArray();
            }
        }

        private static string GetExtension(this GraphicsBackend backendType)
        {
            return (backendType == GraphicsBackend.Direct3D11)
                ? "hlsl.bytes"
                : (backendType == GraphicsBackend.Vulkan)
                    ? "450.glsl.spv"
                    : (backendType == GraphicsBackend.Metal)
                        ? "ios.metallib"
                        : (backendType == GraphicsBackend.OpenGL)
                            ? "330.glsl"
                            : "300.glsles";
        }
    }
}
