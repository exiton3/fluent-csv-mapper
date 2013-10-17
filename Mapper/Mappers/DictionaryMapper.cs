using System;
using System.Collections;
using System.Collections.Generic;
using Mapper.Configuration;
using Mapper.Helpers;

namespace Mapper.Mappers
{
    internal class DictionaryMapper : IMapper
    {
        private static readonly Type KvpType = typeof(KeyValuePair<,>);

        public object Store(IPropertyMapInfo propertyMapInfo, object objectToStore, IClassMapper classMapper)
        {
            object getterValue = propertyMapInfo.Getter(objectToStore);
            Type keyType = TypeHelper.GetDictionaryKeyType(propertyMapInfo.PropertyType);
            Type valueType = TypeHelper.GetDictionaryValueType(propertyMapInfo.PropertyType);

            var sourceDictionaryType = KvpType.MakeGenericType(keyType,valueType);

            var objectStorages = new Dictionary<string, IObjectStorage>();

            if (getterValue != null)
            {
                var enumerable = getterValue as IEnumerable;
                var source = enumerable;
                foreach (object keyValuePair in source)
                {
                    var sourceKey = sourceDictionaryType.GetProperty("Key").GetValue(keyValuePair, new object[0]);
                    var sourceValue = sourceDictionaryType.GetProperty("Value").GetValue(keyValuePair, new object[0]);

                    var storage = classMapper.Store(sourceValue);
                    objectStorages.Add(sourceKey.ToString(), storage);
                }
            }
            return objectStorages;
        }

        public object Restore(IPropertyMapInfo mapping, object value, IClassMapper classMapper)
        {
            Type dictType = mapping.PropertyType;
            var valueType = TypeHelper.GetDictionaryValueType(dictType);

            var destinationDict = Activator.CreateInstance(dictType);

            var source = ((IEnumerable) value ?? new Dictionary<string, IObjectStorage>());

            var sourceDict = (Dictionary<string, IObjectStorage>) source;

            foreach (var storageItem in sourceDict)
            {
                var restoredItem = classMapper.Restore(valueType, storageItem.Value);

                dictType.GetMethod("Add").Invoke(destinationDict,  new[]{ storageItem.Key, restoredItem});
            }
            return destinationDict;
        }

        public bool IsMatch(IPropertyMapInfo propertyMapInfo)
        {
            return propertyMapInfo.PropertyKind == PropertyKind.Dictionary;
        }
    }
}