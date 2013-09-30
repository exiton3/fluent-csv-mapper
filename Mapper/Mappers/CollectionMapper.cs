using System;
using System.Collections;
using System.Collections.Generic;
using Mapper.Configuration;

namespace Mapper.Mappers
{
    class CollectionMapper : IMapper
    {
        public object Store(IPropertyMapInfo propertyMapInfo, object objectToStore, IClassMapper classMapper)
        {
            object getterValue = propertyMapInfo.Getter(objectToStore);
            var objectStorages = new List<IObjectStorage>();
            foreach (var obj in (IEnumerable)getterValue)
            {
                var storage = classMapper.Store(obj);
                objectStorages.Add(storage);
            }
            return objectStorages;
        }

        public object Restore(IPropertyMapInfo mapping, object value, IClassMapper classMapper)
        {
            var collectionType = typeof(List<>);
            var genericType = collectionType.MakeGenericType(mapping.PropertyType);
            var objectList = (IList)Activator.CreateInstance(genericType);

            foreach (var storageItem in value as IEnumerable)
            {
                var restoredItem = classMapper.Restore(mapping.PropertyType, (IObjectStorage)storageItem);
                objectList.Add(restoredItem);
            }
            return objectList;
        }

        public bool IsMatch(IPropertyMapInfo propertyMapInfo)
        {
            return propertyMapInfo.PropertyKind == PropertyKind.Collection;
        }
    }
}