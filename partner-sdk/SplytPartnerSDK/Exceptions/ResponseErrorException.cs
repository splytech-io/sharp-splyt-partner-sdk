using System;
using SplytPartnerSDK.Model;

namespace SplytPartnerSDK.Exceptions
{

    public class ResponseErrorException : Exception
    {
        public readonly ResponseError ResponseError;

        public ResponseErrorException(ResponseError error) : base() {
            this.ResponseError = error;
        }

        public override string Message {
            get {
                return (ResponseError != null) ? ResponseError.Message : "Unknown response error";
            }
        }
    }
}
