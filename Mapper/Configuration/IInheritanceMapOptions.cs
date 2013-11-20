namespace Mapper.Configuration
{
    public interface IInheritanceMapOptions 
    {
        IDescriminateMapOptions DiscriminateOnField(string name);
    }
}