using System.Net.Http;
using System.Net.Http.Headers;

namespace ApiGateway.Extensions
{
    public static class StringContentExtension
    {
        public static StringContent AddMediaTypeJsonHeader(this StringContent stringContent)
        {
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return stringContent;
        }
    }
}
