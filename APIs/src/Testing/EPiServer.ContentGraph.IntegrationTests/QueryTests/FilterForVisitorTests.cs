﻿using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.ContentGraph.IntegrationTests.TestSupport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiServer.ContentGraph.IntegrationTests.QueryTests
{
    [TestClass]
    [TestCategory("FilterForVisitor")]
    public class FilterForVisitorTests : IntegrationFixture
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var item1 = TestDataCreator.generateIndexActionJson("1", "en", new IndexActionData { 
                ContentType = new[] { "Content" }, Id = "content1", NameSearchable = "Steve Jobs", Author = "optiq", Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item2 = TestDataCreator.generateIndexActionJson("2", "en", new IndexActionData { 
                ContentType = new[] { "Content" }, Id = "content2", NameSearchable = "Tim Cook", Author = "manv", Status = TestDataCreator.STATUS_PUBLISHED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });
            var item3 = TestDataCreator.generateIndexActionJson("3", "en", new IndexActionData { 
                ContentType = new[] { "Content" }, Id = "content3", NameSearchable = "Alan Turing", Author = "manv", Status = TestDataCreator.STATUS_DELETED, RolesWithReadAccess = TestDataCreator.ROLES_EVERYONE });

            SetupData<Content>(item1 + item2 + item3, "t4");
        }
        [TestMethod]
        public void search_without_filter_for_visitors_should_result_3_items()
        {
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<Content>()
                .Fields(x => x.Id, x => x.Name, x => x.Status)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<Content>().Result;
            Assert.IsTrue(rs.Content.Hits.Count() == 3, $"Expected 3 items without filter for visitors, but found {rs.Content.Hits.Count()}.");
            Assert.IsTrue(rs.Content.Hits.Where(x=>x.Status == TestDataCreator.STATUS_PUBLISHED).Count().Equals(2), $"Expected 2 items with status published without filter for visitors, but found {rs.Content.Hits.Where(x=>x.Status == TestDataCreator.STATUS_PUBLISHED).Count()}.");
        }
        [TestMethod]
        public void search_with_filter_for_visitors_should_result_1_item()
        {
            var filterForvistors = testingHost.Services.GetServices<IFilterForVisitor>().ToArray();
            IQuery query = new GraphQueryBuilder(_configOptions, _httpClientFactory)
                .ForType<Content>()
                .Fields(x => x.Id, x => x.Name, x => x.Status)
                .FilterForVisitor(filterForvistors)
                .ToQuery()
                .BuildQueries();
            var rs = query.GetResultAsync<Content>().Result;
            Assert.IsTrue(rs.Content.Hits.Count() == 1, $"Expected 1 item with filter for visitors, but found {rs.Content.Hits.Count()}.");
            Assert.IsTrue(rs.Content.Hits.First().Status.Equals(TestDataCreator.STATUS_PUBLISHED), $"Expected the status of the first item with filter for visitors to be published, but found {rs.Content.Hits.First().Status}.");
        }
    }
}
