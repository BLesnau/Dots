using Windows.Foundation;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using Windows.UI.Xaml.Navigation;
using Dots.WinApps.Shared;

namespace Dots.WinApps.Windows
{
   /// <summary>
   /// An empty page that can be used on its own or navigated to within a Frame.
   /// </summary>
   public sealed partial class MainPage : Page
   {
      public MainPageVm ViewModel { get; set; }

      public MainPage()
      {
         this.InitializeComponent();

         ViewModel = new MainPageVm() { LoginUI = LoginStack, LogoutUI = LogoutStack };
         DataContext = ViewModel;
      }


      protected override void OnNavigatedTo( NavigationEventArgs e )
      {
         base.OnNavigatedTo( e );
         ViewModel.Initialize();
      }
   }
}
