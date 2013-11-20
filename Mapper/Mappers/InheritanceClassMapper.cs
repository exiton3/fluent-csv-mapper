using System;
using System.Linq;
using Mapper.Configuration;

namespace Mapper.Mappers
{
    class InheritanceClassMapper : IClassMapper
    {
        private readonly IClassMapper _classMapper;
        private readonly IPropertyMapInfo _propertyMapInfo;
        public InheritanceClassMapper(IClassMapper classMapper, IPropertyMapInfo propertyMapInfo)
        {
            _classMapper = classMapper;
            _propertyMapInfo = propertyMapInfo;
        }

        public IObjectStorage Store(object objectToStore)
        {
            var storage = _classMapper.Store(objectToStore);

            var discriminatorType = _propertyMapInfo.DiscriminatorTypes.FirstOrDefault(x => x.Value == objectToStore.GetType());
            storage.SetData(_propertyMapInfo.DiscriminatorField, discriminatorType.Key);

            return storage;
        }

        public object Restore(Type type, IObjectStorage storage)
        {
            string key = storage.GetData(_propertyMapInfo.DiscriminatorField).ToString();
            var typeToRestore = _propertyMapInfo.DiscriminatorTypes[key];

            return _classMapper.Restore(typeToRestore, storage);
        }

        public bool CanMap(Type type)
        {
            return _classMapper.CanMap(type);
        }
    }

   static class ClassMapperExt
    {
         public static IClassMapper GetClassMapper(this IClassMapper classMapper, IPropertyMapInfo propertyMapInfo)
         {
             if (propertyMapInfo.IsDiscriminatorSet)
             {
                 return new InheritanceClassMapper(classMapper,propertyMapInfo);
             }
             return classMapper;
         }
    }
}