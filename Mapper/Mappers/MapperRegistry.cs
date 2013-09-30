using System.Collections.Generic;

namespace Mapper.Mappers
{
   public class MapperRegistry : IMapperRegistry
    {
        public IEnumerable<IMapper> GetAllMappers()
        {
            return new List<IMapper> {new ValueMapper(), new ReferenceMapper(), new CollectionMapper(), new NullableMapper()};
        }
    }
}