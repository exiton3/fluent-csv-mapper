using System;
using System.Collections.Generic;
using Mapper.Tests.ConcreteClasses;
using NUnit.Framework;

namespace Mapper.Tests
{
    [TestFixture]
    public class ClassMapperTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        private static TestMapContainer _mapMapModule;

        private static ClassMapper CreateTranslator()
        {
            if (_mapMapModule == null)
            {
                _mapMapModule = new TestMapContainer();
                Console.WriteLine("Factory created");
            }

            return new ClassMapper(_mapMapModule, new ObjectStorageFactory());
        }

        [Test]
        public void RestoreEnumPropertyFromStorage()
        {
            var storage = new ObjectStorage();
            storage["Gender"] = 1;

            var translator = CreateTranslator();

            var restored = translator.Restore(typeof (Person), storage);

            Assert.That(restored, Is.InstanceOf<Person>());
            var person = restored as Person;
            Assert.That(person.Gender, Is.EqualTo(Gender.Female));
        }

        [Test]
        public void RestoreInstanceFromDynamicVariantType()
        {
            var storage = new ObjectStorage();
            storage["Name"] = "Sergey";
            storage["Age"] = 28;
            var dateTime = DateTime.Now;
            storage["DoB"] = dateTime.ToShortDateString();
            var numbers = new List<int> {1, 2, 3, 4, 5};
            storage["Phones"] = numbers;

            var translator = CreateTranslator();

            var restoredObject = translator.Restore(typeof (Person), storage);

            Assert.IsInstanceOf<Person>(restoredObject);
            var person = (Person) restoredObject;
            Assert.That(person.Age, Is.EqualTo(28));
            Assert.That(person.Name, Is.EqualTo("Sergey"));
            Assert.That(person.DoB, Is.EqualTo(dateTime.Date));
            Assert.That(person.Numbers, Is.EqualTo(numbers));
        }

        [Test]
        public void RestoreReferenceProperty()
        {
            var storage = new ObjectStorage();
            storage["Name"] = "Sergey";
            storage["Age"] = 28;
            var dateTime = DateTime.Now;
            storage["DoB"] = dateTime.ToShortDateString();
            var addressDynamic = new ObjectStorage();
            addressDynamic["Street"] = "some";
            addressDynamic["House"] = 123;

            storage["Address"] = addressDynamic;

            var translator = CreateTranslator();
            var restoredObject = translator.Restore(typeof (Person), storage);

            Assert.That(restoredObject, Is.InstanceOf<Person>());

            var person = restoredObject as Person;

            Assert.That(person.Address.Number, Is.EqualTo(123));
            Assert.That(person.Address.Street, Is.EqualTo("some"));
        }

        [Test]
        public void StoreEnumPropertyInStorageTypeAccordingToMapping()
        {
            var dateTime = DateTime.Now;
            var person = new Person
                {
                    Gender = Gender.Female,
                    Age = 28,
                    Name = "John",
                    DoB = dateTime,
                    Address = new Address()
                };


            var translator = CreateTranslator();

            var dvt = translator.Store(person);

            Assert.That(dvt.GetData("Gender"), Is.EqualTo(1));
        }

        [Test]
        public void StoreInstanceInDynamicVariantTypeAccordingToMapping()
        {
            var dateTime = DateTime.Now;
            var numbers = new List<int> {1, 2, 3, 4, 5};
            var person = new Person
                {
                    Age = 28,
                    Name = "John",
                    DoB = dateTime,
                    Numbers = numbers,
                    Address = new Address()
                };


            var translator = CreateTranslator();

            var dvt = translator.Store(person);

            Assert.That(dvt.GetData("Name"), Is.EqualTo("John"));
            Assert.That(dvt.GetData("Age"), Is.EqualTo(28));
            Assert.That(dvt.GetData("Phones"), Is.EqualTo(numbers));
        }

        [Test]
        public void StorePropertyInDifferentTypeInDynamicVariantTypeAccordingToMapping()
        {
            var dateTime = DateTime.Now;
            var person = new Person {Age = 28, Name = "John", DoB = dateTime, Address = new Address()};


            var translator = CreateTranslator();

            var dvt = translator.Store(person);

            Assert.That(dvt.GetData("DoB"), Is.EqualTo(dateTime.ToShortDateString()));
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

            var storage = dvt.GetData("Address") as IObjectStorage;
            Assert.That(storage, Is.Not.Null);
            Assert.That(storage.GetData("Street"), Is.EqualTo("some"));
            Assert.That(storage.GetData("House"), Is.EqualTo(123));
        }
    }
}