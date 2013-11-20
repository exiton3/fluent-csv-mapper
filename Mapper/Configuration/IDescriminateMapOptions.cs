namespace Mapper.Configuration
{
    public interface IDescriminateMapOptions
    {
        IDescriminateMapOptions DiscriminatorValueFor<T>(string value) where T : class;
    }
}