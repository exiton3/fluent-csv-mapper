using System;
using Mapper.Configuration;
using Mapper.Tests.ConcreteClasses;
using NUnit.Framework;

namespace Mapper.Tests
{
    [TestFixture]
    public class MapContainerTests
    {
        [SetUp]
        public void SetUp()
        {
            _mapContainer = new TestMapContainer();
        }

        private IMapContainer _mapContainer;

        [Test]
        public void MappingNotExistIfConatainerWasNotBuild()
        {
            Assert.That(_mapContainer.IsMappingExists(typeof (Person)), Is.False);
            Assert.That(_mapContainer.IsMappingExists(typeof (Address)), Is.False);
            Assert.That(_mapContainer.IsMappingExists(typeof (Department)), Is.False);
        }

        [Test]
        public void RegiterAllMappingsOnBuild()
        {
            _mapContainer.Build();

            Assert.That(_mapContainer.IsMappingExists(typeof (Person)), Is.True);
            Assert.That(_mapContainer.IsMappingExists(typeof (Address)), Is.True);
            Assert.That(_mapContainer.IsMappingExists(typeof (Department)), Is.True);
        }

        [Test]
        public void ThrowsExceptionWhenGettingMappingIfContainerWasNotBuilt()
        {
            Assert.That(
                () => { _mapContainer.GetMappingFor(typeof (Person)); },
                Throws.InstanceOf<InvalidOperationException>()
                      .And.Message.Contains("MapContainer was not build"));
        }
    }
}