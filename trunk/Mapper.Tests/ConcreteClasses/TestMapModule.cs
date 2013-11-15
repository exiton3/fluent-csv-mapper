using Mapper.Configuration;

namespace Mapper.Tests.ConcreteClasses
{
    internal class TestMapModule : MapModule
    {
        public TestMapModule()
        {
            Register<Person, PersonMap<Person>>();
            Register<Manager, ManagerMap>();
            Register<Address, AddressMap>();
            Register<JobInfoClass, JobInfoMap>();
        }
    }
}