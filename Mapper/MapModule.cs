using System;
using System.Collections.Generic;

namespace Mapper
{
   public class MapModule: IMapModule
    {
        private readonly Dictionary<Type, IMapConfiguration> _mappings = new Dictionary<Type, IMapConfiguration>();

        protected void Register<TClass, TClassMap>()
            where TClass : class
            where TClassMap : IMapConfiguration, new()
        {
             _mappings.Add(typeof(TClass), new TClassMap());
        }

        public Dictionary<Type, IMapConfiguration> GetAllMappings()
        {
            return _mappings;
        }
    }
}