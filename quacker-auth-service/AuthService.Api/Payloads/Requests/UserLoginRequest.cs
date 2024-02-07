using AuthService.Api.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Api.Payloads.Requests
{
    public class UserLoginRequest
    {
        [Required]
        public string Username { get; init; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; init; }
    }
}
