using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimelineService.Api.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
        public virtual ICollection<User> Followers { get; set; }
        public virtual ICollection<User> Following { get; set; }
        public DateTime CreatedOn { get; init; }
    }
}
