using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CosmeticStore.Domain.Entities.Base;

namespace CosmeticStore.Domain.Entities;

public class Product : NamedEntity
{
    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    [Required, ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; } = null!;
}