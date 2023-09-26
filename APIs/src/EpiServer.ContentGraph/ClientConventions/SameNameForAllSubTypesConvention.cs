using System;
using EPiServer.Find.Helpers;

namespace EPiServer.Find.ClientConventions
{
    public class SameNameForAllSubTypesConvention : ITypeNameConvention
    {
        Type baseType;
        ITypeNameConvention fallbackConvention;

        public SameNameForAllSubTypesConvention(Type baseType, ITypeNameConvention fallbackConvention)
        {
            if (baseType.IsNull())
            {
                throw new ArgumentNullException("baseType");
            }

            if (fallbackConvention.IsNull())
            {
                throw new ArgumentNullException("fallbackConvention");
            }

            this.baseType = baseType;
            this.fallbackConvention = fallbackConvention;
        }

        public string GetTypeName(Type type)
        {
            if (baseType.IsAssignableFrom(type))
            {
                return baseType.FullName;
            }
            return fallbackConvention.GetTypeName(type);
        }
    }
}
