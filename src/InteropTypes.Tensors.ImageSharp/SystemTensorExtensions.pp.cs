using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics.Tensors;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Advanced;

#nullable disable

using __SIXLABORS = SixLabors.ImageSharp;
using __SIXLABORSPIXFMT = SixLabors.ImageSharp.PixelFormats;

using __XYZ = System.Numerics.Vector3;
using __XYZW = System.Numerics.Vector4;


#if INTEROPTYPES_USEINTEROPNAMESPACE
namespace InteropTypes.Tensors
#elif INTEROPTYPES_TENSORS_USESIXLABORSNAMESPACE
namespace SixLabors.ImageSharp
#else
namespace $rootnamespace$
#endif
{
    internal static partial class InteropTensorsForImageSharp
    {
        #if NET8_0_OR_GREATER

        #pragma warning disable SYSLIB5001

        public static void CopyTo<TPixel>(Image<TPixel> src, System.Numerics.Tensors.TensorSpan<float> dst)
            where TPixel: unmanaged, __SIXLABORSPIXFMT.IPixel<TPixel>
        {
            throw new NotImplementedException();

            var dstx = dst.Squeeze();
            if (dstx.Rank != 3) throw new ArgumentException();
            if (dstx.Lengths[2] != 3) throw new ArgumentException();            

            for (int y=0; y < src.Height; y++)
            {
                // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Numerics.Tensors/src/System/Runtime/InteropServices/TensorMarshal.cs

                

                // var srcRow = src.DangerousGetPixelRowMemory(y).Span;
                // var dstRow = dst.Slice(y); // System.Runtime.InteropServices.TensorMarshall
            }
        }

        public static void CopyTo(Image src, System.Numerics.Tensors.TensorSpan<__XYZ> dst)
        {
            
        }

        #pragma warning disable SYSLIB5001

        #endif
    }

}
