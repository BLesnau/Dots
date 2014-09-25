using Dots.WinApps.Shared;

namespace Dots.WinApps.Windows
{
   public sealed partial class LoginPage : PageBase
   {
      public LoginPageVm ViewModel { get; set; }

      public LoginPage()
      {
         InitializeComponent();

         ViewModel = new LoginPageVm() { Page = this };
         DataContext = ViewModel;
      }
   }
}
