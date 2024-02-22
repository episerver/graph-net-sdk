using EPiServer.ContentGraph.Api;
using EPiServer.ContentGraph.Api.Facets;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    public class FacetsTest : IntegrationFixture
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var item1 = TestDataCreator.generateIndexActionJson("1", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content1", NameSearchable = "Home 1", Priority = 100, IsSecret = true, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item2 = TestDataCreator.generateIndexActionJson("2", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content2", NameSearchable = "Home 2", Priority = 100, IsSecret = true, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item3 = TestDataCreator.generateIndexActionJson("3", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content3", NameSearchable = "Not exists priority", IsSecret = false, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item4 = TestDataCreator.generateIndexActionJson("4", "en", new IndexActionData { ContentType = new[] { "HomePage" }, Id = "content4", NameSearchable = "Home 4", Priority = 300, IsSecret = false, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });

            SetupData<HomePage>(item1 + item2 + item3 + item4);
        }
        [TestMethod]
        public void search_with_string_facet_should_return_2_facets()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                    .Field(x => x.IsSecret)
                    .Facet(x => x.IsSecret, 
                        new StringFacetFilterOperators()
                        .Filters("true","false")
                        .OrderBy(OrderMode.DESC)
                        .OrderType(OrderType.VALUE))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<HomePage>().Result;
            Assert.IsTrue(rs.Content.Facets["IsSecret"].First().Count.Equals(2), "Expected 2 facets for 'IsSecret' with value 'true', but found " + rs.Content.Facets["IsSecret"].First().Count + ".");
            Assert.IsTrue(rs.Content.Facets["IsSecret"].First().Name.Equals("true"), "Expected first facet name to be 'true', but found '" + rs.Content.Facets["IsSecret"].First().Name + "'.");
            Assert.IsTrue(rs.Content.Facets["IsSecret"].Last().Count.Equals(2), "Expected 2 facets for 'IsSecret' with value 'false', but found " + rs.Content.Facets["IsSecret"].Last().Count + ".");
            Assert.IsTrue(rs.Content.Facets["IsSecret"].Last().Name.Equals("false"), "Expected last facet name to be 'false', but found '" + rs.Content.Facets["IsSecret"].Last().Name + "'.");
        }

        [TestMethod]
        public void search_with_facet_in_2_ranges_should_return_2_facets()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                .Facet(x=>x.Priority, new NumericFacetFilterOperators()
                    .Ranges((100,200),(200,300))
                    .Projection(FacetProperty.name, FacetProperty.count))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<HomePage>().Result;
            Assert.IsTrue(rs.Content.Facets["Priority"].Count().Equals(2), "Expected 2 facets for 'Priority', but found " + rs.Content.Facets["Priority"].Count() + ".");
            Assert.IsTrue(rs.Content.Facets["Priority"].First().Name.Equals("[100,200)"), "Expected first priority facet to be in range [100,200), but found '" + rs.Content.Facets["Priority"].First().Name + "'.");
            Assert.IsTrue(rs.Content.Facets["Priority"].First().Count.Equals(2), "Expected 2 items in the first priority range, but found " + rs.Content.Facets["Priority"].First().Count + ".");
            Assert.IsTrue(rs.Content.Facets["Priority"].Last().Name.Equals("[200,300)"), "Expected last priority facet to be in range [200,300), but found '" + rs.Content.Facets["Priority"].Last().Name + "'.");
            Assert.IsTrue(rs.Content.Facets["Priority"].Last().Count.Equals(0), "Expected 0 items in the last priority range, but found " + rs.Content.Facets["Priority"].Last().Count + ".");
        }
        [TestMethod]
        public void search_with_facet_filters_should_return_correct_items()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                    .Field(x=>x.IsSecret)
                    .Facet(x => x.IsSecret, new StringFacetFilterOperators().Filters("true"))
                    .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<HomePage>().Result;
            Assert.IsTrue(rs.Content.Facets["IsSecret"].Count().Equals(2), "Expected 2 facets for 'IsSecret', but found " + rs.Content.Facets["IsSecret"].Count() + ".");
            Assert.IsTrue(rs.Content.Hits.Count().Equals(2), "Expected 2 items when filtering by 'IsSecret'='true', but found " + rs.Content.Hits.Count() + ".");
            Assert.IsTrue(rs.Content.Hits.Select(x=>x.IsSecret).ToList().TrueForAll(x => x));
        }
        [TestMethod]
        public void search_with_2_facet_should_return_2_facets()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                    .Field(x => x.IsSecret)
                    .Facet(x => x.IsSecret, new StringFacetFilterOperators().Filters("true"))
                    .Facet(x => x.Status, new StringFacetFilterOperators().Filters(TestDataCreator.STATUS_PUBLISHED))
                    .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<HomePage>().Result;
            Assert.IsTrue(rs.Content.Facets.Count.Equals(2), "Expected 2 facets, but found " + rs.Content.Facets.Count + ".");
            Assert.IsNotNull(rs.Content.Facets["IsSecret"], "Expected 'IsSecret' facet to be present, but it was not found.");
            Assert.IsNotNull(rs.Content.Facets["Status"], "Expected 'Status' facet to be present, but it was not found.");
        }
        [TestMethod]
        public void search_with_facet_limit_1_should_return_facet_count_equals_1()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<HomePage>()
                    .Facet(x => x.IsSecret, new StringFacetFilterOperators().Filters("true").Limit(1))
                    .Facet(x => x.Status, new StringFacetFilterOperators().Filters(TestDataCreator.STATUS_PUBLISHED))
                    .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<HomePage>().Result;
            Assert.IsTrue(rs.Content.Facets.Count.Equals(2));
            Assert.IsNotNull(rs.Content.Facets["IsSecret"]);
            Assert.IsNotNull(rs.Content.Facets["Status"]);
        }
    }
}
