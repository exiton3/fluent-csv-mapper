﻿using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Mapper.Tests
{
    [TestFixture]
    public class ClassMapperTests
    {
        private static TestFactory _mapFactory;

        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void RestoreInstanceFromDynamicVariantType()
        {
            var dynamicVariantType = new DynamicVariantType();
            dynamicVariantType["Name"] = "Sergey";
            dynamicVariantType["Age"] = 28;
            var dateTime = DateTime.Now;
            dynamicVariantType["DoB"] = dateTime.ToShortDateString();
            var numbers = new List<int> {1, 2, 3, 4, 5};
            dynamicVariantType["Phones"] = numbers;

            var translator = CreateTranslator();

            var restoredObject = translator.Restore(typeof(Person),dynamicVariantType);

            Assert.IsInstanceOf<Person>(restoredObject);
           var person = (Person) restoredObject;
            Assert.That(person.Age, Is.EqualTo(28));
            Assert.That(person.Name, Is.EqualTo("Sergey"));
            Assert.That(person.DoB, Is.EqualTo(dateTime.Date));
            Assert.That(person.Numbers, Is.EqualTo(numbers));
        }

        private static ClassMapper CreateTranslator()
        {
            if (_mapFactory == null)
            {
                _mapFactory = new TestFactory();
                Console.WriteLine("Factory created");
            }

            return new ClassMapper(_mapFactory);
        }

        [Test]
        public void StoreInstanceInDynamicVariantTypeAccordingToMapping()
        {
            var dateTime = DateTime.Now;
            var numbers = new List<int> {1, 2, 3, 4, 5};
            var person = new Person {Age = 28, Name = "John", DoB = dateTime,Numbers = numbers, Address = new Address()};


            var translator = CreateTranslator();

            var dvt = translator.Store(person);

            Assert.That(dvt.Data["Name"], Is.EqualTo("John"));
            Assert.That(dvt.Data["Age"], Is.EqualTo(28));
            Assert.That(dvt.Data["Phones"], Is.EqualTo(numbers));
        }

        [Test]
        public void StorePropertyInDifferentTypeInDynamicVariantTypeAccordingToMapping()
        {
            var dateTime = DateTime.Now;
            var person = new Person { Age = 28, Name = "John", DoB = dateTime, Address = new Address()};


            var translator = CreateTranslator();

            var dvt = translator.Store(person);

            Assert.That(dvt.Data["DoB"], Is.EqualTo(dateTime.ToShortDateString()));
        }

        [Test]
        public void StoreReferenceProperty()
        {
            var dateTime = DateTime.Now;
            var address = new Address {Street = "some", Number = 123};
            var person = new Person
                             {
                                 Age = 28,
                                 Name = "John",
                                 DoB = dateTime,
                                 Address = address
                             };


            var translator = CreateTranslator();

            var dvt = translator.Store(person);

            var addressDynamic = dvt.Data["Address"] as DynamicVariantType;
            Assert.That(addressDynamic,Is.Not.Null);
            Assert.That(addressDynamic["Street"], Is.EqualTo("some"));
            Assert.That(addressDynamic["House"], Is.EqualTo(123));
        }

        [Test]
        public void RestoreReferenceProperty()
        {

            var dynamicVariantType = new DynamicVariantType();
            dynamicVariantType["Name"] = "Sergey";
            dynamicVariantType["Age"] = 28;
            var dateTime = DateTime.Now;
            dynamicVariantType["DoB"] = dateTime.ToShortDateString();
            var addressDynamic = new DynamicVariantType();
            addressDynamic["Street"] = "some";
            addressDynamic["House"] = 123;

            dynamicVariantType["Address"] = addressDynamic;

            var translator = CreateTranslator();
            var restoredObject = translator.Restore(typeof (Person), dynamicVariantType);

            Assert.That(restoredObject, Is.InstanceOf<Person>());

            var person = restoredObject as Person;

            Assert.That(person.Address.Number, Is.EqualTo(123));
            Assert.That(person.Address.Street, Is.EqualTo("some"));
        }
        

        
    }
}