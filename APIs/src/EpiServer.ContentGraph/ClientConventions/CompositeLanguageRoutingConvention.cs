using System;
using EPiServer.Find.Helpers;
using EPiServer.Find.Api;

namespace EPiServer.Find.ClientConventions
{
    public class CompositeLanguageRoutingConvention : ILanguageRoutingConvention
    {
        ILanguageRoutingConvention defaultConvention;
        Func<object, LanguageRouting> languageRoutingGetter;

        public CompositeLanguageRoutingConvention(ILanguageRoutingConvention defaultConvention, Func<object, LanguageRouting> languageRoutingGetter)
        {
            defaultConvention.ValidateNotNullArgument("defaultConvention");
            languageRoutingGetter.ValidateNotNullArgument("languageRoutingGetter");

            this.defaultConvention = defaultConvention;
            this.languageRoutingGetter = languageRoutingGetter;
        }

        public LanguageRouting GetLanguageRouting(object instance)
        {
            instance.ValidateNotNullArgument("instance");
            var language = languageRoutingGetter(instance);
            if (language != null && language.IsValid())
            {
                return language;
            }

            return defaultConvention.GetLanguageRouting(instance);
        }

        public bool HasLanguageRouting(object instance)
        {
            instance.ValidateNotNullArgument("instance");
            var language = languageRoutingGetter(instance);
            if (language != null && language.IsValid())
            {
                return true;
            }

            return defaultConvention.HasLanguageRouting(instance);
        }
    }
}