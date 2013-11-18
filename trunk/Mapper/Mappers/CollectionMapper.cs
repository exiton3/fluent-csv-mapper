using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mapper.Configuration;

namespace Mapper.Mappers
{
    class CollectionMapper : IMapper
    {
        public object Store(IPropertyMapInfo propertyMapInfo, object objectToStore, IClassMapper classMapper)
        {
            object getterValue = propertyMapInfo.Getter(objectToStore);
            var objectStorages = new List<IObjectStorage>();
            if (getterValue != null)
            {
                foreach (var obj in (IEnumerable)getterValue)
                {
                    var storage = classMapper.Store(obj);
                    if (propertyMapInfo.IsDiscriminatorSet)
                    {
                        var discriminatorType = propertyMapInfo.DiscriminatorTypes.FirstOrDefault(x => x.Value == obj.GetType());
                        storage.SetData(propertyMapInfo.DiscriminatorField,
                                        discriminatorType.Key);
                    }
                    objectStorages.Add(storage);
                }
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
                Type typeToRestore = mapping.PropertyType;
                if (mapping.IsDiscriminatorSet)
                {
                    var storage = storageItem as IObjectStorage;
                    string key = storage.GetData(mapping.DiscriminatorField).ToString();
                      typeToRestore = mapping.DiscriminatorTypes[key];
                }

                var restoredItem = classMapper.Restore(typeToRestore, (IObjectStorage)storageItem);
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