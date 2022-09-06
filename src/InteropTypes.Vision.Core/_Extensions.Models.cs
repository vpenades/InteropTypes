﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InteropTypes.Vision
{
    public static partial class _Extensions
    {
        private static bool IsARM(this System.Runtime.InteropServices.Architecture arch)
        {
            if (arch == System.Runtime.InteropServices.Architecture.Arm) return true;
            if (arch == System.Runtime.InteropServices.Architecture.Arm64) return true;
            return false;
        }

        public static (string Name, Byte[] Data) ReadResourceToEnd(this IEnumerable<Assembly> assemblies, string resourceName, string sha256)
        {
            foreach(var a in assemblies)
            {
                try
                {
                    var (name,data) = a.ReadModelToEnd(resourceName, sha256);
                    if (data != null && data.Length > 0) return (name, data);
                }
                catch { }
            }

            return (null, null);
        }

        public static (string Name, Byte[] Data) ReadModelToEnd(this Assembly assembly, string name, string x86sha256 = null, string armsha256 = null)
        {
            if (System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.IsARM())
            {
                name = System.IO.Path.ChangeExtension(name, ".tflite");
                return ReadResourceToEnd(assembly, name, armsha256);
            }

            return ReadResourceToEnd(assembly, name, x86sha256);
        }

        public static (string Name, Byte[] Data) ReadResourceToEnd(this Assembly assembly, string resourceName, string sha256)
        {
            var result = ReadResourceToEnd(assembly, resourceName);

            if (sha256 != null)
            {
                var resultSha256 = result.Data.GetSha256();
                if (resultSha256 != sha256) throw new ArgumentException("SHA256 mismatch");
            }

            return result;            
        }

        public static (string Name, Byte[] Data) ReadResourceToEnd(this Assembly assembly, string resourceName)
        {
            // check in assembly resources manifest            

            var path = resourceName;

            path = path.Replace("/", ".");
            path = path.Replace("\\", ".");

            var allNames = assembly.GetManifestResourceNames();
            path = allNames.FirstOrDefault(item => item.EndsWith(path));

            if (path != null)
            {
                using (var s = assembly.GetManifestResourceStream(path))
                {
                    using (var m = new System.IO.MemoryStream())
                    {
                        s.CopyTo(m);

                        var data = m.ToArray();

                        return (path, data);
                    }
                }
            }

            // check in entry exe path

            path = resourceName;

            var exePath = System.Diagnostics.Process.GetCurrentProcess()?.MainModule?.FileName;

            if (exePath != null)
            {
                var exeDir = System.IO.Path.GetDirectoryName(exePath);

                path = System.IO.Directory
                    .GetFiles(exeDir, "*", System.IO.SearchOption.AllDirectories)
                    .FirstOrDefault(item => item.EndsWith(resourceName, StringComparison.InvariantCultureIgnoreCase));

                if (System.IO.File.Exists(path))
                {
                    var data = System.IO.File.ReadAllBytes(path);
                    return (path, data);
                }
            }

            // failed for all cases

            throw new System.IO.FileNotFoundException($"{resourceName}");
        }

        public static String GetSha256(this Byte[] array)
        {
            return new ArraySegment<Byte>(array).GetSha256();
        }

        public static String GetSha256(this ArraySegment<Byte> array)
        {
            using var calc = System.Security.Cryptography.SHA256.Create("SHA256");

            var result = calc.ComputeHash(array.Array, array.Offset, array.Count);

            return string.Join(string.Empty, result.Select(item => item.ToString("X2")));
        }

        public static String GetSha256(this IEnumerable<ArraySegment<Byte>> arrays)
        {
            using var calc = System.Security.Cryptography.SHA256.Create("SHA256");

            using var m = new System.IO.MemoryStream();

            foreach (var array in arrays)
            {
                m.Write(array.Array, array.Offset, array.Count);
            }

            m.Flush();

            m.Position = 0;

            var result = calc.ComputeHash(m);

            return string.Join(string.Empty, result.Select(item => item.ToString("X2")));
        }
    }
}
