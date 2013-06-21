namespace Mapper.Tests.ConcreteClasses
{
    class DepartmentModule:MapModule
    {
        public DepartmentModule()
        {
            Register<Department, DepartamentMap>();
        }
       
    }
}