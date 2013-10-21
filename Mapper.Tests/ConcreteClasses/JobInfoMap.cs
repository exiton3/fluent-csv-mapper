using Mapper.Configuration;

namespace Mapper.Tests.ConcreteClasses
{
    class JobInfoMap : ClassMap<JobInfoClass>
    {
        public JobInfoMap()
        {
            Map(x => x.Position, "Position");
            Map(x => x.Salary, "Salary");
        }
    }
}