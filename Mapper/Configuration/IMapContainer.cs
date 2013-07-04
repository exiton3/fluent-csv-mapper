using System;

namespace Mapper.Configuration
{
    public interface IMapContainer
    {
        IClassMap GetMapperFor(Type type);

        bool IsMappingExists(Type type);
    }
}