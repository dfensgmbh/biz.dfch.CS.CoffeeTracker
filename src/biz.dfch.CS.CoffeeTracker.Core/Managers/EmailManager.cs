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
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Microsoft.Exchange.WebServices.Data;

namespace biz.dfch.CS.CoffeeTracker.Core.Managers
{
    public class EmailManager
    {
        private readonly ExchangeService service;

        public EmailManager()
        {
            ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;

            var userMail = ExchangeCredentials.EXCHANGE_USERNAME;
            var password = ExchangeCredentials.EXCHANGE_PASSWORD;
            service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);

            service.Credentials = new WebCredentials(userMail, password);
            service.AutodiscoverUrl(userMail, RedirectionUrlValidationCallback);
        }
                
        public void CreateAndSendOutOfStockEmail(IEnumerable<EmailAddress> recipients)
        {
            Contract.Requires(null != recipients);

            var message = new EmailMessage(service)
            {
                Subject = "Coffeestock",
                Body = new MessageBody("A coffee is about to run out of stock m8")
            };

            foreach (var recipient in recipients)
            {
                message.ToRecipients.Add(recipient);
            }

            message.SendAndSaveCopy();
        }

        private bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            var redirectionUri = new Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }

        private static bool CertificateValidationCallBack(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            // If there are errors in the certificate chain, look at each error to determine the cause.
            if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (var status in chain.ChainStatus)
                    {
                        if (certificate.Subject == certificate.Issuer && status.Status == X509ChainStatusFlags.UntrustedRoot)
                        {
                            // Self-signed certificates with an untrusted root are valid. 
                            continue;
                        }
                        else
                        {
                            if (status.Status != X509ChainStatusFlags.NoError)
                            {
                                // If there are any other errors in the certificate chain, the certificate is invalid,
                                // so the method returns false.
                                return false;
                            }
                        }
                    }
                }

                // When processing reaches this line, the only errors in the certificate chain are 
                // untrusted root errors for self-signed certificates. These certificates are valid
                // for default Exchange server installations, so return true.
                return true;
            }
            return false;
        }
    }
}