using System.Threading.Tasks;
using Dots.WinApps.Shared.ServiceDataModels;
using Microsoft.WindowsAzure.MobileServices;

namespace Dots.WinApps.Shared
{
   public static class CredentialsHelper
   {
      public static async Task<bool> AreCredentialsValid()
      {
         try
         {
            // Try to return an item now to determine if the cached credential has expired.
            await ServiceHelper.UserTable.Take( 1 ).ToListAsync();
         }
         catch ( MobileServiceInvalidOperationException ex )
         {
            if ( ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized )
            {
               return false;
            }
         }

         return true;
      }

      public static void Logout()
      {
         ServiceHelper.MobileService.Logout();
         SettingsManager.Logout();
      }

      public static async Task<User> GetDotsUser( string userId )
      {
         var existingUser = await ServiceHelper.UserTable.Where( u => u.UserId == SettingsManager.AzureUser.UserId ).ToListAsync();
         if ( existingUser.Count == 1 )
         {
            return existingUser.First();
         }
         else
         {
            return null;
         }
      }
   }
}

