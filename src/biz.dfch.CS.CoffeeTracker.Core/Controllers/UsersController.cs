using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using biz.dfch.CS.CoffeeTracker.Core.Logging;
using biz.dfch.CS.CoffeeTracker.Core.Managers;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.CoffeeTracker.Core.Security;
using biz.dfch.CS.CoffeeTracker.Core.Stores;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace biz.dfch.CS.CoffeeTracker.Core.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this Controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using biz.dfch.CS.CoffeeTracker.Core.Model;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<ApplicationUser>("ApplicationUsers");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class UsersController : ODataController
    {
        private readonly Lazy<AuthorizationManager> authorizationManagerLazy = new Lazy<AuthorizationManager>(() =>
            new AuthorizationManager());
        private AuthorizationManager authorizationManager => authorizationManagerLazy.Value;

        private readonly Lazy<ApplicationUserManager> userManagerLazy = new Lazy<ApplicationUserManager>(() => 
            new ApplicationUserManager(new AppUserStore()));
        private ApplicationUserManager userManager => userManagerLazy.Value;

        private const string MODELNAME = ControllerLogging.ModelNames.USER;

        //protected override void Initialize(HttpControllerContext controllerContext)
        //{

        //    //if (controllerContext.Request.Method.Equals(HttpMethod.Post))
        //    //{
        //    //    userManager = new ApplicationUserManager(new AppUserStore(), skipPermissionChecks: true);
        //    //}
        //    //else
        //    //{

        //    //    userManager = new ApplicationUserManager(new AppUserStore());
        //    //}
        //    //authorizationManager = new AuthorizationManager();
        //    base.Initialize(controllerContext);
        //}

        [EnableQuery]
        [Authorize]
        public IQueryable<ApplicationUser> Get()
        {
            ControllerLogging.LogGetEntities(MODELNAME);

            return userManager.GetUsers();
        }

        [Authorize]
        [EnableQuery]
        public IHttpActionResult Get([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|404|");

            ControllerLogging.LogGetEntity(MODELNAME, key.ToString());

            return Ok(userManager.GetUser(key));
        }

        // PUT: odata/ApplicationUsers(5)
        [Authorize]
        [HttpPut]
        public IHttpActionResult Put([FromODataUri] long key, ApplicationUser modifiedApplicationUser)
        {
            Contract.Requires(0 < key, "|404|");
            Contract.Requires(null != modifiedApplicationUser, "|400|");

            Validate(modifiedApplicationUser);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ControllerLogging.LogUpdateEntityStartPut(MODELNAME, key.ToString());

            var user = userManager.UpdateUser(key, modifiedApplicationUser);

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

            var unsecuredAuthManager = new AuthorizationManager(
                new ApplicationUserManager(new AppUserStore(), skipPermissionChecks: true));

            ControllerLogging.LogInsertEntityStart(MODELNAME, applicationUser);

            var result = await unsecuredAuthManager.RegisterUser(applicationUser);
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
        [HttpPatch]
        public IHttpActionResult Patch([FromODataUri] long key, ApplicationUser modifiedApplicationUser)
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
