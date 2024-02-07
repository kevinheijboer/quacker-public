using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Api.Models.Messages
{
    public class NewFollowingMessage
    {
        public bool Follow { get; init; }
        public Guid UserId { get; init; }
        public Guid UserToFollowId { get; init; }
    }
}
