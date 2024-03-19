using EPiServer.ContentGraph.Api.Result;
using EPiServer.ContentGraph.Json;
using EPiServer.ContentGraph.Connection;
using GraphQL.Transport;
using Newtonsoft.Json;
using System.Text;
using EPiServer.ContentGraph.Configuration;
using EPiServer.Turnstile.Contracts.Hmac;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using EPiServer.ServiceLocation;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace EPiServer.ContentGraph.Api.Querying
{
    public class GraphQueryBuilder : IQuery
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private GraphQLRequest _query;
        private readonly OptiGraphOptions _optiGraphOptions;
        private const string RequestMethod = "POST";
        private const string UnCachedPath = "?cache=false";
        private Dictionary<string, FragmentBuilder> _fragmentBuilders;
        private readonly List<string> typeQueries = new List<string>();
        public GraphQueryBuilder()
        {
            _optiGraphOptions = new OptiGraphOptions();
            _query = new GraphQLRequest();
        }
        public static GraphQueryBuilder CreateFromConfig(Action<OptiGraphOptions> transformAction = null)
        {
            try
            {
                var options = ServiceLocator.Current.GetService(typeof(OptiGraphOptions)) as OptiGraphOptions;
                var httpClientFactory = ServiceLocator.Current.GetService(typeof(IHttpClientFactory)) as IHttpClientFactory;
                if (transformAction != null)
                {
                    transformAction(options);
                }
                if (options != null)
                {
                    return new GraphQueryBuilder(options, httpClientFactory);
                }
                throw new ApplicationException("Can not create GraphQueryBuilder instance (OptiGraphOptions instance not found)");
            }
            catch (Exception e)
            {
                throw new ApplicationException("Can not create GraphQueryBuilder instance, please check your settings and configuration", e);
            }
        }
        private GraphQueryBuilder(OptiGraphOptions options, IHttpClientFactory httpClientFactory)
        {
            _optiGraphOptions = options;
            _query = new GraphQLRequest
            {
                OperationName = "SampleQuery"
            };
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("HttpClientWithAutoDecompression");
        }

        public GraphQueryBuilder(IOptions<OptiGraphOptions> optiGraphOptions, IHttpClientFactory httpClientFactory) : 
            this(optiGraphOptions.Value, httpClientFactory)
        {
        }
        public GraphQueryBuilder(GraphQLRequest request, ITypeQueryBuilder typeQueryBuilder)
        {
            if (typeQueryBuilder.Parent != null)
            {
                _optiGraphOptions ??= ((GraphQueryBuilder)typeQueryBuilder.Parent)._optiGraphOptions;
                _httpClientFactory ??= ((GraphQueryBuilder)typeQueryBuilder.Parent)._httpClientFactory;
                _fragmentBuilders ??= ((GraphQueryBuilder)typeQueryBuilder.Parent)._fragmentBuilders;
                _httpClient ??= ((GraphQueryBuilder)typeQueryBuilder.Parent)._httpClient;
            }
            _query = request;
        }
        public TypeQueryBuilder<T> ForType<T>()
        {
            var typeQueryBuilder = new TypeQueryBuilder<T>(_query);
            typeQueryBuilder.Parent = this;
            return typeQueryBuilder;
        }
        public TypeQueryBuilder<T> ForType<T>(TypeQueryBuilder<T> typeQueryBuilder)
        {
            typeQueryBuilder.Parent = this;
            return typeQueryBuilder;
        }
        public void AddFragment(FragmentBuilder fragmentBuilder)
        {
            if (_fragmentBuilders == null)
            {
                _fragmentBuilders = new Dictionary<string, FragmentBuilder>();
            }
            if (!_fragmentBuilders.TryAdd(fragmentBuilder.GetName(), fragmentBuilder))
            {
                throw new ArgumentException($"Fragment [{fragmentBuilder.GetName()}] had added already.");
            }
        }
        public IEnumerable<FragmentBuilder> GetFragments()
        {
            return _fragmentBuilders?.Values;
        }
        /// <summary>
        /// Optional name for operation. Name should not start with number (only underscores and letters) and can't have any space.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
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
        /// Get raw response to a string
        /// </summary>
        /// <returns>string</returns>
        /// <exception cref="ServiceException"></exception>
        public async Task<string> GetRawResultAsync()
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
                        return await reader.ReadToEndAsync();
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
                    throw new ServiceException(e.Message, e);
                }
            }
        }
        /// <summary>
        /// Call this method to generate query for all types
        /// </summary>
        /// <returns></returns>
        public GraphQueryBuilder BuildQueries()
        {
            _query.Query = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            if (typeQueries != null && typeQueries.Count > 0)
            {
                foreach (var typeQuery in typeQueries)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append(" ").Append(typeQuery);
                    }
                    else
                    {
                        stringBuilder.Append(typeQuery);
                    }
                }
                typeQueries.Clear();
            }

            _query.Query = $"query {_query.OperationName} {{{stringBuilder}}}";
            if (_fragmentBuilders != null)
            {
                foreach (var fragment in _fragmentBuilders.Values)
                {
                    _query.Query += $"\n{fragment.GetQuery().Query}";
                }
            }
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

        public void AddQuery(string typeQuery)
        {
            typeQueries.Add(typeQuery);
        }
    }
}
