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
            { ContentType = new[] { "Content" }, TypeName =  "Content", Id = "content1", NameSearchable = "Steve Jobs", Author = "Steve Jobs", Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item2 = TestDataCreator.generateIndexActionJson("2", "en", new IndexActionData
            { ContentType = new[] { "Content" }, TypeName = "Content", Id = "content2", NameSearchable = "Steve Howey", Author = "Steve Howey", Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item3 = TestDataCreator.generateIndexActionJson("3", "en", new IndexActionData
            {
                ContentType = new[] { "Content", "HomePage" },
                TypeName = "HomePage",
                Id = "content3",
                NameSearchable = "My Home",
                IsSecret = false,
                Status = TestDataCreator.STATUS_PUBLISHED,
                RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE
            });
            SetupData<Content>(item1 + item2 + item3);

        }

        [TestMethod]
        public void search_with_sub_type_should_return_result()
        {
            IQuery query = new GraphQueryBuilder(_configOptions)
                .ForType<Content>()
                    .ForSubType<HomePage>(x => x.Name, x=>x.IsSecret)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Last().Name.Equals("My Home"));
            Assert.IsFalse(rs.Content.Values.First().Hits.Last().IsSecret);
        }
    }
}
