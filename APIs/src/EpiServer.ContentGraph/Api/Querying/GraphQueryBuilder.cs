using EPiServer.ContentGraph.Api.Result;
using EPiServer.ContentGraph.Json;
using EPiServer.ContentGraph.Connection;
using GraphQL.Transport;
using Newtonsoft.Json;
using System.Net;
using EPiServer.ContentGraph.Helpers;
using System.Text;
using EPiServer.ContentGraph.Configuration;

namespace EPiServer.ContentGraph.Api.Querying
{
    public class GraphQueryBuilder : IQuery
    {
        internal static HttpClient httpClient = new HttpClient(new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
        });

        private GraphQLRequest _query;
        private static OptiGraphOptions _optiGraphOptions;
        ITypeQueryBuilder typeQueryBuilder;
        public GraphQueryBuilder(OptiGraphOptions optiGraphOptions)
        {
            _optiGraphOptions = optiGraphOptions;
            _query = new GraphQLRequest();
            _query.OperationName = "SampleQuery";
        }
        //public GraphQueryBuilder() => _query = new GraphQLRequest();
        public GraphQueryBuilder(GraphQLRequest request) => _query = request;
        public TypeQueryBuilder<T> ForType<T>()
        {
            typeQueryBuilder = new TypeQueryBuilder<T>(_query);
            return (TypeQueryBuilder<T>)typeQueryBuilder;
        }

        public GraphQueryBuilder OperationName(string op)
        {
            _query.OperationName = op;
            return this;
        }
        private string GetServiceUrl()
        {
            return _optiGraphOptions.ServiceUrl;
        }
        private string GetAuthorization()
        {
            return _optiGraphOptions.Authorization;
        }
        public ContentGraphResult<TResult> GetResult<TResult>()
        {
            string url = GetServiceUrl();

            using (JsonRequest jsonRequest = new JsonRequest(url, HttpMethod.Post, httpClient))
            {
                try
                {
                    jsonRequest.AddRequestHeader("Authorization", GetAuthorization());
                    var settings = new JsonSerializerSettings();
                    settings.ContractResolver = new LowercaseContractResolver();
                    string body = JsonConvert.SerializeObject(_query, settings);

                    using (var reader = new StreamReader(jsonRequest.GetResponseStream(body).Result, jsonRequest.Encoding))
                    {
                        var jsonReader = new JsonTextReader(reader);
                        return JsonSerializer.CreateDefault().Deserialize<ContentGraphResult<TResult>>(jsonReader);
                    }
                }
                catch (AggregateException asyncException)
                {
                    var httpRequestException = asyncException.InnerExceptions.FirstOrDefault(e => e.GetType() == typeof(HttpRequestException)) as HttpRequestException;
                    if (httpRequestException != null)
                    {
                        throw new ServiceException(httpRequestException.Message, httpRequestException.InnerException);
                    }
                    throw new ServiceException(asyncException.Message, asyncException);
                }
                
            }
        }
        /// <summary>
        /// Call this method to generate query for all types
        /// </summary>
        /// <returns></returns>
        public GraphQueryBuilder BuildQueries()
        {
            if (typeQueryBuilder != null)
            {
                typeQueryBuilder.ToQuery();
            }

            _query.Query = $"query {_query.OperationName} {{{_query.Query}}}";
            return this;
        }
        public GraphQLRequest GetQuery()
        {
            return _query;
        }
    }
}
