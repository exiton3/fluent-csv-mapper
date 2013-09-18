using Mapper.Configuration;

namespace Mapper.Tests.ConcreteClasses
{
    class DepartamentMap:ClassMap<Department>
    {
        public DepartamentMap()
        {
            MapAsCollection(x => x.Persons, "Persons");
        } 
    }
}