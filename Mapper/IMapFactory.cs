using System;

namespace Mapper
{
    public interface IMapFactory
    {
        IMapConfiguration GetMapperFor(Type type);
    }
}