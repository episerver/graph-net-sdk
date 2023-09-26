using EPiServer.Find.Api;
using EPiServer.Find.Helpers;
using System.Linq;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace EPiServer.Find.ClientConventions
{
    public class DefaultLanguageRoutingConvention : ILanguageRoutingConvention
    {
        private static Dictionary<Type, MethodInfo> _languageRoutingGetters = new Dictionary<Type, MethodInfo>();
        public bool HasLanguageRouting(object instance)
        {
            return GetLanguageRouting(instance).IsNotNull();
        }

        public LanguageRouting GetLanguageRouting(object instance)
        {
            instance.ValidateNotNullArgument("instance");
            var type = instance.GetType();
            MethodInfo method = null;
            if (_languageRoutingGetters.ContainsKey(type))
            {
                method = _languageRoutingGetters[type];
            }
            else
            {
                var members = type.GetMembers(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
                var languageRoutingMember = members.FirstOrDefault(x => x.GetCustomAttributes(typeof(LanguageRoutingAttribute), true).Any());
                if (languageRoutingMember.IsNull())
                {
                    return null;
                }

                var property = (PropertyInfo)languageRoutingMember;
                method = property.GetGetMethod();
                _languageRoutingGetters.Add(type, method);
            }
            
            var languageRoutingObject = method.Invoke(instance, new object[0]);
            if (languageRoutingObject.IsNull())
            {
                return null;
            }
            var language = languageRoutingObject as LanguageRouting;

            if (language.IsNull())
            {
                return null;
            }

            if (language.IsValid())
            {
                return language;
            }

            instance.ValidateTypeArgument<LanguageRouting>("instance");

            return null;
        }

        internal static bool TypeHasLanguageRouting(Type type)
        {
            var members = type.GetMembers(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            var languageRoutingMember = members.FirstOrDefault(x => x.GetCustomAttributes(typeof(LanguageRoutingAttribute), true).Any());
            return languageRoutingMember.IsNotNull();
        }
    }
}