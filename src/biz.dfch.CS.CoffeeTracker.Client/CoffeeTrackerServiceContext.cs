using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Services.Client;
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

        public CoffeeTrackerServiceContext(string hostUri)
        {
            var apiUri = string.Format("{0}/api/", hostUri);
            this.uri = new Uri(apiUri);
            this.container = new CoffeeTrackerService.Container(this.uri);
            this.authenticationHelper = new AuthenticationHelper(new Uri(hostUri));

            container.SendingRequest2 += OnBeforeSendingRequest;
        }

        private void OnBeforeSendingRequest(object sender, SendingRequest2EventArgs sendingRequest2EventArgs)
        {
            throw new NotImplementedException();
        }

        public CoffeeTrackerServiceContext(string hostUri, string userName, string password)
            : this(hostUri)
        {
            this.authenticationHelper = new AuthenticationHelper(new Uri(hostUri), userName, password);
        }
    }
}
