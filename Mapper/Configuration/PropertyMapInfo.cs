using System;
using System.Collections.Generic;
using Mapper.Converters;

namespace Mapper.Configuration
{
    internal sealed class PropertyMapInfo<T> : IPropertyMapInfo
    {
        public PropertyMapInfo()
        {
            DiscriminatorTypes = new Dictionary<string, Type>();
        }

        public Func<T, object> Getter { get; set; }

        public Action<T, object> Setter { get; set; }

        public bool IsDiscriminatorSet
        {
            get { return !string.IsNullOrEmpty(DiscriminatorField); }
        }

        Action<object, object> IPropertyMapInfo.Setter
        {
            get { return (o, p) => Setter((T) o, p); }
        }

        Func<object, object> IPropertyMapInfo.Getter
        {
            get { return o => Getter((T) o); }
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

        public string DiscriminatorField { get; set; }

        public Dictionary<string, Type> DiscriminatorTypes { get; private set; }
    }
}