using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial.Shaders
{
    internal static class _Utils
    {
        public static string ReadAllText(this System.Reflection.Assembly assembly, string name)
        {
            var id = assembly.GetManifestResourceNames().FirstOrDefault(item => item.EndsWith(name));
            if (id == null) return null;

            using (var s = assembly.GetManifestResourceStream(id))
            {
                using (var t = new StreamReader(s))
                {
                    return t.ReadToEnd();
                }
            }
        }
    }
}
