using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace InteropTypes.Graphics.Backends.SilkGL
{
    internal static class _Extensions
    {
        [System.Diagnostics.DebuggerStepThrough]
        public static void ThrowOnError(this Silk.NET.OpenGL.GL context)
        {
            var error = context.GetError();

            // Silk.NET.OpenGL.ErrorCode;

            if (error != Silk.NET.OpenGL.GLEnum.NoError)
            {
                throw new InvalidOperationException("GL.GetError() returned " + error.ToString());
            }                
        }

        public static string ReadAllText(this System.Reflection.Assembly assembly, string name)
        {
            var id = assembly.GetManifestResourceNames().FirstOrDefault(item => item.EndsWith(name));
            if (id == null) return null;

            using(var s = assembly.GetManifestResourceStream(id))
            {
                using(var t = new StreamReader(s))
                {
                    return t.ReadToEnd();
                }
            }
        }
    }
}
