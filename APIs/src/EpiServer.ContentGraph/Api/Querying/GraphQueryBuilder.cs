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
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using System.Net.Http;
using EPiServer.ServiceLocation;

namespace EPiServer.ContentGraph.Api.Querying
{
    public class GraphQueryBuilder : IQuery
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private GraphQLRequest _query;
        private static OptiGraphOptions _optiGraphOptions = new();
        private const string RequestMethod = "POST";
        ITypeQueryBuilder? typeQueryBuilder;
        IList<ITypeQueryBuilder> _typeQueryBuilders;

        public GraphQueryBuilder(IOptions<OptiGraphOptions> optiGraphOptions, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("HttpClientWithAutoDecompression");
            _optiGraphOptions = optiGraphOptions.Value;
            _query = new GraphQLRequest
            {
                OperationName = "SampleQuery"
            };
            _typeQueryBuilders = new List<ITypeQueryBuilder>();
        }
        //public GraphQueryBuilder() => _query = new GraphQLRequest();
        public GraphQueryBuilder(GraphQLRequest request)
        {
            _httpClient = ServiceLocator.Current.GetInstance<IHttpClientFactory>().CreateClient("HttpClientWithAutoDecompression");
            _query = request;
            _typeQueryBuilders ??= new List<ITypeQueryBuilder>();
        }
        public TypeQueryBuilder<T> ForType<T>()
        {
            typeQueryBuilder = new TypeQueryBuilder<T>(_query);
            _typeQueryBuilders.Add(typeQueryBuilder);
            return (TypeQueryBuilder<T>)typeQueryBuilder;
        }
        public GraphQueryBuilder OperationName(string op)
        {
            Regex reg = new Regex(@"^[a-zA-Z_]\w*$");
            if (reg.IsMatch(op))
            {
                _query.OperationName = op;
            }
            return this;
        }
        private string GetServiceUrl()
        {
            return _optiGraphOptions.GatewayAddress + _optiGraphOptions.QueryPath;
        }
        private string GetAuthorization(string body)
        {
            if (_optiGraphOptions.UseHmacKey)
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] byteArray = encoding.GetBytes(body.Trim());
                _optiGraphOptions.SingleKey = GetHmacHeader(byteArray);
            }
            return _optiGraphOptions.Authorization;
        }
        private string GetHmacHeader(byte[] requestBody)
        {
            DefaultHmacDeclarationFactory hmacDeclarationFactory =
                new DefaultHmacDeclarationFactory(new Sha256HmacAlgorithm(Convert.FromBase64String(_optiGraphOptions.Secret)));
            HmacMessage hmacMessage = GetHmacMessage(requestBody);
            HmacDeclaration? hmacDeclaration = hmacDeclarationFactory.Create(hmacMessage);
            return $"{hmacDeclaration}";
        }
        private HmacMessage GetHmacMessage(byte[] requestBody)
        {
            DefaultHmacMessageBuilder? messageBuilder = new DefaultHmacMessageBuilder()
                .AddApplicationKey(_optiGraphOptions.AppKey)
                .AddTarget(new Uri(GetServiceUrl()).PathAndQuery)
                .AddMethod(RequestMethod)
                .AddBody(requestBody);
            return messageBuilder.ToMessage();
        }
        public async Task<ContentGraphResult<TResult>> GetResult<TResult>()
        {
            string url = GetServiceUrl();

            using (JsonRequest jsonRequest = new JsonRequest(url, HttpMethod.Post, _httpClient))
            {
                try
                {
                    var settings = new JsonSerializerSettings();
                    settings.ContractResolver = new LowercaseContractResolver();
                    string body = JsonConvert.SerializeObject(_query, settings);

                    jsonRequest.AddRequestHeader("Authorization", GetAuthorization(body));
                    using (var reader = new StreamReader(await jsonRequest.GetResponseStream(body), jsonRequest.Encoding))
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
        public async Task<ContentGraphResult> GetResult()
        {
            string url = GetServiceUrl();

            using (JsonRequest jsonRequest = new JsonRequest(url, HttpMethod.Post, _httpClient))
            {
                try
                {
                    var settings = new JsonSerializerSettings();
                    settings.ContractResolver = new LowercaseContractResolver();
                    string body = JsonConvert.SerializeObject(_query, settings);

                    jsonRequest.AddRequestHeader("Authorization", GetAuthorization(body));
                    if (_optiGraphOptions.UseHmacKey)
                    {
                        jsonRequest.AddRequestHeader("cg-include-deleted", "true");
                    }
                    using (var reader = new StreamReader(await jsonRequest.GetResponseStream(body), jsonRequest.Encoding))
                    {
                        var jsonReader = new JsonTextReader(reader);
                        return JsonSerializer.CreateDefault().Deserialize<ContentGraphResult>(jsonReader);
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
            //if (_typeQueryBuilders != null && _typeQueryBuilders.Count > 0)
            //{
            //    foreach (var item in _typeQueryBuilders)
            //    {
            //        _query.Query += item.ToQuery().GetQuery().Query;
            //    }
            //}

            _query.Query = $"query {_query.OperationName} {{{_query.Query}}}";

            //_typeQueryBuilders?.Clear();
            return this;
        }
        public GraphQLRequest GetQuery()
        {
            return _query;
        }
    }
}
