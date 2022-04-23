using System.ComponentModel.DataAnnotations;

namespace CosmeticStore.Domain.Entities;

public class OrderItem
{
    [Required]
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }
}