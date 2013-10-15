using System.Linq;
using Mapper.Configuration;
using Mapper.Mappers;
using Moq;
using NUnit.Framework;

namespace Mapper.Tests.Mappers
{
    [TestFixture]
    public class MapperRegistryTests
    {
        [Test]
        public void GetAllMappingsRetursOneInstanceOfMapperAnyTime()
        {
            var mapperRegistry = new MapperRegistry();

            IMapper first = mapperRegistry.GetAllMappers().First();
            IMapper second = mapperRegistry.GetAllMappers().First();

            Assert.That(first, Is.EqualTo(second));
        }

        [Test]
        public void GetMapperByPropertyInfo()
        {
            var mapperRegistry = new MapperRegistry();

            var propertyMapInfo = new Mock<IPropertyMapInfo>();
            propertyMapInfo.Setup(x => x.PropertyKind).Returns(PropertyKind.Array);

            IMapper  mapper = mapperRegistry.GetMapper(propertyMapInfo.Object);

            Assert.That(mapper,Is.InstanceOf<ArrayMapper>());
        }
    }
}