using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Drawing
{
    internal partial class TypeTests
    {
        [Test]
        public void TestGlobalStyle()
        {
            var obj = new _GlobalStyleFrontEndExample();

            Assert.IsTrue(GlobalStyle.TrySetGlobalProperty(obj, GlobalStyle.COLOR, ColorStyle.Red));
            Assert.IsTrue(GlobalStyle.TryGetGlobalProperty<ColorStyle>(obj, GlobalStyle.COLOR, out var color));

            Assert.AreEqual(ColorStyle.Red, color);
        }


        class _GlobalStyleFrontEndExample : IServiceProvider
        {
            private _GlobalStyleBackendExample _Target = new _GlobalStyleBackendExample();

            public object GetService(Type serviceType)
            {
                return typeof(GlobalStyle.ISource).IsAssignableFrom(serviceType)
                    ? _Target
                    : (object)null;
            }
        }

        class _GlobalStyleBackendExample : GlobalStyle.ISource
        {
            private GlobalStyle _GlobalStyles;

            bool GlobalStyle.ISource.TrySetGlobalProperty<T>(string name, T value)
            {
                return GlobalStyle.TrySetGlobalProperty(ref _GlobalStyles, name, value);
            }

            bool GlobalStyle.ISource.TryGetGlobalProperty<T>(string name, out T value)
            {
                return GlobalStyle.TryGetGlobalProperty(_GlobalStyles, name, out value);
            }            
        }
    }
}
