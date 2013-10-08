using System.Collections.Generic;

namespace Mapper.Tests.ConcreteClasses
{
    class Department
    {
        public List<Person> Persons { get; set; } 
        public Person[] Persons2 { get; set; } 
    }
}