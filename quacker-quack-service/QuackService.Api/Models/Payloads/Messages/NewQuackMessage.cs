using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuackService.Api.Models.Payloads.Messages
{
    public class NewQuackMessage
    {
        public Guid Id { get; init; }
        public string Message { get; init; }
        public string Username { get; init; }
        public Guid UserId { get; init; }
        public DateTime CreatedOn { get; init; }
        public virtual List<Mention> Mentions { get; set; }

    }
}
