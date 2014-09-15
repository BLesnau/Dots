using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Dots.WinApps.Shared
{
   public partial class PageBase : Page
   {
      public NavigationHelper NavigationHelper { get; private set; }

      public PageBase()
      {
         NavigationHelper = new NavigationHelper( this );
         NavigationHelper.LoadState += LoadState;
         NavigationHelper.SaveState += SaveState;
      }

      private void LoadState( object sender, LoadStateEventArgs e )
      {
         // Restore the previously saved state associated with this page
         //if ( e.PageState.ContainsKey( "SelectedItem" ) && itemsViewSource.View != null )
         //{
         //   // TODO: Invoke Me.itemsViewSource.View.MoveCurrentTo() with the selected
         //   //       item as specified by the value of pageState("SelectedItem")
         //}
      }

      private void SaveState( object sender, SaveStateEventArgs e )
      {
         // TODO: Derive a serializable navigation parameter and assign it to
         //       pageState("SelectedItem")
      }

      private bool CanGoBack()
      {
         return NavigationHelper.CanGoBack();
      }
      private void GoBack()
      {
         NavigationHelper.GoBack();
      }

      protected override void OnNavigatedTo( NavigationEventArgs e )
      {
         NavigationHelper.OnNavigatedTo( e );
      }

      protected override void OnNavigatedFrom( NavigationEventArgs e )
      {
         NavigationHelper.OnNavigatedFrom( e );
      }
   }
}
