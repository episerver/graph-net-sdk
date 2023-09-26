using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using Xunit;

namespace EPiServer.ContentGraph.UnitTests
{
    public class GenerateQueryTests
    {
        ITypeQueryBuilder typeQueryBuilder;
        public GenerateQueryTests()
        {
            typeQueryBuilder = new TypeQueryBuilder<ExampleType>();
        }
        private string ExpectedFullTextWhereClause(string q)
        {
            return "(where:{_fulltext:{contains:\""+ q +"\"}})";
        }

        [Theory]
        [InlineData("alloy")]
        [InlineData("maybe longer text")]
        public void FullTextSearch_ShouldBuildCorrectQuery(string q)
        {
            string expectedFilter = ExpectedFullTextWhereClause(q);
            string expectedFields = @"{items {Property1 Property2 Property3{NestedProperty}}}";
            StringFilterOperators stringFilter = new StringFilterOperators();
            stringFilter.Contains(q);
            ((TypeQueryBuilder<ExampleType>)typeQueryBuilder)
                .FullTextSearch(stringFilter.Contains(q))
                .Field(x=>x.Property1).Field(x=>x.Property2).Field(x=>x.Property3.NestedProperty);
            GraphQueryBuilder query = typeQueryBuilder.Build();
            Assert.NotNull(query.GetQuery());
            //check filter
            Assert.Contains(expectedFilter, query.GetQuery().Query);
            //check selected fields
            Assert.Contains(expectedFields, query.GetQuery().Query);
            //check query
            Assert.Equal(query.GetQuery().Query, $"ExampleType{expectedFilter}{expectedFields}");
        }
        [Theory]
        [InlineData("alloy")]
        [InlineData("maybe longer text")]
        public void SearchWithFields_ShouldBuildCorrectQuery(string q)
        {
            string expectedFilter = "(where:{Property1:{eq: \"" + q +"\"}})";
            string expectedFields = @"{items {Property1 Property2 Property3{NestedProperty}}}";
            StringFilterOperators stringFilter = new StringFilterOperators();
            ((TypeQueryBuilder<ExampleType>)typeQueryBuilder)
                .Where(x=>x.Property1, stringFilter.Eq(q))
                .Field(x => x.Property1).Field(x => x.Property2).Field(x => x.Property3.NestedProperty);
            GraphQueryBuilder query = typeQueryBuilder.Build();
            Assert.NotNull(query.GetQuery());
            //check filter
            Assert.Contains(expectedFilter, query.GetQuery().Query);
            //check selected fields
            Assert.Contains(expectedFields, query.GetQuery().Query);
            //check query
            Assert.Equal(query.GetQuery().Query, $"ExampleType{expectedFilter}{expectedFields}");
        }
        internal class ExampleType
        {
            public string Property1 { get; set; }
            public int Property2 { get; set; }
            public ExampleNested Property3 { get; set; }
        }

        internal class ExampleNested
        {
            public object NestedProperty { get; set; }
        }
    }
}