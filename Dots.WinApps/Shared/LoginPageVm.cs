using System;
using Dots.WinApps.Shared.ServiceDataModels;
using Dots.WinApps.Windows;
using Microsoft.WindowsAzure.MobileServices;

namespace Dots.WinApps.Shared
{
   public enum LoginPageStates
   {
      LoggedOut,
      LoggingIn
   }
   
   public class LoginPageVm : VmBase
   {
      public MobileServiceAuthenticationProvider? CurrentProvider { get; set; }

      private LoginPageStates _loginState;
      public LoginPageStates LoginState
      {
         get
         {
            return _loginState;
         }
         set
         {
            _loginState = value;
            OnPropertyChanged( "LoginState" );
         }
      }

      private bool _creatingAccount;
      public bool CreatingAccount
      {
         get
         {
            return _creatingAccount;
         }
         set
         {
            _creatingAccount = value;
            OnPropertyChanged( "CreatingAccount" );
         }
      }

      private string _enteredUsername;
      public string EnteredUsername
      {
         get
         {
            return _enteredUsername;
         }
         set
         {
            _enteredUsername = value;
            OnPropertyChanged( "EnteredUsername" );
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

      public RelayCommand CreateAccountClick
      {
         get
         {
            return new RelayCommand( async () =>
            {
               var user = new User()
               {
                  Authenticator = CurrentProvider.ToString(),
                  UserId = SettingsManager.AzureUser.UserId,
                  UserName = EnteredUsername
               };
               await ServiceHelper.UserTable.InsertAsync( user );

               SettingsManager.DotsUser = user;

               SettingsManager.Save();

               Page.Frame.Navigate( typeof( GamesPage ) );
            } );
         }
      }

      private async void Login( MobileServiceAuthenticationProvider provider )
      {
         string msg = null;

         try
         {
            SettingsManager.AzureUser = await ServiceHelper.MobileService.LoginAsync( provider );
            CurrentProvider = provider;

            SettingsManager.DotsUser = await CredentialsHelper.GetDotsUser( SettingsManager.AzureUser.UserId );
            if ( SettingsManager.DotsUser == null )
            {
               CreatingAccount = true;
               LoginState = LoginPageStates.LoggingIn;
            }
            else
            {
               Page.Frame.Navigate( typeof( GamesPage ) );
            }
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
         CurrentProvider = null;
      }
   }
}
