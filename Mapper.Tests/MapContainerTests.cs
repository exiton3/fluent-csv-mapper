using Mapper.Tests.ConcreteClasses;
using NUnit.Framework;

namespace Mapper.Tests
{
    [TestFixture]
    public class MapContainerTests
    {
        [Test]
        public void RegisterAllTypesFormEachModules()
        {
            var container = new TestMapContainer();
            Assert.That(container.IsMappingExist(typeof (Person)), Is.True);
            Assert.That(container.IsMappingExist(typeof (Address)), Is.True);
            Assert.That(container.IsMappingExist(typeof (Department)), Is.True);
        }
    }
}