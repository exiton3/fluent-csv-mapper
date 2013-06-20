namespace Mapper.Tests
{
    internal class TestContainer : MapContainer
    {
        public TestContainer()
        {
            Register<Person, PersonMap>();
            Register<Address, AddressMap>();
        }
    }
}