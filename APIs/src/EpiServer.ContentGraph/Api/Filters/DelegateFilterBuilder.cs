namespace EPiServer.ContentGraph.Api.Filters
{
    public class DelegateFilterBuilder : IFilterWraper
    {
        private Func<string, IFilterWraper> filterDelegate;
        private string fieldName = string.Empty;
        public DelegateFilterBuilder(Func<string, IFilterWraper> filterDelegate = null)
        {
            this.filterDelegate = filterDelegate;
        }

        public IFilterWraper GetFilter(string fieldName)
        {
            this.fieldName = fieldName;
            return filterDelegate(fieldName);
        }

        public string GetFieldName() => this.fieldName;

        public IFilterOperator GetFilter()
        {
          return GetFilter(this.fieldName)?.GetFilter();
        }
    }

    public class WrappedFilter: IFilterWraper
    {
        private IFilterOperator filter;
        private string fieldName;
        public WrappedFilter(string fieldName, IFilterOperator filter)
        {
            this.filter = filter;
            this.fieldName = fieldName;
        }

        public string GetFieldName() => fieldName;
        public IFilterOperator GetFilter() => filter;
    }
    public interface IFilterWraper
    {
        public IFilterOperator GetFilter();
        public string GetFieldName();
    }
}
