using System;
using System.Collections.Generic;
using System.Windows;

namespace InteropTypes.Graphics.Backends
{
    using METADATAOPTIONS = FrameworkPropertyMetadataOptions;

    internal struct PropertyFactory<TClass> where TClass:DependencyObject
    {
        #region diagnostics

        [System.Diagnostics.Conditional("DEBUG")]
        private void _VerifyPropertyExists<TValue>(string name)
        {
            var pinfo = typeof(TClass).GetProperty(name);
            if (pinfo == null) throw new InvalidOperationException($"Property {name} not found in class {typeof(TClass).Name}");
            if (!pinfo.PropertyType.IsAssignableFrom(typeof(TValue))) throw new InvalidOperationException($"Property {name} has type {typeof(TValue).Name} but the dependency declares {pinfo.PropertyType.Name}");
        }

        #endregion

        #region register

        public StaticProperty<TValue> Register<TValue>(string name, TValue defval)
        {
            _VerifyPropertyExists<TValue>(name);

            var p = DependencyProperty.Register(name,typeof(TValue),typeof(TClass),Metadata(defval));

            return new StaticProperty<TValue>(p);
        }

        public StaticProperty<TValue> Register<TValue>(string name, TValue defval, Func<TValue,TValue> coerceValueFunc)
        {
            _VerifyPropertyExists<TValue>(name);

            var p = DependencyProperty.Register(name, typeof(TValue), typeof(TClass), Metadata(defval, coerceValueFunc));

            return new StaticProperty<TValue>(p);
        }

        public StaticProperty<TValue> Register<TValue>(string name, TValue defval, METADATAOPTIONS options)
        {
            _VerifyPropertyExists<TValue>(name);

            var p = DependencyProperty.Register(name,typeof(TValue),typeof(TClass),Metadata(defval, options));

            return new StaticProperty<TValue>(p);
        }

        public StaticProperty<TValue> Register<TValue>(string name, TValue defval, Func<TValue, TValue> coerceValueFunc, METADATAOPTIONS options)
        {
            _VerifyPropertyExists<TValue>(name);

            var p = DependencyProperty.Register(name, typeof(TValue), typeof(TClass), Metadata(defval, options, coerceValueFunc));

            return new StaticProperty<TValue>(p);
        }

        #endregion

        #region register attached

        public StaticProperty<TValue> RegisterCallback<TValue>(string name, TValue defval)
        {
            _VerifyPropertyExists<TValue>(name);

            var p = DependencyProperty.Register(name,typeof(TValue),typeof(TClass),MetadataCallback(defval));

            return new StaticProperty<TValue>(p);
        }            

        public StaticProperty<TValue> RegisterAttached<TValue>(string name, TValue defval)
        {
            return RegisterAttached(name, defval, FrameworkPropertyMetadataOptions.AffectsRender);
        }

        public StaticProperty<TValue> RegisterAttached<TValue>(string name, TValue defval, METADATAOPTIONS flags)
        {
            var p = DependencyProperty.RegisterAttached(name,typeof(TValue),typeof(TClass),Metadata(defval, flags));

            return new StaticProperty<TValue>(p);
        }

        #endregion

        #region metadata functions

        private FrameworkPropertyMetadata Metadata<TValue>(TValue defval)
        {
            return new FrameworkPropertyMetadata(defval);
        }

        private FrameworkPropertyMetadata Metadata<TValue>(TValue defval, METADATAOPTIONS options)
        {
            return new FrameworkPropertyMetadata(defval, options);
        }

        private FrameworkPropertyMetadata Metadata<TValue>(TValue defval, METADATAOPTIONS options, Func<TValue, TValue> coerceValueFunc)
        {
            object _coerceValueCallback(DependencyObject d, object baseValue)
            {
                return baseValue is TValue val
                    ? coerceValueFunc(val)
                    : baseValue;
            }

            return new FrameworkPropertyMetadata(defval, options, null, _coerceValueCallback);
        }

        private FrameworkPropertyMetadata Metadata<TValue>(TValue defval, Func<TValue, TValue> coerceValueFunc)
        {
            object _coerceValueCallback(DependencyObject d, object baseValue)
            {
                return baseValue is TValue val
                    ? coerceValueFunc(val)
                    : baseValue;
            }

            return new FrameworkPropertyMetadata(defval, null, _coerceValueCallback);
        }

        private FrameworkPropertyMetadata MetadataCallback<TValue>(TValue defval)
        {
            return new FrameworkPropertyMetadata(defval, _OnPropertyChanged);
        }

        private FrameworkPropertyMetadata MetadataCallback<TValue>(TValue defval, METADATAOPTIONS options)
        {
            return new FrameworkPropertyMetadata(defval, options, _OnPropertyChanged);
        }

        private FrameworkPropertyMetadata MetadataCallback<TValue>(TValue defval, METADATAOPTIONS options, Func<TValue, TValue> coerceValueFunc)
        {
            object _coerceValueCallback(DependencyObject d, object baseValue)
            {
                return baseValue is TValue val
                    ? coerceValueFunc(val)
                    : baseValue;
            }

            return new FrameworkPropertyMetadata(defval, options, _OnPropertyChanged, _coerceValueCallback);
        }

        private FrameworkPropertyMetadata MetadataCallback<TValue>(TValue defval, Func<TValue, TValue> coerceValueFunc)
        {
            object _coerceValueCallback(DependencyObject d, object baseValue)
            {
                return baseValue is TValue val
                    ? coerceValueFunc(val)
                    : baseValue;
            }

            return new FrameworkPropertyMetadata(defval, _OnPropertyChanged, _coerceValueCallback);
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

        #endregion
    }

    [System.Diagnostics.DebuggerDisplay("{Property.PropertyType.Name,nq} {Property.Name,nq}")]
    internal struct StaticProperty<TValue>
    {
        #region constructor
        internal StaticProperty(DependencyProperty dep)
        {
            _Property = dep;
        }

        #endregion

        #region data

        private readonly DependencyProperty _Property;

        #endregion

        #region properties

        public DependencyProperty Property => _Property;

        #endregion

        #region API

        public TValue GetValue(DependencyObject context)
        {
            var obj = context.GetValue(_Property);
            return obj is TValue val ? val : default;
        }

        public void SetValue(DependencyObject context, TValue value)
        {
            context.SetValue(_Property, value);            
        }

        #endregion
    }
}
