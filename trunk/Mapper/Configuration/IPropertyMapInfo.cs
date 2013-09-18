using System;
using Mapper.Converters;

namespace Mapper.Configuration
{
    public interface IPropertyMapInfo
    {
        Func<object, object> Getter { get;  }
        Action<object, object> Setter { get; }
        IValueFormatter ValueFormatter { get; set; }
        bool IsValueFormatterSet { get; }
        bool IsReferenceProperty { get; }
        Type ReferenceType{ get; set; }
        ITypeConverter TypeConverter { get; set; }
        bool IsTypeConverterSet { get; }
        bool IsCollectionProperty { get; }
        Type CollectionElementType { get; set; }
        Type CollectionType { get; set; }
    }
}