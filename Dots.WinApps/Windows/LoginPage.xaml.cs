using Dots.WinApps.Shared;

namespace Dots.WinApps.Windows
{
   public sealed partial class LoginPage : PageBase
   {
      public LoginPage()
      {
         InitializeComponent();
         ViewModel = new LoginPageVm( this );
      }
   }
}
