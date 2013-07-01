using Mapper.Configuration;

namespace Mapper.Tests.ConcreteClasses
{
    internal class EnumToIntConverter : TypeConverter<Gender, int>
    {
        protected override int Convert(Gender source)
        {
            return (int) source;
        }

        protected override Gender ConvertBack(int source)
        {
            return (Gender) source;
        }
    }
}