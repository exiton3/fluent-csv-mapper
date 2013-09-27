using System;
using System.Collections;
using System.Collections.Generic;
using Mapper.Configuration;
using Mapper.Helpers;

namespace Mapper
{
    public class ClassMapper : IClassMapper
    {
        private readonly IMapContainer _mapContainer;
        private readonly IObjectStorageFactory _objectStorageFactory;

        public ClassMapper(IMapContainer mapContainer, IObjectStorageFactory objectStorageFactory)
        {
            _mapContainer = mapContainer;
            _objectStorageFactory = objectStorageFactory;
        }

        public bool CanMap(Type type)
        {
            Check.NotNull(type, "type");

            return _mapContainer.IsMappingExists(type);
        }

        #region IClassMapper Members

        public IObjectStorage Store(object objectToStore)
        {
            Check.NotNull(objectToStore,"objectToStore");

            var classMap = _mapContainer.GetMappingFor(objectToStore.GetType());
            var objectStorage = _objectStorageFactory.Create();

            foreach (var propInfo in classMap.Mappings)
            {
                var getterValue = propInfo.Value.Getter(objectToStore);
                switch (propInfo.Value.PropertyKind)
                {
                    case PropertyKind.Value:
                        {
                            if (propInfo.Value.IsValueFormatterSet)
                            {
                                getterValue = propInfo.Value.ValueFormatter.Format(getterValue);
                            }

                            if (propInfo.Value.IsTypeConverterSet)
                            {
                                getterValue = propInfo.Value.TypeConverter.Convert(getterValue);
                            }

                            objectStorage.SetData(propInfo.Key, getterValue);
                        }
                        break;
                    case PropertyKind.Reference:
                        {
                            objectStorage.SetData(propInfo.Key, Store(getterValue));
                        }
                        break;
                    case PropertyKind.Collection:
                        {
                            var objectStorages = new List<IObjectStorage>();
                            foreach (var obj in (IEnumerable)getterValue)
                            {
                                var storage = Store(obj);
                                objectStorages.Add(storage);
                            }
                            objectStorage.SetData(propInfo.Key, objectStorages);
                        }
                        break;
                    case PropertyKind.Nullable:
                        {
                            objectStorage.SetData(propInfo.Key, Store(getterValue));
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }

            return objectStorage;
        }

        public object Restore(Type type, IObjectStorage storage)
        {
            Check.NotNull(storage, "storage");
            Check.NotNull(type, "type");

            var classMap = _mapContainer.GetMappingFor(type);
            var restoredObject = classMap.Instance;
            foreach (var data in storage.Data)
            {
                if (!classMap.IsMappingForPropertyExist(data.Key))
                {
                    continue;
                }
                var mapping = classMap.GetMapping(data.Key);
                var value = data.Value;
                switch (mapping.PropertyKind)
                {
                    case PropertyKind.Value:
                        {
                            if (mapping.IsValueFormatterSet)
                            {
                                value = mapping.ValueFormatter.Parse((string)data.Value);
                            }

                            if (mapping.IsTypeConverterSet)
                            {
                                value = mapping.TypeConverter.ConvertBack(data.Value);
                            }

                            mapping.Setter(restoredObject, value);
                        }
                        break;
                    case PropertyKind.Reference:
                        {
                            var subObj = Restore(mapping.PropertyType, value as IObjectStorage);
                            mapping.Setter(restoredObject, subObj);
                        }
                        break;
                    case PropertyKind.Collection:
                        {
                            var collectionType = typeof(List<>);
                            var genericType = collectionType.MakeGenericType(mapping.PropertyType);
                            var objectList = (IList)Activator.CreateInstance(genericType);

                            foreach (var storageItem in value as IEnumerable)
                            {
                                var restoredItem = Restore(mapping.PropertyType, (IObjectStorage)storageItem);
                                objectList.Add(restoredItem);
                            }
                            mapping.Setter(restoredObject, objectList);
                        }
                        break;
                    case PropertyKind.Nullable:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return restoredObject;
        }

        #endregion
    }
}