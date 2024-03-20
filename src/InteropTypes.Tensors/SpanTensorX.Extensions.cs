﻿// <auto-generated />
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Numerics.Tensors;
using System.Text;

namespace InteropTypes.Tensors
{
    public static partial class SpanTensor
    {
        
        public static void ApplySoftMax(this SpanTensor1<float> tensor)
        {
            SpanTensor.ApplySoftMax(tensor.Span);
        }

        
        public static int IndexOf<T>(this SpanTensor1<T> tensor, T value)
        where T : unmanaged, IEquatable<T>
        {
            return tensor._Buffer.IndexOf(value);
        }

        public static int IndexOf<T>(this SpanTensor1<T> tensor, Predicate<T> predicate)
        where T : unmanaged, IEquatable<T>
        {
            var span = tensor._Buffer;
            for (int i = 0; i < span.Length; ++i)
            {
                if (predicate(span[i])) return i;
            }

            return -1;
        }

        public static int IndexOfMax<T>(this SpanTensor1<T> tensor, IComparer<T> comparer = null)
        where T : unmanaged
        {
            var span = tensor._Buffer;

            if (span.IsEmpty) return -1;

            if (comparer == null) comparer = Comparer<T>.Default;

            T max = span[0];
            var idx = 0;

            for (int i = 1; i < span.Length; ++i)
            {
                var item = span[i];
                if (comparer.Compare(max, item) > 0) { max = item; idx = i; }
            }

            return idx;
        }

                

    
        public static void ApplySoftMax(this SpanTensor2<float> tensor)
        {
            SpanTensor.ApplySoftMax(tensor.Span);
        }

                
        public static TensorIndices2 IndexOf<T>(this SpanTensor2<T> tensor, T value)
        where T : unmanaged, IEquatable<T>
        {
            var idx = tensor._Buffer.IndexOf(value);
            if (idx < 0) return TensorSize2.Invalid;
            return tensor._Dimensions.GetDecomposedIndex(idx);
        }

        
        public static TensorIndices2 IndexOf<T>(this SpanTensor2<T> tensor, Predicate<T> predicate)
        where T : unmanaged, IEquatable<T>
        {
            var span = tensor._Buffer;

            for (int i = 0; i < span.Length; ++i)
            {
                if (predicate(span[i])) return tensor._Dimensions.GetDecomposedIndex(i);
            }

            return TensorIndices2.Invalid;
        }

        
        public static TensorIndices2 IndexOfMax<T>(this SpanTensor2<T> tensor, IComparer<T> comparer = null)
        where T : unmanaged
        {
            var span = tensor._Buffer;            

            if (typeof(T) == typeof(float))
            {
                var floats = System.Runtime.InteropServices.MemoryMarshal.Cast<T,float>(span);
                var fidx = System.Numerics.Tensors.TensorPrimitives.IndexOfMax(floats);
                return fidx < 0 ? TensorIndices2.Invalid : tensor._Dimensions.GetDecomposedIndex(fidx);
            }

            if (span.IsEmpty) return TensorIndices2.Invalid;

            if (comparer == null) comparer = Comparer<T>.Default;

            T max = span[0];
            var idx = 0;

            for (int i = 1; i < span.Length; ++i)
            {
                var item = span[i];
                if (comparer.Compare(max , item) >  0) { max = item; idx = i; }
            }

            return tensor._Dimensions.GetDecomposedIndex(idx);
        }        

                

    
        public static void ApplySoftMax(this SpanTensor3<float> tensor)
        {
            SpanTensor.ApplySoftMax(tensor.Span);
        }

                
        public static TensorIndices3 IndexOf<T>(this SpanTensor3<T> tensor, T value)
        where T : unmanaged, IEquatable<T>
        {
            var idx = tensor._Buffer.IndexOf(value);
            if (idx < 0) return TensorSize3.Invalid;
            return tensor._Dimensions.GetDecomposedIndex(idx);
        }

        
        public static TensorIndices3 IndexOf<T>(this SpanTensor3<T> tensor, Predicate<T> predicate)
        where T : unmanaged, IEquatable<T>
        {
            var span = tensor._Buffer;

            for (int i = 0; i < span.Length; ++i)
            {
                if (predicate(span[i])) return tensor._Dimensions.GetDecomposedIndex(i);
            }

            return TensorIndices3.Invalid;
        }

        
        public static TensorIndices3 IndexOfMax<T>(this SpanTensor3<T> tensor, IComparer<T> comparer = null)
        where T : unmanaged
        {
            var span = tensor._Buffer;            

            if (typeof(T) == typeof(float))
            {
                var floats = System.Runtime.InteropServices.MemoryMarshal.Cast<T,float>(span);
                var fidx = System.Numerics.Tensors.TensorPrimitives.IndexOfMax(floats);
                return fidx < 0 ? TensorIndices3.Invalid : tensor._Dimensions.GetDecomposedIndex(fidx);
            }

            if (span.IsEmpty) return TensorIndices3.Invalid;

            if (comparer == null) comparer = Comparer<T>.Default;

            T max = span[0];
            var idx = 0;

            for (int i = 1; i < span.Length; ++i)
            {
                var item = span[i];
                if (comparer.Compare(max , item) >  0) { max = item; idx = i; }
            }

            return tensor._Dimensions.GetDecomposedIndex(idx);
        }        

                

    
        public static void ApplySoftMax(this SpanTensor4<float> tensor)
        {
            SpanTensor.ApplySoftMax(tensor.Span);
        }

                
        public static TensorIndices4 IndexOf<T>(this SpanTensor4<T> tensor, T value)
        where T : unmanaged, IEquatable<T>
        {
            var idx = tensor._Buffer.IndexOf(value);
            if (idx < 0) return TensorSize4.Invalid;
            return tensor._Dimensions.GetDecomposedIndex(idx);
        }

        
        public static TensorIndices4 IndexOf<T>(this SpanTensor4<T> tensor, Predicate<T> predicate)
        where T : unmanaged, IEquatable<T>
        {
            var span = tensor._Buffer;

            for (int i = 0; i < span.Length; ++i)
            {
                if (predicate(span[i])) return tensor._Dimensions.GetDecomposedIndex(i);
            }

            return TensorIndices4.Invalid;
        }

        
        public static TensorIndices4 IndexOfMax<T>(this SpanTensor4<T> tensor, IComparer<T> comparer = null)
        where T : unmanaged
        {
            var span = tensor._Buffer;            

            if (typeof(T) == typeof(float))
            {
                var floats = System.Runtime.InteropServices.MemoryMarshal.Cast<T,float>(span);
                var fidx = System.Numerics.Tensors.TensorPrimitives.IndexOfMax(floats);
                return fidx < 0 ? TensorIndices4.Invalid : tensor._Dimensions.GetDecomposedIndex(fidx);
            }

            if (span.IsEmpty) return TensorIndices4.Invalid;

            if (comparer == null) comparer = Comparer<T>.Default;

            T max = span[0];
            var idx = 0;

            for (int i = 1; i < span.Length; ++i)
            {
                var item = span[i];
                if (comparer.Compare(max , item) >  0) { max = item; idx = i; }
            }

            return tensor._Dimensions.GetDecomposedIndex(idx);
        }        

                

    
        public static void ApplySoftMax(this SpanTensor5<float> tensor)
        {
            SpanTensor.ApplySoftMax(tensor.Span);
        }

                
        public static TensorIndices5 IndexOf<T>(this SpanTensor5<T> tensor, T value)
        where T : unmanaged, IEquatable<T>
        {
            var idx = tensor._Buffer.IndexOf(value);
            if (idx < 0) return TensorSize5.Invalid;
            return tensor._Dimensions.GetDecomposedIndex(idx);
        }

        
        public static TensorIndices5 IndexOf<T>(this SpanTensor5<T> tensor, Predicate<T> predicate)
        where T : unmanaged, IEquatable<T>
        {
            var span = tensor._Buffer;

            for (int i = 0; i < span.Length; ++i)
            {
                if (predicate(span[i])) return tensor._Dimensions.GetDecomposedIndex(i);
            }

            return TensorIndices5.Invalid;
        }

        
        public static TensorIndices5 IndexOfMax<T>(this SpanTensor5<T> tensor, IComparer<T> comparer = null)
        where T : unmanaged
        {
            var span = tensor._Buffer;            

            if (typeof(T) == typeof(float))
            {
                var floats = System.Runtime.InteropServices.MemoryMarshal.Cast<T,float>(span);
                var fidx = System.Numerics.Tensors.TensorPrimitives.IndexOfMax(floats);
                return fidx < 0 ? TensorIndices5.Invalid : tensor._Dimensions.GetDecomposedIndex(fidx);
            }

            if (span.IsEmpty) return TensorIndices5.Invalid;

            if (comparer == null) comparer = Comparer<T>.Default;

            T max = span[0];
            var idx = 0;

            for (int i = 1; i < span.Length; ++i)
            {
                var item = span[i];
                if (comparer.Compare(max , item) >  0) { max = item; idx = i; }
            }

            return tensor._Dimensions.GetDecomposedIndex(idx);
        }        

                

    
        public static void ApplySoftMax(this SpanTensor6<float> tensor)
        {
            SpanTensor.ApplySoftMax(tensor.Span);
        }

                
        public static TensorIndices6 IndexOf<T>(this SpanTensor6<T> tensor, T value)
        where T : unmanaged, IEquatable<T>
        {
            var idx = tensor._Buffer.IndexOf(value);
            if (idx < 0) return TensorSize6.Invalid;
            return tensor._Dimensions.GetDecomposedIndex(idx);
        }

        
        public static TensorIndices6 IndexOf<T>(this SpanTensor6<T> tensor, Predicate<T> predicate)
        where T : unmanaged, IEquatable<T>
        {
            var span = tensor._Buffer;

            for (int i = 0; i < span.Length; ++i)
            {
                if (predicate(span[i])) return tensor._Dimensions.GetDecomposedIndex(i);
            }

            return TensorIndices6.Invalid;
        }

        
        public static TensorIndices6 IndexOfMax<T>(this SpanTensor6<T> tensor, IComparer<T> comparer = null)
        where T : unmanaged
        {
            var span = tensor._Buffer;            

            if (typeof(T) == typeof(float))
            {
                var floats = System.Runtime.InteropServices.MemoryMarshal.Cast<T,float>(span);
                var fidx = System.Numerics.Tensors.TensorPrimitives.IndexOfMax(floats);
                return fidx < 0 ? TensorIndices6.Invalid : tensor._Dimensions.GetDecomposedIndex(fidx);
            }

            if (span.IsEmpty) return TensorIndices6.Invalid;

            if (comparer == null) comparer = Comparer<T>.Default;

            T max = span[0];
            var idx = 0;

            for (int i = 1; i < span.Length; ++i)
            {
                var item = span[i];
                if (comparer.Compare(max , item) >  0) { max = item; idx = i; }
            }

            return tensor._Dimensions.GetDecomposedIndex(idx);
        }        

                

    
        public static void ApplySoftMax(this SpanTensor7<float> tensor)
        {
            SpanTensor.ApplySoftMax(tensor.Span);
        }

                
        public static TensorIndices7 IndexOf<T>(this SpanTensor7<T> tensor, T value)
        where T : unmanaged, IEquatable<T>
        {
            var idx = tensor._Buffer.IndexOf(value);
            if (idx < 0) return TensorSize7.Invalid;
            return tensor._Dimensions.GetDecomposedIndex(idx);
        }

        
        public static TensorIndices7 IndexOf<T>(this SpanTensor7<T> tensor, Predicate<T> predicate)
        where T : unmanaged, IEquatable<T>
        {
            var span = tensor._Buffer;

            for (int i = 0; i < span.Length; ++i)
            {
                if (predicate(span[i])) return tensor._Dimensions.GetDecomposedIndex(i);
            }

            return TensorIndices7.Invalid;
        }

        
        public static TensorIndices7 IndexOfMax<T>(this SpanTensor7<T> tensor, IComparer<T> comparer = null)
        where T : unmanaged
        {
            var span = tensor._Buffer;            

            if (typeof(T) == typeof(float))
            {
                var floats = System.Runtime.InteropServices.MemoryMarshal.Cast<T,float>(span);
                var fidx = System.Numerics.Tensors.TensorPrimitives.IndexOfMax(floats);
                return fidx < 0 ? TensorIndices7.Invalid : tensor._Dimensions.GetDecomposedIndex(fidx);
            }

            if (span.IsEmpty) return TensorIndices7.Invalid;

            if (comparer == null) comparer = Comparer<T>.Default;

            T max = span[0];
            var idx = 0;

            for (int i = 1; i < span.Length; ++i)
            {
                var item = span[i];
                if (comparer.Compare(max , item) >  0) { max = item; idx = i; }
            }

            return tensor._Dimensions.GetDecomposedIndex(idx);
        }        

                

    
        public static void ApplySoftMax(this SpanTensor8<float> tensor)
        {
            SpanTensor.ApplySoftMax(tensor.Span);
        }

                
        public static TensorIndices8 IndexOf<T>(this SpanTensor8<T> tensor, T value)
        where T : unmanaged, IEquatable<T>
        {
            var idx = tensor._Buffer.IndexOf(value);
            if (idx < 0) return TensorSize8.Invalid;
            return tensor._Dimensions.GetDecomposedIndex(idx);
        }

        
        public static TensorIndices8 IndexOf<T>(this SpanTensor8<T> tensor, Predicate<T> predicate)
        where T : unmanaged, IEquatable<T>
        {
            var span = tensor._Buffer;

            for (int i = 0; i < span.Length; ++i)
            {
                if (predicate(span[i])) return tensor._Dimensions.GetDecomposedIndex(i);
            }

            return TensorIndices8.Invalid;
        }

        
        public static TensorIndices8 IndexOfMax<T>(this SpanTensor8<T> tensor, IComparer<T> comparer = null)
        where T : unmanaged
        {
            var span = tensor._Buffer;            

            if (typeof(T) == typeof(float))
            {
                var floats = System.Runtime.InteropServices.MemoryMarshal.Cast<T,float>(span);
                var fidx = System.Numerics.Tensors.TensorPrimitives.IndexOfMax(floats);
                return fidx < 0 ? TensorIndices8.Invalid : tensor._Dimensions.GetDecomposedIndex(fidx);
            }

            if (span.IsEmpty) return TensorIndices8.Invalid;

            if (comparer == null) comparer = Comparer<T>.Default;

            T max = span[0];
            var idx = 0;

            for (int i = 1; i < span.Length; ++i)
            {
                var item = span[i];
                if (comparer.Compare(max , item) >  0) { max = item; idx = i; }
            }

            return tensor._Dimensions.GetDecomposedIndex(idx);
        }        

                

    
    }
}
