using System;
using System.Net;
using System.Resources;
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

   public enum ErrorStates
   {
      NoError,
      Error
   }

   public class LoginPageVm : PageVmBase
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

      private ErrorStates _errorState;
      public ErrorStates ErrorState
      {
         get
         {
            return _errorState;
         }
         set
         {
            _errorState = value;
            OnPropertyChanged( "ErrorState" );
         }
      }

      private string _errorText;
      public string ErrorText
      {
         get
         {
            return _errorText;
         }
         set
         {
            _errorText = value;
            OnPropertyChanged( "ErrorText" );
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

      public LoginPageVm( PageBase page )
         : base( page )
      {

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
               ErrorState = ErrorStates.NoError;

               try
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
               }
               catch ( MobileServiceInvalidOperationException ex )
               {
                  if ( ex.Response.StatusCode == WebDAVStatusCode.UnprocessableEntity )
                  {
                     ErrorText = "Your username must be between 5 and 20 characters long.";
                  }
                  else if ( ex.Response.StatusCode == HttpStatusCode.Conflict )
                  {
                     ErrorText = "That username already exists.";
                  }
                  else
                  {
                     ErrorText = "An error occurred while creating your account. Please try again.";
                  }

                  ErrorState = ErrorStates.Error;
               }
            } );
         }
      }

      private async void Login( MobileServiceAuthenticationProvider provider )
      {
         ErrorState = ErrorStates.NoError;

         try
         {
            SettingsManager.AzureUser = await ServiceHelper.MobileService.LoginAsync( provider );
            CurrentProvider = provider;

            SettingsManager.DotsUser = await CredentialsHelper.GetDotsUser( SettingsManager.AzureUser.UserId );
            if ( SettingsManager.DotsUser == null )
            {
               LoginState = LoginPageStates.LoggingIn;
            }
            else
            {
               Page.Frame.Navigate( typeof( GamesPage ) );
            }

            SettingsManager.Save();
         }
         catch ( Exception )
         {
            Logout();
            ErrorText = "An error occurred while signing in. Please try again.";
            ErrorState = ErrorStates.Error;
         }
      }

      public void Logout()
      {
         ErrorState = ErrorStates.NoError;

         CredentialsHelper.Logout();
         CurrentProvider = null;
      }
   }
}
