using EPiServer.Find.Api;

namespace EPiServer.Find.ClientConventions
{
    /// <summary>
    /// This convention is used to set up the mapping for the property to be used for language routing.
    /// </summary>
    public interface ILanguageRoutingConvention
    {
        LanguageRouting GetLanguageRouting(object instance);

        bool HasLanguageRouting(object instance);
    }
}