using EPiServer.ContentGraph.Api.Result;
using EPiServer.ContentGraph.Json;
using EPiServer.ContentGraph.Connection;
using GraphQL.Transport;
using Newtonsoft.Json;
using System.Net;

namespace EPiServer.ContentGraph.Api.Querying
{
    public class GraphQueryBuilder
    {
        private GraphQLRequest _query;
        ITypeQueryBuilder typeQueryBuilder;

        public GraphQueryBuilder()
        {
            _query = new GraphQLRequest();
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
            _query.OperationName = op;
            return this;
        }
        public GraphResult<TResult> GetResult<TResult>()
        {
            string url = "https://rc-3-0-0.cg.optimizely.com/content/v2";

            using (JsonRequest jsonRequest = new JsonRequest(url, HttpVerbs.Post, null))
            {
                try
                {
                    jsonRequest.AddRequestHeader("Authorization", "epi-single TGRpDCCMxB2j0HOiVuR2CnobFBQRHK3sS2fMtcyjOQCRNYay");
                    var settings = new Newtonsoft.Json.JsonSerializerSettings();
                    settings.ContractResolver = new LowercaseContractResolver();
                    string body = JsonConvert.SerializeObject(_query, settings);
                    jsonRequest.WriteBody(body);

                    using (var reader = new StreamReader(jsonRequest.GetResponseStream(), jsonRequest.Encoding))
                    {
                        var jsonReader = new JsonTextReader(reader);
                        return JsonSerializer.CreateDefault().Deserialize<GraphResult<TResult>>(jsonReader);
                    }
                }
                catch (WebException originalException)
                {
                    //var message = originalException.Message;

                    //if (originalException.Response.IsNotNull())
                    //{
                    //    var responseStream = originalException.Response.GetResponseStream();
                    //    StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                    //    var response = streamReader.ReadToEnd();
                    //    if (!string.IsNullOrEmpty(response))
                    //    {
                    //        try
                    //        {
                    //            response = JsonConvert.DeserializeObject<ServiceError>(response).Error;
                    //        }
                    //        catch (Exception)
                    //        {

                    //        }
                    //    }
                    //    message = message + Environment.NewLine + response;
                    //}
                    //throw new ServiceException(message, originalException);
                    throw originalException;
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
