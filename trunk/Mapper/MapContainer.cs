using System;
using System.Collections.Generic;

namespace Mapper
{
    public abstract class MapContainer : IMapContainer
    {
        private readonly Dictionary<Type, IMapConfiguration> _mapConfigurations = new Dictionary<Type, IMapConfiguration>();

        protected void RegisterModule<TModule>() where TModule : IMapModule, new()
        {
            var module = new TModule();
            RegisterMappings(module);
        }

        private void RegisterMappings<TModule>(TModule module) where TModule : IMapModule, new()
        {
            foreach (var mapping in module.GetAllMappings())
            {
                _mapConfigurations.Add(mapping.Key, mapping.Value);
            }
        }

        public IMapConfiguration GetMapperFor(Type type)
        {
            IMapConfiguration mapping;
            if (_mapConfigurations.TryGetValue(type, out mapping))
            {
                return mapping;
            }
            throw new InvalidOperationException(string.Format("Mapping class {0} was not found", type.Name));
        }

        public bool IsMappingExists(Type type)
        {
           return _mapConfigurations.ContainsKey(type);
        }
    }
}