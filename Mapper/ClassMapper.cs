using System;
using System.Linq;
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
            var classMap = _mapContainer.GetMappingFor(objectToStore.GetType());
            var objectStorage = _objectStorageFactory.Create();

            foreach (var propInfo in classMap.Mappings)
            {
                IMapper mapper = _mapperRegistry.GetAllMappers().First(x => x.IsMatch(propInfo.Value));

                object o = mapper.Store(propInfo.Value, objectToStore, this);
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
                IPropertyMapInfo mapping = classMap.GetMapping(data.Key);
                object value = data.Value;
                IMapper mapper = _mapperRegistry.GetAllMappers().First(x => x.IsMatch(mapping));
                object obj = mapper.Restore(mapping, value, this);
                mapping.Setter(restoredObject, obj);
            }

            return restoredObject;
        }

        #endregion
    }
}