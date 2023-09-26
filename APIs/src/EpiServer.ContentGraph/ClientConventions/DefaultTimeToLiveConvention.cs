using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using EPiServer.Find.Helpers;
using EPiServer.Find.Helpers.Text;
using EPiServer.Find.Api;

namespace EPiServer.Find.ClientConventions
{
    public class DefaultTimeToLiveConvention : ITimeToLiveConvention
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo> methodInfos = new ConcurrentDictionary<Type, MethodInfo>();

        public TimeToLive GetTimeToLive(object instance)
        {
            instance.ValidateNotNullArgument("instance");
            var type = instance.GetType();
            var method = methodInfos.GetOrAdd(type, t =>
            {
                var members = type.GetMembers(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
                var timeToLiveMember = members.FirstOrDefault(x => x.GetCustomAttributes(typeof(TimeToLiveAttribute), true).Any<object>());
                if (timeToLiveMember.IsNotNull())
                {
                    var property = (PropertyInfo) timeToLiveMember;
                    return property.GetGetMethod();
                }

                return null;
            });

            if (method != null)
            {
                var timeToLiveObject = method.Invoke(instance, new object[0]);
                if (timeToLiveObject.IsNotNull())
                {
                    if (timeToLiveObject is TimeToLive)
                    {
                        return timeToLiveObject as TimeToLive;
                    }
                    else if (timeToLiveObject is TimeSpan)
                    {
                        return new TimeToLive(((TimeSpan)timeToLiveObject).TotalMilliseconds);
                    }
                    else if (Nullable.GetUnderlyingType(timeToLiveObject.GetType()) == typeof(TimeSpan))
                    {
                        return new TimeToLive(((TimeSpan?) timeToLiveObject).Value.TotalMilliseconds);
                    }
                    else
                    {
                        instance.ValidateTypeArgument<TimeToLive>("instance");
                    }
                }
            }
            return null;
        }
    }
}