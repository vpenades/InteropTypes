using System;
using System.Collections.Generic;
using System.Windows;

namespace InteropTypes.Graphics.Backends
{
    internal struct PropertyFactory<TClass> where TClass:DependencyObject
    {
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

        public StaticProperty<TValue> Register<TValue>(string name, TValue defval, FrameworkPropertyMetadataOptions options)
        {
            var p = DependencyProperty.Register
            (
                name,
                typeof(TValue),
                typeof(TClass),
                new FrameworkPropertyMetadata(defval, options)
            );

            return new StaticProperty<TValue>(p);
        }

        public StaticProperty<TValue> RegisterCallback<TValue>(string name, TValue defval)
        {
            var p = DependencyProperty.Register
            (
                name,
                typeof(TValue),
                typeof(TClass),
                new FrameworkPropertyMetadata(defval, _OnPropertyChanged)
            );

            return new StaticProperty<TValue>(p);
        }

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

        public StaticProperty<TValue> RegisterAttached<TValue>(string name, TValue defval)
        {
            return RegisterAttached(name, defval, FrameworkPropertyMetadataOptions.AffectsRender);
        }

        public StaticProperty<TValue> RegisterAttached<TValue>(string name, TValue defval, FrameworkPropertyMetadataOptions flags)
        {
            var p = DependencyProperty.RegisterAttached
            (
                name,
                typeof(TValue),
                typeof(TClass),
                new FrameworkPropertyMetadata(defval, flags)
            );

            return new StaticProperty<TValue>(p);
        }

        private static void _OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // https://github.com/dotnet/wpf/blob/a30c4edea55a95ec9d7c2d29d79b2d4fb12ed973/src/Microsoft.DotNet.Wpf/src/PresentationFramework/System/Windows/Controls/ItemsControl.cs#L190

            if (d is IPropertyChanged instance)
            {
                instance.OnPropertyChangedEx(e);
            }
        }

        public interface IPropertyChanged
        {
            bool OnPropertyChangedEx(DependencyPropertyChangedEventArgs args);
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
