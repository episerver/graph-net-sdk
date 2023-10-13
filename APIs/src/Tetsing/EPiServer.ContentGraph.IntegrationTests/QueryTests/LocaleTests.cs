using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    public class LocaleTests : IntegrationFixture
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            string data = "{\"index\":{\"_id\":\"1\",\"language_routing\":\"en\"}}\n" +
                "{\"ContentType\":[\"HomePage\"],\"Id\":\"content1\", \"Name___searchable\":\"Home 1\",\"Language\":{\"Name\":\"en\",\"DisplayName\":\"English\"},\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}\n" +
                "{\"index\":{\"_id\":\"2\",\"language_routing\":\"en-US\"}}\n" +
                "{\"ContentType\":[\"HomePage\"],\"Id\":\"content2\", \"Name___searchable\":\"Home 2\",\"Language\":{\"Name\":\"en-US\",\"DisplayName\":\"English\"},\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}\n" +
                "{\"index\":{\"_id\":\"3\",\"language_routing\":\"en-GB\"}}\n" +
                "{\"ContentType\":[\"HomePage\"],\"Id\":\"content3\", \"Name___searchable\":\"Home 3\",\"Language\":{\"Name\":\"en-GB\",\"DisplayName\":\"English\"},\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}";
            SetupData<HomePage>(data);
        }
        [TestMethod]
        public void search_with_fields_should_result_2_items()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Locales(new System.Globalization.CultureInfo("en-GB"), new System.Globalization.CultureInfo("en-US"))
                .Fields(x => x.Id, x => x.Name, x => x.Status)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count == 2);
            Assert.IsTrue(rs.Content.Values.First().Hits.TrueForAll(x => !x.Id.Equals("content1")));
        }
    }
}
