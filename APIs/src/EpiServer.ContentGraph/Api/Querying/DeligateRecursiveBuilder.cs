namespace EPiServer.ContentGraph.Api.Querying
{
    public class DeligateRecursiveBuilder : Recursion
    {
        private Func<string, Recursion> filterDelegate;
        public DeligateRecursiveBuilder(Func<string, Recursion> filterDelegate = null)
        {
            this.filterDelegate = filterDelegate;
        }
        public Recursion GetReturnType(string fieldName)
        {
            return filterDelegate(fieldName);
        }
    }
}
