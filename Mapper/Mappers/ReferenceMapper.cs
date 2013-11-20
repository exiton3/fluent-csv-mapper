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

        public object Restore(IPropertyMapInfo propertyMapInfo, object value, IClassMapper classMapper)
        {
            Type typeToRestore = propertyMapInfo.PropertyType;
            object restoredObject;
            if (propertyMapInfo.IsTypeConverterSet)
            {
                typeToRestore = propertyMapInfo.TypeConverter.DestinationType;
                restoredObject = classMapper.Restore(typeToRestore, value as IObjectStorage);

                return propertyMapInfo.TypeConverter.ConvertBack(restoredObject);
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