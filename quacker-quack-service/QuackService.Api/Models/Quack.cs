using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuackService.Api.Models
{
    public class Quack
    {
        [Key]
        public Guid Id { get; init; }

        [MaxLength(140)]
        public string Message { get; init; }
        public string Username { get; init; }

        public virtual List<Topic> Topics { get; set; }
        public virtual List<Mention> Mentions { get; set; }

        public Guid UserId { get; init; }

        public DateTime CreatedOn { get; init; }
    }
}
