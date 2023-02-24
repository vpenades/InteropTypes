using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Backends.SilkOpenGL.SilkGL
{
    internal class _BindingsGuard<TKey,TValue>
        where TValue: IEquatable<TValue>
    {
        private readonly Dictionary<TKey, TValue> _Bindings = new Dictionary<TKey, TValue>();

        public void Set(TKey key, TValue value)
        {
            if (Equals(value , default(TValue))) Unbind(key);
            else Bind(key, value);
        }

        public void Bind(TKey key, TValue value)
        {
            if (_Bindings.TryGetValue(key, out var currVal))
            {
                if (Equals(value, currVal)) return;
                throw new InvalidOperationException("Already bound");
            }

            _Bindings.Add(key, value);
        }

        public void Unbind(TKey key)
        {
            _Bindings.Remove(key);
        }
    }
}
