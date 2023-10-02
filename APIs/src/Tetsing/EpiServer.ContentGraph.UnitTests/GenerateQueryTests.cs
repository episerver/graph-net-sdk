using EpiServer.ContentGraph.UnitTests.QueryTypeObjects;
using EPiServer.ContentGraph.Api;
using EPiServer.ContentGraph.Api.Autocomplete;
using EPiServer.ContentGraph.Api.Facets;
using EPiServer.ContentGraph.Api.Querying;
using Xunit;

namespace EPiServer.ContentGraph.UnitTests
{
    public class GenerateQueryTests
    {
        TypeQueryBuilder<RequestTypeObject> typeQueryBuilder;
        public GenerateQueryTests()
        {
            typeQueryBuilder = new TypeQueryBuilder<RequestTypeObject>();
        }
        [Fact]
        public void SelectNoneOfFieldsShouldThrowException()
        {
            typeQueryBuilder.Limit(100).Skip(0);
            Assert.Throws<ArgumentNullException>(() => typeQueryBuilder.Build());
        }
        [Fact]
        public void SelectFields()
        {
            string expectedFields = @"{items{Property1 Property2}}";
            typeQueryBuilder.Fields(x => x.Property1, x => x.Property2);
            GraphQueryBuilder query = typeQueryBuilder.Build();

            Assert.NotNull(query.GetQuery());
            //check selected fields
            Assert.Contains(expectedFields, query.GetQuery().Query);
            query = query.BuildQueries();
        }
        [Fact]
        public void SelectNestedFields()
        {
            string expectedFields = @"{items{Property1 Property2 Property3{NestedProperty}}}";
            typeQueryBuilder
                .Field(x => x.Property1)
                .Field(x => x.Property2)
                .Field(x => x.Property3.NestedProperty);
            GraphQueryBuilder query = typeQueryBuilder.Build();

            Assert.NotNull(query.GetQuery());
            //check selected fields
            Assert.Contains(expectedFields, query.GetQuery().Query);
        }

        [Theory]
        [InlineData(0,100)]
        [InlineData(int.MinValue, int.MaxValue)]
        public void QueryWithPaging(int skip, int limit)
        {
            string expectedPaging = $"(limit:{limit},skip:{skip})";
            string expectedFields = @"{items{Property1 Property2 Property3{NestedProperty}}}";
            typeQueryBuilder
                .Limit(limit)
                .Skip(skip)
                .Field(x => x.Property1)
                .Field(x => x.Property2)
                .Field(x => x.Property3.NestedProperty);
            GraphQueryBuilder query = typeQueryBuilder.Build();

            Assert.NotNull(query.GetQuery());
            Assert.Contains(expectedPaging, query.GetQuery().Query);
            Assert.Contains(expectedFields, query.GetQuery().Query);
            Assert.Equal($"RequestTypeObject{expectedPaging}{expectedFields}", query.GetQuery().Query);
        }

        [Fact]
        public void FacetsQuery()
        {
            string expectedFields = @"items{Property1 Property2}";
            string expectedFacets = @"facets{Property1{name count} Property3{NestedProperty{name count}}}";
            typeQueryBuilder
                .Field(x => x.Property1)
                .Field(x => x.Property2)
                .Facet(x => x.Property1)
                .Facet(x => x.Property3.NestedProperty);
            GraphQueryBuilder query = typeQueryBuilder.Build();

            Assert.NotNull(query.GetQuery());
            Assert.Contains(expectedFacets, query.GetQuery().Query);
            Assert.Contains(expectedFields, query.GetQuery().Query);
            Assert.Equal($"RequestTypeObject{{{expectedFields} {expectedFacets}}}", query.GetQuery().Query);
        }
        [Fact]
        public void FacetsQueryWithFilter()
        {
            string expectedFields = @"items{Property1 Property2}";
            string expectedFacets = @"facets{Property1(filters: ""somevalue""){name count} Property3{NestedProperty(orderBy: DESC){name count}}}";
            typeQueryBuilder
                .Field(x => x.Property1)
                .Field(x => x.Property2)
                .Facet(x => x.Property1, new FacetFilter().Filters("somevalue"))
                .Facet(x => x.Property3.NestedProperty, new FacetFilter().OrderBy(OrderMode.DESC));
            GraphQueryBuilder query = typeQueryBuilder.Build();

            Assert.NotNull(query.GetQuery());
            Assert.Contains(expectedFacets, query.GetQuery().Query);
            Assert.Contains(expectedFields, query.GetQuery().Query);
            Assert.Equal($"RequestTypeObject{{{expectedFields} {expectedFacets}}}", query.GetQuery().Query);
        }

        [Fact]
        public void AutocompleteQuery()
        {
            string expectedFields = @"items{Property1 Property2}";
            string expectedFacets = @"facets{Property3{NestedProperty{name count}}}";
            string expectedAutoComplete = @"autocomplete{Property1(limit:10,value:""test"")}";
            typeQueryBuilder
                .Field(x => x.Property1)
                .Field(x => x.Property2)
                .Autocomplete(x => x.Property1, new AutoCompleteOperators().Limit(10).Value("test"))
                .Facet(x => x.Property3.NestedProperty);
            GraphQueryBuilder query = typeQueryBuilder.Build();

            Assert.NotNull(query.GetQuery());
            Assert.Contains(expectedFacets, query.GetQuery().Query);
            Assert.Contains(expectedFields, query.GetQuery().Query);
            Assert.Contains(expectedAutoComplete, query.GetQuery().Query);
            Assert.Equal($"RequestTypeObject{{{expectedFields} {expectedFacets} {expectedAutoComplete}}}", query.GetQuery().Query);
        }
        [Fact]
        public void SubtypeQueryTests()
        {
            string subtypeFields = @"... on SubTypeObject{SubProperty}";
            string expectedFields = $"items{{Property1 Property2 {subtypeFields}}}";
            string expectedFacets = @"facets{Property3{NestedProperty{name count}}}";
            SubTypeQueryBuilder<SubTypeObject> subQuery = new SubTypeQueryBuilder<SubTypeObject>()
                .Field(x => x.SubProperty);

            typeQueryBuilder
                .Field(x => x.Property1)
                .Field(x => x.Property2)
                .ForSubType(subQuery)
                .Facet(x => x.Property3.NestedProperty);
            GraphQueryBuilder query = typeQueryBuilder.Build();

            Assert.NotNull(query.GetQuery());
            Assert.Contains(expectedFacets, query.GetQuery().Query);
            Assert.Contains(expectedFields, query.GetQuery().Query);
            Assert.Equal($"RequestTypeObject{{{expectedFields} {expectedFacets}}}", query.GetQuery().Query);
        }
    }
}