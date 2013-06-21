using Mapper.Configuration;

namespace Mapper.Tests.ConcreteClasses
{
    class DepartamentMap:ClassMap<Department>
    {
        public DepartamentMap()
        {
            MapAsReference(x => x.Persons, "Persons");
        } 
    }
}