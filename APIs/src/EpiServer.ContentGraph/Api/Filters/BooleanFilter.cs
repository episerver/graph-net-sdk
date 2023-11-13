namespace EPiServer.ContentGraph.Api.Filters
{
    public class BooleanFilter<T>
    {
        public static AndFilter<T> AndFilter { get { return new AndFilter<T>(); } }
        public static OrFilter<T> OrFilter { get { return new OrFilter<T>(); } }
        public static NotFilter<T> NotFilter { get { return new NotFilter<T>(); } }
    }
}
