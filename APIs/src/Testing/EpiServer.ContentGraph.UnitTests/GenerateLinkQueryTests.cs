using EpiServer.ContentGraph.UnitTests.QueryTypeObjects;
using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using Xunit;

namespace EpiServer.ContentGraph.UnitTests
{
    public class GenerateLinkQueryTests
    {
        private TypeQueryBuilder<RequestTypeObject> typeQueryBuilder;
        public GenerateLinkQueryTests()
        {
            typeQueryBuilder = new TypeQueryBuilder<RequestTypeObject>();
        }

        [Fact]
        public void LinkQueryTests()
        {
            string childQuery = "SubTypeObject(where:{SubProperty:{match: \"test\"}}){items{SubProperty} facets{Property3{name count}}}";
            string expectedFields = $"items{{Property1 Property2 _link(type:CUSTOMERREFERENCES) {{{childQuery}}}}}";
            string expectedFacets = @"facets{Property3{NestedProperty{name count}}}";
            var linkQuery = new LinkQueryBuilder<SubTypeObject>("CUSTOMERREFERENCES")
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
        public void multiple_links_query_should_generate_correct_query()
        {
            string expectedLink1 = "SubTypeObject(where:{SubProperty:{match: \"test1\"}}){items{SubProperty} facets{Property3{name count}}}";
            string expectedLink2 = "SubTypeObject(where:{Property1:{match: \"test2\"}}){items{Property1} facets{Property3{name count}}}";
            string expectedFields = $"items{{Property1 Property2 _link(type:CUSTOMERREFERENCES) {{{expectedLink1}}} _link(type:DEFAULT) {{{expectedLink2}}}}}";
            string expectedFacets = @"facets{Property3{NestedProperty{name count}}}";
            var linkQuery1 = new LinkQueryBuilder<SubTypeObject>("CUSTOMERREFERENCES")
                .Field(x => x.SubProperty)
                .Where(x => x.SubProperty, new StringFilterOperators().Match("test1"))
                .Facet(x => x.Property3);

            var linkQuery2 = new LinkQueryBuilder<SubTypeObject>("DEFAULT")
                .Field(x => x.Property1)
                .Where(x => x.Property1, new StringFilterOperators().Match("test2"))
                .Facet(x => x.Property3);

            typeQueryBuilder
                .Field(x => x.Property1)
                .Field(x => x.Property2)
                .Link(linkQuery1)
                .Link(linkQuery2)
                .Facet(x => x.Property3.NestedProperty);
            GraphQueryBuilder query = typeQueryBuilder.ToQuery();

            Assert.Equal(linkQuery1.GetQuery().Query, expectedLink1);
            Assert.Equal(linkQuery2.GetQuery().Query, expectedLink2);

            Assert.Contains(expectedFacets, query.GetQuery().Query);
            Assert.Contains(expectedFields, query.GetQuery().Query);
            Assert.Equal($"RequestTypeObject{{{expectedFields} {expectedFacets}}}", query.GetQuery().Query);
        }

        [Fact]
        public void nested_links_query_should_generate_nested_link_query()
        {
            string expectedLink1 = "SubTypeObject(where:{SubProperty:{match: \"test1\"}}){items{SubProperty} facets{Property3{name count}}}";
            string expectedLink2 = "SubTypeObject(where:{Property1:{match: \"test2\"}}){items{Property1 _link(type:CUSTOMERREFERENCES) {" + expectedLink1 + "}} facets{Property3{name count}}}";
            string expectedFields = $"items{{Property1 Property2 _link(type:DEFAULT) {{{expectedLink2}}}}}";
            string expectedFacets = @"facets{Property3{NestedProperty{name count}}}";
            var linkQuery1 = new LinkQueryBuilder<SubTypeObject>()
                .WithLinkType("CUSTOMERREFERENCES")
                .Field(x => x.SubProperty)
                .Where(x => x.SubProperty, new StringFilterOperators().Match("test1"))
                .Facet(x => x.Property3);

            var linkQuery2 = new LinkQueryBuilder<SubTypeObject>("DEFAULT")
                .Field(x => x.Property1)
                .Where(x => x.Property1, new StringFilterOperators().Match("test2"))
                .Facet(x => x.Property3)
                .Link(linkQuery1);

            typeQueryBuilder
                .Field(x => x.Property1)
                .Field(x => x.Property2)
                .Link(linkQuery2)
                .Facet(x => x.Property3.NestedProperty);
            GraphQueryBuilder query = typeQueryBuilder.ToQuery();

            Assert.Equal(linkQuery1.GetQuery().Query, expectedLink1);
            Assert.Equal(linkQuery2.GetQuery().Query, expectedLink2);

            Assert.Contains(expectedFacets, query.GetQuery().Query);
            Assert.Contains(expectedFields, query.GetQuery().Query);
            Assert.Equal($"RequestTypeObject{{{expectedFields} {expectedFacets}}}", query.GetQuery().Query);
        }
        [Fact]
        public void nested_link_query_with_aliases()
        {
            string expectedLink1 = "SubTypeObject(where:{SubProperty:{match: \"test1\"}}){items{SubProperty} facets{Property3{name count}}}";
            string expectedLink2 = "SubTypeObject(where:{Property1:{match: \"test2\"}}){items{Property1 mylink1:_link(type:CUSTOMERREFERENCES) {" + expectedLink1 + "}} facets{Property3{name count}}}";
            string expectedFields = $"items{{Property1 Property2 mylink2:_link(type:DEFAULT) {{{expectedLink2}}}}}";
            string expectedFacets = @"facets{Property3{NestedProperty{name count}}}";
            var linkQuery1 = new LinkQueryBuilder<SubTypeObject>()
                .WithLinkType("CUSTOMERREFERENCES")
                .Field(x => x.SubProperty)
                .Where(x => x.SubProperty, new StringFilterOperators().Match("test1"))
                .Facet(x => x.Property3);

            var linkQuery2 = new LinkQueryBuilder<SubTypeObject>("DEFAULT")
                .Field(x => x.Property1)
                .Where(x => x.Property1, new StringFilterOperators().Match("test2"))
                .Facet(x => x.Property3)
                .Link(linkQuery1, "mylink1");

            typeQueryBuilder
                .Field(x => x.Property1)
                .Field(x => x.Property2)
                .Link(linkQuery2, "mylink2")
                .Facet(x => x.Property3.NestedProperty);
            GraphQueryBuilder query = typeQueryBuilder.ToQuery();

            Assert.Equal(linkQuery1.GetQuery().Query, expectedLink1);
            Assert.Equal(linkQuery2.GetQuery().Query, expectedLink2);

            Assert.Contains(expectedFacets, query.GetQuery().Query);
            Assert.Contains(expectedFields, query.GetQuery().Query);
            Assert.Equal($"RequestTypeObject{{{expectedFields} {expectedFacets}}}", query.GetQuery().Query);
        }
    }
}
