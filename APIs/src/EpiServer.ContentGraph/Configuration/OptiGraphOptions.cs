using System.ComponentModel.DataAnnotations;

namespace EPiServer.ContentGraph.Configuration
{
    public class OptiGraphOptions
    {
        public OptiGraphOptions(string serviceUrl, string secretKey, string key, string appKey, bool useHmacKey = true)
        {
            UseHmacKey = useHmacKey;
            ServiceUrl = serviceUrl;
            SecretKey = secretKey;
            Key = key;
            AppKey = appKey;
        }
        public OptiGraphOptions(bool useHmacKey = true)
        {
            UseHmacKey = useHmacKey;
            ServiceUrl = string.Empty;
            SecretKey = string.Empty;
            Key = string.Empty;
            AppKey = string.Empty;
        }
        /// <summary>
        /// Flag to mark that request should uese HMAC key
        /// </summary>
        public bool UseHmacKey { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string ServiceUrl { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string SecretKey { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Key { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string AppKey { get; set; }
        public string Authorization
        {
            get
            {
                if (UseHmacKey)
                {
                    return $"epi-hmac {Key}";
                }
                return $"epi-single {Key}";
            }
        }
    }
}
