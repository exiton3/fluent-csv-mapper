namespace Mapper.Tests
{
    internal class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Map(x => x.Name, "Name");
            Map(x => x.Age, "Age");
            Map(x => x.DoB, "DoB").UseFormatter<DateTimeFormatter>();
            Map(x => x.Numbers, "Phones");
            Map(x => x.Address, "Address").UseMapping<AddressMap>();
        }

       
    }
}