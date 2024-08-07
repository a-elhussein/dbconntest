﻿using Microsoft.AspNetCore.Identity;

namespace TestIdentity.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; }
        public Boolean MustResetPassword { get; set; } = true;

        public virtual ICollection<UserCompany> UserCompanies { get; set; }
    }
}
