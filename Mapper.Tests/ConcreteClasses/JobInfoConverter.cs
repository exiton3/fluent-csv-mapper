using Mapper.Converters;

namespace Mapper.Tests.ConcreteClasses
{
    class JobInfoConverter:TypeConverter<JobInfo,JobInfoClass>
    {
        protected override JobInfoClass Convert(JobInfo source)
        {
            return new JobInfoClass
                {
                    Position = source.Position + "Class",
                    Salary = source.Salary*10
                };
        }

        protected override JobInfo ConvertBack(JobInfoClass source)
        {
            return new JobInfo
                {
                    Position = source.Position.Substring(0,source.Position.Length - 5),
                    Salary = source.Salary / 10
                };
        }
    }
}