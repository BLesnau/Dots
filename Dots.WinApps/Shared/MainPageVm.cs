using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Dots.WinApps.Shared.ServiceDataModels;
using GalaSoft.MvvmLight.Command;
using Microsoft.WindowsAzure.MobileServices;

namespace Dots.WinApps.Shared
{
   public class MainPageVm
   {
      private MobileServiceUser User { get; set; }
      private User CurrentDotsUser { get; set; }

      private IMobileServiceTable<User> _userTable = App.MobileService.GetTable<User>();

      public async void Initialize()
      {
         string msg = null;

         if ( await LoginWithSavedCredentials() )
         {
            LoginUI.Visibility = Visibility.Collapsed;
            LogoutUI.Visibility = Visibility.Visible;
            msg = string.Format( "You are already logged in as {0} : {1}", CurrentDotsUser.UserId, CurrentDotsUser.UserName );
         }
         else
         {
            LoginUI.Visibility = Visibility.Visible;
            LogoutUI.Visibility = Visibility.Collapsed;
         }

         if ( msg != null )
         {
            var dialog = new MessageDialog( msg );
            dialog.Commands.Add( new UICommand( "OK" ) );
            await dialog.ShowAsync();
         }
      }

      public RelayCommand FacebookClick
      {
         get
         {
            return new RelayCommand( () => Login( MobileServiceAuthenticationProvider.Facebook ) );
         }
      }

      public RelayCommand TwitterClick
      {
         get
         {
            return new RelayCommand( () => Login( MobileServiceAuthenticationProvider.Twitter ) );
         }
      }

      public RelayCommand GoogleClick
      {
         get
         {
            return new RelayCommand( () => Login( MobileServiceAuthenticationProvider.Google ) );
         }
      }

      public RelayCommand MicrosoftClick
      {
         get
         {
            return new RelayCommand( () => Login( MobileServiceAuthenticationProvider.MicrosoftAccount ) );
         }
      }

      public RelayCommand LogoutClick
      {
         get
         {
            return new RelayCommand( () => Logout() );
         }
      }

      public StackPanel LoginUI { get; set; }
      public StackPanel LogoutUI { get; set; }

      private async Task<bool> LoginWithSavedCredentials()
      {
         string msg = null;

         try
         {
            var vault = new PasswordVault();

            PasswordCredential credential = null;

            try
            {
               // Try to get an existing credential from the vault.
               credential = vault.FindAllByResource( "SavedCredentials" ).FirstOrDefault();
            }
            catch ( Exception )
            {
               // When there is no matching resource an error occurs, which we ignore.
            }

            if ( credential != null )
            {
               // Create a user from the stored credentials.
               User = new MobileServiceUser( credential.UserName );
               credential.RetrievePassword();
               User.MobileServiceAuthenticationToken = credential.Password;

               // Set the user from the stored credentials.
               App.MobileService.CurrentUser = User;

               try
               {
                  // Try to return an item now to determine if the cached credential has expired.
                  await App.MobileService.GetTable<User>().Take( 1 ).ToListAsync();

                  //TODO: Everything up to return probably should be moved somewhere else but still need to handle returning here and at end, etc.
                  var existingUser = await _userTable.Where( u => u.UserId == User.UserId ).ToListAsync();

                  if ( existingUser.Count != 1 )
                  {
                     msg = "An error occurred while signing in";
                  }
                  else
                  {
                     CurrentDotsUser = existingUser.First();
                  }

                  return true;
               }
               catch ( MobileServiceInvalidOperationException ex )
               {
                  if ( ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized )
                  {
                     // Remove the credential with the expired token.
                     vault.Remove( credential );
                     msg = "Your login has expired. Please log in again.";
                  }
               }
            }
         }
         catch ( Exception ex )
         {
            msg = "An error occurred while signing in";
         }

         if ( msg != null )
         {
            var dialog = new MessageDialog( msg );
            dialog.Commands.Add( new UICommand( "OK" ) );
            await dialog.ShowAsync();
         }

         return false;
      }

      private async void Login( MobileServiceAuthenticationProvider provider )
      {
         string msg = string.Empty;

         try
         {
            var vault = new PasswordVault();

            PasswordCredential credential = null;

            User = await App.MobileService.LoginAsync( provider );

            credential = new PasswordCredential( "SavedCredentials",
              User.UserId, User.MobileServiceAuthenticationToken );
            vault.Add( credential );

            //TODO: Refactor to GetCurrentUser
            var existingUser = await _userTable.Where( u => u.UserId == User.UserId ).ToListAsync();

            if ( existingUser.Count == 1 )
            {
               CurrentDotsUser = existingUser.First();
               msg = string.Format( "Logged in using {0} as {1}", provider, CurrentDotsUser.UserId );

               LoginUI.Visibility = Visibility.Collapsed;
               LogoutUI.Visibility = Visibility.Visible;
            }
            else if ( existingUser.Count == 0 )
            {
               var user = new User()
               {
                  Authenticator = provider.ToString(),
                  UserId = User.UserId,
                  UserName = "MyAwesomeName"
               };
               await _userTable.InsertAsync( user );

               CurrentDotsUser = user;

               msg = string.Format( "Logged in using {0} as {1}", provider, CurrentDotsUser.UserId );

               LoginUI.Visibility = Visibility.Collapsed;
               LogoutUI.Visibility = Visibility.Visible;
            }
            else
            {
               msg = string.Format( "An error occurred while signing in" );
            }
         }
         catch ( Exception ex )
         {
            msg = "An error occurred while signing in";
         }

         var dialog = new MessageDialog( msg );
         dialog.Commands.Add( new UICommand( "OK" ) );
         await dialog.ShowAsync();
      }

      public void Logout()
      {
         App.MobileService.Logout();

         var vault = new PasswordVault();

         PasswordCredential credential = null;

         try
         {
            // Try to get an existing credential from the vault.
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

         LoginUI.Visibility = Visibility.Visible;
         LogoutUI.Visibility = Visibility.Collapsed;
      }
   }
}
