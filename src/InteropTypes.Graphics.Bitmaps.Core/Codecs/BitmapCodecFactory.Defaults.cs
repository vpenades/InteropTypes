using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace InteropTypes.Codecs
{
    partial class BitmapCodecFactory
    {
        // https://docs.microsoft.com/en-us/dotnet/core/deploying/trimming/prepare-libraries-for-trimming#dynamicdependency

        private static Type[] _Decoders;
        private static Type[] _Encoders;

        private static IEnumerable<string> _GetDefaultCodecAssemblies()
        {
            var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
            var isLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);

            #if DEBUG
            yield return "Dummy.Assembly";
            #endif

            if (isWindows)
            {
                yield return "InteropTypes.Graphics.Backends.GDI";
                yield return "InteropTypes.Graphics.Backends.WPF";
            }

            yield return "InteropTypes.Graphics.Backends.SkiaSharp";
            yield return "InteropTypes.Graphics.Backends.ImageSharp";            
            yield return "InteropTypes.Codecs.STB";
        }
        
        public static IEnumerable<IBitmapEncoder> GetDefaultEncoders()
        {
            if (_Encoders == null) _Encoders = _GetExternalTypes<IBitmapEncoder>().ToArray();

            foreach (var t in _Encoders)
            {
                var instance = Activator.CreateInstance(t, true) as IBitmapEncoder;
                if (instance != null) yield return instance;
            }

            yield return new _InBuiltCodec();
        }
        
        public static IEnumerable<IBitmapDecoder> GetDefaultDecoders()
        {
            if (_Decoders == null) _Decoders = _GetExternalTypes<IBitmapDecoder>().ToArray();

            foreach(var t in _Decoders)
            {
                if (Activator.CreateInstance(t, true) is IBitmapDecoder instance) yield return instance;
            }

            yield return new _InBuiltCodec();
        }
        
        private static IEnumerable<Type> _GetExternalTypes<T>()
        {
            #pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
            return _GetDefaultCodecAssemblies().SelectMany(name => _FindInterfaceInstances(name, typeof(T).Name));
            #pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
        }

        #if NET5_0_OR_GREATER
        [RequiresUnreferencedCode("Calls System.Reflection.Assembly.GetTypes()")]
        #endif
        private static IEnumerable<Type> _FindInterfaceInstances(string assemblyName, string interfaceName)
        {
            /*
            var assembly = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SingleOrDefault(assembly => assembly.GetName().Name == assemblyName);
                    */

            try
            {
                var assembly = System.Reflection.Assembly.Load(assemblyName);

                if (assembly == null) return Enumerable.Empty<Type>();

                return assembly
                    .GetTypes()
                    .Where(t => t.GetInterface(interfaceName) != null);
            }
            catch (System.IO.FileNotFoundException) { return Enumerable.Empty<Type>(); }
            catch (System.IO.FileLoadException) { return Enumerable.Empty<Type>(); }
            catch (BadImageFormatException) { return Enumerable.Empty<Type>(); }
        }
    }
}
