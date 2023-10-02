using EPiServer.ContentGraph.Api.Filters;
using Xunit;

namespace EpiServer.ContentGraph.UnitTests.OperatorTests
{
    public class DateOperatorTests
    {
        readonly static string expectedstring = $"boost: 10," +
            $"eq: \"{DateTime.Now.ToShortDateString()}\",notEq: \"test\"," +
            $"lt: \"{DateTime.MaxValue.ToShortDateString()}\",lte: \"{DateTime.MaxValue.ToShortDateString()}\"," +
            $"gt: \"{DateTime.MinValue.ToShortDateString()}\",gte: \"{DateTime.MinValue.ToShortDateString()}\"";
        [Fact]
        public void ChainDateOperatorTests()
        {
            DateFilterOperators dateFilterOperators = new DateFilterOperators()
           .Boost(10)
           .Eq(DateTime.Now.ToShortDateString())
           .NotEq("test")
           .Lt(DateTime.MaxValue.ToShortDateString())
           .Lte(DateTime.MaxValue.ToShortDateString())
           .Gt(DateTime.MinValue.ToShortDateString())
           .Gte(DateTime.MinValue.ToShortDateString());
            Assert.Equal(dateFilterOperators.Query, expectedstring);
        }

    }
}
