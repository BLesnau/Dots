using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Dots.WinApps.Shared
{
   public partial class PageBase : Page
   {
      public NavigationHelper NavigationHelper { get; set; }

      public PageBase()
      {
         //Loaded += PageLoaded;
      }

      void PageLoaded( object sender, global::Windows.UI.Xaml.RoutedEventArgs e )
      {
         NavigationHelper = new NavigationHelper( this );
         NavigationHelper.LoadState += LoadState;
         NavigationHelper.SaveState += SaveState;
      }

      public void LoadState( object sender, LoadStateEventArgs e )
      {
         // Restore the previously saved state associated with this page
         //if ( e.PageState.ContainsKey( "SelectedItem" ) && itemsViewSource.View != null )
         //{
         //   // TODO: Invoke Me.itemsViewSource.View.MoveCurrentTo() with the selected
         //   //       item as specified by the value of pageState("SelectedItem")
         //}
      }

      public void SaveState( object sender, SaveStateEventArgs e )
      {
         // TODO: Derive a serializable navigation parameter and assign it to
         //       pageState("SelectedItem")
      }

      protected override void OnNavigatedTo( NavigationEventArgs e )
      {
        // NavigationHelper.OnNavigatedTo( e );
      }

      protected override void OnNavigatedFrom( NavigationEventArgs e )
      {
        // NavigationHelper.OnNavigatedFrom( e );
      }
   }
}
