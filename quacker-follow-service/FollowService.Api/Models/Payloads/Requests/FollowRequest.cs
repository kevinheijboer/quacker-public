using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Api.Models.Payloads.Requests
{
    public class FollowRequest
    {
        public Guid UserId { get; init; }
    }
}
