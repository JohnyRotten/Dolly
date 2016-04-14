using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security;

namespace Dolly.Models
{

    public enum OrderStatus
    {
        [Display(Name = "Создан")]
        Created,
        [Display(Name = "В процессе")]
        Processed,
        [Display(Name = "В обработке")]
        Worked,
        [Display(Name = "Отправлен")]
        Transported,
        [Display(Name = "Выполнен")]
        Completed
    }

    public class Order
    {
        [Key] public int OrderId { get; set; }
        [Required] public Guid UserId { get; set; }  
        public ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required] public decimal Price { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime StatusChanged { get; set; } = DateTime.Now;
        [Required] public string FirstName { get; set; }
        public string Patronymic { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Address { get; set; }
        [Required] public string Phone { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        [NotMapped] public List<Item> Items => OrderItems.Select(o => o.Item).ToList();
    }
}