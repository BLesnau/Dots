using Dots.WinApps.Shared.ServiceDataModels;
using Microsoft.WindowsAzure.MobileServices;

namespace Dots.WinApps.Shared
{
   public static class ServiceHelper
   {
      public static MobileServiceClient MobileService { get; private set; }
      //public static MobileServiceClient MobileService = new MobileServiceClient( "http://169.254.80.80:2214" );
      public static IMobileServiceTable<User> UserTable { get; private set; }

      static ServiceHelper()
      {
         MobileService = new MobileServiceClient( "https://dots.azure-mobile.net/", "dEmInfiokMvJZTBagaDqISqWgiydMw55" );
         UserTable = MobileService.GetTable<User>();
      }
   }
}
