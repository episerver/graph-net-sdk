using System;
using System.Collections.Generic;
using System.Linq;

namespace EPiServer.ContentGraph.Helpers.Reflection
{
    public static class TypeExtensions
    {
        public static bool IsCustomStruct(this Type type)
        {
            return (type != null && type.Namespace != null && type.IsValueType && !type.IsEnum && !type.IsPrimitive && type != typeof(Decimal) && !type.Namespace.StartsWith("System", StringComparison.Ordinal));
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            if (type == typeof(Object))
            {
                yield break;
            }
            yield return type.BaseType;
            foreach (Type baseType in type.BaseType.GetBaseTypes())
            {
                yield return baseType;
            }
        }

        public static IEnumerable<Type> GetTypes(this Type type)
        {
            yield return type;
            
            foreach (Type baseType in type.GetBaseTypes())
            {
                yield return baseType;
            }
        }

        public static IEnumerable<Type> AssignableTo(this IEnumerable<Type> types, Type superType)
        {
            return types.Where(superType.IsAssignableFrom);
        }

        public static IEnumerable<Type> Concrete(this IEnumerable<Type> types)
        {
            return types.Where(type => !type.IsAbstract);
        }

        public static bool HasAttributeOfType<TAttribute>(this Type type, bool inherit = true)
        {
            return type.GetCustomAttributes(inherit).OfType<TAttribute>().Any();
        }

        public static bool IsCastleProxy(this Type type)
        {
            return (type != null && type.Assembly.FullName.StartsWith("DynamicProxyGenAssembly2"));
        }

        public static bool IsDynamicTypeWithBaseType(this Type type) => (type?.Assembly.IsDynamic).GetValueOrDefault() && type.BaseType is object; 
    }
}
