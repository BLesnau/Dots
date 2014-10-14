namespace Dots.WinApps.Shared
{
   public class PageVmBase : VmBase
   {
      public PageVmBase( PageBase page )
      {
         Page = page;
      }

      public PageBase Page { get; set; }
   }
}
