using System;
using Newtonsoft.Json.Linq;

namespace SplytPartnerSDK.Model
{
    public class OnPushArgs
    {
        public readonly string Method;
        public readonly JObject Data;

        public OnPushArgs(string method, JObject data) {
            Method = method;
            Data = data;
        }

        public override string ToString() {
            return string.Format("[OnPushArgs: Method={0}, Data={1}]", Method, Data);
        }
    }
}
