using Xunit;
using Newtonsoft.Json;
using EPiServer.ContentGraph.Api.Result;

namespace EpiServer.ContentGraph.UnitTests.DeserializationTets
{
    public class DynamicAutoCompleteResponseTests
    {
        const string response = "{\"data\":{\"Object\":{\"autocomplete\":{\"Categories\":{\"ProviderName\":[\"Sample Category\"]},\"WebsiteUrl\":[\"http://example.com/\",\"https://test.com/\"]},\"total\":2}},\"extensions\":{\"correlationId\":\"866380417b4f1fb0\",\"cost\":72,\"costSummary\":[\"CompanyBlock(72)=basicFilter(1)*2+autocomplete(2)*35\"]}}";
        [Fact]
        public void nested_aotucompletes_should_convert_successfully()
        {
            var results = JsonSerializer.CreateDefault().Deserialize<ContentGraphResult<object>>(new JsonTextReader(new StringReader(response)));
            var autoCompletesDict = results.Content.AutoComplete;
            Assert.True(autoCompletesDict.Count.Equals(2));

            Assert.NotNull(autoCompletesDict["Categories.ProviderName"]);
            Assert.True(autoCompletesDict["Categories.ProviderName"].Count().Equals(1));

            Assert.NotNull(autoCompletesDict["WebsiteUrl"]);
            Assert.True(autoCompletesDict["WebsiteUrl"].Count().Equals(2));
        }
    }
}
