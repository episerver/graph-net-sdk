using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    public class SubTypeQueryTests : IntegrationFixture
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var item1 = TestDataCreator.generateIndexActionJson("1", "en", new IndexActionData
            {
                ContentType = new[] { "Content" },
                TypeName = "Content",
                Id = "content1",
                NameSearchable = "Action Movie",
                MainBodySearchable = "Wild Wild West is a 1999 American steampunk Western film co-produced and directed by Barry Sonnenfeld and written by S.",
                Priority = 100,
                IsSecret = true,
                Status = TestDataCreator.STATUS_PUBLISHED,
                RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE
            });
            var item2 = TestDataCreator.generateIndexActionJson("2", "en", new IndexActionData
            {
                ContentType = new[] { "Content", "HomePage" },
                TypeName = "HomePage",
                Id = "content2",
                NameSearchable = "Histories",
                MainBodySearchable = "The American frontier, also known as the Old West, popularly known as the Wild West.",
                Priority = 100,
                IsSecret = true,
                Status = TestDataCreator.STATUS_PUBLISHED,
                RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE
            });
            var item3 = TestDataCreator.generateIndexActionJson("3", "en", new IndexActionData
            {
                ContentType = new[] { "Content", "HomePage" },
                TypeName = "HomePage",
                Id = "content3",
                NameSearchable = "Optimizely AI",
                MainBodySearchable = "Semantic search is supported on searchable string fields, and for the full-text search operators contains and match.",
                Priority = 0,
                IsSecret = false,
                Status = TestDataCreator.STATUS_PUBLISHED,
                RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE
            });

            SetupData<HomePage>(item1 + item2 + item3, "t14");
        }

        [TestCategory("Subtype test")]
        [TestMethod]
        public void given_1_parent_and_2_children_objects_search_response_should_returns_total_3_hits()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<Content>()
                .Fields(x => x.Name)
                    .AsType<HomePage>(x => x.MainBody)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync().Result;
            int actualHitsCount = rs.GetContent<Content, HomePage>().Hits.Count();
            Assert.IsTrue(actualHitsCount.Equals(3), $"Expected 3 hits, but got {actualHitsCount}.");
        }

        [TestCategory("Subtype test")]
        [TestMethod]
        public void given_1_parent_and_2_children_objects_search_response_should_returns_children_items()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<Content>()
                .Fields(x => x.Name)
                    .AsType<HomePage>(x => x.MainBody)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync().Result;
            var homepages = rs.GetContent<Content, HomePage>().Hits.Where(x => x.MainBody?.Length > 0);
            int actualChildrenCount = homepages.Count();
            Assert.IsTrue(actualChildrenCount.Equals(2), $"Expected 2 children items, but got {actualChildrenCount}.");
        }
    }
}
