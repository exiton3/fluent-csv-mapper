namespace Mapper.Tests.ConcreteClasses
{
    public class ObjectStorageFactory:IObjectStorageFactory
    {
        public IObjectStorage Create()
        {
            return new ObjectStorage();
        }
    }
}