using System;

namespace Mapper
{
    public class ClassMapper: IClassMapper
    {
        private readonly IMapContainer _mapContainer;
        private readonly IObjectStorageFactory _objectStorageFactory;

        public ClassMapper(IMapContainer mapContainer, IObjectStorageFactory objectStorageFactory)
        {
            _mapContainer = mapContainer;
            _objectStorageFactory = objectStorageFactory;
        }

        #region IClassMapper Members

        public IObjectStorage Store(object memento)
        {
            var classMap = _mapContainer.GetMapperFor(memento.GetType());
            var objectStorage = _objectStorageFactory.Create();

            foreach (var propInfo in classMap.Mappings)
            {
                var getterValue = propInfo.Value.Getter(memento);
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
                    objectStorage.SetData(propInfo.Key, getterValue);
                }
            }

            return objectStorage;
        }

        public object Restore(Type type, IObjectStorage storage)
        {
            var classMap = _mapContainer.GetMapperFor(type);
            var restoredObject = classMap.Instance;
            foreach (var data in storage.Data)
            {
                var mapping = classMap.GetMapping(data.Key);
                var value = data.Value;
                if (mapping.IsReferenceProperty)
                {
                   var subObj = Restore(mapping.ReferenceType, value as IObjectStorage);
                    mapping.Setter(restoredObject, subObj);
                }
                else
                {
                    if (mapping.IsValueFormatterSetted)
                    {
                        value = mapping.ValueFormatter.Parse((string) data.Value);
                    }
                    mapping.Setter(restoredObject, value);
                }
            }

            return restoredObject;
        }

        #endregion

       
    }

    public interface IObjectStorageFactory
    {
        IObjectStorage Create();
    }
}