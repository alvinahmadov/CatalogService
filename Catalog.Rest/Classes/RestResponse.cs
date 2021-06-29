using System;
using System.Net;

namespace Catalog.RestClient
{
    /// <summary>
    /// Normal REST response.
    /// </summary>
    public class RestResponse : IRestResponse
    {
        public string Content { get; set; }

        public string ContentType { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string ErrorMessage { get; set; }

        public Exception ErrorException { get; set; }
    }
}