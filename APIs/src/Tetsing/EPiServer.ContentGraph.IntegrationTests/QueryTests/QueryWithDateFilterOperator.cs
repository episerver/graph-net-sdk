using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    public class QueryWithDateFilterOperator : IntegrationFixture
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            SetupData();
        }
        private static void SetupData()
        {
            string path = $@"{WorkingDirectory}\TestingData\SimpleTypeMapping.json";
            using (StreamReader mappingReader = new StreamReader(path))
            {
                string data = "{\"index\":{\"_id\":\"1\",\"language_routing\":\"en\"}}\n" +
                    "{\"ContentType\":[\"HomePage\"],\"Id\":\"content1\", \"Name___searchable\":\"Home 1\",\"MainBody___searchable\":\"Steve Jobs\",\"StartPublish\":\"2023-10-11T17:17:56Z\",\"Priority\":1,\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}\n" +
                    "{\"index\":{\"_id\":\"2\",\"language_routing\":\"en\"}}\n" +
                    "{\"ContentType\":[\"HomePage\"],\"Id\":\"content2\", \"Name___searchable\":\"Home 2\",\"MainBody___searchable\":\"Steve Howey\",\"StartPublish\":\"2023-09-11T20:17:56Z\",\"Priority\":3,\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}\n" +
                    "{\"index\":{\"_id\":\"3\",\"language_routing\":\"en\"}}\n" +
                    "{\"ContentType\":[\"HomePage\"],\"Id\":\"content3\", \"Name___searchable\":\"Home 3\",\"MainBody___searchable\":\"Alan Turing\",\"StartPublish\":\"2023-11-11T05:17:56Z\",\"Priority\":5,\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}";
                string mapping = mappingReader.ReadToEnd();
                ClearData();
                PushMapping(mapping);
                BulkIndexing<HomePage>(data);
            }
        }
    }
}
