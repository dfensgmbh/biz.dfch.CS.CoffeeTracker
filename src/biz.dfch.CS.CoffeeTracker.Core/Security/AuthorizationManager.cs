using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.ModelBinding;
using biz.dfch.CS.CoffeeTracker.Core.Logging;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using Microsoft.AspNet.Identity;

namespace biz.dfch.CS.CoffeeTracker.Core.Security
{
    public class AuthorizationManager
    {
        private readonly AuthRepository repository = null;

        public AuthorizationManager()
        {
            repository = new AuthRepository();
        }

        public async Task<IdentityResult> Register(User user)
        {
            Contract.Requires(null != user, "|400|");
            Contract.Requires(user.IsPasswordSafe(), "|400|");

            

            var result = await repository.RegisterUser(user);
            return result;
        }
    }
}
