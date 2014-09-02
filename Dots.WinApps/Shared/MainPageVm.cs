using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using GalaSoft.MvvmLight.Command;
using Microsoft.WindowsAzure.MobileServices;

namespace Dots.WinApps.Shared
{
   public class MainPageVm
   {
      private MobileServiceUser User { get; set; }

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
         string msg;

         try
         {
            User = await App.MobileService.LoginAsync( provider );
            msg = string.Format( "Logged in using {0} as {1}", provider, User.UserId );
         }
         catch ( Exception ex )
         {
            msg = "Error occurred while signing in";
         }

         var dialog = new MessageDialog( msg );
         dialog.Commands.Add( new UICommand( "OK" ) );
         await dialog.ShowAsync();
      }
   }
}
