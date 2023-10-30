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
            var result = new GraphQueryBuilder(_configOptions)
                .ForType<HomePage>()
                .Fields(x => x.Name, x => x.MainBody )
                .OrderBy(x => x.Name)
                .ToQuery()
                .BuildQueries()
                .GetResult<HomePage>();
            Assert.IsTrue(result.Content["HomePage"].Hits.First().Name.Equals("Beatles"));
            Assert.IsTrue(result.Content["HomePage"].Hits.Last().Name.Equals("Rolling Stones"));
        }

        [TestMethod]
        public void orderby_on_datetime_should_return_correct_order()
        {
            var result = new GraphQueryBuilder(_configOptions)
                .ForType<HomePage>()
                .Fields(x => x.Name, x => x.StartPublish)
                .OrderBy(x => x.StartPublish)
                .ToQuery()
                .BuildQueries()
                .GetResult<HomePage>();
            var dates = new DateTime[4];
            for (int i = 0; i < 4; i++)
            {
                dates[i] = result.Content["HomePage"].Hits[i].StartPublish;
            }

            Assert.IsTrue(dates[0] < dates[1]);
            Assert.IsTrue(dates[1] < dates[2]);
            Assert.IsTrue(dates[2] < dates[3]);
        }

        [TestMethod]
        public void orderby_on_int_should_return_correct_order()
        {
            var result = new GraphQueryBuilder(_configOptions)
                .ForType<HomePage>()
                .Fields(x => x.Name, x => x.Priority)
                .OrderBy(x => x.Priority)
                .ToQuery()
                .BuildQueries()
                .GetResult<HomePage>();
            var ints = new int?[4];
            for (int i = 0; i < 4; i++)
            {
                ints[i] = result.Content["HomePage"].Hits[i].Priority;
            }

            Assert.IsTrue(ints[0] < ints[1]);
            Assert.IsTrue(ints[1] < ints[2]);
            Assert.IsTrue(ints[3].IsNull());
        }
    }
}
