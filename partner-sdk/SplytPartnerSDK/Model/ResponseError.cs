using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SplytPartnerSDK.Model
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ResponseError
    {
        public string Type { get; set; }

        [JsonProperty(PropertyName = "errno")]
        public int ErrorCode { get; set; }

        [JsonProperty(PropertyName = "errgr")]
        public int ErrorGroup { get; set;  }

        public string Message { get; set; }
        public string Description { get; set; }
    }
}
