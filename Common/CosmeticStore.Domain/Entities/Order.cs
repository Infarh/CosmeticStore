using System.ComponentModel.DataAnnotations;

namespace CosmeticStore.Domain.Entities;

public class Order
{
    [Required]
    public Customer Customer { get; set; } = null!;

    public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;

    [Required]
    public string Phone { get; set; } = null!;

    public string? Description { get; set; } = null!;

    public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
}