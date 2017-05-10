using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace SplytPartnerSDK.Model
{   
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Message
    {
        public string Id { get; set; }
        public string Type { get; set; }
		public string Method { get; set; }
		public Object Data { get; set; }

        public static readonly string TYPE_REQUEST = "request";
        public static readonly string TYPE_RESPONSE = "response";
        public static readonly string TYPE_PUSH = "push";

        /// <summary>
        /// Is this a response type?
        /// </summary>
        /// <returns><c>true</c>, if response was ised, <c>false</c> otherwise.</returns>
        public bool IsResponse() {
            return TYPE_RESPONSE.Equals(this.Type);
        }

        /// <summary>
        /// Is this a push message?
        /// </summary>
        /// <returns><c>true</c>, if push was ised, <c>false</c> otherwise.</returns>
        public bool IsPush() {
            return TYPE_PUSH.Equals(this.Type);
        }

        /// <summary>
        /// Hases the data.
        /// </summary>
        /// <returns><c>true</c>, if data was hased, <c>false</c> otherwise.</returns>
        public bool HasData() {
            return !(Data == null);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SplytPartnerSDK.Model.Message"/> class.
		/// </summary>
		/// <param name="data">Data.</param>
		public static Message Request(String method, Object data) {
            return new Message()
            {
                Id = Guid.NewGuid().ToString(),
                Type = TYPE_REQUEST,
                Method = method,
                Data = data,
            };
		}
    }

}
