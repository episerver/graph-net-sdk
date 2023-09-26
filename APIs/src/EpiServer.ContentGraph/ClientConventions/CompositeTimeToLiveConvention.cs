using System;
using EPiServer.Find.Helpers;
using EPiServer.Find.Api;

namespace EPiServer.Find.ClientConventions
{
    public class CompositeTimeToLiveConvention : ITimeToLiveConvention
    {
        ITimeToLiveConvention defaultConvention;
        Func<object, TimeToLive> timeToLiveGetter;

        public CompositeTimeToLiveConvention(ITimeToLiveConvention defaultConvention, Func<object, TimeToLive> timeToLiveGetter)
        {
            defaultConvention.ValidateNotNullArgument("defaultConvention");
            timeToLiveGetter.ValidateNotNullArgument("timeToLiveGetter");

            this.defaultConvention = defaultConvention;
            this.timeToLiveGetter = timeToLiveGetter;
        }

        public TimeToLive GetTimeToLive(object instance)
        {
            instance.ValidateNotNullArgument("instance");
            var timeToLive = timeToLiveGetter(instance);
            if (timeToLive.IsNotNull())
            {
                return timeToLive;
            }

            return defaultConvention.GetTimeToLive(instance);
        }
    }
}