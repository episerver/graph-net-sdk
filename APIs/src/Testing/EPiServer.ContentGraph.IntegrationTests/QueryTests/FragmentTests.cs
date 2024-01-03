using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    public class FragmentTests : IntegrationFixture
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var item1 = TestDataCreator.generateIndexActionJson("1", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content1", NameSearchable = "Home 1", Priority = 100, IsSecret = true, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item2 = TestDataCreator.generateIndexActionJson("2", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content2", NameSearchable = "Home 2", Priority = 100, IsSecret = true, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item3 = TestDataCreator.generateIndexActionJson("3", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content3", NameSearchable = "Home 3", IsSecret = false, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item4 = TestDataCreator.generateIndexActionJson("4", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content4", NameSearchable = "Home 4", Priority = 300, IsSecret = false, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });

            SetupData<HomePage>(item1 + item2 + item3 + item4);
        }
        [TestMethod]
        public void select_2_fields_in_fragment_should_return_2_fields_in_result()
        {
            FragmentBuilder<HomePage> fragment = new FragmentBuilder<HomePage>()
                .Fields(x => x.Name, x => x.ContentType);

            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                 .ForType<HomePage>()
                    .Fields(x => x.Id)
                    .Fragment(fragment)
                 .ToQuery()
                .BuildQueries();

            var response = query.GetResultAsync<HomePage>().Result;
            var names = response.Content.Hits.Select(x => x.Name);
            var types = response.Content.Hits.Select(x => x.ContentType);
            var emptyName = names.FirstOrDefault(name => string.IsNullOrEmpty(name));
            Assert.IsNull(names.FirstOrDefault(n => string.IsNullOrEmpty(n)));
            Assert.IsNull(types.FirstOrDefault(t => t is null));
        }
    }
}
