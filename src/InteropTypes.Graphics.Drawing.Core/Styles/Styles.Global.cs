using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    public class GlobalStyle : GlobalStyle.ISource,
        GlobalStyle._ISource<String>,
        GlobalStyle._ISource<ColorStyle>
    {
        #region current styles

        public const string COLOR = "Color";

        public const string NAME = "Name";

        private ColorStyle _Color;

        private string _Name;

        bool _ISource<String>.TryGetGlobalProperty(string name, out String val)
        {
            switch (name)
            {
                case NAME: { val = _Name; return true; }
            }

            val = default;
            return false;
        }

        bool _ISource<String>.TrySetGlobalProperty(string name, String value)
        {
            switch (name)
            {
                case NAME: _Name = value; return true;
            }

            return false;
        }

        bool _ISource<ColorStyle>.TryGetGlobalProperty(string name, out ColorStyle val)
        {
            switch (name)
            {
                case COLOR: { val = _Color; return true; }
            }

            val = default;
            return false;
        }

        bool _ISource<ColorStyle>.TrySetGlobalProperty(string name, ColorStyle value)
        {
            switch (name)
            {
                case COLOR: _Color = value; return true;
            }

            return false;
        }

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
