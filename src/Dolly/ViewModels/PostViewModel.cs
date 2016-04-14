using System.Collections.Generic;
using Dolly.Models;

namespace Dolly.ViewModels
{
    public class PostViewModel
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }  
    }
}