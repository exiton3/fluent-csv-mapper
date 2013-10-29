using System;
using System.Collections.Generic;

namespace Mapper.Configuration
{
    public interface IMapModule
    {
        Dictionary<Type, Func<IClassMap>> GetMappings();
    }
}