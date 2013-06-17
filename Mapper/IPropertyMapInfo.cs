using System;

namespace Mapper
{
    public interface IPropertyMapInfo
    {
        Func<object, object> Getter { get;  }
        Action<object, object> Setter { get; }
        IValueFormatter ValueFormatter { get; set; }
        Type ClassMapping { get; set; }

    }
}