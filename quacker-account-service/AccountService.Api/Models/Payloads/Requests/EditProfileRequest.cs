using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Api.Models.Payloads.Requests
{
    public class EditProfileRequest
    {
        [MaxLength(50)]
        public string Name { get; init; }
        [MaxLength(30)]
        public string Location { get; init; }
        [MaxLength(160)]
        public string Bio { get; init; }
        [MaxLength(100)]
        public string Website { get; init; }
    }
}
