using System;

namespace Mapper.Configuration
{
    public interface IMapContainer
    {
        IMapConfiguration GetMapperFor(Type type);

        bool IsMappingExists(Type type);
    }
}