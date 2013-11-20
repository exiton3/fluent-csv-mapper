using Mapper.Configuration;

namespace Mapper.Mappers
{
    internal class ValueMapper : IMapper
    {
        public object Store(IPropertyMapInfo propInfo, object objectToStore, IClassMapper classMapper)
        {
            object getterValue = propInfo.Getter(objectToStore);
            if (propInfo.IsValueFormatterSet)
            {
                getterValue = propInfo.ValueFormatter.Format(getterValue);
            }

            if (propInfo.IsTypeConverterSet)
            {
                getterValue = propInfo.TypeConverter.Convert(getterValue);
            }

            return getterValue;
        }

        public object Restore(IPropertyMapInfo propertyMapInfo, object value, IClassMapper classMapper)
        {
            if (propertyMapInfo.IsValueFormatterSet)
            {
                value = propertyMapInfo.ValueFormatter.Parse((string) value);
            }

            if (propertyMapInfo.IsTypeConverterSet)
            {
                value = propertyMapInfo.TypeConverter.ConvertBack(value);
            }

            return value;
        }

        public bool IsMatch(IPropertyMapInfo propertyMapInfo)
        {
            return propertyMapInfo.PropertyKind == PropertyKind.Value;
        }
    }
}