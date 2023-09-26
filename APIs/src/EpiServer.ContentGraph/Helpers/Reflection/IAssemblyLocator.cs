using System.Collections.Generic;
using System.Reflection;

namespace EPiServer.ContentGraph.Helpers.Reflection
{
    public interface IAssemblyLocator
    {
        IEnumerable<Assembly> GetAssemblies();
    }
}
