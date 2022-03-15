using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{    
    static partial class _PrivateExtensions
    {
        public static object TryGetService(this IServiceProvider owner, Type serviceType, Object parent = null)
        {
            // parent service provider has preference
            if (parent is IServiceProvider psp)
            {                
                if (serviceType == typeof(IServiceProvider)) return psp;

                return serviceType.IsAssignableFrom(owner.GetType())
                ? owner
                : (psp?.GetService(serviceType));
            }

            return serviceType.IsAssignableFrom(owner.GetType())
                ? owner
                : null;
        }        
    }
}