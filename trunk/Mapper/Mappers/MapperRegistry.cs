using System.Collections.Generic;
using System.Linq;
using Mapper.Configuration;

namespace Mapper.Mappers
{
   public class MapperRegistry : IMapperRegistry
    {
       private readonly List<IMapper> _mappers;

       public MapperRegistry()
       {
           _mappers = new List<IMapper>
               {
                   new ValueMapper(),
                   new ReferenceMapper(),
                   new CollectionMapper(),
                   new NullableMapper(),
                   new ArrayMapper(),
                   new DictionaryMapper()
               };

       }
        public IEnumerable<IMapper> GetAllMappers()
        {
            return _mappers;
        }

       public IMapper GetMapper(IPropertyMapInfo propertyMapInfo)
       {
          return _mappers.First(x => x.IsMatch(propertyMapInfo));
       }
    }
}