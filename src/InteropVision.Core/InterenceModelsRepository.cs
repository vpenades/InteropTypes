using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InteropVision
{
    /// <summary>
    /// WIP
    /// </summary>
    static class InterenceModelsRepository
    {
        private static readonly List<ModelLoader> _Readers = new List<ModelLoader>();

        public abstract class ModelLoader
        {
            public abstract (String Name, Byte[] Data) TryLoad(string name, string sha256);
        }

        private sealed class _AssemblyModelLoader : ModelLoader
        {
            public _AssemblyModelLoader(System.Reflection.Assembly src) { Source = src; }
            public System.Reflection.Assembly Source { get; }

            public override (string Name, byte[] Data) TryLoad(string name, string sha256)
            {
                return _TryLoadFromResource(name, sha256);
            }

            private (string Name, byte[] Data) _TryLoadFromResource(string name, string sha256)
            {
                try
                {
                    return Source.ReadResourceToEnd(name, sha256);
                }
                catch
                {
                    return (null, null);
                }
            }
        }

        public static void RegisterReader(System.Reflection.Assembly assembly)
        {
            var exists = _Readers
                .OfType<_AssemblyModelLoader>()
                .Any(item => item.Source == assembly);

            if (exists) return;

            var r = new _AssemblyModelLoader(assembly);

            _Readers.Add(r);
        }        

        public static (string Name, Byte[] Data) ReadModel(string name, string sha256 = null)
        {
            foreach(var reader in _Readers)
            {
                try
                {
                    var result = reader.TryLoad(name, sha256);
                    if (result.Data.Length > 0) return result;
                }
                catch { }
            }

            return (null, null);
        }
    }
}
