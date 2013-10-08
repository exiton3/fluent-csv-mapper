using Mapper.Configuration;

namespace Mapper.Tests.ConcreteClasses
{
    class DepartmentMap:ClassMap<Department>
    {
        public DepartmentMap()
        {
            MapAsCollection(x => x.Persons, "Persons");
            MapAsCollection(x => x.Persons2, "PersonsArray");
        } 
    }
}