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
            object keyObject = storage.GetData(_propertyMapInfo.DiscriminatorField);
            string key;
            if (keyObject == null)
            {
                throw new MapperMappingException(string.Format("Discriminator field {0} was not found in storage for type {1} {2}",_propertyMapInfo.DiscriminatorField, type,_propertyMapInfo.Getter.ToString()),_propertyMapInfo.Getter.ToString());
            }

            key = keyObject.ToString();

            Type typeToRestore;
            if (_propertyMapInfo.DiscriminatorTypes.TryGetValue(key, out typeToRestore))
            {
                typeToRestore = _propertyMapInfo.DiscriminatorTypes[key];
            }
            else
            {
                throw new MapperMappingException(
                    string.Format("The discriminator value {0} in type {1} ", key, type.Name),
                    _propertyMapInfo.Getter.ToString());
            }
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