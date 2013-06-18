using System;

namespace Mapper
{
    public interface IMapRegistrationContainer
    {
        IMapConfiguration GetMapperFor(Type type);
    }
}