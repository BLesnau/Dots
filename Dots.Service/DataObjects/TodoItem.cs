using Microsoft.WindowsAzure.Mobile.Service;

namespace Dots.Service.DataObjects
{
   public class TodoItem : EntityData
   {
      public string Text { get; set; }

      public bool Complete { get; set; }
   }
}