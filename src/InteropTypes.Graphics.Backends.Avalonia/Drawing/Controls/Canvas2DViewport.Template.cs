using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;

using InteropTypes.Graphics.Backends.Controls.Primitives;

namespace InteropTypes.Graphics.Backends.Controls
{
    /// <summary>
    /// Represents the Camera Viewport template used at <see cref="Canvas2DView.ViewportTemplate"/>
    /// </summary>
    /// <remarks>
    /// The content of this Template may be:<br/>
    /// - <see cref="AutoCanvas2DViewport"/><br/>    
    /// </remarks>
    [ControlTemplateScope]
    public class Canvas2DViewportTemplate : ITemplate<Canvas2DViewport>
    {
        public Canvas2DViewportTemplate() { }

        public Canvas2DViewportTemplate(Func<Canvas2DViewport> factory)
        {
            _Factory = factory;
        }

        private readonly Func<Canvas2DViewport> _Factory;

        [Content]
        [TemplateContent]
        public object Content { get; set; }

        public Canvas2DViewport Build() => _Factory?.Invoke() ?? (Canvas2DViewport)TemplateContent.Load(Content)?.Result;

        object Avalonia.Styling.ITemplate.Build()
        {
            return Build();
        }
    }
}
