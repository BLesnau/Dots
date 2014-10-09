using Dots.WinApps.Shared;

namespace Dots.WinApps.Windows
{
   public sealed partial class LoginPage : PageBase
   {
      public LoginPageVm ViewModel { get; set; }

      public LoginPage()
      {
         InitializeComponent();

         ViewModel = new LoginPageVm( this );
         DataContext = ViewModel;

         Loaded += PageLoaded;
      }

      void PageLoaded( object sender, global::Windows.UI.Xaml.RoutedEventArgs e )
      {
         NavigationHelper = new NavigationHelper( this );
         NavigationHelper.LoadState += LoadState;
         NavigationHelper.SaveState += SaveState;
      }
   }
}
