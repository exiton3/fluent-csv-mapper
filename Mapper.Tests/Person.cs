using System;
using System.Collections.Generic;

namespace Mapper.Tests
{
    internal class Person
    {
        public Person()
        {
            Numbers = new List<int>();
        }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime DoB { get; set; }
        public List<int> Numbers { get; set; }

        public Address Address { get; set; }
    }
}