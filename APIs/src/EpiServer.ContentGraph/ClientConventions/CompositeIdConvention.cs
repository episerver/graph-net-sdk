using System;
using EPiServer.Find.Helpers;

namespace EPiServer.Find.ClientConventions
{
    public class CompositeIdConvention : IIdConvention
    {
        IIdConvention defaultConvention;
        Func<object, string> idGetter;

        public CompositeIdConvention(IIdConvention defaultConvention, Func<object, string> idGetter)
        {
            defaultConvention.ValidateNotNullArgument("defaultConvention");
            idGetter.ValidateNotNullArgument("idGetter");

            this.defaultConvention = defaultConvention;
            this.idGetter = idGetter;
        }


        public string GetId(object instance)
        {
            instance.ValidateNotNullArgument("instance");
            var id = idGetter(instance);
            if (id.IsNotNull())
            {
                return id;
            }

            return defaultConvention.GetId(instance);
        }
    }
}