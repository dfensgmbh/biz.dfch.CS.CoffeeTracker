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
using biz.dfch.CS.CoffeeTracker.Core.Managers;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.CoffeeTracker.Core.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace biz.dfch.CS.CoffeeTracker.Core.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using biz.dfch.CS.CoffeeTracker.Core.Model;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<ApplicationUser>("ApplicationUsers");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class UsersController : ODataController
    {
        private readonly AuthorizationManager authorizationManager;
        private readonly ApplicationUserManager userManager;
        private const string MODELNAME = ControllerLogging.ModelNames.USER;

        public UsersController()
        {
            authorizationManager = new AuthorizationManager();
            userManager = new ApplicationUserManager(new UserStore<IdentityUser>());
        }

        [EnableQuery]
        [Authorize]
        public IQueryable<ApplicationUser> Get()
        {
            ControllerLogging.LogGetEntities(MODELNAME);

            return userManager.GetUsers();
        }

        [Authorize]
        [EnableQuery]
        public SingleResult<ApplicationUser> Get([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|404|");

            ControllerLogging.LogGetEntity(MODELNAME, key.ToString());

            return SingleResult.Create(userManager.GetUserAsQueryable(key));
        }

        // PUT: odata/ApplicationUsers(5)
        [Authorize]
        public async Task<IHttpActionResult> Put([FromODataUri] long key, ApplicationUser modifiedApplicationUser)
        {
            Contract.Requires(0 < key, "|404|");
            Contract.Requires(null != modifiedApplicationUser, "|404|");

            Validate(modifiedApplicationUser);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = userManager.GetUser(key);
            Contract.Assert(null != user, "|404|");

            ControllerLogging.LogUpdateEntityStartPut(MODELNAME, key.ToString());

            userManager.UpdateUser(key, modifiedApplicationUser);

            ControllerLogging.LogUpdateEntityStopPut(MODELNAME, user);

            return Updated(user);
        }

        // POST: odata/ApplicationUsers
        public async Task<IHttpActionResult> Post(ApplicationUser applicationUser)
        {
            Contract.Requires(null != applicationUser, "|400|");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ControllerLogging.LogInsertEntityStart(MODELNAME, applicationUser);

            var result = await authorizationManager.RegisterUser(applicationUser);
            Contract.Assert(result.Succeeded);
            

            var errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            ControllerLogging.LogInsertEntityStop(MODELNAME, applicationUser);

            return Created(applicationUser);
        }

        // PATCH: odata/ApplicationUsers(5)
        [Authorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] long key, ApplicationUser modifiedApplicationUser)
        {
            Contract.Requires(0 < key, "|404|");
            Contract.Requires(null != modifiedApplicationUser, "|404|");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = userManager.GetUser(key);
            Contract.Assert(null != user, "|404|");

            ControllerLogging.LogUpdateEntityStartPatch(MODELNAME, key.ToString());

            userManager.UpdateUser(key, modifiedApplicationUser);

            ControllerLogging.LogUpdateEntityStopPatch(MODELNAME, user);

            return Updated(user);
        }

        // DELETE: odata/ApplicationUsers(5)
        [Authorize]
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|404|");

            var user = userManager.GetUser(key);
            Contract.Assert(null != user, "|404|");

            ControllerLogging.LogDeleteEntityStart(MODELNAME, user);

            userManager.DeleteUser(key);

            ControllerLogging.LogDeleteEntityStop(MODELNAME, user);

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                authorizationManager.Dispose();
                userManager.Dispose();
            }
            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
