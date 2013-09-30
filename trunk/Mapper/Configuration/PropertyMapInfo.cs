using System;
using Mapper.Converters;

namespace Mapper.Configuration
{
    internal sealed class PropertyMapInfo<T> :  IPropertyMapInfo 
    {
        public Func<T, object> Getter { get; set; }

        public Action<T, object> Setter { get; set; }

        Action<object, object> IPropertyMapInfo.Setter
        {
            get { return (o, p) => Setter((T) o, p); }
        }

        Func<object, object> IPropertyMapInfo.Getter
        {
            get {return o => Getter((T)o); }
        }

        public IValueFormatter ValueFormatter { get; set; }

        public bool IsValueFormatterSet
        {
            get { return ValueFormatter != null; }
        }

        public Type PropertyType { get; set; }

        public ITypeConverter TypeConverter { get; set; }

        public bool IsTypeConverterSet
        {
            get { return TypeConverter != null; }
        }

        public PropertyKind PropertyKind { get; set; }
    }
    
}