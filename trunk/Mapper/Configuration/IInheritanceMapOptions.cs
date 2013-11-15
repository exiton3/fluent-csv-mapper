namespace Mapper.Configuration
{
    public interface IInheritanceMapOptions
    {
        IInheritanceMapOptions DiscriminateOnField(string name);
        IInheritanceMapOptions DiscriminatorValueFor<T>(string value) where T : class;
    }
}