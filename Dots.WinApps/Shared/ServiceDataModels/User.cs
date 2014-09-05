using Newtonsoft.Json;

namespace Dots.WinApps.Shared.ServiceDataModels
{
   public class User
   {
      [JsonProperty( PropertyName = "id" )]
      public string Id { get; set; }

      [JsonProperty(PropertyName = "userId")]
      public string UserId { get; set; }

      [JsonProperty( PropertyName = "userName" )]
      public string UserName { get; set; }

      [JsonProperty( PropertyName = "authenticator" )]
      public string Authenticator { get; set; }
   }
}
