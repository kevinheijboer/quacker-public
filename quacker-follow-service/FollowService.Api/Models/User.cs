using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Api.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; init; }
        public string Username { get; init; }
        public virtual ICollection<User> Followers { get; set; }
        public virtual ICollection<User> Following { get; set; }
    }
}
