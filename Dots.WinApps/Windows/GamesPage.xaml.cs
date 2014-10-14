using Dots.WinApps.Shared;

namespace Dots.WinApps.Windows
{
   public sealed partial class GamesPage : PageBase
   {
      public GamesPage()
      {
         InitializeComponent();
         ViewModel = new GamesPageVm( this );
      }
   }
}
