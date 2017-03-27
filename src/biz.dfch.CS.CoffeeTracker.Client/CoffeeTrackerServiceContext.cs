using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using biz.dfch.CS.CoffeeTracker.Client.Tests;

namespace biz.dfch.CS.CoffeeTracker.Client
{
    public class CoffeeTrackerServiceContext
    {
        private Uri uri;
        private AuthenticationHelper authenticationHelper;
        public CoffeeTrackerService.Container container;

        public CoffeeTrackerServiceContext(string uri)
        {
            this.uri = new Uri(uri);
            this.container = new CoffeeTrackerService.Container(this.uri);
            this.authenticationHelper = new AuthenticationHelper(new Uri(uri));

            container.SendingRequest2 += (s, e) =>
            {
                Console.WriteLine("Http Request sent");
            };
        }
    }
}
