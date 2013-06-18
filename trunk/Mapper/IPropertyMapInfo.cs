using System;

namespace Mapper
{
    public interface IPropertyMapInfo
    {
        Func<object, object> Getter { get;  }
        Action<object, object> Setter { get; }
        IValueFormatter ValueFormatter { get; set; }
        bool IsValueFormatterSetted { get; }
        bool IsReferenceProperty { get; }
        Type ClassMapping { get; set; }
        Type ReferenceType{ get; set; }
    }
}