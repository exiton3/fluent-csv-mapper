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
            Assert.That(container.IsMappingExists(typeof (Person)), Is.True);
            Assert.That(container.IsMappingExists(typeof (Address)), Is.True);
            Assert.That(container.IsMappingExists(typeof (Department)), Is.True);
        }
    }
}