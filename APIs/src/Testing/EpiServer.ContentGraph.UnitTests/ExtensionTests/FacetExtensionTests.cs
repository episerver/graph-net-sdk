using EPiServer.ContentGraph.Api.Querying;
using EpiServer.ContentGraph.UnitTests.QueryTypeObjects;
using Xunit;
using EPiServer.ContentGraph.Extensions;

namespace EpiServer.ContentGraph.UnitTests.ExtensionTests
{
    [CollectionDefinition("Facet extension tests")]
    public class FacetExtensionTests
    {
        TypeQueryBuilder<RequestTypeObject> typeQueryBuilder;
        public FacetExtensionTests()
        {
            typeQueryBuilder = new TypeQueryBuilder<RequestTypeObject>();
        }

        [Fact]
        public void generate_facet_filter_with_extension()
        {
            const string expectedFields = "items{Property1}";
            const string expectedFacet = "facets{Property1(filters: [\"test\"]){name count}}";
            const string expectedFullQuery = $"RequestTypeObject{{{expectedFields} {expectedFacet}}}";

            typeQueryBuilder.Field(x => x.Property1);
            typeQueryBuilder.Facet(x => x.Property1.FacetFilters("test"));

            var query = typeQueryBuilder.ToQuery().GetQuery();

            Assert.NotNull(query);
            Assert.Contains(expectedFacet, query.Query);
            Assert.Equal(query.Query, expectedFullQuery);
        }
        [Fact]
        public void generate_facet_limit_with_extension()
        {
            const string expectedFields = "items{Property1}";
            const string expectedFacet = "facets{Property1(limit: 10){name count}}";
            const string expectedFullQuery = $"RequestTypeObject{{{expectedFields} {expectedFacet}}}";

            typeQueryBuilder.Field(x => x.Property1);
            typeQueryBuilder.Facet(x => x.Property1.FacetLimit(10));

            var query = typeQueryBuilder.ToQuery().GetQuery();

            Assert.NotNull(query);
            Assert.Contains(expectedFacet, query.Query);
            Assert.Equal(query.Query, expectedFullQuery);
        }
        [Fact]
        public void generate_facet_with_IEnumerable()
        {
            const string expectedFields = "items{Property1}";
            const string expectedFacet = "facets{NestedObjects{NestedProperty{name count}}}";
            const string expectedFullQuery = $"RequestTypeObject{{{expectedFields} {expectedFacet}}}";

            typeQueryBuilder.Field(x => x.Property1);
            typeQueryBuilder.Facet(x => x.NestedObjects, f=> f.NestedProperty);

            var query = typeQueryBuilder.ToQuery().GetQuery();

            Assert.NotNull(query);
            Assert.Contains(expectedFacet, query.Query);
            Assert.Equal(query.Query, expectedFullQuery);
        }
        [Fact]
        public void generate_facet_limit_with_IEnumerable()
        {
            const string expectedFields = "items{Property1}";
            const string expectedFacet = "facets{NestedObjects{NestedProperty(limit: 10){name count}}}";
            const string expectedFullQuery = $"RequestTypeObject{{{expectedFields} {expectedFacet}}}";

            typeQueryBuilder.Field(x => x.Property1);
            typeQueryBuilder.Facet(x => x.NestedObjects, f => f.NestedProperty.FacetLimit(10));

            var query = typeQueryBuilder.ToQuery().GetQuery();

            Assert.NotNull(query);
            Assert.Contains(expectedFacet, query.Query);
            Assert.Equal(query.Query, expectedFullQuery);
        }
    }
}
