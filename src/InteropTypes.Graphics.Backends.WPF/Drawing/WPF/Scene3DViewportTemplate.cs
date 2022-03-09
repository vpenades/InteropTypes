using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InteropTypes.Graphics.Backends.WPF
{
    public class Scene3DViewportTemplate : ControlTemplate
    {
        public Scene3DViewportTemplate() : base(typeof(Scene3DView)) { }        

        protected override void ValidateTemplatedParent(FrameworkElement templatedParent)
        {
            base.ValidateTemplatedParent(templatedParent);

            if (!(templatedParent is Scene3DView))
            {
                throw new ArgumentException("TemplateTargetTypeMismatch; expected Scene3DView", templatedParent.GetType().Name);
            }
        }
    }
}
