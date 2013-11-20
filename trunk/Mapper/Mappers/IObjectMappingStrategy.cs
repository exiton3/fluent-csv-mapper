using System.Linq;
using Mapper.Configuration;

namespace Mapper.Mappers
{
    internal interface IObjectMappingStrategy
    {
        IObjectStorage Store(IPropertyMapInfo propertyMapInfo, object objectToStore);
        object Restore(IPropertyMapInfo propertyMapInfo, IObjectStorage storage);
    }

    class InheritanceObjectMappingStrategy : IObjectMappingStrategy
    {
        private readonly IClassMapper _classMapper;

        public InheritanceObjectMappingStrategy(IClassMapper classMapper)
        {
            _classMapper = classMapper;
        }

        public IObjectStorage Store(IPropertyMapInfo propertyMapInfo, object objectToStore)
        {
            var storage = _classMapper.Store(objectToStore);

            var discriminatorType = propertyMapInfo.DiscriminatorTypes.FirstOrDefault(x => x.Value == objectToStore.GetType());
            storage.SetData(propertyMapInfo.DiscriminatorField, discriminatorType.Key);

            return storage;
        }

        public object Restore(IPropertyMapInfo propertyMapInfo, IObjectStorage storage)
        {
            string key = storage.GetData(propertyMapInfo.DiscriminatorField).ToString();
            var typeToRestore = propertyMapInfo.DiscriminatorTypes[key];

            return _classMapper.Restore(typeToRestore, storage);
        }
    }

    class SimpleObjectMappingStrategy : IObjectMappingStrategy
    {
        private readonly IClassMapper _classMapper;

        public SimpleObjectMappingStrategy(IClassMapper classMapper)
        {
            _classMapper = classMapper;
        }

        public IObjectStorage Store(IPropertyMapInfo propertyMapInfo, object objectToStore)
        {
            return _classMapper.Store(objectToStore);
        }

        public object Restore(IPropertyMapInfo propertyMapInfo, IObjectStorage storage)
        {
            return _classMapper.Restore(propertyMapInfo.PropertyType, storage);
        }
    }
}