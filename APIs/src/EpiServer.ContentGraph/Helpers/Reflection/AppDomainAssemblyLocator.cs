using System;
using System.Collections.Generic;
using System.Reflection;

namespace EPiServer.ContentGraph.Helpers.Reflection
{
    public class AppDomainAssemblyLocator : IAssemblyLocator
    {
        public IEnumerable<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}
