using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Api.Payloads.Requests
{
    public class UserRegistrationRequest
    {
        [Required]
        public string Username { get; init; }

        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; init; }

        [Required]
        public DateTime Birthdate { get; init; }
    }
}
