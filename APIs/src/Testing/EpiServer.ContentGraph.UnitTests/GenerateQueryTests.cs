using EpiServer.ContentGraph.UnitTests.QueryTypeObjects;
using EPiServer.ContentGraph.Api;
using EPiServer.ContentGraph.Api.Autocomplete;
using EPiServer.ContentGraph.Api.Facets;
using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using Xunit;

namespace EPiServer.ContentGraph.UnitTests
{
    public class GenerateQueryTests
    {
        private TypeQueryBuilder<RequestTypeObject> typeQueryBuilder;

        public GenerateQueryTests()
        {
            typeQueryBuilder = new TypeQueryBuilder<RequestTypeObject>();
        }

        [Fact]
        public void SelectNoneOfFieldsShouldThrowException()
        {
            typeQueryBuilder.Limit(100).Skip(0);
            Assert.Throws<ArgumentNullException>(() => typeQueryBuilder.ToQuery());
        }

        [Fact]
        public void SelectFields()
        {
            string expectedFields = @"{items{Property1 Property2}}";
            typeQueryBuilder.Fields(x => x.Property1, x => x.Property2);
            GraphQueryBuilder query = typeQueryBuilder.ToQuery();

            Assert.NotNull(query.GetQuery());
            //check selected fields
            Assert.Contains(expectedFields, query.GetQuery().Query);
            query = query.BuildQueries();
        }

        [Fact]
        public void SelectFieldWithAlias()
        {
            string expectedFields = @"{items{property1:Property1 property2:Property2}}";
            typeQueryBuilder.Field(x => x.Property1, "property1").Field(x => x.Property2, "property2");
            GraphQueryBuilder query = typeQueryBuilder.ToQuery();

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
            GraphQueryBuilder query = typeQueryBuilder.ToQuery();

            Assert.NotNull(query.GetQuery());
            //check selected fields
            Assert.Contains(expectedFields, query.GetQuery().Query);
        }

        [Theory]
        [InlineData(0, 100)]
        [InlineData(0, int.MaxValue)]
        public void QueryWithPaging(int skip, int limit)
        {
            string expectedPaging = $"(skip:{skip},limit:{limit})";
            string expectedFields = @"{items{Property1 Property2 Property3{NestedProperty}}}";
            typeQueryBuilder
                .Limit(limit)
                .Skip(skip)
                .Field(x => x.Property1)
                .Field(x => x.Property2)
                .Field(x => x.Property3.NestedProperty);
            GraphQueryBuilder query = typeQueryBuilder.ToQuery();

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
            GraphQueryBuilder query = typeQueryBuilder.ToQuery();

            Assert.NotNull(query.GetQuery());
            Assert.Contains(expectedFacets, query.GetQuery().Query);
            Assert.Contains(expectedFields, query.GetQuery().Query);
            Assert.Equal($"RequestTypeObject{{{expectedFields} {expectedFacets}}}", query.GetQuery().Query);
        }

        [Fact]
        public void FacetsQueryWithFilter()
        {
            string expectedFields = @"items{Property1 Property2}";
            string expectedFacets = @"facets{Property1(filters: [""somevalue"",""other value""]){name count} Property3{NestedProperty(ranges:[{from:1,to:2},{from:9,to:10}]){name count}}}";
            var facetFilter1 = new StringFacetFilterOperators()
                    .Filters("somevalue", "other value")
                    .Projection(FacetProperty.name, FacetProperty.count);
            var facetFilter2 = new NumericFacetFilterOperators()
                    .Ranges((1, 2), (9, 10))
                    .Projection(FacetProperty.name, FacetProperty.count);
            typeQueryBuilder
                .Field(x => x.Property1)
                .Field(x => x.Property2)
                .Facet(x => x.Property1, facetFilter1)
                .Facet(x => x.Property3.NestedProperty, facetFilter2);
            GraphQueryBuilder query = typeQueryBuilder.ToQuery();

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
            GraphQueryBuilder query = typeQueryBuilder.ToQuery();

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
                .AsType(subQuery)
                .Facet(x => x.Property3.NestedProperty);
            GraphQueryBuilder query = typeQueryBuilder.ToQuery();

            Assert.NotNull(query.GetQuery());
            Assert.Contains(expectedFacets, query.GetQuery().Query);
            Assert.Contains(expectedFields, query.GetQuery().Query);
            Assert.Equal($"RequestTypeObject{{{expectedFields} {expectedFacets}}}", query.GetQuery().Query);
        }

        [Fact]
        public void LinkQueryTests()
        {
            string childQuery = "SubTypeObject(where:{SubProperty:{match: \"test\"}}){items{SubProperty} facets{Property3{name count}}}";
            string expectedFields = $"items{{Property1 Property2 _link{{{childQuery}}}}}";
            string expectedFacets = @"facets{Property3{NestedProperty{name count}}}";
            TypeQueryBuilder<SubTypeObject> linkQuery = new TypeQueryBuilder<SubTypeObject>()
                .Field(x => x.SubProperty)
                .Where(x => x.SubProperty, new StringFilterOperators().Match("test"))
                .Facet(x => x.Property3);

            typeQueryBuilder
                .Field(x => x.Property1)
                .Field(x => x.Property2)
                .Link(linkQuery)
                .Facet(x => x.Property3.NestedProperty);
            GraphQueryBuilder query = typeQueryBuilder.ToQuery();

            Assert.NotNull(query.GetQuery());
            Assert.Contains(expectedFacets, query.GetQuery().Query);
            Assert.Contains(expectedFields, query.GetQuery().Query);
            Assert.Equal($"RequestTypeObject{{{expectedFields} {expectedFacets}}}", query.GetQuery().Query);
        }

        [Fact]
        public void Multiple_types_query()
        {
            string expectedQuery1 = "RequestTypeObject(where:{Property1:{eq: \"test\"}}){items{Property1}}";
            string expectedQuery2 = "SubTypeObject(where:{Property2:{eq: 100}}){items{Property2}}";
            string expectedFullQuery = $"query myquery {{{expectedQuery1} {expectedQuery2}}}";
            GraphQueryBuilder graphQueryBuilder = new GraphQueryBuilder();
            for (int i = 0; i < 2; i++)
            {
                graphQueryBuilder
                    .OperationName("myquery")
                    .ForType<RequestTypeObject>()
                        .Field(x => x.Property1)
                        .Where(x => x.Property1, new StringFilterOperators().Eq("test"))
                    .ToQuery()
                    .ForType<SubTypeObject>()
                        .Field(x => x.Property2)
                        .Where(x => x.Property2, new NumericFilterOperators().Eq(100))
                    .ToQuery()
                .BuildQueries();
            }

            var query = graphQueryBuilder.GetQuery();
            Assert.Contains(expectedQuery1, query.Query);
            Assert.Contains(expectedQuery2, query.Query);
            Assert.Equal(expectedFullQuery, query.Query);
        }

        [Obsolete]
        [Fact(Skip = "This functionality is no longer maintain")]
        public void ChildrenQueryTests()
        {
            string childQuery = "SubTypeObject(where:{SubProperty:{match: \"test\"}}){items{SubProperty} facets{Property3{name count}}}";
            string expectedFields = $"items{{Property1 Property2 _children{{{childQuery}}}}}";
            string expectedFacets = @"facets{Property3{NestedProperty{name count}}}";
            TypeQueryBuilder<SubTypeObject> linkQuery = new TypeQueryBuilder<SubTypeObject>()
                .Field(x => x.SubProperty)
                .Where(x => x.SubProperty, new StringFilterOperators().Match("test"))
                .Facet(x => x.Property3);

            typeQueryBuilder
                .Field(x => x.Property1)
                .Field(x => x.Property2)
                .Children(linkQuery)
                .Facet(x => x.Property3.NestedProperty);
            GraphQueryBuilder query = typeQueryBuilder.ToQuery();

            Assert.NotNull(query.GetQuery());
            Assert.Contains(expectedFacets, query.GetQuery().Query);
            Assert.Contains(expectedFields, query.GetQuery().Query);
            Assert.Equal($"RequestTypeObject{{{expectedFields} {expectedFacets}}}", query.GetQuery().Query);
        }
    }
}