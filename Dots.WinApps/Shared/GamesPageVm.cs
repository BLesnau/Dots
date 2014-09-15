using Dots.WinApps.Windows;

namespace Dots.WinApps.Shared
{
    public class GamesPageVm : VmBase
    {
       public RelayCommand LogoutClick
       {
          get
          {
             return new RelayCommand( Logout );
          }
       }

       public void Logout()
       {
          CredentialsHelper.Logout();

          Page.Frame.Navigate( typeof( LoginPage ) );
       }
    }
}
