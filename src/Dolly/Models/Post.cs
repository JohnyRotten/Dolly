using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dolly.Models
{
    public sealed class Post
    {
        [Key] public int PostId { get; set; }

        [Required] public string Title { get; set; }
        [Required] public string Body { get; set; }
        public DateTime CretedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        [Required] public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } 
        public List<Comment> Comments { get; set; } = new List<Comment>(); 

    }
}