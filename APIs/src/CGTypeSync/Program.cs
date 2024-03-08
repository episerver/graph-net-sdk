using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Optimizely.Graph.Client.Tools
{
    public class Program
    {
        private static string USER_AGENT => $"Optimizely-Graph-Tools/{typeof(Program).Assembly.GetName().Version}";
        static void Main(string[] args)
        {
            Console.WriteLine("************************************ Optimizely Content Graph Client Tools ************************************");
            Console.WriteLine("This tool will generate C# classes from Optimizely Graph schema. Ensure you config correct account");

            string configPath = "appsettings.json", directory = ".", source = "default";
            if (args.Length > 0)
            {
                configPath = args[0];
            }
            
            IConfigurationRoot config = new ConfigurationBuilder()
                                        .AddJsonFile(configPath)
                                        .AddEnvironmentVariables()
                                        .Build();
            
            if (config != null)
            {
                if (args.Length > 1)
                {
                    directory = args[1];
                }

                if (args.Length > 2)
                {
                    source = args[2];
                }

                Run(config, directory, source);
            }
        }
        public static void Run(IConfiguration configuration, string output, string source)
        {
            //parse the CGTypes.json file
            var schemaTypes = new Dictionary<string, List<Tuple<string, string>>>();
            const string fileName = "GraphModels.cs";
            JObject json = GetSchemaDataTypes(configuration, source);
            if (json == null)
            {
                Console.WriteLine("Error! schema is null");
                return;
            }
            var propertyTypes = json["propertyTypes"];
            var contentTypes = json["contentTypes"];

            Console.WriteLine("Writing files...");
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using EPiServer.Core;");
            sb.AppendLine("using EPiServer.Framework.Blobs;");
            sb.AppendLine("using EPiServer.SpecializedProperties;");
            sb.AppendLine("using EPiServer.DataAnnotations;");
            sb.AppendLine("using System.Globalization;");
            sb.AppendLine();
            sb.AppendLine("namespace Optimizely.ContentGraph.DataModels");
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
            if (string.IsNullOrEmpty(output))
            {
                output = Directory.GetCurrentDirectory();
            }
            if (!Directory.Exists(output)){
                Directory.CreateDirectory(output);
            }
            var outFile = Path.Combine(output, fileName);
            using (var writer = new StreamWriter(outFile, false))
            {
                writer.Write(sb.ToString());
            }
            Console.WriteLine($"Classes had been generated to {output}/{fileName}.");
        }

        private static string ConvertType(string propType)
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

        private static JObject GetSchemaDataTypes(IConfiguration configuration, string source)
        {
            Console.WriteLine("Getting data types from schema...");
            try
            {
                if (string.IsNullOrEmpty(source) || string.IsNullOrWhiteSpace(source))
                {
                    source = "default";
                }

                HttpClient client = CreateHttpClient(configuration);
                var httpResponse = client.GetAsync($"api/content/v3/types?id={source}").GetAwaiter().GetResult();
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return JObject.Parse(httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
                else
                {
                    Console.WriteLine($"Can not get data types. Status code: {httpResponse.StatusCode}; Reason:{httpResponse.ReasonPhrase}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Errors: {e.Message}");
            }
            return null;
        }
        private static HttpClient CreateHttpClient(IConfiguration configuration)
        {
            var config = ReadConfig(configuration);
            var authenticationString = $"{config.AppKey}:{config.SecretKey}";

            var base64AuthString = Convert.ToBase64String(Encoding.UTF8.GetBytes(authenticationString));
            return new HttpClient()
            {
                BaseAddress = new Uri(config.GatewayAddress),
                DefaultRequestHeaders = {
                    { "User-Agent", USER_AGENT },
                    { "Authorization", $"Basic {base64AuthString}"},
                    { "ContentType", "application/json" }
                }
            };
        }

        private static Config ReadConfig(IConfiguration configuration)
        {
            Console.WriteLine("Reading appsettings.json...");
            var myconfigs = configuration.GetSection("Optimizely");
            return new Config()
            {
                GatewayAddress = myconfigs.GetSection("ContentGraph:GatewayAddress").Value,
                AppKey = myconfigs.GetSection("ContentGraph:AppKey").Value,
                SecretKey = myconfigs.GetSection("ContentGraph:Secret").Value
            };
        }
    }
    internal class Config
    {
        public string GatewayAddress { get; set; }
        public string AppKey { get; set; }
        public string SecretKey { get; set; }
    }
}