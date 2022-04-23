using System.ComponentModel.DataAnnotations;

namespace CosmeticStore.Interfaces.Entities;

public interface INamedEntity : IEntity
{
    [Required]
    string Name { get; set; }
}
