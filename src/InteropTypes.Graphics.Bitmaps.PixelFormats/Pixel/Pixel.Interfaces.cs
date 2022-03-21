using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {
        public interface IReflection
        {
            uint GetCode();
            PixelFormat GetPixelFormat();            
            bool IsOpaque { get; }
            bool IsPremultiplied { get; }            
        }        

        public interface ICopyValueTo<T> where T : unmanaged
        {
            void CopyTo(ref T value);
        }
        

        public interface IValueGetter<T> where T : unmanaged
        {
            T GetValue();
        }

        

        public interface IValueSetter<T> where T : unmanaged
        {
            void SetValue(T value);
        }

        interface IDelegateProvider<TDelegate>
        {
            TDelegate GetDelegate();
        }

    }
}
