using System;
using Windows.UI.Xaml.Controls;
using Dots.WinApps.Shared.ServiceDataModels;
using Dots.WinApps.Windows;
using Microsoft.WindowsAzure.MobileServices;

namespace Dots.WinApps.Shared
{
   public class LoginPageVm : VmBase
   {
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
            }

            SettingsManager.Save();

            Page.Frame.Navigate( typeof( GamesPage ) );
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
      }
   }
}
