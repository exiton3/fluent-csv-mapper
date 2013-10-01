using Mapper.Configuration;

namespace Mapper.Tests.ConcreteClasses
{
    internal class TestMapModule : MapModule
    {
        public TestMapModule()
        {
            Register<Person, PersonMap>();
            Register<Address, AddressMap>();
            Register<JobInfo, JobInfoMap>();
        }
    }
}