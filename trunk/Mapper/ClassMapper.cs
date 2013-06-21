using System;

namespace Mapper
{
    public class ClassMapper: IClassMapper
      
    {
        private readonly IMapContainer _mapContainer;

        public ClassMapper(IMapContainer mapContainer)
        {
            _mapContainer = mapContainer;
        }

        #region IClassMapper Members

        public ObjectStorage Store(object memento)
        {
            var classMap = _mapContainer.GetMapperFor(memento.GetType());
            var objectStorage = new ObjectStorage();

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

        public object Restore(Type type, ObjectStorage storage)
        {
            var classMap = _mapContainer.GetMapperFor(type);
            var restoredObject = classMap.Instance;
            foreach (var data in storage.Data)
            {
                var mapping = classMap.GetMapping(data.Key);
                var value = data.Value;
                if (mapping.IsReferenceProperty)
                {
                   var subObj = Restore(mapping.ReferenceType, value as ObjectStorage);
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