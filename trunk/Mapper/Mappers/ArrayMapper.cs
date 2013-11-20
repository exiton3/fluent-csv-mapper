using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mapper.Configuration;

namespace Mapper.Mappers
{
    class ArrayMapper : IMapper
    {
        public object Store(IPropertyMapInfo propertyMapInfo, object objectToStore, IClassMapper classMapper)
        {
            object getterValue = propertyMapInfo.Getter(objectToStore);
            var objectStorages = new List<IObjectStorage>();
            if (getterValue != null)
            {
                foreach (var obj in (IEnumerable) getterValue)
                {
                    var storage = classMapper.Store(obj);
                    objectStorages.Add(storage);
                }
            }
            return objectStorages;
        }

        public object Restore(IPropertyMapInfo propertyMapInfo, object value, IClassMapper classMapper)
        {
            var sourceValues = ((IEnumerable) value?? new IObjectStorage[0]).Cast<IObjectStorage>().ToList();
            var array = Array.CreateInstance(propertyMapInfo.PropertyType, sourceValues.Count);
            int i = 0;
            foreach (var storageItem in sourceValues)
            {
                var restoredItem = classMapper.Restore(propertyMapInfo.PropertyType, storageItem);
                array.SetValue(restoredItem, i);
                i++;
            }
            return array;
        }

        public bool IsMatch(IPropertyMapInfo propertyMapInfo)
        {
            return propertyMapInfo.PropertyKind == PropertyKind.Array;
        }
    }
}