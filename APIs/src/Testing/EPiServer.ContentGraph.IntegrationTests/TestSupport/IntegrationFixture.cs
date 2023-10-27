using EPiServer.Cms.Shell;
using EPiServer.Cms.Shell.UI;
using EPiServer.Cms.TinyMce;
using EPiServer.Cms.UI.Admin;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Cms.UI.VisitorGroups;
using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Configuration;
using EPiServer.Data;
using EPiServer.DependencyInjection;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc.Html;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Optimizely.ContentGraph.Cms.Configuration;
using Optimizely.ContentGraph.Cms.Services.Internal;

namespace EPiServer.ContentGraph.IntegrationTests.TestSupport
{
    [TestClass]
    public class IntegrationFixture
    {
        private static readonly int MAX_RETRY = 100;
        protected static IOptions<QueryOptions> queryOptions;
        protected static IHost? testingHost;
        protected static readonly string QUERY_PATH = "content/v2?cache=false";
        protected static readonly string INDEXING_PATH = "api/content/v2/data";
        protected static readonly string CLEAR_MAPPING_AND_DATA_PATH = "api/content/v3/sources";
        protected static readonly string MAPPING_PATH = "api/content/v3/types";
        protected static string? WorkingDirectory;
        private static HttpClient? _httpClient;
        private static string USER_AGENT => $"Optimizely-Graph-NET-API/{typeof(IntegrationFixture).Assembly.GetName().Version}";
        protected static OptiGraphOptions _configOptions;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            if (testingHost != null)
            {
                return;
            }

            WorkingDirectory = $@"{Environment.CurrentDirectory}\..\..\..\TestSupport";
            testingHost = Host
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config
                        .AddJsonFile(WorkingDirectory + "/../appsettings.json")
                        .AddEnvironmentVariables();
                })
                .ConfigureServices(services => ConfigureServices(services))
                .Build();
            queryOptions = testingHost.Services.GetService<IOptions<QueryOptions>>();
            _httpClient = CreateHttpClient();
            _configOptions = TransformConfigOptions(queryOptions, true);
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            try
            {
                if (queryOptions != null && queryOptions.Value != null)
                {
                    queryOptions = null;
                }
                if (_httpClient != null)
                {
                    _httpClient.Dispose();
                }
                if (testingHost != null)
                {
                    testingHost.Dispose();
                }
            }
            catch (Exception)
            {

            }
        }
        private static void ConfigureServices(IServiceCollection services)
        {
            var dbPath = Path.Combine(WorkingDirectory, "TempSqlDb\\Alloy.mdf");
            dbPath = new Uri(dbPath).LocalPath;

            var connectionstring = $"Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename={dbPath};Initial Catalog=alloy_mvc_netcore;Integrated Security=True;Connect Timeout=30;MultipleActiveResultSets=True";

            services.Configure<DataAccessOptions>(o =>
            {
                o.SetConnectionString(connectionstring);
            });

            services.AddMvc();
            services.AddOptions();
            services.ConfigureContentApiOptions(o =>
            {
                o.IncludeInternalContentRoots = true;
                o.IncludeSiteHosts = true;
                //o.EnablePreviewFeatures = true;// optional
            });
            services.AddContentDeliveryApi(); // required, for further configurations, see https://docs.developers.optimizely.com/content-cloud/v1.5.0-content-delivery-api/docs/configuration
            services.AddContentGraph();
            services.AddScoped<IFilterForVisitor, CustomForVisitor>();
            services.AddScoped<IFilterForVisitor, FilterDeletedForVisitor>();
        }
        private static HttpClient CreateHttpClient()
        {
            var authenticationString = $"{queryOptions.Value.AppKey}:{queryOptions.Value.Secret}";
            var base64AuthString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authenticationString));
            return new HttpClient()
            {
                BaseAddress = new Uri(queryOptions.Value.GatewayAddress),
                DefaultRequestHeaders = {
                    { "User-Agent", USER_AGENT },
                    { "Authorization", $"Basic {base64AuthString}"},
                    { "ContentType", "application/json" }
                }
            };
        }
        protected static void ClearData<T>(string id = "test")
        {
            var res = _httpClient.DeleteAsync(INDEXING_PATH + $"?id={id}").Result;
            if (res.StatusCode == System.Net.HttpStatusCode.OK || res.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                //wait until docs had been deleted
                int retry = 0;
                while (CountDoc<T>() && retry < MAX_RETRY)
                {
                    Console.WriteLine("Deleting data...");
                    Task.Delay(500);
                    retry++;
                }
                Console.WriteLine($"Deleted contents for source {id}");
                ClearMapping(id);
            }
            else
            {
                throw new Exception("Can not delete contents");
            }
        }        
        protected static void ClearMapping(string id = "test")
        {
            var res = _httpClient.DeleteAsync(CLEAR_MAPPING_AND_DATA_PATH + $"?id={id}").Result;
            if (res.StatusCode == System.Net.HttpStatusCode.OK || res.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Deleted mappings for source {id}");
            }
            else
            {
                throw new Exception("Can not delete mappings");
            }
        }
        protected static void PushMapping(string json, string id = "test")
        {
            var res = _httpClient.PutAsync(MAPPING_PATH + $"?id={id}", new StringContent(json)).Result;
            if (res.StatusCode == System.Net.HttpStatusCode.OK || res.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Console.WriteLine("Mapping done");
            }
            else
            {
                throw new Exception("Can not create mapping");
            }
        }
        protected static void BulkIndexing<T>(string bulk, string id = "test")
        {
            var res = _httpClient.PostAsync(INDEXING_PATH + $"?id={id}", new StringContent(bulk)).Result;
            if (res.StatusCode == System.Net.HttpStatusCode.OK || res.StatusCode == System.Net.HttpStatusCode.Created)
            {
                //wait until docs had been indexed
                int retry = 0;
                while (!CountDoc<T>() && retry < MAX_RETRY)
                {
                    Console.WriteLine("Indexing data...");
                    Task.Delay(500);
                    retry++;
                }
                Console.WriteLine("Data has been created");
            }
            else
            {
                throw new Exception("Can not index data");
            }
        }
        private static bool CountDoc<T>()
        {
            try
            {
                IQuery query = new GraphQueryBuilder(_configOptions)
               .ForType<T>()
               .Total()
               .ToQuery()
               .BuildQueries();
                var rs = query.GetResult<T>();
                return rs.Content.Values.First().Total > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        protected static void SetupData<T>(string indexingData)
        {
            string path = $@"{WorkingDirectory}\TestingData\SimpleTypeMapping.json";
            using (StreamReader mappingReader = new StreamReader(path))
            {
                string mapping = mappingReader.ReadToEnd();
                ClearData<T>();
                PushMapping(mapping);
                BulkIndexing<T>(indexingData);
            }
        }
        private static OptiGraphOptions TransformConfigOptions(IOptions<QueryOptions> source, bool useHmacKey)
        {
            if (source.Value != null)
            {
                return new OptiGraphOptions(useHmacKey)
                {
                    ServiceUrl = source.Value.GatewayAddress + QUERY_PATH,
                    Key = source.Value.SingleKey,
                    SecretKey = source.Value.Secret,
                    AppKey = source.Value.AppKey
                };
            }
            return new OptiGraphOptions(useHmacKey);
        }
    }
}
