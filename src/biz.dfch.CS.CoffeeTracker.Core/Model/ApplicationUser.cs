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

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using Microsoft.AspNet.Identity.EntityFramework;

namespace biz.dfch.CS.CoffeeTracker.Core.Model
{
    public class ApplicationUser : BaseEntity
    {
        // Due to renaming complications, ApplicationUser.Name is a Email Address
        public ApplicationUser(string email, string password)
        {
            var isValidMail = IsValidEmail(email);
            Contract.Assert(isValidMail);

            this.Name = email;
            this.Password = password;
            this.IsAdmin = false;
        }

        public ApplicationUser()
        {
            IsAdmin = false;
        }

        [Required]
        public string Password { get; set; }

        public string AspNetUserId { get; set; }

        [ForeignKey("AspNetUserId")]
        public virtual IdentityUser CorrespondingAspNetUser { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}