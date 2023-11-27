using EPiServer.ContentGraph.Api.Result;
using EPiServer.ContentGraph.Json;
using EPiServer.ContentGraph.Connection;
using GraphQL.Transport;
using Newtonsoft.Json;
using System.Net;
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
        //TODO: remove static keyword for IHttpClientFactory & HttpClient
        private static IHttpClientFactory _httpClientFactory;
        private static HttpClient _httpClient;
        private GraphQLRequest _query;
        private static OptiGraphOptions _optiGraphOptions = new();
        private const string RequestMethod = "POST";
        private const string UnCachedPath = "?cache=false";
        ITypeQueryBuilder? typeQueryBuilder;
        public static GraphQueryBuilder CreateFromConfig()
        {
            try
            {
                var options = ServiceLocator.Current.GetService(typeof(OptiGraphOptions));
                if (options != null)
                {
                    return new GraphQueryBuilder(options as OptiGraphOptions);
                }
                throw new ApplicationException("Can not create GraphQueryBuilder instance (OptiGraphOptions instance not found)");
            }
            catch (Exception e)
            {
                throw new ApplicationException("Can not create GraphQueryBuilder instance, please check your settings and configuration", e);
            }
        }
        private GraphQueryBuilder(OptiGraphOptions options)
        {
            _optiGraphOptions = options;
            _query = new GraphQLRequest
            {
                OperationName = "SampleQuery"
            };
        }

        public GraphQueryBuilder(IOptions<OptiGraphOptions> optiGraphOptions, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("HttpClientWithAutoDecompression");
            _optiGraphOptions = optiGraphOptions.Value;
            _query = new GraphQLRequest
            {
                OperationName = "SampleQuery"
            };
        }
        public GraphQueryBuilder(GraphQLRequest request)
        {
            _query = request;
        }
        public TypeQueryBuilder<T> ForType<T>()
        {
            typeQueryBuilder = new TypeQueryBuilder<T>(_query);
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
        public async Task<ContentGraphResult<TResult>> GetResultAsync<TResult>()
        {
            string url = GetServiceUrl();

            using (JsonRequest jsonRequest = new JsonRequest(url, HttpMethod.Post, _httpClient))
            {
                try
                {
                    var settings = new JsonSerializerSettings();
                    settings.ContractResolver = new LowercaseContractResolver();
                    string body = JsonConvert.SerializeObject(_query, settings);

                    AdditionalInformation(jsonRequest, body);
                    using (var reader = new StreamReader(await jsonRequest.GetResponseStream(body), jsonRequest.Encoding))
                    {
                        var jsonReader = new JsonTextReader(reader);
                        return JsonSerializer.CreateDefault().Deserialize<ContentGraphResult<TResult>>(jsonReader) ?? new ContentGraphResult<TResult>();
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
                catch (HttpRequestException e)
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<ContentGraphResult<TResult>>(e.Message) ?? new ContentGraphResult<TResult>();
                    }
                    catch (Exception)
                    {
                        throw new ServiceException(e.Message, e);
                    }
                }
            }
        }
        public async Task<ContentGraphResult> GetResultAsync()
        {
            string url = GetServiceUrl();

            using (JsonRequest jsonRequest = new JsonRequest(url, HttpMethod.Post, _httpClient))
            {
                try
                {
                    var settings = new JsonSerializerSettings();
                    settings.ContractResolver = new LowercaseContractResolver();
                    string body = JsonConvert.SerializeObject(_query, settings);

                    AdditionalInformation(jsonRequest, body);
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
                catch (HttpRequestException e)
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<ContentGraphResult>(e.Message);
                    }
                    catch (Exception)
                    {
                        throw new ServiceException(e.Message, e);
                    }
                }
            }
        }
        /// <summary>
        /// Call this method to generate query for all types
        /// </summary>
        /// <returns></returns>
        public GraphQueryBuilder BuildQueries()
        {
            _query.Query = $"query {_query.OperationName} {{{_query.Query}}}";
            return this;
        }
        public GraphQLRequest GetQuery()
        {
            return _query;
        }
        
        private void AdditionalInformation(JsonRequest request, string body)
        {
            request.AddRequestHeader("Authorization", GetAuthorization(body));
            if (_optiGraphOptions.UseHmacKey)
            {
                request.AddRequestHeader("cg-include-deleted", "true");
            }
            if (!_optiGraphOptions.Cache)
            {
                Regex regex = new Regex(@"\?cache=\w*");
                _optiGraphOptions.QueryPath = _optiGraphOptions.QueryPath.Replace(regex.Match(_optiGraphOptions.QueryPath).Value, UnCachedPath);
            }
        }
    }
}
