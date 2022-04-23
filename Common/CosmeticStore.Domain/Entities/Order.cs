using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CosmeticStore.Domain.Entities.Base;

namespace CosmeticStore.Domain.Entities;

public class Order : Entity
{
    public int CustomerId { get; set; }

    [Required, ForeignKey(nameof(CustomerId))]
    public Customer Customer { get; set; } = null!;

    public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;

    [Required]
    public string Phone { get; set; } = null!;

    public string? Description { get; set; } = null!;

    public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
}