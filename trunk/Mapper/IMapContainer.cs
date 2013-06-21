using System;

namespace Mapper
{
    public interface IMapContainer
    {
        IMapConfiguration GetMapperFor(Type type);

        bool IsMappingExists(Type type);
    }
}