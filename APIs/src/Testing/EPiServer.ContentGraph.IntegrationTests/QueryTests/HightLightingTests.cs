using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Extensions;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    public class HightLightingTests : IntegrationFixture
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var item1 = TestDataCreator.generateIndexActionJson("1", "en", new IndexActionData
            {
                ContentType = new[] { "Content","HomePage" },
                TypeName = "HomePage",
                Id = "content1",
                NameSearchable = "Action Movie",
                MainBodySearchable = "Wild Wild West is a 1999 American steampunk Western film co-produced and directed by Barry Sonnenfeld and written by S. S. Wilson and Brent Maddock alongside Jeffrey Price and Peter S. Seaman, from a story penned by brothers Jim and John Thomas. Loosely adapted from The Wild Wild West, a 1960s television series created by Michael Garrison, it is the only production since the television film More Wild Wild West (1980) to feature the characters from the original series. The film stars Will Smith (who previously collaborated with Sonnenfeld on Men in Black two years earlier in 1997) and Kevin Kline as two U.S. Secret Service agents who work together to protect U.S. President Ulysses S. Grant (Kline, in a dual role) and the United States from all manner of dangerous threats during the American Old West.",
                Priority = 100,
                IsSecret = true,
                Status = TestDataCreator.STATUS_PUBLISHED,
                RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE
            });

            SetupData<HomePage>(item1);
        }
        [TestMethod]
        public void search_data_with_highlight_should_result_wrap_keyword_in_tag()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Field(x => x.MainBody, HighLightOptions.Create().Enable(true))
                .Where(x=> x.MainBody.Match("American"))
                .ToQuery()
                .BuildQueries();

            var rs = query.GetResultAsync<HomePage>().Result;
            Assert.IsTrue(rs.Content.Hits.First().MainBody.Contains("<em>American</em>"));
        }
    }
}
