using System.Net;

public static class WebDAVStatusCode
{
   public static HttpStatusCode UnprocessableEntity { get { return (HttpStatusCode) 422; } }
}