using System.ComponentModel.DataAnnotations;
using CosmeticStore.Interfaces.Entities;

namespace CosmeticStore.Domain.Entities.Base;

public class NamedEntity : Entity, INamedEntity
{
    [Required]
    public string Name { get; set; } = null!;
}
