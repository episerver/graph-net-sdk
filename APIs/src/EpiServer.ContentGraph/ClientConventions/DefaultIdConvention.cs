using System.Linq;
using System.Reflection;
using EPiServer.Find.Helpers;
using EPiServer.Find.Helpers.Text;

namespace EPiServer.Find.ClientConventions
{
    public class DefaultIdConvention : IIdConvention
    {
        public string GetId(object instance)
        {
            instance.ValidateNotNullArgument("instance");
            var type = instance.GetType();
            var members = type.GetMembers(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            var idMember = members.Where(x => x.GetCustomAttributes(typeof (IdAttribute), true).Any()).FirstOrDefault();
            if (idMember.IsNotNull())
            {
                var property = (PropertyInfo) idMember;
                var method = property.GetGetMethod();
                var idObject = method.Invoke(instance, new object[0]);
                if (idObject.IsNotNull())
                {
                    var id = idObject.ToString();
                    if (id.IsNullOrEmpty())
                    {
                        return null;
                    }

                    return id;
                }
            }
            return null;
        }
    }
}
