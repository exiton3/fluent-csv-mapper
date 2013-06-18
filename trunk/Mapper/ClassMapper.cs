using System;

namespace Mapper
{
    public class ClassMapper: IClassMapper
      
    {
        private readonly IMapFactory _mapFactory;

        public ClassMapper(IMapFactory mapFactory)
        {
            _mapFactory = mapFactory;
        }

        #region IClassMapper Members

        public DynamicVariantType Store(object memento)
        {
            var classMap = _mapFactory.GetMapperFor(memento.GetType());
            var dynamicVariantType = new DynamicVariantType();

            foreach (var propInfo in classMap.Mappings)
            {
                var getterValue = propInfo.Value.Getter(memento);
                if (propInfo.Value.IsReferenceProperty)
                {
                    dynamicVariantType[propInfo.Key] = Store(getterValue);
                }
                else
                {
                    if (propInfo.Value.IsValueFormatterSetted)
                    {
                        getterValue = propInfo.Value.ValueFormatter.Format(getterValue);
                    }
                    dynamicVariantType[propInfo.Key] = getterValue;
                }
            }

            return dynamicVariantType;
        }

        public object Restore(Type type, DynamicVariantType storage)
        {
            var classMap = _mapFactory.GetMapperFor(type);
            var restoredObject = classMap.Instance;
            foreach (var data in storage.Data)
            {
                var mapping = classMap.GetMapping(data.Key);
                var value = data.Value;
                if (mapping.IsReferenceProperty)
                {
                   var subObj = Restore(mapping.ReferenceType, value as DynamicVariantType);
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
}