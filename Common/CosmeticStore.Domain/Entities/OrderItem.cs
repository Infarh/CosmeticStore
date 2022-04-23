using System.ComponentModel.DataAnnotations;

using CosmeticStore.Domain.Entities.Base;

namespace CosmeticStore.Domain.Entities;

public class OrderItem : Entity
{
    [Required]
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }

    public int OrderId { get; set; }

    [Required]
    public Order Order { get; set; } = null!;
}