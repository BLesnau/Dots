using System.Net;

namespace Dots.WinApps.Shared
{
   public static class WebDAVStatusCode
   {
      public static HttpStatusCode UnprocessableEntity { get { return (HttpStatusCode) 422; } }
   }
}
