using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.Configuration;
using EPiServer.ContentGraph.IntegrationTests.TestModels;
using EPiServer.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Optimizely.ContentGraph.Cms.Configuration;

namespace EPiServer.ContentGraph.IntegrationTests.TestSupport
{
    [TestClass]
    public class IntegrationFixture
    {
        private static readonly int MAX_RETRY = 10;
        protected static IOptions<QueryOptions>? queryOptions;
        protected static IHost? testingHost;
        protected static readonly string QUERY_PATH = "/content/v2?cache=false";
        protected static readonly string INDEXING_PATH = "/api/content/v2/data";
        protected static readonly string MAPPING_PATH = "/api/content/v3/types";
        protected static string? WorkingDirectory;
        private static HttpClient? _httpClient;
        private static string USER_AGENT => $"Optimizely-Graph-NET-API/{typeof(IntegrationFixture).Assembly.GetName().Version}";
        protected static OptiGraphOptions _options;

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
            _options = new OptiGraphOptions { ServiceUrl = queryOptions.Value.GatewayAddress + QUERY_PATH, Authorization = $"epi-single {queryOptions.Value.SingleKey}" };
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
                    Task.Delay(500);
                    retry++;
                }
                Console.WriteLine($"Deleted contents for index {id}");
            }
            else
            {
                throw new Exception("Can not delete contents");
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
                IQuery query = new GraphQueryBuilder(_options)
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
    }
}
