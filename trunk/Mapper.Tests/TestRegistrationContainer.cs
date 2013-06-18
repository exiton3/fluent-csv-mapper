namespace Mapper.Tests
{
    internal class TestRegistrationContainer : MapRegistrationContainer
    {
        public TestRegistrationContainer()
        {
            Register<Person, PersonMap>();
            Register<Address, AddressMap>();
        }
    }
}