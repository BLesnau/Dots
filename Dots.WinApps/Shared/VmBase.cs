using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Dots.WinApps.Shared
{
    public class VmBase : INotifyPropertyChanged
    {
       public PageBase Page { get; set; }

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
