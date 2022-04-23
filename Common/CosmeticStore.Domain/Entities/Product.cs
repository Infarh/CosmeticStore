using System.ComponentModel.DataAnnotations;
using CosmeticStore.Domain.Entities.Base;

namespace CosmeticStore.Domain.Entities;

public class Product : NamedEntity
{
    public string? Description { get; set; }

    public decimal Price { get; set; }

    [Required]
    public Category Category { get; set; } = null!;
}