using System;

namespace Mapper.Configuration
{
    public interface IMapContainer
    {
        IClassMap GetMappingFor(Type type);

        bool IsMappingExists(Type type);
        void Build();
    }
}