using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dolly.Models
{
    public sealed class Comment
    {
        [Key] public int CommentId { get; set; }
        [Required] public string Content { get; set; }
        [Required] public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Required] public int ObjectId { get; set; }
        [Required] public string ItemType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}