using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Dots.WinApps.Shared.ServiceDataModels;
using GalaSoft.MvvmLight.Command;
using Microsoft.WindowsAzure.MobileServices;

namespace Dots.WinApps.Shared
{
   public class MainPageVm
   {
      public async void Initialize()
      {
         string msg = null;

         try
         {
            if ( await LoginWithSavedCredentials() )
            {
               LoginUI.Visibility = Visibility.Collapsed;
               LogoutUI.Visibility = Visibility.Visible;

               msg = string.Format( "Logged in using {0} as {1}", SettingsManager.DotsUser.Authenticator, SettingsManager.DotsUser.UserName );
            }
            else
            {
               Logout();
            }
         }
         catch ( Exception ex )
         {
            Logout();
            msg = ex.Message ?? "An error occurred while signing in.";
         }

         if ( msg != null )
         {
            await PopupHelper.ShowMessage( msg );
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
            return new RelayCommand( Logout );
         }
      }

      public StackPanel LoginUI { get; set; }
      public StackPanel LogoutUI { get; set; }

      private async Task<bool> LoginWithSavedCredentials()
      {
         if ( SettingsManager.AzureUser == null || SettingsManager.DotsUser == null )
         {
            SettingsManager.AzureUser = null;
            SettingsManager.DotsUser = null;
            SettingsManager.Save();
            return false;
         }

         ServiceHelper.MobileService.CurrentUser = SettingsManager.AzureUser;

         if ( !await CredentialsHelper.AreCredentialsValid() )
         {
            throw new Exception( "Your login has expired. Please log in again." );
         }

         SettingsManager.DotsUser = await CredentialsHelper.GetDotsUser( SettingsManager.AzureUser.UserId );
         if ( SettingsManager.DotsUser == null )
         {
            throw new Exception();
         }

         return true;
      }

      private async void Login( MobileServiceAuthenticationProvider provider )
      {
         string msg = null;

         try
         {
            SettingsManager.AzureUser = await ServiceHelper.MobileService.LoginAsync( provider );

            SettingsManager.DotsUser = await CredentialsHelper.GetDotsUser( SettingsManager.AzureUser.UserId );
            if ( SettingsManager.DotsUser == null )
            {
               var user = new User()
               {
                  Authenticator = provider.ToString(),
                  UserId = SettingsManager.AzureUser.UserId,
                  UserName = "MyAwesomeName"
               };
               await ServiceHelper.UserTable.InsertAsync( user );

               SettingsManager.DotsUser = user;

               LoginUI.Visibility = Visibility.Collapsed;
               LogoutUI.Visibility = Visibility.Visible;
            }

            msg = string.Format( "Logged in using {0} as {1}", provider, SettingsManager.DotsUser.UserName );

            SettingsManager.Save();
         }
         catch ( Exception )
         {
            Logout();
            msg = "An error occurred while signing in.";
         }

         if ( msg != null )
         {
            await PopupHelper.ShowMessage( msg );
         }
      }

      public void Logout()
      {
         CredentialsHelper.Logout();

         LoginUI.Visibility = Visibility.Visible;
         LogoutUI.Visibility = Visibility.Collapsed;
      }
   }
}
