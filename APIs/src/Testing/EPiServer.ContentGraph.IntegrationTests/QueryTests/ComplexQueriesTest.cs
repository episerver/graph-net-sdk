﻿using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    public class ComplexQueriesTest : IntegrationFixture
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var item1 = TestDataCreator.generateIndexActionJson("1", "en", new IndexActionData { 
                ContentType = new[] { "Content","HomePage" }, TypeName = "HomePage", Id = "content1",
                NameSearchable= "Action Movie",
                MainBodySearchable = "Wild Wild West is a 1999 American steampunk Western film co-produced and directed by Barry Sonnenfeld and written by S. S. Wilson and Brent Maddock alongside Jeffrey Price and Peter S. Seaman, from a story penned by brothers Jim and John Thomas. Loosely adapted from The Wild Wild West, a 1960s television series created by Michael Garrison, it is the only production since the television film More Wild Wild West (1980) to feature the characters from the original series. The film stars Will Smith (who previously collaborated with Sonnenfeld on Men in Black two years earlier in 1997) and Kevin Kline as two U.S. Secret Service agents who work together to protect U.S. President Ulysses S. Grant (Kline, in a dual role) and the United States from all manner of dangerous threats during the American Old West.", 
                Priority = 100, IsSecret = true, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item2 = TestDataCreator.generateIndexActionJson("2", "en", new IndexActionData { 
                ContentType = new[] { "HomePage" }, TypeName = "HomePage", Id = "content2", NameSearchable = "Histories", 
                MainBodySearchable= "The American frontier, also known as the Old West, popularly known as the Wild West, encompasses the geography, history, folklore, and culture associated with the forward wave of American expansion in mainland North America that began with European colonial settlements in the early 17th century and ended with the admission of the last few contiguous western territories as states in 1912. This era of massive migration and settlement was particularly encouraged by President Thomas Jefferson following the Louisiana Purchase, giving rise to the expansionist attitude known as \"Manifest Destiny\" and the historians' \"Frontier Thesis\". The legends, historical events and folklore of the American frontier have embedded themselves into United States culture so much so that the Old West, and the Western genre of media specifically, has become one of the defining periods of American national identity.",
                Priority = 100, IsSecret = true, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item3 = TestDataCreator.generateIndexActionJson("3", "en", new IndexActionData { 
                ContentType = new[] { "HomePage" }, TypeName = "HomePage", Id = "content3", NameSearchable = "Optimizely AI",
                MainBodySearchable = "Semantic search is supported on searchable string fields, and for the full-text search operators contains and match. It is recommended to set fields that have a lot of content (such as the MainBody in the Optimizely CMS) as searchable to unlock the full-text search capabilities. Optimizely Graph uses a pre-trained model for semantic search.",
                Priority = 0, IsSecret = false, Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });

            SetupData<HomePage>(item1 + item2 + item3);
        }

        [TestMethod]
        public void semantic_search_order_should_result_correct_data()
        {
            IQuery query = new GraphQueryBuilder(_configOptions)
                .ForType<HomePage>()
                .Fields(x => x.Name, x=> x.MainBody)
                .FullTextSearch(new StringFilterOperators().Contains("Wild West"))
                .Where(x=>x.IsSecret, new StringFilterOperators().Eq("true").Boost(10))
                .OrderBy(Ranking.SEMANTIC).OrderBy(x=>x.Name)
                .GetCursor()
                .Facet(x=>x.ContentType)
                .Total()
                .Skip(0).Limit(3)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResult<HomePage>();
            Assert.IsTrue(rs.Content["HomePage"].Total.Equals(2));
            Assert.IsTrue(rs.Content["HomePage"].Hits.First().Name.Equals("Action Movie"));
            Assert.IsNotNull(rs.Content["HomePage"].Cursor);
            Assert.IsNotNull(rs.Content["HomePage"].Facets["ContentType"].Exists(x=>x.Name.Equals("HomePage")));
            Assert.IsNotNull(rs.Content["HomePage"].Facets["ContentType"].Exists(x=>x.Name.Equals("Content")));
        }
        //add more later
    }
}