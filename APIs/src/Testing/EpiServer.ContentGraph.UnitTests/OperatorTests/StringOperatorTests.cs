using EPiServer.ContentGraph.Api;
using EPiServer.ContentGraph.Api.Filters;
using Xunit;

namespace EpiServer.ContentGraph.UnitTests.OperatorTests
{
    public class StringOperatorTests
    {
        readonly static string inOpt = "in: [\"1\",\"2\",\"3\"]";
        readonly static string notInOpt = "notIn: [\"4\",\"5\"]";
        readonly static string expectedstring = $"boost: 10,exist: true,startsWith: \"start\",endsWith: \"end\",synonyms: [ONE],contains: \"test\",{inOpt},{notInOpt},like: \"alloy\",notEq: \"bad\"";

        [Fact]
        public void ChainOperatorsBuildTest()
        {
            StringFilterOperators filterOperators = new StringFilterOperators()
                .Boost(10)
                .Exists(true)
                .StartWith("start")
                .EndWith("end")
                .Synonym(Synonyms.ONE)
                .Contains("test")
                .In("1", "2", "3")
                .NotIn("4", "5")
                .Like("alloy")
                .NotEq("bad");
            Assert.Equal(expectedstring, filterOperators.Query);
        }
    }
}
