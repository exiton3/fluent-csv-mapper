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
            _classMapper = CreateTranslator();
        }

        private static TestMapContainer _mapMapModule;
        private ClassMapper _classMapper;

        private static ClassMapper CreateTranslator()
        {
            if (_mapMapModule == null)
            {
                _mapMapModule = new TestMapContainer();
                Console.WriteLine("Factory created");
            }

            return new ClassMapper(_mapMapModule, new ObjectStorageFactory());
        }

        private static Person MakePerson(Action<Person> action)
        {
            var person = new Person
                {
                    Age = 28,
                    Name = "John",
                    DoB = DateTime.Now,
                    Address = new Address()
                };
            action(person);
            return person;
        }

        [Test]
        public void RestoreEnumPropertyFromStorage()
        {
            var storage = new ObjectStorage();
            storage["Gender"] = 1;

            var restored = _classMapper.Restore(typeof (Person), storage);

            Assert.That(restored, Is.InstanceOf<Person>());
            var person = restored as Person;
            Assert.That(person.Gender, Is.EqualTo(Gender.Female));
        }

        [Test]
        public void RestoreInstanceFromObjectStorage()
        {
            var storage = new ObjectStorage();
            storage["Name"] = "Sergey";
            storage["Age"] = 28;
            var dateTime = DateTime.Now;
            storage["DoB"] = dateTime.ToShortDateString();
            var numbers = new List<int> {1, 2, 3, 4, 5};
            storage["Phones"] = numbers;


            var restoredObject = _classMapper.Restore(typeof (Person), storage);

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

            var restoredObject = _classMapper.Restore(typeof (Person), storage);

            Assert.That(restoredObject, Is.InstanceOf<Person>());

            var person = restoredObject as Person;

            Assert.That(person.Address.Number, Is.EqualTo(123));
            Assert.That(person.Address.Street, Is.EqualTo("some"));
        }

        [Test]
        public void StoreEnumPropertyInStorageTypeAccordingToMapping()
        {
            var person = MakePerson(x => x.Gender = Gender.Female);

            var dvt = _classMapper.Store(person);

            Assert.That(dvt.GetData("Gender"), Is.EqualTo(1));
        }

        [Test]
        public void StoreInstanceInDynamicVariantTypeAccordingToMapping()
        {
            var numbers = new List<int> {1, 2, 3, 4, 5};
            var person = MakePerson(x=> x.Numbers = numbers);

            var dvt = _classMapper.Store(person);

            Assert.That(dvt.GetData("Name"), Is.EqualTo("John"));
            Assert.That(dvt.GetData("Age"), Is.EqualTo(28));
            Assert.That(dvt.GetData("Phones"), Is.EqualTo(numbers));
        }

        [Test]
        public void StorePropertyUsingFormatterStorageAccordingToMapping()
        {
            var dateTime = new DateTime(1234, 2, 1);
            var person = MakePerson(x=>x.DoB = dateTime);
            var dvt = _classMapper.Store(person);

            Assert.That(dvt.GetData("DoB"), Is.EqualTo(dateTime.ToShortDateString()));
        }

        [Test]
        public void StoreReferenceProperty()
        {
            var address = new Address {Street = "some", Number = 123};
            var person = MakePerson(x=>x.Address = address);

            var dvt = _classMapper.Store(person);

            var storage = dvt.GetData("Address") as IObjectStorage;
            Assert.That(storage, Is.Not.Null);
            Assert.That(storage.GetData("Street"), Is.EqualTo("some"));
            Assert.That(storage.GetData("House"), Is.EqualTo(123));
        }

        [Test]
        public void ThorwsExceptionCanMapIfArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => { _classMapper.CanMap(null); });
        }


        [Test]
        public void SkipsPropertiesWichMappingNotPresentInMappingButExistInStorage()
        {
            var storage = new ObjectStorage();
            storage["NotValid"] = "some";
            storage["Name"] = "Bill";
            storage["Age"] = 28;

            var person = _classMapper.Restore(typeof (Person), storage) as Person;
            Assert.That(person, Is.Not.Null);
            Assert.That(person.Name,Is.EqualTo("Bill"));
            Assert.That(person.Age,Is.EqualTo(28));
        }

    }
}