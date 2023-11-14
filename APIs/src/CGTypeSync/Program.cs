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
            var schemaFile = @"C:\source\opti-graph-netclient\APIs\src\CGTypeSync\CGTypes.json";
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

                sb.AppendLine($"    public class {propertyTypeName}");
                sb.AppendLine("    {");

                var properties = contentType.First().Children().Children().Children();

                foreach (JProperty propertyProperty in properties.Where(x => x is JProperty))
                {
                    var name = propertyProperty.Name;
                    var dataType = (propertyProperty.Children()["type"].First() as JValue).Value.ToString();
                    dataType = ConvertType(dataType);
                    sb.Append($"        public {dataType} {name} ");
                    sb.AppendLine("{ get; set; }");

                }

                sb.AppendLine("    }");
            }


            sb.AppendLine("}");
            var classes = sb.ToString();

            using (var writer = new StreamWriter(@"C:\source\opti-graph-netclient\APIs\src\Templates\EPiServer.ContentGraph.DataModels\Class1.cs", false))
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
                if (!propType.Equals("LinkItemNode") && !propType.Equals("CategoryModel") && !propType.Equals("MainContentArea"))
                {
                    isIEnumerable = true;
                }
            }
            if (propType.Equals("ContentModelReference"))
            {
                propType = "PageReference";
            }
            if (propType.Equals("ContentModelReferenceSearch"))
            {
                propType = "PageReference";
            }
            if (propType.Equals("ContentLanguageModel"))
            {
                propType = "CultureInfo";
            }
            if (propType.Equals("Date"))
            {
                propType = "DateTime";
            }
            if (propType.Equals("BlobModel"))
            {
                propType = "Blob";
            }
            if (propType.Equals("LinkItemNode"))
            {
                propType = "LinkItemCollection";
            }
            if (propType.Equals("CategoryModel"))
            {
                propType = "CategoryList";
            }
            if (propType.Equals("ContentAreaItemModelSearch"))
            {
                propType = "ContentArea";
            }
            if (propType.Equals("Bool"))
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
            if (propType.Equals("FooterSectionFooterRow"))
            {
                propType = "FooterRow";
            }
            if (propType.Equals("NewsPagePageListBlock"))
            {
                propType = "PageListBlock";
            }
            if (propType.Equals("StartPageSiteLogotypeBlock"))
            {
                propType = "SiteLogotypeBlock";
            }
            if (propType.Equals("FooterCategoryFooterSection"))
            {
                propType = "FooterSection";
            }
            if (propType.Equals("StartPageFooterCategory"))
            {
                propType = "FooterCategory";
            }
            if (propType.Equals("ContentLanguageModelSearch"))
            {
                propType = "CultureInfo";
            }
            if (isIEnumerable)
            {
                propType = $"IEnumerable<{propType}>";
            }

            return propType;

        }
    }
}