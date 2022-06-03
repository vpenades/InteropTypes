using System;
using System.Collections.Generic;
using System.Numerics.Tensors;
using System.Text;

namespace InteropTypes.Tensors
{
    /// <summary>
    /// <see cref="IDenseTensor{T}"/> implementation over <see cref="System.Numerics.Tensors.DenseTensor{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// <see href="https://devblogs.microsoft.com/dotnet/introducing-tensor-for-multi-dimensional-machine-learning-and-ai-data/">DevBlogs - introducing tensors</see><br/>
    /// <seealso href="https://github.com/dotnet/runtime/tree/main/src/libraries/System.Numerics.Tensors">DotNet - System.Numerics.Tensors</seealso><br/>
    /// <seealso href="https://github.com/dotnet/runtime/labels/area-System.Numerics.Tensors">DotNet - Issues</seealso>
    /// </remarks>
    public readonly struct SystemDenseTensor<T> : IDenseTensor<T> , IEquatable<SystemDenseTensor<T>>
        where T : unmanaged
    {
        #region constructor
        public SystemDenseTensor(DenseTensor<T> tensor, string name)
        {
            _Tensor = tensor;
            _Name = name;
        }

        #endregion

        #region data

        private readonly DenseTensor<T> _Tensor;
        private readonly string _Name;

        /// <inheritdoc/>
        public readonly override int GetHashCode()
        {
            var h = _Tensor?.GetHashCode() ?? 0;
            h ^= _Name?.GetHashCode() ?? 0;
            return h;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is SystemDenseTensor<T> other && this.Equals(other);
        }

        /// <inheritdoc/>
        public readonly bool Equals(SystemDenseTensor<T> other)
        {
            if (this._Tensor != other._Tensor) return false;
            if (this._Name != other._Name) return false;
            return true;
        }

        /// <inheritdoc/>
        public static bool operator ==(SystemDenseTensor<T> a, SystemDenseTensor<T> b) => a.Equals(b);

        /// <inheritdoc/>
        public static bool operator !=(SystemDenseTensor<T> a, SystemDenseTensor<T> b) => !a.Equals(b);

        #endregion

        #region properties

        public readonly string Name => _Name;

        public readonly ReadOnlySpan<int> Dimensions => _Tensor.Dimensions;

        public readonly IntPtr DataPointer => throw new NotSupportedException();

        public readonly Span<T> Span => _Tensor.Buffer.Span;        

        #endregion
    }

    partial class SpanTensor
    {
        public static void Sum(Tensor<float> left, Tensor<float> right, ref Tensor<float> result)
        {
            if (!left.Dimensions.SequenceEqual(right.Dimensions)) throw new ArgumentException("incompatible dimensions", nameof(right));

            if (result.Dimensions.SequenceEqual(left.Dimensions)) result = null;

            if (left is DenseTensor<float> denseLeft && right is DenseTensor<float> denseRight && result is DenseTensor<float> denseResult)
            {
                if (result == null) result = new DenseTensor<float>(left.Dimensions);

                _ArrayUtilities.VectorSum(denseLeft.Buffer.Span, denseRight.Buffer.Span, denseResult.Buffer.Span);
                return;
            }

            if (result == null)
            {
                result = left is SparseTensor<float> && right is SparseTensor<float>
                    ? new SparseTensor<float>(left.Dimensions)
                    : (Tensor<float>)new DenseTensor<float>(left.Dimensions);
            }

            var count = left.DimensionsDotProduct();

            for (int i = 0; i < count; ++i)
            {
                var lv = left.GetValue(i);
                var rv = right.GetValue(i);
                result.SetValue(i, lv + rv);
            }
        }

        // OrdinalMultiply, MatrixMultiply  

        [Obsolete("Use SystemDenseTensor wrapper")]
        public static SpanTensor1<T> AsSpanTensor1<T>(this DenseTensor<T> tensor)
            where T : unmanaged, IEquatable<T>
        {
            return new SpanTensor1<T>(tensor.Buffer.Span, tensor.Dimensions);
        }

        [Obsolete("Use SystemDenseTensor wrapper")]
        public static SpanTensor2<T> AsSpanTensor2<T>(this DenseTensor<T> tensor)
            where T : unmanaged, IEquatable<T>
        {
            return new SpanTensor2<T>(tensor.Buffer.Span, tensor.Dimensions);
        }

        [Obsolete("Use SystemDenseTensor wrapper")]
        public static SpanTensor3<T> AsSpanTensor3<T>(this DenseTensor<T> tensor)
            where T : unmanaged, IEquatable<T>
        {
            return new SpanTensor3<T>(tensor.Buffer.Span, tensor.Dimensions);
        }

        [Obsolete("Use SystemDenseTensor wrapper")]
        public static SpanTensor4<T> AsSpanTensor4<T>(this DenseTensor<T> tensor)
            where T : unmanaged, IEquatable<T>
        {
            return new SpanTensor4<T>(tensor.Buffer.Span, tensor.Dimensions);
        }
    }
}
