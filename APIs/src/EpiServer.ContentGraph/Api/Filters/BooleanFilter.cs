namespace EPiServer.ContentGraph.Api.Filters
{
    public class BooleanFilter
    {
        public static AndFilter<T> AndFilter<T>()
        {
            return new AndFilter<T>();
        }
        public static OrFilter<T> OrFilter<T>()
        {
            return new OrFilter<T>();
        }
        public static NotFilter<T> NotFilter<T>()
        {
            return new NotFilter<T>();
        }
    }
}
