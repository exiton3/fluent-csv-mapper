using System;
using System.Collections.Generic;

namespace Mapper
{
    public interface IMapModule
    {
        Dictionary<Type, IMapConfiguration> GetAllMappings();
    }
}