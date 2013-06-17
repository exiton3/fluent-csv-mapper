namespace Mapper.Tests
{
    class AddressMap:ClassMap<Address>
    {
        public AddressMap()
        {
            Map(x => x.Code, "Code");
            Map(x => x.Number, "House");
            Map(x => x.Street, "Street");
        }
    }
}