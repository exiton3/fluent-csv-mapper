using System;
using Mapper.Configuration;

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
            return _mapContainer.IsMappingExists(type);
        }

        #region IClassMapper Members

        public IObjectStorage Store(object memento)
        {
            IMapConfiguration classMap = _mapContainer.GetMapperFor(memento.GetType());
            IObjectStorage objectStorage = _objectStorageFactory.Create();

            foreach (var propInfo in classMap.Mappings)
            {
                object getterValue = propInfo.Value.Getter(memento);
                if (propInfo.Value.IsReferenceProperty)
                {
                    objectStorage.SetData(propInfo.Key, Store(getterValue));
                }
                else
                {
                    if (propInfo.Value.IsValueFormatterSetted)
                    {
                        getterValue = propInfo.Value.ValueFormatter.Format(getterValue);
                    }

                    if (propInfo.Value.IsTypeConverterSetted)
                    {
                        getterValue = propInfo.Value.TypeConverter.Convert(getterValue);
                    }

                    objectStorage.SetData(propInfo.Key, getterValue);
                }
            }

            return objectStorage;
        }

        public object Restore(Type type, IObjectStorage storage)
        {
            IMapConfiguration classMap = _mapContainer.GetMapperFor(type);
            object restoredObject = classMap.Instance;
            foreach (var data in storage.Data)
            {
                IPropertyMapInfo mapping = classMap.GetMapping(data.Key);
                object value = data.Value;
                if (mapping.IsReferenceProperty)
                {
                    object subObj = Restore(mapping.ReferenceType, value as IObjectStorage);
                    mapping.Setter(restoredObject, subObj);
                }
                else
                {
                    if (mapping.IsValueFormatterSetted)
                    {
                        value = mapping.ValueFormatter.Parse((string) data.Value);
                    }

                    if (mapping.IsTypeConverterSetted)
                    {
                        value = mapping.TypeConverter.ConvertBack(data.Value);
                    }

                    mapping.Setter(restoredObject, value);
                }
            }

            return restoredObject;
        }

        #endregion
    }
}