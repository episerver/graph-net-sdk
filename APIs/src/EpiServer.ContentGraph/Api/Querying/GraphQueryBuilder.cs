using EPiServer.ContentGraph.Api.Result;
using EPiServer.ContentGraph.Json;
using EPiServer.ContentGraph.Connection;
using GraphQL.Transport;
using Newtonsoft.Json;
using System.Net;
using EPiServer.ContentGraph.Helpers;
using System.Text;
using EPiServer.ContentGraph.Configuration;
using EPiServer.Turnstile.Contracts.Hmac;

namespace EPiServer.ContentGraph.Api.Querying
{
    public class GraphQueryBuilder : IQuery
    {
        internal static HttpClient httpClient = new HttpClient(new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
        });

        private GraphQLRequest _query;
        private static OptiGraphOptions _optiGraphOptions = new();
        private const string RequestMethod = "POST";
        ITypeQueryBuilder? typeQueryBuilder;
        public GraphQueryBuilder(OptiGraphOptions optiGraphOptions)
        {
            _optiGraphOptions = optiGraphOptions;
            _query = new GraphQLRequest
            {
                OperationName = "SampleQuery"
            };
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
        private string GetAuthorization(string body)
        {
            if (_optiGraphOptions.UseHmacKey)
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] byteArray = encoding.GetBytes(body.Trim());
                _optiGraphOptions.Key = GetHmacHeader(byteArray);
            }
            return _optiGraphOptions.Authorization;
        }
        private string GetHmacHeader(byte[] requestBody)
        {
            DefaultHmacDeclarationFactory hmacDeclarationFactory =
                new DefaultHmacDeclarationFactory(new Sha256HmacAlgorithm(Convert.FromBase64String(_optiGraphOptions.SecretKey)));
            HmacMessage hmacMessage = GetHmacMessage(requestBody);
            HmacDeclaration? hmacDeclaration = hmacDeclarationFactory.Create(hmacMessage);
            return $"{hmacDeclaration}";
        }
        private HmacMessage GetHmacMessage(byte[] requestBody)
        {
            DefaultHmacMessageBuilder? messageBuilder = new DefaultHmacMessageBuilder()
                .AddApplicationKey(_optiGraphOptions.AppKey)
                .AddTarget(new Uri(_optiGraphOptions.ServiceUrl).PathAndQuery)
                .AddMethod(RequestMethod)
                .AddBody(requestBody);
            return messageBuilder.ToMessage();
        }
        public ContentGraphResult<TResult> GetResult<TResult>()
        {
            string url = GetServiceUrl();

            using (JsonRequest jsonRequest = new JsonRequest(url, HttpMethod.Post, httpClient))
            {
                try
                {
                    var settings = new JsonSerializerSettings();
                    settings.ContractResolver = new LowercaseContractResolver();
                    string body = JsonConvert.SerializeObject(_query, settings);

                    jsonRequest.AddRequestHeader("Authorization", GetAuthorization(body));
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
