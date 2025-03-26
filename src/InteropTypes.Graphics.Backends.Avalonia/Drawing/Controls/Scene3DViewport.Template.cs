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
    /// Represents the Camera Viewport template used at <see cref="Scene3DView.ViewportTemplate"/>
    /// </summary>
    /// <remarks>
    /// The content of this Template may be:<br/>
    /// - <see cref="AutoCamera3DViewport"/><br/>
    /// - <see cref="OrbitCamera3DViewport"/><br/>
    /// </remarks>
    [ControlTemplateScope]
    public class Scene3DViewportTemplate : ITemplate<Scene3DViewport>
    {
        public Scene3DViewportTemplate() { }

        public Scene3DViewportTemplate(Func<Scene3DViewport> factory)
        {
            _Factory = factory;
        }

        private readonly Func<Scene3DViewport> _Factory;

        [Content]
        [TemplateContent]
        public object Content { get; set; }

        public Scene3DViewport Build() => _Factory?.Invoke() ?? (Scene3DViewport)TemplateContent.Load(Content)?.Result;

        object Avalonia.Styling.ITemplate.Build()
        {
            return Build();
        }
    }
}
