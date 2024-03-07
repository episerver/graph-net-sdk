using EpiServer.ContentGraph.UnitTests.QueryTypeObjects;
using EPiServer.ContentGraph.Api.Querying;
using Xunit;
using EPiServer.ContentGraph.Extensions;

namespace EpiServer.ContentGraph.UnitTests.ExtensionTests
{
    [CollectionDefinition("Query extension tests")]
    public class ExtensionTests
    {
        TypeQueryBuilder<RequestTypeObject> typeQueryBuilder;
        public ExtensionTests()
        {
            typeQueryBuilder = new TypeQueryBuilder<RequestTypeObject>();
        }

        [Fact]
        public void GetDeleted_should_build_query_with_deleted_field()
        {
            const string expectedFields = "{items{Property1 _deleted}}";
            const string expectedFullQuery = $"RequestTypeObject{expectedFields}";

            typeQueryBuilder.Field(x => x.Property1);
            typeQueryBuilder.GetDeleted();
            var query = typeQueryBuilder.ToQuery().GetQuery();

            Assert.NotNull(query);
            Assert.Contains(expectedFields, query.Query);
            Assert.Equal(query.Query, expectedFullQuery);
        }
        [Fact]
        public void GetId_should_build_query_with_id_field()
        {
            const string expectedFields = "{items{Property1 _id}}";
            const string expectedFullQuery = $"RequestTypeObject{expectedFields}";

            typeQueryBuilder.Field(x => x.Property1);
            typeQueryBuilder.GetId();
            var query = typeQueryBuilder.ToQuery().GetQuery();

            Assert.NotNull(query);
            Assert.Contains(expectedFields, query.Query);
            Assert.Equal(query.Query, expectedFullQuery);
        }
        [Fact]
        public void GetModified_should_build_query_with_modified_field()
        {
            const string expectedFields = "{items{Property1 _modified}}";
            const string expectedFullQuery = $"RequestTypeObject{expectedFields}";

            typeQueryBuilder.Field(x => x.Property1);
            typeQueryBuilder.GetModified();
            var query = typeQueryBuilder.ToQuery().GetQuery();

            Assert.NotNull(query);
            Assert.Contains(expectedFields, query.Query);
            Assert.Equal(query.Query, expectedFullQuery);
        }
        [Fact]
        public void GetScore_should_build_query_with_score_field()
        {
            const string expectedFields = "{items{Property1 _score}}";
            const string expectedFullQuery = $"RequestTypeObject{expectedFields}";

            typeQueryBuilder.Field(x => x.Property1);
            typeQueryBuilder.GetScore();
            var query = typeQueryBuilder.ToQuery().GetQuery();

            Assert.NotNull(query);
            Assert.Contains(expectedFields, query.Query);
            Assert.Equal(query.Query, expectedFullQuery);
        }
    }
}
