using System;
using System.ComponentModel.DataAnnotations;

namespace QuackService.Api.Models
{
    public class Topic
    {
        [Key]
        public Guid Id { get; init; }
        public string Value { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}