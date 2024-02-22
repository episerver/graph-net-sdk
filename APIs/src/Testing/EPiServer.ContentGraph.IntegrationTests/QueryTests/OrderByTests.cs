using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EPiServer.ContentGraph.Api.Result;
using System.Globalization;
using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    public class OrderByTests : IntegrationFixture
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var item1 = TestDataCreator.generateIndexActionJson("1", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content1", NameSearchable = "John Mayall and The Bluesbreakers", Priority = 200, IsSecret = true, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE, StartPublish = DateTime.Parse("2022-10-19T17:17:56Z", null, DateTimeStyles.AdjustToUniversal )});
            var item2 = TestDataCreator.generateIndexActionJson("2", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content2", NameSearchable = "Rolling Stones", Priority = 100, IsSecret = true, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE, StartPublish = DateTime.Parse("2022-10-11T17:18:56Z", null, DateTimeStyles.AdjustToUniversal) });
            var item3 = TestDataCreator.generateIndexActionJson("3", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content3", NameSearchable = "Kiss", IsSecret = false, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE, StartPublish = DateTime.Parse("2022-10-11T17:17:56Z", null, DateTimeStyles.AdjustToUniversal) });
            var item4 = TestDataCreator.generateIndexActionJson("4", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content4", NameSearchable = "Beatles", Priority = 300, IsSecret = false, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE, StartPublish = DateTime.Parse("1967-10-11T17:17:56Z", null, DateTimeStyles.AdjustToUniversal) });

            SetupData<HomePage>(item1 + item2 + item3 + item4);
        }
        [TestMethod]
        public void orderby_on_searchable_string_should_return_correct_order()
        {
            var result = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Name, x => x.MainBody )
                .OrderBy(x => x.Name)
                .ToQuery()
                .BuildQueries()
                .GetResultAsync<HomePage>().Result;
            Assert.IsTrue(result.Content.Hits.First().Name.Equals("Beatles"), "Expected 'Beatles' to be the first item when ordered by name, but found '" + result.Content.Hits.First().Name + "'.");
            Assert.IsTrue(result.Content.Hits.Last().Name.Equals("Rolling Stones"), "Expected 'Rolling Stones' to be the last item when ordered by name, but found '" + result.Content.Hits.Last().Name + "'.");
        }

        [TestMethod]
        public void orderby_on_datetime_should_return_correct_order()
        {
            var result = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Name, x => x.StartPublish)
                .OrderBy(x => x.StartPublish)
                .ToQuery()
                .BuildQueries()
                .GetResultAsync<HomePage>().Result;
            var resultArray = result.Content.Hits.ToArray();
            var dates = new DateTime[4];
            for (int i = 0; i < 4; i++)
            {
                dates[i] = resultArray[i].StartPublish.Value;
            }

            Assert.IsTrue(dates[0] < dates[1], $"Expected date at index 0 to be earlier than date at index 1, but found '{dates[0]}' and '{dates[1]}'.");
            Assert.IsTrue(dates[1] < dates[2], $"Expected date at index 1 to be earlier than date at index 2, but found '{dates[1]}' and '{dates[2]}'.");
            Assert.IsTrue(dates[2] < dates[3], $"Expected date at index 2 to be earlier than date at index 3, but found '{dates[2]}' and '{dates[3]}'.");
        }

        [TestMethod]
        public void orderby_on_int_should_return_correct_order()
        {
            var result = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Fields(x => x.Name, x => x.Priority)
                .OrderBy(x => x.Priority)
                .ToQuery()
                .BuildQueries()
                .GetResultAsync<HomePage>().Result;
            var ints = new int?[4];
            var resultArray = result.Content.Hits.ToArray();
            for (int i = 0; i < 4; i++)
            {
                ints[i] = resultArray[i].Priority;
            }

            Assert.IsTrue(ints[0] < ints[1], $"Expected int at index 0 to be less than int at index 1, but found '{ints[0]}' and '{ints[1]}'.");
            Assert.IsTrue(ints[1] < ints[2], $"Expected int at index 1 to be less than int at index 2, but found '{ints[1]}' and '{ints[2]}'.");
            Assert.IsTrue(ints[3].IsNull(), "Expected int at index 3 to be null.");
        }
    }
}
