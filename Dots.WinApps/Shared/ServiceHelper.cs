using Dots.WinApps.Shared.ServiceDataModels;
using Microsoft.WindowsAzure.MobileServices;

namespace Dots.WinApps.Shared
{
   public static class ServiceHelper
   {
      public static MobileServiceClient MobileService { get; private set; }
      public static IMobileServiceTable<User> UserTable { get; private set; }

      static ServiceHelper()
      {
         MobileService = new MobileServiceClient( "https://dots.azure-mobile.net/", "dEmInfiokMvJZTBagaDqISqWgiydMw55" );
         //MobileService = new MobileServiceClient( "http://localhost:2214/");
         //MobileService = new MobileServiceClient( "https://localhost:44300/" );
         UserTable = MobileService.GetTable<User>();
      }
   }
}