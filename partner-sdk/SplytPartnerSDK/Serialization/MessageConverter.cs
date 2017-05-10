using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SplytPartnerSDK.Model;

namespace SplytPartnerSDK.Serialization
{
    public class MessageConverter : CustomCreationConverter<Message>
    {
        /// <summary>
        /// Create the specified objectType.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="objectType">Object type.</param>
        public override Message Create(Type objectType) {
            return new Message();
        }

        /// <summary>
        /// Reads the json.
        /// </summary>
        /// <returns>The json.</returns>
        /// <param name="reader">Reader.</param>
        /// <param name="objectType">Object type.</param>
        /// <param name="existingValue">Existing value.</param>
        /// <param name="serializer">Serializer.</param>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var root = (JObject)JToken.Load(reader);
            var type = root.Value<string>("type");

            if (Message.TYPE_RESPONSE.Equals(type)) {
                //return serializer.Deserialize<Response>(reader);
                return root.ToObject<Response>();
            } else {
                //return serializer.Deserialize<Message>(reader);
                return root.ToObject<Message>();
            }
        }

    }
}
