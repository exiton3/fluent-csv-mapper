using System;

namespace Mapper.Configuration
{
    public class EnumToIntConverter<TEnum> : TypeConverter<TEnum, int>
    {
        protected override int Convert(TEnum source)
        {
            if (!typeof (TEnum).IsEnum)
            {
                throw new ArgumentException("The generic type must be Enum type");
            }

            var convert = System.Convert.ToInt32(source);
            return convert;
        }

        protected override TEnum ConvertBack(int source)
        {
            if (!Enum.IsDefined(typeof (TEnum), source))
            {
                throw new ArgumentOutOfRangeException("source");
            }
            return (TEnum) Enum.ToObject(typeof (TEnum), source);
        }
    }
}