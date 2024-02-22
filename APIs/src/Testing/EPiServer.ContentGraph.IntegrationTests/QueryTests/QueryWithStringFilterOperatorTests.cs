using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    public class Query_With_String_Filter_Operator : IntegrationFixture
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var item1 = TestDataCreator.generateIndexActionJson("1", "en", new IndexActionData { ContentType = new[] { "Content" }, Id = "content1", NameSearchable = "Steve Jobs", Author = "Steve Jobs", Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item2 = TestDataCreator.generateIndexActionJson("2", "en", new IndexActionData { ContentType = new[] { "Content" }, Id = "content2", NameSearchable = "Steve Howey", Author = "Steve Howey", Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item3 = TestDataCreator.generateIndexActionJson("3", "en", new IndexActionData { ContentType = new[] { "Content" }, Id = "content3", NameSearchable = "Alan Turing", Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });

            SetupData<Content>(item1 + item2 + item3);

        }

        #region UnSearchable field
        [TestMethod]
        public void search_name_Equals_operator_should_result_exactly_1_item()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<Content>()
                .Fields(x => x.Id, x => x.Name, x => x.Status)
                .Where(x => x.Name, new StringFilterOperators().Eq("Steve Jobs"))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<Content>().Result;
            Assert.IsTrue(rs.Content.Hits.First().Name.Equals("Steve Jobs"), $"Expected to find 'Steve Jobs', but found '{rs.Content.Hits.First().Name}'.");
        }
        [TestMethod]
        public void search_Exists_operator_should_result_2_item()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<Content>()
                .Fields(x => x.Author)
                .Where(x => x.Author, new StringFilterOperators().Exists(true))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<Content>().Result;
            Assert.IsTrue(rs.Content.Hits.Count() == 2, $"Expected 2 items, but found {rs.Content.Hits.Count()}.");
        }
        [TestMethod]
        public void search_startWith_endsWith_operator_should_result_1_item()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<Content>()
                .Fields(x => x.Author)
                .Where(x => x.Author, new StringFilterOperators().StartWith("Ste").EndWith("wey"))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<Content>().Result;
            Assert.IsTrue(rs.Content.Hits.Count() == 1, $"Expected 1 item, but found {rs.Content.Hits.Count()}.");
            Assert.IsTrue(rs.Content.Hits.First().Author.Equals("Steve Howey"), $"Expected author 'Steve Howey', but found '{rs.Content.Hits.First().Author}'.");
        }

        [TestMethod]
        public void search_with_filter_on_2_fields_should_result_1_item()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<Content>()
                .Fields(x => x.Author, x=>x.Name)
                .Where(x => x.Name, new StringFilterOperators().Match("Steve"))
                .Where(x => x.Author, new StringFilterOperators().EndWith("Jobs"))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<Content>().Result;
            Assert.IsTrue(rs.Content.Hits.Count() == 1, $"Expected 1 item, but found {rs.Content.Hits.Count()}.");
            Assert.IsTrue(rs.Content.Hits.First().Author.Equals("Steve Jobs"), $"Expected author 'Steve Jobs', but found '{rs.Content.Hits.First().Author}'.");
        }       
        [TestMethod]
        public void search_with_Like_operator_should_result_2_items()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<Content>()
                .Fields(x => x.Author, x=>x.Name)
                .Where(x => x.Name, new StringFilterOperators().Like("st_ve%"))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<Content>().Result;
            Assert.IsTrue(rs.Content.Hits.Count() == 2, $"Expected 2 items, but found {rs.Content.Hits.Count()}.");
        }
        [TestMethod]
        public void search_with_In_operator_should_result_2_items()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<Content>()
                .Fields(x => x.Author, x => x.Name)
                .Where(x => x.Id, new StringFilterOperators().In("content1", "content2"))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<Content>().Result;
            Assert.IsTrue(rs.Content.Hits.Count() == 2, $"Expected 2 items, but found {rs.Content.Hits.Count()}.");
        }
        #endregion

        #region Searchable field
        [TestMethod]
        public void search_Contains_operator_should_result_2_items()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<Content>()
                .Fields(x => x.Id, x => x.Name, x => x.Status)
                .Where(x => x.Name, new StringFilterOperators().Contains("Steve"))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<Content>().Result;
            Assert.IsTrue(rs.Content.Hits.Count() == 2, $"Expected 2 items containing 'Steve', but found {rs.Content.Hits.Count()}.");
        }
        [TestMethod]
        public void search_Match_operator_should_result_2_items()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<Content>()
                .Fields(x => x.Name)
                .Where(x => x.Name, new StringFilterOperators().Match("Steve").Fuzzy(true))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<Content>().Result;
            Assert.IsTrue(rs.Content.Hits.Count() == 2, $"Expected 2 items matching 'Steve' with fuzzy search, but found {rs.Content.Hits.Count()}.");
        }
        #endregion
    }
}
