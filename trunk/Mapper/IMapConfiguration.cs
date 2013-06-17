using System.Collections.Generic;

namespace Mapper
{
    public interface IMapConfiguration
    {
        Dictionary<string, IPropertyMapInfo> Mappings { get; }
        object Instance { get; }
        IPropertyMapInfo GetMapping(string name);
    }
}