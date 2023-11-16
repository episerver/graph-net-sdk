using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;

namespace CGTypeSync
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new Program().Run();
        }

        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public void Run()
        {
            //parse the CGTypes.json file
            var schemaTypes = new Dictionary<string, List<Tuple<string, string>>>();
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);
            var schemaFile = Path.Combine(directory, "CGTypes.json");
            JObject json = null;
            using (StreamReader r = new StreamReader(schemaFile))
            {
                var jsonText = r.ReadToEnd();
                json = JObject.Parse(jsonText);

            }

            var propertyTypes = json["propertyTypes"];
            var contentTypes = json["contentTypes"];

            
            var sb = new StringBuilder();
            sb.AppendLine("using EPiServer.Core;");
            sb.AppendLine("using EPiServer.Framework.Blobs;");
            sb.AppendLine("using EPiServer.SpecializedProperties;");
            sb.AppendLine("using EPiServer.DataAnnotations;");
            sb.AppendLine("using System.Globalization;");
            sb.AppendLine();
            sb.AppendLine("namespace EPiServer.ContentGraph.DataModels");
            sb.AppendLine("{");

            foreach (JProperty propertyType in propertyTypes)
            {
                var propertyTypeName = propertyType.Name;
                
                sb.AppendLine($"    public class {propertyTypeName}");
                sb.AppendLine ("    {");

                var properties = propertyType.First().Children().Children().Children();

                foreach (JProperty propertyProperty in properties)
                {
                    var name = propertyProperty.Name;
                    var dataType = (propertyProperty.Children()["type"].First() as JValue).Value.ToString();
                    dataType = ConvertType(dataType);
                    sb.Append($"        public {dataType} {name} ");
                    sb.AppendLine("{ get; set; }");

                }

                sb.AppendLine("    }");
            }

            foreach (JProperty contentType in contentTypes)
            {
                var propertyTypeName = contentType.Name;
                var parentTypes = contentType.First()?["contentType"]?.Values();
                var inheritedFromType = string.Empty;

                if (parentTypes != null && parentTypes.Count() > 0)
                {
                    inheritedFromType = string.Join(',', parentTypes.Select(type=> type.ToString()));
                    inheritedFromType = $":{inheritedFromType}";
                }
                sb.AppendLine($"    public class {propertyTypeName}{inheritedFromType}");
                sb.AppendLine("    {");

                var properties = contentType.First().Children().Children().Children();

                foreach (JProperty propertyProperty in properties.Where(x => x is JProperty))
                {
                    var name = propertyProperty.Name;
                    var searchableAttr = (bool)(propertyProperty.Children()["searchable"].First() as JValue).Value;
                    var dataType = (propertyProperty.Children()["type"].First() as JValue).Value.ToString();
                    dataType = ConvertType(dataType);
                    if (searchableAttr)
                    {
                        sb.AppendLine($"        [Searchable]");
                    }
                    sb.Append($"        public {dataType} {name} ");
                    sb.AppendLine("{ get; set; }");

                }

                sb.AppendLine("    }");
            }


            sb.AppendLine("}");
            var classes = sb.ToString();

            var outFile = Path.Combine(directory, @"..\..\..\..\Templates\EPiServer.ContentGraph.DataModels\ProxyClasses.cs");
            using (var writer = new StreamWriter(outFile, false))
            {
                writer.Write(sb.ToString());
            }


        }


        private string ConvertType(string propType)
        {
            bool isIEnumerable = false;
            if (propType.StartsWith("[") && propType.EndsWith("]"))//Is IEnumerable
            {
                propType = propType.Substring(1, propType.Length - 2);
                isIEnumerable = true;
            }
            if (propType.Equals("Date"))
            {
                propType = "DateTime";
            }
            if (propType.Equals("Bool") || propType.Equals("Boolean"))
            {
                propType = "bool";
            }
            if (propType.Equals("String"))
            {
                propType = "string";
            }
            if (propType.Equals("LongString"))
            {
                propType = "string";
            }
            if (propType.Equals("Int"))
            {
                propType = "int";
            }
            if (propType.Equals("Float"))
            {
                propType = "float";
            }
            if (isIEnumerable)
            {
                propType = $"IEnumerable<{propType}>";
            }

            return propType;

        }
    }
}