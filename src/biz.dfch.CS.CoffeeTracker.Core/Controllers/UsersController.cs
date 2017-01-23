using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Model;

namespace biz.dfch.CS.CoffeeTracker.Core.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using biz.dfch.CS.CoffeeTracker.Core.Model;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<User>("Users");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class UsersController : ODataController
    {
        private readonly CoffeeTrackerDbContext db = new CoffeeTrackerDbContext();

        // GET: odata/Users
        [EnableQuery]
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: odata/Users(5)
        [EnableQuery]
        public SingleResult<User> GetUser([FromODataUri] long key)
        {
            return SingleResult.Create(db.Users.Where(user => user.Id == key));
        }

        // PUT: odata/Users(5)
        public async Task<IHttpActionResult> Put([FromODataUri] long key, Delta<User> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await db.Users.FindAsync(key);
            if (user == null)
            {
                return NotFound();
            }

            patch.Put(user);

            await db.SaveChangesAsync();

            return Updated(user);
        }

        // POST: odata/Users
        public async Task<IHttpActionResult> Post(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Created(user);
        }

        // PATCH: odata/Users(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] long key, Delta<User> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await db.Users.FindAsync(key);
            if (user == null)
            {
                return NotFound();
            }

            patch.Patch(user);

            await db.SaveChangesAsync();

            return Updated(user);
        }

        // DELETE: odata/Users(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            User user = await db.Users.FindAsync(key);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
