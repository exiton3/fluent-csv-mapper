using Mapper.Configuration;

namespace Mapper.Tests.ConcreteClasses
{
    class DepartmentMap:ClassMap<Department>
    {
        public DepartmentMap()
        {
            MapAsCollection(x => x.Persons, "Persons")
               .DiscriminateOnField("Type")
               .DiscriminatorValueFor<Person>("Person")
               .DiscriminatorValueFor<Manager>("Manager");
            MapAsCollection(x => x.Persons2, "PersonsArray");
            MapAsDictionary(x => x.PersonsPerGroup, "PersonsPerGroup");
        } 
    }
}