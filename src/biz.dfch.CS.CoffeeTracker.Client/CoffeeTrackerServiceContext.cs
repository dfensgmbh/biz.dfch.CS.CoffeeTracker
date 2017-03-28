using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Services.Client;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using biz.dfch.CS.CoffeeTracker.Client.Tests;

namespace biz.dfch.CS.CoffeeTracker.Client
{
    public class CoffeeTrackerServiceContext : CoffeeTrackerService.Container
    {
        private const string authorizationHeaderName = "Authorization";

        public AuthenticationHelper authenticationHelper;

        public CoffeeTrackerServiceContext(string hostUri)
            : base(new Uri(hostUri))
        {
            var apiUri = new Uri(hostUri + "/api");
            this.BaseUri = apiUri;
            this.authenticationHelper = new AuthenticationHelper(new Uri(hostUri));

            Format.UseJson();
            SendingRequest2 += OnBeforeSendingRequest;
        }

        private void OnBeforeSendingRequest(object sender, SendingRequest2EventArgs sendingRequest2EventArgs)
        {
            if (string.IsNullOrEmpty(authenticationHelper.bearerToken))
            {
                var url = sendingRequest2EventArgs.RequestMessage.Url;
                var absolutePath = url.AbsolutePath;
                if (!sendingRequest2EventArgs.RequestMessage.Method.Equals(HttpMethod.Post) &&
                    !absolutePath.Contains("Users"))
                {
                    throw new Exception(
                        "No Bearer token provided. Did you forgot to call ReceiveAndSetToken-Method of the AuthenticationHelper-Object?");
                }
            }
            else
            {
                var bearerString = string.Format("bearer {0}", authenticationHelper.bearerToken);
                sendingRequest2EventArgs.RequestMessage.SetHeader(authorizationHeaderName, bearerString);
            }
        }

        public CoffeeTrackerServiceContext(string hostUri, string userName, string password)
            : this(hostUri)
        {
            this.authenticationHelper = new AuthenticationHelper(new Uri(hostUri), userName, password);
        }
    }
}
