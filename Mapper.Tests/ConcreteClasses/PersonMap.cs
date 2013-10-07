using Mapper.Configuration;
using Mapper.Converters;

namespace Mapper.Tests.ConcreteClasses
{
    internal class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Map(x => x.Name, "Name");
            Map(x => x.Age, "Age");
            Map(x => x.DoB, "DoB").UseFormatter<DateTimeFormatter>();
            Map(x => x.Numbers, "Phones");
            MapAsReference(x => x.Address, "Address");
            Map(x => x.Gender, "Gender").UseTypeConverter<EnumToIntConverter<Gender>>();
            MapAsNullable(x => x.JobInfo, "JobInfo");
        }
    }
}