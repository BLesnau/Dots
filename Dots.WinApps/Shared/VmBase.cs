using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;

namespace Dots.WinApps.Shared
{
    public class VmBase : INotifyPropertyChanged
    {
       public event PropertyChangedEventHandler PropertyChanged;

       protected virtual void OnPropertyChanged( [CallerMemberName] string propertyName = null )
       {
          PropertyChangedEventHandler handler = PropertyChanged;
          if ( handler != null )
          {
             handler( this, new PropertyChangedEventArgs( propertyName ) );
          }
       }
    }
}
