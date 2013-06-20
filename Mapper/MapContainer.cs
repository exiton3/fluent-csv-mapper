using System;
using System.Collections.Generic;

namespace Mapper
{
    public class MapContainer : IMapContainer
    {
        private readonly Dictionary<Type, IMapConfiguration> _mappings = new Dictionary<Type, IMapConfiguration>();

        public IMapConfiguration GetMapperFor(Type type)
        {
            IMapConfiguration mapping;
            if (_mappings.TryGetValue(type, out mapping))
            {
                return mapping;
            }
            throw new InvalidOperationException(string.Format("Mapping class {0} was not found", type.Name));
        }

        protected void Register<TClass,TClassMap>() where TClass:class where TClassMap:IMapConfiguration, new()
        {
            _mappings.Add(typeof (TClass), new TClassMap());
        }
    }
}