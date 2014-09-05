using Microsoft.WindowsAzure.Mobile.Service;

namespace Dots.Service.DataObjects
{
   public class User : EntityData
   {
      public string UserId { get; set; }
      public string UserName { get; set; }
      public string Authenticator { get; set; }
   }
}