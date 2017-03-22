/**
 * Copyright 2017 d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace biz.dfch.CS.CoffeeTracker.Client.Tests
{
    class AuthenticationHelper
    {
        public string bearerToken = "";
        public Uri tokenUri;
        private static readonly HttpClient client = new HttpClient();

        public AuthenticationHelper(Uri hostUri)
        {
            Contract.Requires(null != hostUri);

            var tokenUriStr = string.Format("{0}{1}", hostUri.AbsoluteUri, "token");
            this.tokenUri = new Uri(tokenUriStr);
        }

        private async void ReceiveAndSetToken(string userName, string password)
        {
            var bodyValuesDictionary = new Dictionary<string, string>
            {
                { "Name", userName},
                { "Password", password},
                { "grant_type", "password"}
            };

            var body = new FormUrlEncodedContent(bodyValuesDictionary);

            var response = await client.PostAsync(tokenUri, body);

            var responseString = await response.Content.ReadAsStringAsync();
            int i = 0;
        }
    }
}
