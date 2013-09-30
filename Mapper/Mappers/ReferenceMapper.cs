using Mapper.Configuration;

namespace Mapper.Mappers
{
    class ReferenceMapper : IMapper
    {
        public object Store(IPropertyMapInfo propertyMapInfo, object objectToStore, IClassMapper classMapper)
        {
            object getterValue = propertyMapInfo.Getter(objectToStore);
            return classMapper.Store(getterValue);
        }

        public object Restore(IPropertyMapInfo mapping, object value, IClassMapper classMapper)
        {
            var subObj = classMapper.Restore(mapping.PropertyType, value as IObjectStorage);
            return subObj;
        }

        public bool IsMatch(IPropertyMapInfo propertyMapInfo)
        {
            return propertyMapInfo.PropertyKind == PropertyKind.Reference;
        }
    }
}