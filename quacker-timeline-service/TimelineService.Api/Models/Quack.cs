using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimelineService.Api.Models
{
    public class Quack
    {
        [Key]
        public Guid Id { get; init; }

        public string Message { get; init; }
        public string Username { get; init; }
        public virtual List<Mention> Mentions { get; set; }

        public Guid UserId { get; init; }

        public DateTime CreatedOn { get; init; }
    }
}