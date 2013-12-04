using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            var sourceKvpType = KvpType.MakeGenericType(keyType,valueType);

            Type dictType = typeof (Dictionary<,>).MakeGenericType(keyType, typeof (IObjectStorage));
            var dictInstance = Activator.CreateInstance(dictType) as IDictionary;
            if (getterValue != null)
            {
                var source = getterValue as IEnumerable;

                foreach (object keyValuePair in source)
                {
                    var sourceKey = sourceKvpType.GetProperty("Key").GetValue(keyValuePair, new object[0]);
                    var sourceValue = sourceKvpType.GetProperty("Value").GetValue(keyValuePair, new object[0]);

                    var storage = classMapper.Store(sourceValue);
                    dictInstance.Add(sourceKey, storage);
                }
            }
            return dictInstance;
        }

        public object Restore(IPropertyMapInfo propertyMapInfo, object value, IClassMapper classMapper)
        {
            Type dictType = propertyMapInfo.PropertyType;
            var valueType = TypeHelper.GetDictionaryValueType(dictType);

            var destinationDict = Activator.CreateInstance(dictType);

            if (value != null)
            {
                var sourceDict = (IDictionary) value;
                int i = 0;
                var keys = sourceDict.Keys.Cast<object>().ToList();
                var values = sourceDict.Values.Cast<IObjectStorage>().ToList();
                foreach (var key in keys)
                {
                    var restoredItem = classMapper.Restore(valueType, values[i]);
                    dictType.GetMethod("Add").Invoke(destinationDict, new[] {key, restoredItem});
                    i++;
                }
            }
            return destinationDict;
        }

        public bool IsMatch(IPropertyMapInfo propertyMapInfo)
        {
            return propertyMapInfo.PropertyKind == PropertyKind.Dictionary;
        }
    }
}