using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Dots.WinApps.Shared
{
   public static class PopupHelper
   {
      public static async Task ShowMessage( string msg )
      {
         var dialog = new MessageDialog( msg );
         dialog.Commands.Add( new UICommand( "OK" ) );
         await dialog.ShowAsync();
      }
   }
}
