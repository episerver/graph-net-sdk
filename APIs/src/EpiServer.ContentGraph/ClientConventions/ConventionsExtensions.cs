using System;

namespace EPiServer.Find.ClientConventions
{
    public static class ConventionsExtensions
    {
        public static TypeConventionBuilder ForTypesMatching(this IClientConventions conventions, Func<Type, bool> typeSelector)
        {
            return new TypeConventionBuilder(conventions, typeSelector);
        }

        public static TypeConventionBuilder<T> ForType<T>(this IClientConventions conventions) where T : class
        {
            if (typeof(T).IsInterface)
            {
                var errorMessage = GetNonConcreteTypeErrorMessage(typeof (T).Name, "interface");

                throw new ArgumentException(errorMessage);
            }

            if (typeof(T).IsAbstract)
            {
                var errorMessage = GetNonConcreteTypeErrorMessage(typeof(T).Name, "abstract class");

                throw new ArgumentException(errorMessage);
            }

            return new TypeConventionBuilder<T>(conventions, x => x == typeof(T));
        }

        internal static string GetNonConcreteTypeErrorMessage(string typename, string typeType)
        {
            return string.Format(
                    "Type {0} is an {1}. The ForType method is used for modifying instance of exactly that specific type is handled. To configure how all instance of an {1} is handled use the ForInstancesOf method instead.",
                    typename, typeType);
        }

        public static TypeConventionBuilder<T> ForInstancesOf<T>(this IClientConventions conventions) where T : class
        {
            return new TypeConventionBuilder<T>(conventions, x => typeof(T).IsAssignableFrom(x));
        }

        public static IClientConventions AddTypeAsNamingRoot<TBaseType>(this IClientConventions conventions)
        {
            return conventions.AddTypeAsNamingRoot(typeof(TBaseType));
        }

        public static IClientConventions AddTypeAsNamingRoot(this IClientConventions conventions, Type baseType)
        {
            var convention = new SameNameForAllSubTypesConvention(baseType, conventions.TypeNameConvention);
            conventions.TypeNameConvention = convention;

            return conventions;
        }
    }
}
