using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    public class BoolFilterTest : IntegrationFixture
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            string data = "{\"index\":{\"_id\":\"1\",\"language_routing\":\"en\"}}\n" +
                    "{\"ContentType\":[\"HomePage\"],\"Id\":\"content1\", \"Name___searchable\":\"Home 1\",\"Priority\":100,\"StartPublish\":\"2022-10-11T17:17:56Z\",\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}\n" +
                    "{\"index\":{\"_id\":\"2\",\"language_routing\":\"en\"}}\n" +
                    "{\"ContentType\":[\"HomePage\"],\"Id\":\"content2\", \"Name___searchable\":\"Home 2\",\"Priority\":100,\"StartPublish\":\"2022-10-12T17:17:56Z\",\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}\n" +
                    "{\"index\":{\"_id\":\"3\",\"language_routing\":\"en\"}}\n" +
                    "{\"ContentType\":[\"HomePage\"],\"Id\":\"content3\", \"Name___searchable\":\"Not exists priority\",\"StartPublish\":\"2022-10-13T17:17:56Z\",\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}\n" +
                    "{\"index\":{\"_id\":\"4\",\"language_routing\":\"en\"}}\n" +
                    "{\"ContentType\":[\"HomePage\"],\"Id\":\"content4\", \"Name___searchable\":\"Home 4\",\"Priority\":300,\"StartPublish\":\"2022-10-14T17:17:56Z\",\"Status\":\"Published\",\"RolesWithReadAccess\":\"Everyone\"}";
            SetupData(data);
        }
        #region And filter
        [TestMethod]
        public void search_single_and_filter_with_priority_100_should_return_2_items()
        {
            IFilter andFilter = BooleanFilter<HomePage>.AndFilter
                .And(x => x.Priority, new NumericFilterOperators().Eq(100));

            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(andFilter)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(2));
        }
        [TestMethod]
        public void search_2_conditions_in_and_filter_should_return_1_item()
        {
            IFilter andFilter = BooleanFilter<HomePage>.AndFilter
                .And(x => x.Priority, new NumericFilterOperators().Eq(100))
                .And(x => x.StartPublish, new DateFilterOperators().Eq("2022-10-12T17:17:56Z"));

            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Priority, x => x.Name)
                .Where(andFilter)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(1));
            Assert.IsTrue(rs.Content.Values.First().Hits.First().Name.Equals("Home 2"));
        }

        #endregion
        #region Or filter
        [TestMethod]
        public void search_single_or_filter_with_priority_100_should_return_2_items()
        {
            IFilter orFilter = BooleanFilter<HomePage>.OrFilter
                .Or(x => x.Priority, new NumericFilterOperators().Eq(100));

            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(orFilter)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(2));
        }
        [TestMethod]
        public void search_2_conditions_in_or_filter_should_return_3_items()
        {
            IFilter orFilter = BooleanFilter<HomePage>.OrFilter
                .Or(x => x.Priority, new NumericFilterOperators().Eq(100))
                .Or(x => x.Priority, new NumericFilterOperators().Exists(false));

            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Priority, x => x.Name)
                .Where(orFilter)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(3));
        }
        #endregion
        #region Not filter
        [TestMethod]
        public void search_single_not_filter_with_priority_100_should_return_2_items()
        {
            IFilter notFilter = BooleanFilter<HomePage>.NotFilter
                .Not(x => x.Priority, new NumericFilterOperators().Eq(100));

            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Priority)
                .Where(notFilter)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(2));
        }
        [TestMethod]
        public void search_2_conditions_in_not_filter_should_return_2_items()
        {
            IFilter notFilter = BooleanFilter<HomePage>.NotFilter
                .Not(x => x.Priority, new NumericFilterOperators().Eq(100))
                .Not(x => x.Priority, new NumericFilterOperators().Exists(false));

            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Priority, x => x.Name)
                .Where(notFilter)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.First().Name.Equals("Home 4"));
        }
        #endregion

        #region more complex boolean queries
        [TestMethod]
        public void search_combine_3_boolean_filters_should_return_3_items()
        {
            // expect 1 item missing Priority field
            IFilter orFilter = BooleanFilter<HomePage>.OrFilter
                .Or(x=>x.Priority, new NumericFilterOperators().Exists(false));

            //expect 1 item content2 on result here
            IFilter andFilter1 = BooleanFilter<HomePage>.AndFilter
                .And(x => x.Priority, new NumericFilterOperators().Eq(100))
                .And(x=>x.StartPublish, new DateFilterOperators().Lt("2022-10-12T17:17:56Z"));

            //expect 1 item content4 on result here
            IFilter andFilter2 = BooleanFilter<HomePage>.AndFilter
                .And(x => x.Priority, new NumericFilterOperators().Eq(300));

            //combine all filters we expect 3 items will be returned
            orFilter.Filters = new List<IFilter>();
            orFilter.Filters.Add(andFilter1);
            orFilter.Filters.Add(andFilter2);
            IQuery query = new GraphQueryBuilder(_options)
                .ForType<HomePage>()
                .Fields(x => x.Priority, x => x.Name, x => x.StartPublish)
                .Where(orFilter)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content.Values.First().Hits.Count.Equals(3));
        }
        #endregion
    }
}
