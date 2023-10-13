using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    public class QueryAutocompleteOperatorTests : IntegrationFixture
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            string data = "{\"index\":{\"_id\":\"1\",\"language_routing\":\"en\"}}\n" +
                "{\"ContentType\":[\"HomePage\"],\"Id\":\"content1\", \"Name___searchable\":\"Home 1\",\"Priority\":100,\"IsSecret\": true,\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}\n" +
                "{\"index\":{\"_id\":\"2\",\"language_routing\":\"en\"}}\n" +
                "{\"ContentType\":[\"HomePage\"],\"Id\":\"content2\", \"Name___searchable\":\"Home 2\",\"Priority\":100,\"IsSecret\": true,\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}\n" +
                "{\"index\":{\"_id\":\"3\",\"language_routing\":\"en\"}}\n" +
                "{\"ContentType\":[\"HomePage\"],\"Id\":\"content3\", \"Name___searchable\":\"Not exists priority\",\"IsSecret\": false,\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}\n" +
                "{\"index\":{\"_id\":\"4\",\"language_routing\":\"en\"}}\n" +
                "{\"ContentType\":[\"HomePage\"],\"Id\":\"content4\", \"Name___searchable\":\"Home 4\",\"Priority\":300,\"IsSecret\": false,\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}";
            SetupData(data);
        }

    }
}
