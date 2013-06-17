namespace Mapper.Tests
{
    internal class TestFactory : MapFactory
    {
        public TestFactory()
        {
            Add<Person, PersonMap>();
            Add<Address, AddressMap>();
        }
    }
}