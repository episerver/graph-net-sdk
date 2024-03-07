using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EPiServer.ContentGraph.Helpers.Reflection
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Assembly> AssembliesWithReferenceToAssemblyOf<T>(this IAssemblyLocator assemblyLocator)
        {
            return assemblyLocator.AssembliesWithReferenceToAssemblyOf(typeof(T));
        }

        public static IEnumerable<Assembly> AssembliesWithReferenceToAssemblyOf(this IAssemblyLocator assemblyLocator, Type type)
        {
            string assemblyName = type.Assembly.GetName().Name;

            return assemblyLocator.GetAssemblies()
                    .Where(a => a.GetReferencedAssemblies()
                        .Any(r => r.Name == assemblyName));
        }

        public static IEnumerable<Type> AssignableTo<T>(this IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(a => a.GetTypes().AssignableTo(typeof(T)));
        }

        public static IEnumerable<Type> Types(this IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(a => a.GetLoadableTypes());
        }

        /// <summary>
        /// GetTypes() requires all transitive dependencies to be loadable, so there's a great chance of ReflectionTypeLoadException being thrown.
        /// E.g.: If the assembly contains types referencing an assembly which is currently not available.
        /// The ReflectionTypeLoadException thrown when a type can’t be loaded contains all the information we need,
        /// so we catch the exception from GetTypes() and get the types from the exception instead.
        /// </summary>
        private static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}
