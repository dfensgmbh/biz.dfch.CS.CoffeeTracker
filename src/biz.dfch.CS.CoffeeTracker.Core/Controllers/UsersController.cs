using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Logging;
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
        private const string MODELNAME = ControllerLogging.ModelNames.USER;

        // GET: odata/Users
        [EnableQuery]
        public IQueryable<User> GetUsers()
        {
            ControllerLogging.LogGetEntities(MODELNAME);

            return db.Users;
        }

        // GET: odata/Users(5)
        [EnableQuery]
        public SingleResult<User> GetUser([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|404|");

            ControllerLogging.LogGetEntity(MODELNAME, key.ToString());

            return SingleResult.Create(db.Users.Where(user => user.Id == key));
        }

        // PUT: odata/Users(5)
        public async Task<IHttpActionResult> Put([FromODataUri] long key, User modifiedUser)
        {
            Contract.Requires(0 < key, "|404|");
            Contract.Requires(null != modifiedUser, "|404|");

            Validate(modifiedUser);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await db.Users.FindAsync(key);
            Contract.Assert(null != user, "|404|");

            ControllerLogging.LogUpdateEntityStartPut(MODELNAME, key.ToString());

            user.Name = modifiedUser.Name;
            user.Password = modifiedUser.Password;

            await db.SaveChangesAsync();
            ControllerLogging.LogUpdateEntityStopPut(MODELNAME, user);

            return Updated(user);
        }

        // POST: odata/Users
        public async Task<IHttpActionResult> Post(User user)
        {
            Contract.Requires(null != user, "|404|");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ControllerLogging.LogInsertEntityStart(MODELNAME, user);

            db.Users.Add(user);
            await db.SaveChangesAsync();

            ControllerLogging.LogInsertEntityStop(MODELNAME, user);

            return Created(user);
        }

        // PATCH: odata/Users(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] long key, Delta<User> patch)
        {
            Contract.Requires(0 < key, "|404|");
            Contract.Requires(null != patch, "|404|");

            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await db.Users.FindAsync(key);
            Contract.Assert(null != user, "|404|");

            ControllerLogging.LogUpdateEntityStartPatch(MODELNAME, key.ToString());

            patch.Patch(user);
            await db.SaveChangesAsync();

            ControllerLogging.LogUpdateEntityStopPatch(MODELNAME, user);

            return Updated(user);
        }

        // DELETE: odata/Users(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|404|");

            var user = await db.Users.FindAsync(key);
            Contract.Assert(null != user, "|404|");

            ControllerLogging.LogDeleteEntityStart(MODELNAME, user);

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            ControllerLogging.LogDeleteEntityStop(MODELNAME, user);

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
