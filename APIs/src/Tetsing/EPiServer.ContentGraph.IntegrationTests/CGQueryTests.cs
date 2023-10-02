using EPiServer.ContentGraph.Api.Querying;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EPiServer.Core;
using Optimizely.ContentGraph.Cms.NetCore;
using EPiServer.ContentGraph.Api.Result;
using EPiServer.ContentGraph.Configuration;
using Optimizely.ContentGraph.Cms.Configuration;
using Microsoft.Extensions.Options;

namespace EPiServer.ContentGraph.IntegrationTests
{
    [TestClass]
    public class CGQueryTests
    {
        protected readonly IOptions<QueryOptions> _queryOption;
        public CGQueryTests(IOptions<QueryOptions> queryOptions)
        {
            _queryOption = queryOptions;
        }
        [TestMethod]
        public void SimpleQueryTest()
        {
            IQuery query = new GraphQueryBuilder()
                .ForType<IContent>()
                .Fields(x=>x.Name)
                .Build();
          var rs = query.GetResult<IContent>();
        }
    }
}
