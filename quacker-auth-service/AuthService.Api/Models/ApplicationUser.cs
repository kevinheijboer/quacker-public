using Microsoft.AspNetCore.Identity;
using System;

namespace AuthService.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime Birthdate { get; init; }
        public DateTime CreatedOn { get; init; }
    }
}
