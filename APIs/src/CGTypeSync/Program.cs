using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace CGTypeSync
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new Program().Run();
        }

        public void Run()
        {
            //parse the CGTypes.json file
            var schemaTypes = new Dictionary<string, List<Tuple<string, string>>>();
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);
            var schemaFile = Path.Combine(directory, "CGTypes.json");

            Console.WriteLine($"Loading content graph types from {schemaFile}");

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

                Console.WriteLine($"Processing property type {propertyTypeName}");

                sb.AppendLine($"    public class {propertyTypeName}");
                sb.AppendLine ("    {");

                var properties = propertyType.First().Children().Children().Children();

                foreach (JProperty propertyProperty in properties)
                {
                    
                    var name = propertyProperty.Name;
                    var dataType = (propertyProperty.Children()["type"].First() as JValue).Value.ToString();

                    Console.WriteLine($"  Found property '{name}' with data type {dataType}");
                    dataType = ConvertType(dataType);

                    Console.WriteLine($"  Adding property '{name}' with data type {dataType}");
                    sb.Append($"        public {dataType} {name} ");
                    sb.AppendLine("{ get; set; }");

                }

                sb.AppendLine("    }");
            }
            
            
            foreach (JProperty contentType in contentTypes)
            {

                var propertyTypeName = contentType.Name;

                Console.WriteLine($"Processing content type {propertyTypeName}");

                sb.AppendLine($"    public class {propertyTypeName}");
                sb.AppendLine("    {");

                var properties = contentType.First().Children().Children().Children();

                foreach (JProperty propertyProperty in properties.Where(x => x is JProperty))
                {
                    var name = propertyProperty.Name;
                    var dataType = (propertyProperty.Children()["type"].First() as JValue).Value.ToString();
                    Console.WriteLine($"  Found property '{name}' with data type {dataType}");
                    dataType = ConvertType(dataType);

                    Console.WriteLine($"  Adding property '{name}' with data type {dataType}");
                    sb.Append($"        public {dataType} {name} ");
                    sb.AppendLine("{ get; set; }");

                }

                sb.AppendLine("    }");
            }
            sb.AppendLine("}");
            var classes = sb.ToString();
            Console.WriteLine($"Finished processing schema");

            var outFile = Path.Combine(directory, @"..\..\..\..\Templates\EPiServer.ContentGraph.DataModels\ProxyClasses.cs");
            Console.WriteLine($"Generate classes to file {outFile}");
            using (var writer = new StreamWriter(outFile, false))
            {
                writer.Write(sb.ToString());
            }
            Console.WriteLine("Done!");
            Console.WriteLine("Press any key to exit");
            Console.Read();


        }


        private string ConvertType(string propType)
        {
            var oldProp = propType;
            
            bool isIEnumerable = false;
            if (propType.StartsWith("[") && propType.EndsWith("]"))//Is IEnumerable
            {
                propType = propType.Substring(1, propType.Length - 2);
                if (!propType.Equals("LinkItemNode") && !propType.Equals("CategoryModel") && !propType.Equals("MainContentArea"))
                {
                isIEnumerable = true;
            }
            }
            if (propType.Equals("Date"))
            {
                propType = "DateTime";
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
            if (isIEnumerable)
            {
                propType = $"IEnumerable<{propType}>";
            }

            if(!oldProp.Equals(propType))
            {
                Console.WriteLine($"  Converting type from {oldProp} to {propType}");
            }
            return propType;

        }
    }
}