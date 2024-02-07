using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Api.Payloads.Messages
{
    public class AccountMessage
    {
        public Guid UserId { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
        public DateTime CreatedOn { get; init; }
    }
}
