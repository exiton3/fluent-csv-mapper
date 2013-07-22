using System.Collections.Generic;

namespace Mapper.Configuration
{
    public interface IClassMap
    {
        Dictionary<string, IPropertyMapInfo> Mappings { get; }
        object Instance { get; }
        IPropertyMapInfo GetMapping(string name);
        bool IsMappingForPropertyExist(string name);
    }
}