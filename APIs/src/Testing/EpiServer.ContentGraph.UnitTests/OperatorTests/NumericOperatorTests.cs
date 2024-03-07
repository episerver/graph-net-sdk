using EPiServer.ContentGraph.Api.Filters;
using Xunit;

namespace EpiServer.ContentGraph.UnitTests.OperatorTests
{
    public class NumericOperatorTests
    {
        readonly static string inOpt = "in: [1,2,3]";
        readonly static string notInOpt = "notIn: [4,5]";
        readonly static string expectedstring = $"boost: 10,exist: true,eq: 100,notEq: 0,lt: {int.MaxValue},lte: {int.MaxValue},gt: {int.MinValue},gte: {int.MinValue},{inOpt},{notInOpt}";
        [Fact]
        public void ChainOperatorsBuildTest()
        {
            int lt = int.MaxValue;
            int gt = int.MinValue;

            NumericFilterOperators numericFilterOperators = new NumericFilterOperators()
                .Boost(10)
                .Exists(true)
                .Eq(100)
                .NotEq(0)
                .Lt(lt)
                .Lte(lt)
                .Gt(gt)
                .Gte(gt)
                .In(1, 2, 3)
                .NotIn(4, 5);
            Assert.Equal(expectedstring, numericFilterOperators.Query);
        }
    }
}
