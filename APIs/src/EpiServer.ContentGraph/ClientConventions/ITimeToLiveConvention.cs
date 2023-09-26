using System;
using EPiServer.Find.Api;

namespace EPiServer.Find.ClientConventions
{
    public interface ITimeToLiveConvention
    {
        TimeToLive GetTimeToLive(object instance);
    }
}