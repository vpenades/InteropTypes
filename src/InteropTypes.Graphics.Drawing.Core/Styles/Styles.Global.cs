using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// This is a generic property bag used to store shared properties.
    /// </summary>
    public class GlobalStyle : ICloneable, GlobalStyle.ISource,
        GlobalStyle._ISource<String>,
        GlobalStyle._ISource<Int32>,
        GlobalStyle._ISource<Single>,
        GlobalStyle._ISource<ColorStyle>
    {
        #region lifecycle

        object ICloneable.Clone() { return this.Clone(); }

        public GlobalStyle Clone()
        {
            var clone = new GlobalStyle();
            if (this._Properties == null) return clone;

            clone._Properties = new Dictionary<string, object>();

            foreach(var kvp in this._Properties)
            {
                clone._Properties[kvp.Key] = kvp.Value;
            }

            return clone;
        }

        #endregion

        #region current styles

        public const string COLOR = "Color";

        public const string NAME = "Name";

        private Dictionary<string, Object> _Properties;        

        #endregion

        #region API        

        private static ISource _GetGlobalStyleSource<T>(Object obj)
        {
            if (obj is ISource src) return src;

            else if (obj is IServiceProvider serviceProvider)
            {
                src = serviceProvider.GetService(typeof(ISource)) as ISource;
                if (src != null) return src;
            }

            return null;
        }
        
        public static bool TrySetGlobalProperty<T>(ref GlobalStyle style, string name, T value)
        {
            if (style == null) style = new GlobalStyle();
            return TrySetGlobalProperty(style, name, value);            
        }

        public static bool TrySetGlobalProperty<T>(Object obj, string name, T value)
        {
            var src = _GetGlobalStyleSource<T>(obj);
            if (src == null) return false;
            return src.TrySetGlobalProperty(name, value);
        }

        public static bool TryGetGlobalProperty<T>(Object obj, string name, out T value)
        {
            var src = _GetGlobalStyleSource<T>(obj);
            if (src == null) { value = default; return false; }
            return src.TryGetGlobalProperty(name, out value);
        }

        bool ISource.TryGetGlobalProperty<T>(string name, out T value)
        {
            if (this is _ISource<T> src) return src.TryGetGlobalProperty(name, out value);
            else { value = default; return false; }
        }

        bool ISource.TrySetGlobalProperty<T>(string name, T value)
        {
            if (this is _ISource<T> src) return src.TrySetGlobalProperty(name, value);
            else return false;
        }

        #endregion

        #region Core

        private bool _TryGetPropertyValue<T>(string key, out T val)
        {
            if (_Properties != null && _Properties.TryGetValue(key, out var obj))
            {
                if (obj is T xval) { val = xval; return true; }
            }

            val = default; return false;
        }

        private void _SetPropertyValue<T>(string key, T val)
        {
            if (_Properties == null) _Properties = new Dictionary<string, object>();
            _Properties[key] = val;
        }

        bool _ISource<String>.TryGetGlobalProperty(string name, out String val)
        {
            return _TryGetPropertyValue(name, out val);
        }

        bool _ISource<String>.TrySetGlobalProperty(string name, String value)
        {
            _SetPropertyValue(name, value);
            return true;
        }

        bool _ISource<Int32>.TryGetGlobalProperty(string name, out Int32 val)
        {
            return _TryGetPropertyValue(name, out val);
        }

        bool _ISource<Int32>.TrySetGlobalProperty(string name, Int32 value)
        {
            _SetPropertyValue(name, value);
            return true;
        }

        bool _ISource<Single>.TryGetGlobalProperty(string name, out Single val)
        {
            return _TryGetPropertyValue(name, out val);
        }

        bool _ISource<Single>.TrySetGlobalProperty(string name, Single value)
        {
            _SetPropertyValue(name, value);
            return true;
        }

        bool _ISource<ColorStyle>.TryGetGlobalProperty(string name, out ColorStyle val)
        {
            return _TryGetPropertyValue(name, out val);
        }

        bool _ISource<ColorStyle>.TrySetGlobalProperty(string name, ColorStyle value)
        {
            _SetPropertyValue(name, value);
            return true;
        }        

        #endregion

        #region nested types

        public interface ISource
        {
            bool TryGetGlobalProperty<T>(string name, out T value);
            bool TrySetGlobalProperty<T>(string name, T value);
        }

        internal interface _ISource<TT>
        {
            bool TryGetGlobalProperty(string name, out TT value);
            bool TrySetGlobalProperty(string name, TT value);
        }

        #endregion
    }
}
