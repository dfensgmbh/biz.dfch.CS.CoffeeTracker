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
        public ApplicationUser(string name, string password)
        {
            this.Name = name;
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

    }
}