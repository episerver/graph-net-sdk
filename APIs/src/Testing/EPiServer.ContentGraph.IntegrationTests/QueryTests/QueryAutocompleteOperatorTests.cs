using EPiServer.ContentGraph.Api.Autocomplete;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
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
            var item1 = TestDataCreator.generateIndexActionJson("1", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "myid1", NameSearchable = "Home 1", Priority = 100, IsSecret = true, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item2 = TestDataCreator.generateIndexActionJson("2", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "myid2", NameSearchable = "Home 2", Priority = 100, IsSecret = true, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item3 = TestDataCreator.generateIndexActionJson("3", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "myid3", NameSearchable = "Not exists priority", IsSecret = false, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item4 = TestDataCreator.generateIndexActionJson("4", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "notmyidandtheidtoolong", NameSearchable = "Home 4", Priority = 300, IsSecret = false, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });

            SetupData<HomePage>(item1 + item2 + item3 + item4, "t9");
        }
		[TestMethod]
        public void autocomplete_id_contains_myid_should_result_3_phrases()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                    .Autocomplete(x=>x.Id, new AutoCompleteOperators().Value("myid"))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<HomePage>().Result;
            Assert.IsTrue(rs.Content.AutoComplete["Id"].Count().Equals(3), $"Expected 3 phrases for autocomplete on 'Id' containing 'myid', but found {rs.Content.AutoComplete["Id"].Count()}.");
        }
        [TestMethod]
        public void autocomplete_id_contains_myid_and_limit_1_should_result_1_phrase()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                    .Autocomplete(x => x.Id, 
                        new AutoCompleteOperators()
                        .Value("myid")
                        .Limit(1))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<HomePage>().Result;
            Assert.IsTrue(rs.Content.AutoComplete["Id"].Count().Equals(1), $"Expected 1 phrase for autocomplete on 'Id' containing 'myid' with limit 1, but found {rs.Content.AutoComplete["Id"].Count()}.");
        }
        [TestMethod]
        public void autocomplete_has_value_more_than_10_characters_will_be_ignore()
        {
            string value = "notmyidandthe";
            Assert.IsTrue(value.Length > 10);
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                    .Autocomplete(x => x.Id, new AutoCompleteOperators().Value(value))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<HomePage>().Result;
            Assert.IsTrue(rs.Content.AutoComplete["Id"].Count().Equals(0), $"Expected 0 phrases for autocomplete on 'Id' with value more than 10 characters, but found {rs.Content.AutoComplete["Id"].Count()}.");
        }
    }
}
