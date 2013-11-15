namespace Mapper.Tests.ConcreteClasses
{
    class ManagerMap:PersonMap<Manager>
    {
        public ManagerMap()
        {
            Map(x => x.Salary,"Salary");
        }    
    }
}