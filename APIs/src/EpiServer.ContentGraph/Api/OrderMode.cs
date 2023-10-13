namespace EPiServer.ContentGraph.Api
{
    public enum OrderMode
    {
        ASC,
        DESC
    }

    public enum OrderType
    {
        COUNT,
        VALUE
    }
    public enum Ranking
    {
        RELEVANCE,
        SEMANTIC,
        BOOST_ONLY,
        DOC
    }
}
