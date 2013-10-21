using System;
using System.Collections.Generic;
using Mapper.Mappers;
using Mapper.Tests.ConcreteClasses;
using NUnit.Framework;
/*
 * Task for Mapper
4. Impove performance of late resolving mappers
5. Add checking in ClassMap of parameters types
6. Use lamda expression to create instances of types 
7. Create Generic extension methods for Mapping
8. Performance tests
 * 9. Support inheritance of objects
//Example of reusing ClassMap
 */
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

            return new ClassMapper(_mapMapModule, new ObjectStorageFactory(),new MapperRegistry());
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
            var person = MakePerson(x=> x.Numbers.AddRange(numbers));

            var dvt = _classMapper.Store(person);

            Assert.That(dvt.GetData("Name"), Is.EqualTo("John"));
            Assert.That(dvt.GetData("Age"), Is.EqualTo(28));
            Assert.That(dvt.GetData("Phones"), Is.EqualTo(numbers));
        }

        [Test]
        public void RestoreNonpublicProperty()
        {
            var numbers = new List<int> { 1, 2, 3, 4, 5 };
            var storage = new ObjectStorage();
            storage["Phones"] = numbers;

            var restored = _classMapper.Restore(typeof(Person),storage) as Person;

            Assert.That(restored.Numbers, Is.EqualTo(numbers));
        }

        [Test]
        public void StorePropertyUsingFormatterStorageAccordingToMapping()
        {
            var dateTime = new DateTime(1234, 2, 1);
            var person = MakePerson(x => x.DoB = dateTime);
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
        [Ignore("Skip until fix strcut types")]
        public void RestoreNullableProperty()
        {
            var storage = new ObjectStorage();
            var objectStorage = new ObjectStorage();
            objectStorage["Position"] = "some";
            objectStorage["Salary"] = 123.0;
            storage["JobInfo"] = objectStorage;

            var restoredObject = _classMapper.Restore(typeof(Person), storage);

            Assert.That(restoredObject, Is.InstanceOf<Person>());

            var person = restoredObject as Person;

            Assert.That(person.JobInfo.Salary, Is.EqualTo(123));
            Assert.That(person.JobInfo.Position, Is.EqualTo("some"));
        }

        //[Test]
        //public void StoreNullableProperty()
        //{
        //    var jobInfo = new JobInfo { Position = "Developer", Salary = 1000 };
        //    var person = MakePerson(x => x.JobInfoNull = jobInfo);

        //    var objectStorage = _classMapper.Store(person);

        //    var storage = objectStorage.GetData("JobInfo") as IObjectStorage;
        //    Assert.That(storage, Is.Not.Null);
        //    Assert.That(storage.GetData("Position"), Is.EqualTo("Developer"));
        //    Assert.That(storage.GetData("Salary"), Is.EqualTo(1000));
        //}

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

        [Test]
        public void StoreCollectionProperty()
        {
            var department = new Department
                {
                    Persons = new List<Person>
                        {
                            MakePerson(x=>x.Name = "First"),
                            MakePerson(x=>x.Name = "Second")
                        }
                };

            var objectStorage = _classMapper.Store(department);

            var data = objectStorage.GetData("Persons");

            Assert.That(data,Is.InstanceOf<List<IObjectStorage>>());

            var first = ((List<IObjectStorage>) data)[0] ;
            var second = ((List<IObjectStorage>) data)[1] ;

            Assert.That("First",Is.EqualTo(first.GetData("Name")));
            Assert.That("Second",Is.EqualTo(second.GetData("Name")));
        }

        [Test]
        public void RestoreCollectionPropertyFromStorage()
        {
            var storage = new ObjectStorage();
            var objectStorages = new List<IObjectStorage>
                {
                    MakePersonObjectStorage("First"),
                    MakePersonObjectStorage("Second"),
                };
            storage["Persons"] = objectStorages;

            var restoredObject = _classMapper.Restore(typeof(Department), storage);

            Assert.That(restoredObject ,Is.InstanceOf<Department>());
            var department = (Department) restoredObject;

            Assert.That(department.Persons.Count, Is.EqualTo(2));
            Assert.That(department.Persons[0].Name,Is.EqualTo("First"));
            Assert.That(department.Persons[1].Name,Is.EqualTo("Second"));
        }


        [Test]
        public void RestoreArrayPropertyFromStorage()
        {
            var storage = new ObjectStorage();
            var objectStorages = new List<IObjectStorage>
                {
                    MakePersonObjectStorage("First"),
                    MakePersonObjectStorage("Second"),
                };
            storage["PersonsArray"] = objectStorages.ToArray();

            var restoredObject = _classMapper.Restore(typeof(Department), storage);

            Assert.That(restoredObject, Is.InstanceOf<Department>());
            var department = (Department)restoredObject;

            Assert.That(department.Persons2.Length, Is.EqualTo(2));
            Assert.That(department.Persons2[0].Name, Is.EqualTo("First"));
            Assert.That(department.Persons2[1].Name, Is.EqualTo("Second"));
        }

        [Test]
        public void StoreArrayProperty()
        {
            var department = new Department
            {
                Persons2 = new List<Person>
                        {
                            MakePerson(x=>x.Name = "First"),
                            MakePerson(x=>x.Name = "Second")
                        }.ToArray()
            };

            var objectStorage = _classMapper.Store(department);

            var data = objectStorage.GetData("PersonsArray");

            Assert.That(data, Is.InstanceOf<List<IObjectStorage>>());

            var first = ((List<IObjectStorage>)data)[0];
            var second = ((List<IObjectStorage>)data)[1];

            Assert.That("First", Is.EqualTo(first.GetData("Name")));
            Assert.That("Second", Is.EqualTo(second.GetData("Name")));
        }

        [Test]
        public void TestTwoTimesStore()
        {
            var storage = new ObjectStorage();
            storage["Name"] = "Bill";

            var storage2 = new ObjectStorage();
            storage2["Name"] = "Second";

            var person = _classMapper.Restore(typeof(Person), storage) as Person;
            var person2 = _classMapper.Restore(typeof(Person), storage2) as Person;

            Assert.That(person, Is.Not.Null);
            Assert.That(person.Name, Is.EqualTo("Bill"));
            Assert.That(person2, Is.Not.Null);
            Assert.That(person2.Name, Is.EqualTo("Second"));
        }

        [Test]
        public void StoreDictionaryProperty()
        {
            var department = new Department
            {
                PersonsPerGroup = new Dictionary<string, Person>
                        {
                           {"Group1", MakePerson(x=>x.Name = "First")},
                           {"Group2", MakePerson(x=>x.Name = "Second")}
                        }
            };

            var objectStorage = _classMapper.Store(department);

            var data = objectStorage.GetData("PersonsPerGroup");

            Assert.That(data, Is.InstanceOf<Dictionary<string, IObjectStorage>>());
            var dict = (Dictionary<string, IObjectStorage>) data;

            var first = dict["Group1"];
            var second = dict["Group2"];

            Assert.That(first.GetData("Name"), Is.EqualTo("First"));
            Assert.That(second.GetData("Name"), Is.EqualTo("Second"));
        }

        [Test]
        public void RestoreDictionaryProperty()
        {
            var storage1 = new ObjectStorage();
            storage1["Name"] = "First";

            var storage2 = new ObjectStorage();
            storage2["Name"] = "Second";
            var storage = new ObjectStorage();

            var dict = new Dictionary<string, ObjectStorage>
                {
                    {"Group1", storage1},
                    {"Group2", storage2}
                };

            storage["PersonsPerGroup"] = dict;

            var restored = _classMapper.Restore(typeof (Department), storage);

            Assert.That(restored,Is.Not.Null);
            var department = (Department) restored;

            Assert.That(department.PersonsPerGroup.Count, Is.EqualTo(2));

        }

        [Test]
        public void StoreReferencePropertyUseTypeConverter()
        {
            var person = MakePerson(x => x.JobInfo = new JobInfo {Salary = 123.0, Position = "Pos"});

            var stored = _classMapper.Store(person);

            var jobInfoStorage = stored.GetData("JobInfo") as IObjectStorage; 

            Assert.That(jobInfoStorage.GetData("Position"),Is.EqualTo("PosClass"));
            Assert.That(jobInfoStorage.GetData("Salary"),Is.EqualTo(1230.0));

        }

        [Test]
        public void RestoreReferencePropertyUseTypeConverter()
        {
            var storage = new ObjectStorage();
            var jobStorage = new ObjectStorage();
            jobStorage["Position"] = "SomeClass";
            jobStorage["Salary"] = 1230.0;
            storage["JobInfo"] = jobStorage;

            var person = _classMapper.Restore(typeof (Person), storage) as Person;

            Assert.That(person.JobInfo.Position,Is.EqualTo("Some"));
            Assert.That(person.JobInfo.Salary,Is.EqualTo(123.0));
        }

        private static ObjectStorage MakePersonObjectStorage(string name)
        {
            var storage = new ObjectStorage();
            storage["Name"] = name;
            storage["Age"] = 28;
            var dateTime = DateTime.Now;
            storage["DoB"] = dateTime.ToShortDateString();
            var addressDynamic = new ObjectStorage();
            addressDynamic["Street"] = "some";
            addressDynamic["House"] = 123;
            return storage;
        }

    }
}