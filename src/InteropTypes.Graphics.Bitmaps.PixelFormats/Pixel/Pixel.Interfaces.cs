using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {
        public interface IReflection
        {
            /// <summary>
            /// Gets the pixel format code, like <see cref="RGBA32.Code"/>
            /// </summary>
            /// <returns></returns>
            uint GetCode();
            PixelFormat GetPixelFormat();
            bool IsOpaque { get; }
            bool IsPremultiplied { get; }
            bool IsQuantized { get; }
        }

        public interface IValueSetter<T> where T : unmanaged
        {
            void SetValue(T value);
        }

        public interface IReferenceSetter<T> where T : unmanaged
        {
            void SetValue(in T value);
        }

        /// <summary>
        /// interface to allow converting the current pixel type to another pixel type.
        /// </summary>
        /// <remarks>
        /// Implementation is usually done as a sequence of entries like:
        /// <code>if (typeof(TPixel) == typeof(RGBA32) {... }</code>
        /// which are expected to be optimized out by the compiler when it untangles the generic code:<br/>
        /// <list type="bullet">
        /// <item> <see href="https://stackoverflow.com/a/51723774">performance tests</see> </item>
        /// <item> <see href="https://forum.unity.com/threads/il2cpp-proposal-replace-typeof-t-typeof-struct-type-il-sequence-with-constant-boolean-value.986313/">Unity seems to be lagging behind (again)</see> </item>
        /// <item> <see href="https://github.com/dotnet/csharplang/discussions/356">roslyn: switch-case on types</see> </item>
        /// </list>        
        /// </remarks>        
        public interface IConvertTo
        {
            TPIxel To<TPIxel>() where TPIxel : unmanaged;
            void CopyTo<TPIxel>(ref TPIxel target) where TPIxel : unmanaged;            
        }        

        interface IDelegateProvider<TDelegate>
        {
            TDelegate GetDelegate();
        }

    }
}
