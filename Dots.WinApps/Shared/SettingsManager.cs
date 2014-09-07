using System;
using System.Linq;
using Windows.Security.Credentials;
using Windows.Storage;
using Dots.WinApps.Shared.ServiceDataModels;
using Microsoft.WindowsAzure.MobileServices;

namespace Dots.WinApps.Shared
{
   public static class SettingsManager
   {
      public static MobileServiceUser AzureUser { get; set; }
      public static User DotsUser { get; set; }

      public static void Load()
      {
         AzureUser = LoadAzureUser();
         DotsUser = LoadDotsUser();
      }

      public static void Save()
      {
         SaveAzureUser( AzureUser );
         SaveDotsUser( DotsUser );
      }

      private static void SaveAzureUser( MobileServiceUser azureUser )
      {
         if ( azureUser != null )
         {
            var vault = new PasswordVault();
            var credential = new PasswordCredential( "SavedCredentials", azureUser.UserId, azureUser.MobileServiceAuthenticationToken );
            vault.Add( credential );
         }
      }

      private static void SaveDotsUser( User dotsUser )
      {
         if ( dotsUser != null )
         {
            var composite = new ApplicationDataCompositeValue();
            composite.Add( "Id", dotsUser.Id );
            composite.Add( "UserId", dotsUser.UserId );
            composite.Add( "UserName", dotsUser.UserName );
            composite.Add( "Authenticator", dotsUser.Authenticator );

            ApplicationData.Current.LocalSettings.Values["User"] = composite;
         }
      }

      private static MobileServiceUser LoadAzureUser()
      {
         var vault = new PasswordVault();
         PasswordCredential credential = null;

         try
         {
            credential = vault.FindAllByResource( "SavedCredentials" ).FirstOrDefault();
         }
         catch ( Exception )
         {
            // When there is no matching resource an error occurs, which we ignore.
         }

         if ( credential != null )
         {
            var user = new MobileServiceUser( credential.UserName );
            credential.RetrievePassword();
            user.MobileServiceAuthenticationToken = credential.Password;
            return user;
         }

         return null;
      }

      private static User LoadDotsUser()
      {
         if ( ApplicationData.Current.LocalSettings.Values.ContainsKey( "User" ) )
         {
            var composite = ApplicationData.Current.LocalSettings.Values["User"] as ApplicationDataCompositeValue;
            if ( composite != null
               && composite.ContainsKey( "Id" )
               && composite.ContainsKey( "UserId" )
               && composite.ContainsKey( "UserName" )
               && composite.ContainsKey( "Authenticator" ) )
            {
               return new User()
               {
                  Id = composite["Id"] as string,
                  UserId = composite["UserId"] as string,
                  UserName = composite["UserName"] as string,
                  Authenticator = composite["Authenticator"] as string,
               };
            }
         }

         return null;
      }

      public static void Logout()
      {
         var vault = new PasswordVault();

         PasswordCredential credential = null;

         try
         {
            credential = vault.FindAllByResource( "SavedCredentials" ).FirstOrDefault();
         }
         catch ( Exception )
         {
            // When there is no matching resource an error occurs, which we ignore.
         }

         if ( credential != null )
         {
            vault.Remove( credential );
         }

         AzureUser = null;
         DotsUser = null;
      }
   }
}
