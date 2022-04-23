using CosmeticStore.Domain.Entities.Base;

namespace CosmeticStore.Domain.Entities;

public class Category : NamedEntity
{
    public ICollection<Product> Products { get; set; } = new HashSet<Product>();
}