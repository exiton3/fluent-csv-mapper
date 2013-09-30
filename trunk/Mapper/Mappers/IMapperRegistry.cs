using System.Collections.Generic;

namespace Mapper.Mappers
{
    public interface IMapperRegistry
    {
        IEnumerable<IMapper> GetAllMappers();
    }
}