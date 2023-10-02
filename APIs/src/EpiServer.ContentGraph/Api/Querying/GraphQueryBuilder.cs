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
        private GraphQLRequest _query;
        private OptiGraphOptions optiGraphOptions;
        ITypeQueryBuilder typeQueryBuilder;
        public GraphQueryBuilder(OptiGraphOptions optiGraphOptions)
        {
            this.optiGraphOptions = optiGraphOptions;
        }
        public GraphQueryBuilder() => _query = new GraphQLRequest();
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
            return optiGraphOptions.ServiceUrl;
            //return "https://rc-3-0-0.cg.optimizely.com/content/v2";
        }
        private string GetAuthorization()
        {
            return optiGraphOptions.Authorization;
            //return "epi-single TGRpDCCMxB2j0HOiVuR2CnobFBQRHK3sS2fMtcyjOQCRNYay";
        }
        public ContentGraphResult<TResult> GetResult<TResult>()
        {
            string url = GetServiceUrl();

            using (JsonRequest jsonRequest = new JsonRequest(url, HttpVerbs.Post, null))
            {
                try
                {
                    jsonRequest.AddRequestHeader("Authorization", GetAuthorization());
                    var settings = new JsonSerializerSettings();
                    settings.ContractResolver = new LowercaseContractResolver();
                    string body = JsonConvert.SerializeObject(_query, settings);
                    jsonRequest.WriteBody(body);

                    using (var reader = new StreamReader(jsonRequest.GetResponseStream(), jsonRequest.Encoding))
                    {
                        var jsonReader = new JsonTextReader(reader);
                        return JsonSerializer.CreateDefault().Deserialize<ContentGraphResult<TResult>>(jsonReader);
                    }
                }
                catch (WebException originalException)
                {
                    var message = originalException.Message;

                    if (originalException.Response.IsNotNull())
                    {
                        var responseStream = originalException.Response.GetResponseStream();
                        StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                        var response = streamReader.ReadToEnd();
                        if (!string.IsNullOrEmpty(response))
                        {
                            try
                            {
                                response = JsonConvert.DeserializeObject<ServiceError>(response).Errors;
                            }
                            catch (Exception)
                            {

                            }
                        }
                        message = message + Environment.NewLine + response;
                    }
                    throw new ServiceException(message, originalException);
                }
            }
        }
        /// <summary>
        /// Call this method to generate a full query
        /// </summary>
        /// <returns></returns>
        public GraphQueryBuilder BuildQueries()
        {
            if (typeQueryBuilder != null)
            {
                typeQueryBuilder.Build();
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
