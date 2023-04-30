using System.Net;

namespace SRI02_Api.Models
{
    public class ResponseEntity<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public T? result { get; set; }
    }
}
