using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Api.Models
{
    public class Account
    {
        [Key]
        public Guid UserId { get; init; }
        public string Username { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public string Email { get; init; }
        [MaxLength(30)]
        public string Location { get; set; }
        [MaxLength(160)]
        public string Bio { get; set; }
        [MaxLength(100)]
        public string Website { get; set; }
        public string ProfilePictureURL { get; set; }
        public DateTime CreatedOn { get; init; }
    }
}
