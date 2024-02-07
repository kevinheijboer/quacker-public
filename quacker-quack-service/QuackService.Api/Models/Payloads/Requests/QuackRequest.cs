using System.ComponentModel.DataAnnotations;

namespace QuackService.Api.Models.Payloads.Requests
{
    public class QuackRequest
    {
        [Required]
        [MaxLength(140)]
        public string Message { get; init; }
    }
}
