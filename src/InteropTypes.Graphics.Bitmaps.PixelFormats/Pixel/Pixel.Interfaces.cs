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
            void Set<TPixel>(TPixel pixel) where TPixel : unmanaged;
            void SetFrom<TPixel>(ref TPixel pixel) where TPixel : unmanaged;

            TPIxel To<TPIxel>() where TPIxel : unmanaged;
            void CopyTo<TPIxel>(ref TPIxel target) where TPIxel : unmanaged;            
        }

        /// <summary>
        /// applies an operation over the given pixel
        /// </summary>
        /// <remarks>
        /// <para>
        /// Implemented by:
        /// <see cref="RGB24.MulAdd"/> 
        /// <see cref="BGR24.MulAdd"/> 
        /// <see cref="RGB96F.MulAdd"/> 
        /// <see cref="BGR96F.MulAdd"/> 
        /// </para>
        /// </remarks>
        /// <typeparam name="TPixel">the target pixel</typeparam>
        public interface IApplyTo<TPixel>
            where TPixel: unmanaged
        {
            void ApplyTo(ref TPixel target);
        }

        interface IDelegateProvider<TDelegate>
        {
            TDelegate GetDelegate();
        }

    }
}
