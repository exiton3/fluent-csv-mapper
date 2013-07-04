using System;
using System.Collections.Generic;

namespace Mapper.Configuration
{
   public abstract class MapModule: IMapModule
    {
        private readonly Dictionary<Type, IClassMap> _mappings = new Dictionary<Type, IClassMap>();

        protected void Register<TClass, TClassMap>()
            where TClass : class
            where TClassMap : IClassMap, new()
        {
             _mappings.Add(typeof(TClass), new TClassMap());
        }

        public Dictionary<Type, IClassMap> GetAllMappings()
        {
            return _mappings;
        }
    }
}