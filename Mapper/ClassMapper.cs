using System;
using Mapper.Configuration;
using Mapper.Helpers;
using Mapper.Mappers;

namespace Mapper
{
    public class ClassMapper : IClassMapper
    {
        private readonly IMapContainer _mapContainer;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IObjectStorageFactory _objectStorageFactory;

        public ClassMapper(IMapContainer mapContainer,
                           IObjectStorageFactory objectStorageFactory,
                           IMapperRegistry mapperRegistry)
        {
            _mapContainer = mapContainer;
            _objectStorageFactory = objectStorageFactory;
            _mapperRegistry = mapperRegistry;
        }

        public bool CanMap(Type type)
        {
            Check.NotNull(type, "type");

            return _mapContainer.IsMappingExists(type);
        }

        #region IClassMapper Members

        public IObjectStorage Store(object objectToStore)
        {
            Check.NotNull(objectToStore, "objectToStore");
            IClassMap classMap = _mapContainer.GetMappingFor(objectToStore.GetType());
            IObjectStorage objectStorage = _objectStorageFactory.Create();

            foreach (var propInfo in classMap.Mappings)
            {
                IMapper mapper = _mapperRegistry.GetMapper(propInfo.Value);
                object o = mapper.Store(propInfo.Value, objectToStore, this.GetClassMapper(propInfo.Value));

                objectStorage.SetData(propInfo.Key, o);
            }

            return objectStorage;
        }

        public object Restore(Type type, IObjectStorage storage)
        {
            Check.NotNull(storage, "storage");
            Check.NotNull(type, "type");

            IClassMap classMap = _mapContainer.GetMappingFor(type);
            object restoredObject = classMap.Instance;

            foreach (var data in storage.Data)
            {
                if (!classMap.IsMappingForPropertyExist(data.Key))
                {
                    continue;
                }
                IPropertyMapInfo propertyMapInfo = classMap.GetMapping(data.Key);
                object value = data.Value;
                IMapper mapper = _mapperRegistry.GetMapper(propertyMapInfo);

                try
                {
                    object obj = mapper.Restore(propertyMapInfo, value, this.GetClassMapper(propertyMapInfo));
              
                    propertyMapInfo.Setter(restoredObject, obj);
                }
                catch (Exception e)
                {
                    throw new MapperMappingException(string.Format("Cannot restore property {0} for type {1}", data.Key,type.Name), e);
                }
            }

            return restoredObject;
        }

        #endregion
    }
}