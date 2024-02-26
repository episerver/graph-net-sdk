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
            var item1 = TestDataCreator.generateIndexActionJson("1", "en", new IndexActionData { ContentType = new[] { "Content", "HomePage" }, Language = new TestSupport.Language { DisplayName= "English", Name = "en" }, Id = "content1", NameSearchable = "Steve Job", Author = "manv", Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item2 = TestDataCreator.generateIndexActionJson("2", "en-GB", new IndexActionData { ContentType = new[] { "Content", "HomePage" }, Language = new TestSupport.Language { DisplayName = "English", Name = "en-GB" }, Id = "content2", NameSearchable = "Tim Cook", Author = "manv", Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item3 = TestDataCreator.generateIndexActionJson("3", "en-US", new IndexActionData { ContentType = new[] { "Content", "HomePage" }, Language = new TestSupport.Language { DisplayName = "English", Name = "en-US" }, Id = "content3", NameSearchable = "Alan Turing", Author = "manv", Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item4 = TestDataCreator.generateIndexActionJson("4", "en", new IndexActionData { ContentType = new[] { "Content", "HomePage" }, Language = new TestSupport.Language { DisplayName = "English", Name = "en" }, Id = "content4", NameSearchable = "Home 4", Priority = 300, IsSecret = false, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            SetupData<HomePage>(item1 + item2 + item3 + item4);
        }
        [TestMethod]
        public void search_with_fields_should_result_2_items()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Locales(new System.Globalization.CultureInfo("en-GB"), new System.Globalization.CultureInfo("en-US"))
                .Fields(x => x.Id, x => x.Name, x => x.Status)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync().Result;
            Assert.IsTrue(rs.GetContent<HomePage>().Hits.Count() == 2);
            Assert.IsTrue(rs.GetContent<HomePage>().Hits.ToList().TrueForAll(x => !x.Id.Equals("content1")));
        }
    }
}
