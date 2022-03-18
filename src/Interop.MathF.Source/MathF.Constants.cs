// Taken from: https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/MathF.cs
// And modified to remove intrinsics and SIMD support.

namespace System
{    
    /// <summary>
    /// Emulates System.MathF on platforms that don't have native support.
    /// </summary>
    internal static partial class MathF
    {
        public const float E = 2.71828183f;

        public const float PI = 3.14159265f;

        public const float Tau = 6.283185307f;        
    }
}
