using System;
using Mapper.Configuration;

namespace Mapper.Mappers
{
    internal class ReferenceMapper : IMapper
    {
        public object Store(IPropertyMapInfo propertyMapInfo, object objectToStore, IClassMapper classMapper)
        {
            object getterValue = propertyMapInfo.Getter(objectToStore);
            if (propertyMapInfo.IsTypeConverterSet)
            {
                object convertedObj = propertyMapInfo.TypeConverter.Convert(getterValue);
                return classMapper.Store(convertedObj);
            }

            return classMapper.Store(getterValue);
        }

        public object Restore(IPropertyMapInfo mapping, object value, IClassMapper classMapper)
        {
            Type typeToRestore = mapping.PropertyType;
            object restoredObject;
            if (mapping.IsTypeConverterSet)
            {
                typeToRestore = mapping.TypeConverter.DestinationType;
                restoredObject = classMapper.Restore(typeToRestore, value as IObjectStorage);

                return mapping.TypeConverter.ConvertBack(restoredObject);
            }

            if (mapping.IsDiscriminatorSet)
            {
                var storage = value as IObjectStorage;
                string key = storage.GetData(mapping.DiscriminatorField).ToString();
                typeToRestore = mapping.DiscriminatorTypes[key];
            }

            restoredObject = classMapper.Restore(typeToRestore, value as IObjectStorage);
            return restoredObject;
        }

        public bool IsMatch(IPropertyMapInfo propertyMapInfo)
        {
            return propertyMapInfo.PropertyKind == PropertyKind.Reference;
        }
    }
}