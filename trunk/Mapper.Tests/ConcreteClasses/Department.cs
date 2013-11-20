using System.Collections.Generic;

namespace Mapper.Tests.ConcreteClasses
{
    class Department
    {
        public Department()
        {
            Persons = new List<Person>();
        }

        public Person Person { get; set; }

        public List<Person> Persons { get; set; } 
        public Person[] Persons2 { get; set; }
        public Dictionary<string,Person> PersonsPerGroup { get; set; }
    }
}