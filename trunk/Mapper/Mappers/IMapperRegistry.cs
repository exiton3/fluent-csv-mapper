using System.Collections.Generic;
using Mapper.Configuration;

namespace Mapper.Mappers
{
    public interface IMapperRegistry
    {
        IEnumerable<IMapper> GetAllMappers();
        IMapper GetMapper(IPropertyMapInfo propertyMapInfo);
    }
}