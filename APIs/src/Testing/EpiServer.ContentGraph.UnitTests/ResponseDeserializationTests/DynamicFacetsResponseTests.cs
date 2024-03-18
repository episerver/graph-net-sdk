using Xunit;
using Newtonsoft.Json;
using EPiServer.ContentGraph.Api.Result;

namespace EpiServer.ContentGraph.UnitTests.DeserializationTets
{
    public class DynamicFacetsResponseTests
    {
        const string response = "{\"data\":{\"Object\":{\"items\":[{\"Name\":\"Coreware\"}],\"facets\":{\"Categories\":{\"Id\":[{\"name\":\"100\",\"count\":3}],\"Language\":{\"Name\":[{\"name\":\"en\",\"count\":3}]}},\"Name\":[{\"name\":\"Farfetch\",\"count\":3},{\"name\":\"Geta\",\"count\":3}]},\"total\":1154}},\"extensions\":{\"correlationId\":\"8662284c0889108a\",\"cost\":62,\"costSummary\":[\"CompanyBlock(62)=basicFilter(1)*2+facets(2)*30\"]}}";
        [Fact]
        public void nested_facets_response_should_convert_successfully()
        {
            var results = JsonSerializer.CreateDefault().Deserialize<ContentGraphResult<object>>(new JsonTextReader(new StringReader(response)));
            var facetsDict = results.Content.Facets;
            Assert.True(facetsDict.Count.Equals(3));

            Assert.NotNull(facetsDict["Categories.Id"]);
            Assert.True(facetsDict["Categories.Id"].Count().Equals(1));

            Assert.NotNull(facetsDict["Categories.Language.Name"]);
            Assert.True(facetsDict["Categories.Language.Name"].Count().Equals(1));

            Assert.NotNull(facetsDict["Name"]);
            Assert.True(facetsDict["Name"].Count().Equals(2));
        }
    }
}
