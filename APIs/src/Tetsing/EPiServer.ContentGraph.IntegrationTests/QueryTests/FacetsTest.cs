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
            string data = "{\"index\":{\"_id\":\"1\",\"language_routing\":\"en\"}}\n" +
                "{\"ContentType\":[\"HomePage\"],\"Id\":\"content1\", \"Name___searchable\":\"Home 1\",\"Priority\":100,\"IsSecret\": true,\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}\n" +
                "{\"index\":{\"_id\":\"2\",\"language_routing\":\"en\"}}\n" +
                "{\"ContentType\":[\"HomePage\"],\"Id\":\"content2\", \"Name___searchable\":\"Home 2\",\"Priority\":100,\"IsSecret\": true,\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}\n" +
                "{\"index\":{\"_id\":\"3\",\"language_routing\":\"en\"}}\n" +
                "{\"ContentType\":[\"HomePage\"],\"Id\":\"content3\", \"Name___searchable\":\"Not exists priority\",\"IsSecret\": false,\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}\n" +
                "{\"index\":{\"_id\":\"4\",\"language_routing\":\"en\"}}\n" +
                "{\"ContentType\":[\"HomePage\"],\"Id\":\"content4\", \"Name___searchable\":\"Home 4\",\"Priority\":300,\"IsSecret\": false,\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}";
            SetupData<HomePage>(data);
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
                    .Projection())
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

        [TestMethod]
        public void search_with_complex_facets_should_return_2_facets()
        {
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Facet(x => x.IsSecret, new StringFacetFilterOperator().Filters("true","false").Projection(FacetProjection.name))
                .Facet(x=> x.Priority, new NumericFacetFilterOperator().Ranges((100, 200), (200, 300)).Projection(FacetProjection.name))
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            //Assert.IsTrue(rs.Content["HomePage"].Facets["IsSecret"].First().Name.Equals("true"));
            //Assert.IsTrue(rs.Content["HomePage"].Facets["IsSecret"].First().Count.Equals("true"));
            Assert.IsTrue(rs.Content["HomePage"].Facets["Priority"].Count.Equals(2));
        }
    }
}
