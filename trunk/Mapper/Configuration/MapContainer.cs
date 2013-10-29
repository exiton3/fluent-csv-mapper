using System;
using System.Collections.Generic;
using Mapper.Helpers;

namespace Mapper.Configuration
{
    public abstract class MapContainer : IMapContainer
    {
        private readonly Dictionary<Type, IClassMap> _mapConfigurations = new Dictionary<Type, IClassMap>();
        private readonly List<IMapModule> _modules = new List<IMapModule>();
        private bool _wasBuild;

        public IClassMap GetMappingFor(Type type)
        {
            if (!_wasBuild)
            {
                throw new InvalidOperationException("MapContainer was not build");
            }

            IClassMap mapping;
            if (_mapConfigurations.TryGetValue(type, out mapping))
            {
                return mapping;
            }
            throw new InvalidOperationException(string.Format("Mapping class {0} was not found in container", type.Name));
        }

        public bool IsMappingExists(Type type)
        {
            Check.NotNull(type, "type");

            return _mapConfigurations.ContainsKey(type);
        }

        public void Build()
        {
            foreach (IMapModule mapModule in _modules)
            {
                RegisterMappings(mapModule);
            }
            _wasBuild = true;
        }

        protected void RegisterModule<TModule>() where TModule : IMapModule, new()
        {
            var module = new TModule();
            _modules.Add(module);
        }

        private void RegisterMappings<TModule>(TModule module) where TModule : IMapModule
        {
            foreach (var mapping in module.GetMappings())
            {
                _mapConfigurations.Add(mapping.Key, mapping.Value());
            }
        }
    }
}