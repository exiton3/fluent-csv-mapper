using System;
using System.Collections.Generic;
using Mapper.Converters;

namespace Mapper.Configuration
{
    public interface IPropertyMapInfo
    {
        Func<object, object> Getter { get;  }
        Action<object, object> Setter { get; }
        IValueFormatter ValueFormatter { get; set; }
        bool IsValueFormatterSet { get; }
        ITypeConverter TypeConverter { get; set; }
        bool IsTypeConverterSet { get; }
        Type PropertyType { get; set; }
        PropertyKind PropertyKind { get; set; }
        string DiscriminatorField { get; set; }
        Dictionary<string, Type> DiscriminatorTypes { get; }
        bool IsDiscriminatorSet { get; }
    }
}