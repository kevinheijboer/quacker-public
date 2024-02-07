using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelineService.Api.Models
{
    public class Mention
    {
        [Key]
        public Guid Id { get; init; }
        public string Value { get; set; }

        [ForeignKey("QuackId")]
        public Guid QuackId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}