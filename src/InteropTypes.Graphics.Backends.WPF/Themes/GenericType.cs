using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace InteropTypes.Graphics.Backends.Themes
{
    // WPF team recomends using a TemplateSelector for interfaces
    // https://social.msdn.microsoft.com/Forums/vstudio/en-US/1e774a24-0deb-4acd-a719-32abd847041d/data-templates-and-interfaces?forum=wpf

    /// <summary>
    /// used to define generic types in Xaml
    /// </summary>
    /// <remarks>
    /// <see href="https://stackoverflow.com/questions/10005187/how-to-reference-a-generic-type-in-the-datatype-attribute-of-a-datatemplate"/>
    /// <see href="https://stackoverflow.com/questions/54092789/datatemplates-and-generics"/>
    /// </remarks>
    [MarkupExtensionReturnType(typeof(Type))]
    public class GenericType : MarkupExtension
    {
        public GenericType() { }

        public GenericType(Type baseType, params Type[] innerTypes)
        {
            BaseType = baseType;
            InnerTypes = innerTypes;
        }

        public Type BaseType { get; set; }

        public Type[] InnerTypes { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Type result = BaseType.MakeGenericType(InnerTypes);
            return result;
        }
    }

    [MarkupExtensionReturnType(typeof(Type))]
    public class GenericTypeOne : MarkupExtension
    {
        public GenericTypeOne() { }

        public GenericTypeOne(Type baseType, Type innerType0)
        {
            BaseType = baseType;
            InnerType0 = innerType0;
        }

        public Type BaseType { get; set; }

        public Type InnerType0 { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Type result = BaseType.MakeGenericType(InnerType0);
            return result;
        }
    }

    [MarkupExtensionReturnType(typeof(Type))]
    public class GenericMemoryBitmapType : TypeExtension
    {
        public GenericMemoryBitmapType() { }
        public GenericMemoryBitmapType(string typeName) : base(typeName) { }

        public GenericMemoryBitmapType(Type type) : base(type) { }        

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var innerType = base.ProvideValue(serviceProvider) as Type;
            if (innerType == null) throw new InvalidOperationException("null inner type");

            Type result = typeof(Bitmaps.MemoryBitmap<>).MakeGenericType(innerType);
            return result;
        }
    }
    
}
