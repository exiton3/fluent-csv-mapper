using System;

namespace Mapper.Configuration
{
    internal sealed class PropertyMapInfo<T> :  IPropertyMapInfo where T:class
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

        public bool IsValueFormatterSetted
        {
            get { return ValueFormatter != null; }
        }

        public bool IsReferenceProperty
        {
            get {return ReferenceType != null; }
        }

        public Type ReferenceType {get; set; }

        public ITypeConverter TypeConverter { get; set; }

        public bool IsTypeConverterSetted
        {
            get { return TypeConverter != null; }
        }
    }
    
}