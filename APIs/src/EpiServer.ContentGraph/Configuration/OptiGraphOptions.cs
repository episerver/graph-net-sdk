using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using EPiServer.ServiceLocation;

namespace EPiServer.ContentGraph.Configuration
{
    [Options]
    public class OptiGraphOptions
    {
        public const string ConfigSection = "Optimizely:ContentGraph";

        public OptiGraphOptions(string serviceUrl, string secretKey, string key, string appKey, bool useHmacKey = true)
        {
            UseHmacKey = useHmacKey;
            GatewayAddress = serviceUrl;
            Secret = secretKey;
            SingleKey = key;
            AppKey = appKey;
            QueryPath = string.Empty;
        }
        public OptiGraphOptions()
        {
            UseHmacKey = true;
            GatewayAddress = string.Empty;
            Secret = string.Empty;
            SingleKey = string.Empty;
            AppKey = string.Empty;
            QueryPath = string.Empty;
        }
        /// <summary>
        /// Flag to mark that request should uese HMAC key. Default to true.
        /// </summary>
        public bool UseHmacKey { get; set; }
        /// <summary>
        /// Override the value of enabled cache on CG. Default to true.
        /// </summary>
        public bool Cache { get; set; } = true;
        [Required(AllowEmptyStrings = false)]
        public string GatewayAddress { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Secret { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string SingleKey { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string AppKey { get; set; }

        public string QueryPath { get; set; }
        public string Authorization
        {
            get
            {
                if (UseHmacKey)
                {
                    return $"epi-hmac {SingleKey}";
                }
                return $"epi-single {SingleKey}";
            }
        }
    }
}
