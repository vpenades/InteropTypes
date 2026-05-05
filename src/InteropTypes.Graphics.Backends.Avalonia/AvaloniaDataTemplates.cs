using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

using InteropTypes.Graphics.Backends.Bitmaps;
using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics
{
    public class AvaloniaDataTemplates : IDataTemplate
    {
        static AvaloniaDataTemplates()
        {
            _Bindings[typeof(MemoryBitmap)] = () => new Backends.Views.MemoryBitmapView();
            _Bindings[typeof(BindableBitmap)] = () => new Backends.Views.BindableBitmapView();
            _Bindings[typeof(AvaloniaBitmapSwapChain)] = () => new Backends.Views.AvaloniaBitmapSwapChainView();
            _Bindings[typeof(IDrawingBrush<ICanvas2D>)] = () => new Backends.Views.DrawingBrushCanvas2DView();
        }

        private static readonly Dictionary<Type, Func<Control>> _Bindings = new Dictionary<Type, Func<Control>>();

        public bool Match(object data)
        {
            return GetFactory(data) != null;
        }

        public Control Build(object data)
        {
            var factory = GetFactory(data);

            if (factory == null) return new TextBlock { Text = $"Not Found: {data}" };

            return factory.Invoke();            
        }

        private Func<Control> GetFactory(Object data)
        {
            if (data == null) return null;

            var dataType = data.GetType();

            // exact
            _Bindings.TryGetValue(dataType, out var factory);
            if (factory != null) return factory;

            // derived types
            foreach (var kvp in _Bindings)
            {
                if (dataType.IsAssignableTo(kvp.Key)) return kvp.Value;
            }

            return null;
        }
    }
}
