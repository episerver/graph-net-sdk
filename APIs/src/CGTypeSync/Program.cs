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
            Console.WriteLine("**** Optimizely Content Graph Client Tools ****");
            Console.WriteLine("Version: " + USER_AGENT);
            Console.WriteLine("This tool will generate C# classes from Optimizely Graph schema.");
            Console.WriteLine("Learn more: https://github.com/episerver/graph-net-sdk");
            Console.WriteLine("");
            Console.WriteLine("Usage:");
            Console.WriteLine("dotnet ogschema <settingsfile> <output> <optional-source> <optional-namespace>");
            Console.WriteLine("settingsfile: Relative or full path to an appSettings.json file with Graph configuration");
            Console.WriteLine("output: Relative or full path to a directory or .cs file to write generated C# models to");
            Console.WriteLine("optional-source: The content source in your Graph instance, default value is: \"default\"");
            Console.WriteLine("optional-namespace: The namespace for the generated C# classes");
            
            
            const string defaultFileName = "GraphModels.cs";
            string configPath = "appsettings.json",
                output = ".",
                source = "default",
                modelNamespace = "Optimizely.ContentGraph.DataModels";
            if (args.Length > 0)
            {
                configPath = args[0];
            }

            FileInfo fi = new FileInfo(configPath);
            if (fi.Exists == false)
            {
                Console.WriteLine("Cannot locate settings file: {0}", fi.FullName);
                Environment.ExitCode = -1;
                return;
            }
            
            // Make sure we operate on a full path
            configPath = fi.FullName;
            
            IConfigurationRoot config = new ConfigurationBuilder()
                                        .AddJsonFile(configPath)
                                        .AddEnvironmentVariables()
                                        .Build();
            
            if (config != null)
            {
                if (args.Length > 1)
                {
                    // First, check if output is a file (we don't care if it exists or not)
                    fi = new FileInfo(args[1]);
                    if (string.IsNullOrEmpty(fi.Extension) == false)
                    {
                        // we have a file, just pass that on
                        output = fi.FullName;
                    }
                    else
                    {
                        // Not a file, assume it is a directory
                        var di = new DirectoryInfo(args[1]);
                        output = Path.Combine(di.FullName, defaultFileName);
                    }
                }

                if (args.Length > 2)
                {
                    source = args[2];
                }

                if (args.Length > 3)
                {
                    modelNamespace = args[3];
                }

                Console.WriteLine("");
                Console.WriteLine("Running with the following settings");
                Console.WriteLine("  Configuration file: {0}", configPath);
                Console.WriteLine("  Output: {0}", output);
                Console.WriteLine("  Content Source: {0}", source);
                Console.WriteLine("  Namespace: {0}", modelNamespace);
                Console.WriteLine("");
                Run(config, output, source, modelNamespace);
            }
            else
            {
                Console.WriteLine("Cannot build settings from settings file: {0}", configPath);
                Environment.ExitCode = -1;
            }
        }
        public static void Run(IConfiguration configuration,
            string output,
            string source,
            string modelNamespace)
        {
            //parse the CGTypes.json file
            var schemaTypes = new Dictionary<string, List<Tuple<string, string>>>();
            JObject json = GetSchemaDataTypes(configuration, source);
            if (json == null)
            {
                Console.WriteLine("Error! schema is null");
                Environment.ExitCode = -1;
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
            sb.AppendLine("namespace " + modelNamespace);
            sb.AppendLine("{");

            foreach (JProperty propertyType in propertyTypes)
            {
                var propertyTypeName = propertyType.Name;
                
                sb.AppendLine($"    public partial class {propertyTypeName}");
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
                sb.AppendLine($"    public partial class {propertyTypeName}{inheritedFromType}");
                sb.AppendLine("    {");

                var properties = contentType.First().Children().Children().Children();

                foreach (JProperty propertyProperty in properties.Where(x => x is JProperty))
                {
                    var name = propertyProperty.Name;
                    var searchableAttr = (bool)((propertyProperty.Children()["searchable"].FirstOrDefault() as JValue)?.Value ?? false);
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
            FileInfo fi = new FileInfo(output);
            // Check if directory exists, create if not
            if(fi.Directory != null && fi.Directory.Exists == false)
            {
                fi.Directory.Create();
            }
            
            using (var writer = new StreamWriter(output, false))
            {
                writer.Write(sb.ToString());
            }
            Console.WriteLine($"Optimizely Graph model C# classes have been written to {output}.");
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