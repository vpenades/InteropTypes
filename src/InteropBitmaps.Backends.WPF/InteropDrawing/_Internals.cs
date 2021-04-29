using System;
using System.Collections.Generic;
using System.Windows;

namespace InteropDrawing.Backends
{
    internal struct PropertyFactory<TClass> where TClass:DependencyObject
    {
        public StaticProperty<TValue> Register<TValue>(string name, TValue defval, PropertyChangedCallback callback)
        {
            var p = DependencyProperty.Register
            (
                name,
                typeof(TValue),
                typeof(TClass),
                new FrameworkPropertyMetadata(defval, callback)
            );

            return new StaticProperty<TValue>(p);
        }

        public StaticProperty<TValue> Register<TValue>(string name, TValue defval)
        {
            var p = DependencyProperty.Register
            (
                name,
                typeof(TValue),
                typeof(TClass),
                new FrameworkPropertyMetadata(defval)
            );

            return new StaticProperty<TValue>(p);
        }

        public StaticProperty<TValue> RegisterAttached<TValue>(string name, TValue defval)
        {
            var p = DependencyProperty.RegisterAttached
            (
                name,
                typeof(TValue),
                typeof(TClass),
                new FrameworkPropertyMetadata(defval, FrameworkPropertyMetadataOptions.AffectsRender)
            );

            return new StaticProperty<TValue>(p);
        }
    }

    internal struct StaticProperty<TValue>
    {
        public StaticProperty(DependencyProperty dep)
        {
            _Property = dep;
        }

        private readonly DependencyProperty _Property;

        public DependencyProperty Property => _Property;

        public TValue GetValue(DependencyObject context)
        {
            var obj = context.GetValue(_Property);
            return obj is TValue val ? val : default;
        }

        public void SetValue(DependencyObject context, TValue value)
        {
            context.SetValue(_Property, value);            
        }
    }
}
