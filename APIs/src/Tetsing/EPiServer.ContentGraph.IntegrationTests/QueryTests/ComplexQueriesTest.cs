using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    public class ComplexQueriesTest : IntegrationFixture
    {
        [TestMethod]
        public void semantic_search_should_result_correct_data()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<Content>()
                .Fields(x => x.Name)
                .FullTextSearch(new StringFilterOperators().Contains("Alan Turing"))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<Content>();
            Assert.IsTrue(rs.Content.Values.First().Hits.First().Name.Equals("Alan Turing"));
        }
    }
}
