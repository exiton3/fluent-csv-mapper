using Mapper.Configuration;

namespace Mapper.Mappers
{
    internal class NullableMapper : IMapper
    {
        public object Store(IPropertyMapInfo propertyMapInfo, object objectToStore, IClassMapper classMapper)
        {
            object getterValue = propertyMapInfo.Getter(objectToStore);
            if (getterValue == null)
            {
                return null;
            }
            return classMapper.Store(getterValue);
        }

        public object Restore(IPropertyMapInfo propertyMapInfo, object value, IClassMapper classMapper)
        {
            if (value == null)
            {
                return null;
            }
            return classMapper.Restore(propertyMapInfo.PropertyType, value as IObjectStorage);
        }

        public bool IsMatch(IPropertyMapInfo propertyMapInfo)
        {
            return propertyMapInfo.PropertyKind == PropertyKind.Nullable;
        }
    }
}