using System.Linq;
using Mapper.Mappers;
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
    }
}