using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Api.Models.Messages
{
    public class NewAccountMessage
    {
        public string UserId { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
    }
}
