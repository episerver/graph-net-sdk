using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EPiServer.ContentGraph.IntegrationTests.TestSupport
{
    public class TestDataCreator
    {
        public static string generateIndexActionJson(string indexId, string languageRouting, string[] contentTypes, string contentId, string nameSearchable, string author, string status, string roles)
        {
            var indexAction = new IndexAction { Values = new IndexActionValues { Id = indexId, LanguageRouting = languageRouting } };
            var indexActionData = new IndexActionData { ContentType = contentTypes, Id = contentId, NameSearchable = nameSearchable, Author = author, Status = status, RolesWithReadAccess = roles };

            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                writer.Write(JsonSerializer.Serialize(indexAction));
                writer.Write("\n");
                writer.Write(JsonSerializer.Serialize(indexActionData));
                writer.Write("\n");
                writer.Flush();
                return Encoding.Default.GetString((memoryStream.ToArray()));
            }
        }
    }

    public class IndexActionData
    {
        [JsonPropertyName("ContentType")]
        public string[]? ContentType { get; set; }

        [JsonPropertyName("Id")]
        public string? Id { get; set; }

        [JsonPropertyName("Name___searchable")]
        public string? NameSearchable { get; set; }

        [JsonPropertyName("Author")]
        public string? Author { get; set; }

        [JsonPropertyName("Status")]
        public string? Status { get; set; }

        [JsonPropertyName("RolesWithReadAccess")]
        public string? RolesWithReadAccess { get; set; }

    }
    public class IndexAction
    {
        [JsonPropertyName("index")]
        public IndexActionValues? Values { get; set; }
    }
    public class IndexActionValues
    {
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        [JsonPropertyName("language_routing")]
        public string? LanguageRouting { get; set; }
    }
}
