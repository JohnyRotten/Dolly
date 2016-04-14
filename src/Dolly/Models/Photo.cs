using System.ComponentModel.DataAnnotations;

namespace Dolly.Models
{
    public class Photo
    {
        [Key] public int PhotoId { get; set; }
        [Required] public int ItemId { get; set; }
        public virtual Item Item { get; set; }
        public byte[] Source { get; set; }
    }
}