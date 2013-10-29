using System;
using System.Collections.Generic;

namespace Mapper.Configuration
{
    public abstract class MapModule : IMapModule
    {
        private readonly Dictionary<Type, Func<IClassMap>> _mappingsFactories = new Dictionary<Type, Func<IClassMap>>();

        public Dictionary<Type, Func<IClassMap>> GetMappings()
        {
            return _mappingsFactories;
        }

        protected void Register<TClass, TClassMap>()
            where TClassMap : IClassMap, new()
        {
            _mappingsFactories.Add(typeof (TClass), () => new TClassMap());
        }
    }
}