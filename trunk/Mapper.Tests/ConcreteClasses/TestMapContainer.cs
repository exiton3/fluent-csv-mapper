namespace Mapper.Tests.ConcreteClasses
{
   public class TestMapContainer : MapContainer
    {
        public TestMapContainer()
        {
            RegisterModule<TestMapModule>();
            RegisterModule<DepartmentModule>();
        }
    }
}