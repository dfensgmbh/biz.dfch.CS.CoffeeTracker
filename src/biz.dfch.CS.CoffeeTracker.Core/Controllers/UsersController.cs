using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using biz.dfch.CS.CoffeeTracker.Core.Models;

namespace biz.dfch.CS.CoffeeTracker.Core.Controllers
{
    public class UsersController : ApiController
    {
        User[] users = new User[]
        {
            new User { Id = 1, Name = "Tralala"}, 
            new User { Id = 2, Name = "Hans"}, 
            new User { Id = 3, Name = "Peter"} 
        };

        public IEnumerable<User> GetAllUsers()
        {
            return users;
        }

        public IHttpActionResult GetUser(int id)
        {
            var user = users.FirstOrDefault((u) => u.Id == id);
            Contract.Assert(null != user);

            return Ok(user);
        }

    }
}
