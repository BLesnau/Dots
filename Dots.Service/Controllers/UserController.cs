using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using System.Web.Http.Results;
using Microsoft.WindowsAzure.Mobile.Service;
using Dots.Service.DataObjects;
using Dots.Service.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace Dots.Service.Controllers
{
   [AuthorizeLevel( AuthorizationLevel.User )]
   public class UserController : TableController<User>
   {
      protected override void Initialize( HttpControllerContext controllerContext )
      {
         base.Initialize( controllerContext );
         var context = new DotsServiceContext();
         DomainManager = new EntityDomainManager<User>( context, Request, Services );
      }

      // GET tables/TodoItem
      public IQueryable<User> GetAllUsers()
      {
         return Query();
      }

      // GET tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
      public SingleResult<User> GetUser( string id )
      {
         return Lookup( id );
      }

      // PATCH tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
      public Task<User> PatchUser( string id, Delta<User> patch )
      {
         return UpdateAsync( id, patch );
      }

      // POST tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
      public async Task<IHttpActionResult> PostUser( User item )
      {
         if ( item.UserName.Count() > 20 || item.UserName.Count() < 5 )
         {
            return new StatusCodeResult( WebDAVStatusCode.UnprocessableEntity, this );
         }

         if ( Query().Any( u => u.UserName == item.UserName ) )
         {
            return new ConflictResult( this );
         }

         User current = await InsertAsync( item );
         return CreatedAtRoute( "Tables", new { id = current.Id }, current );
      }

      // DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
      public Task DeleteUser( string id )
      {
         return DeleteAsync( id );
      }
   }
}