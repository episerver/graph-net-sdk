using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EPiServer.ContentGraph.IntegrationTests.TestSupport
{
    public class TestDataCreator
    {
        public const string STATUS_PUBLISHED = "Published";
        public const string ROLES_EVERYONE = "Everyone";

        public static string generateIndexActionJson(string indexId, string languageRouting, IndexActionData indexActionData)
        {
            var indexAction = new IndexAction { Values = new IndexActionValues { Id = indexId, LanguageRouting = languageRouting } };

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
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? ContentType { get; set; }

        [JsonPropertyName("Id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Id { get; set; }

        [JsonPropertyName("Name___searchable")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? NameSearchable { get; set; }
        
        [JsonPropertyName("MainBody___searchable")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MainBodySearchable { get; set; }

        [JsonPropertyName("__typename")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TypeName { get; set; }

        [JsonPropertyName("Author")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Author { get; set; }

        [JsonPropertyName("Status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Status { get; set; }

        [JsonPropertyName("RolesWithReadAccess")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? RolesWithReadAccess { get; set; }
        
        [JsonPropertyName("Priority")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Priority { get; set; }

        [JsonPropertyName("StartPublish")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? StartPublish{ get; set; }
        
        [JsonPropertyName("IsSecret")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsSecret { get; set; }

        


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
