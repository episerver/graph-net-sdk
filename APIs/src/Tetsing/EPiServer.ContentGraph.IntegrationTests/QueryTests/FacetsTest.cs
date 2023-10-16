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

            SetupData(item1 + item2 + item3 + item4);
        }
        [TestMethod]
        public void search_with_string_facet_should_return_2_facets()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Facet(x => x.IsSecret, 
                    new StringFacetFilterOperator()
                    .Filters("true","false")
                    .OrderBy(OrderMode.DESC)
                    .OrderType(OrderType.VALUE)
                    .Projection(FacetProjection.name, FacetProjection.count))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content["HomePage"].Facets["IsSecret"].First().Count.Equals(2));
            Assert.IsTrue(rs.Content["HomePage"].Facets["IsSecret"].First().Name.Equals("true"));
            Assert.IsTrue(rs.Content["HomePage"].Facets["IsSecret"].Last().Count.Equals(2));
            Assert.IsTrue(rs.Content["HomePage"].Facets["IsSecret"].Last().Name.Equals("false"));
        }

        [TestMethod]
        public void search_with_facet_in_2_ranges_should_return_2_facets()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Facet(x=>x.Priority, new NumericFacetFilterOperator()
                    .Ranges((100,200),(200,300))
                    .Projection(FacetProjection.name, FacetProjection.count))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content["HomePage"].Facets["Priority"].Count.Equals(2));
            Assert.IsTrue(rs.Content["HomePage"].Facets["Priority"].First().Name.Equals("[100,200)"));
            Assert.IsTrue(rs.Content["HomePage"].Facets["Priority"].First().Count.Equals(2));
            Assert.IsTrue(rs.Content["HomePage"].Facets["Priority"].Last().Name.Equals("[200,300)"));
            Assert.IsTrue(rs.Content["HomePage"].Facets["Priority"].Last().Count.Equals(0));
        }
        //TODO: Should mock data for date facet because it uses current time
        //[TestMethod]
        //public void search_with_date_facet_should_return_2_facets()
        //{
        //    IQuery query = new GraphQueryBuilder(_options)
        //        .ForType<HomePage>()
        //        .Facet(x => x.StartPublish, new DateFacetFilterOperator().Unit(DateUnit.HOUR).Value(11))
        //        .ToQuery()
        //        .BuildQueries();
        //    var rs = query.GetResult<HomePage>();
        //    Assert.IsTrue(rs.Content["HomePage"].Facets["StartPublish"].Count.Equals(1));
        //}
    }
}
