using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Api.Models.Messages
{
    public class NewFollowMessage
    {
        public string UserId { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
    }
}
