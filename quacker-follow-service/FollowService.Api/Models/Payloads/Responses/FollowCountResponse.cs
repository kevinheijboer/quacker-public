using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Api.Models.Payloads.Responses
{
    public class FollowCountResponse
    {
        public int FollowersCount { get; init; }
        public int FollowingCount { get; init; }
    }
}
