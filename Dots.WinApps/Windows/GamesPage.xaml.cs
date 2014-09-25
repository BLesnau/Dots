using Dots.WinApps.Shared;

namespace Dots.WinApps.Windows
{
   public sealed partial class GamesPage : PageBase
   {
      public GamesPageVm ViewModel { get; set; }

      public GamesPage()
      {
         InitializeComponent();
   
         ViewModel = new GamesPageVm() { Page = this };
         DataContext = ViewModel;
      }
   }
}
