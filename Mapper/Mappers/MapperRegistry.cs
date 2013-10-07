using System.Collections.Generic;

namespace Mapper.Mappers
{
   public class MapperRegistry : IMapperRegistry
    {
       private readonly List<IMapper> _mappers;

       public MapperRegistry()
       {
           _mappers = new List<IMapper> { new ValueMapper(), new ReferenceMapper(), new CollectionMapper(), new NullableMapper() };
           
       }
        public IEnumerable<IMapper> GetAllMappers()
        {
            return _mappers;
        }
    }
}