namespace EPiServer.ContentGraph.Connection
{
    public interface IJsonRequestFactory
    {
        IJsonRequest CreateRequest(string url, HttpVerbs method, int? explicitRequestTimeout);
    }
}