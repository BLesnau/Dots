using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Dots.WinApps.Shared
{
   public partial class PageBase : Page
   {
      public VmBase ViewModel { get; set; }
      public NavigationHelper NavigationHelper { get; set; }

      public PageBase()
      {
         NavigationHelper = new NavigationHelper( this );
         NavigationHelper.LoadState += LoadState;
         NavigationHelper.SaveState += SaveState;

         Loaded += PageLoaded;
      }

      void PageLoaded( object sender, global::Windows.UI.Xaml.RoutedEventArgs e )
      {
         DataContext = ViewModel;
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
         NavigationHelper.OnNavigatedTo( e );
      }

      protected override void OnNavigatedFrom( NavigationEventArgs e )
      {
         NavigationHelper.OnNavigatedFrom( e );
      }
   }
}
