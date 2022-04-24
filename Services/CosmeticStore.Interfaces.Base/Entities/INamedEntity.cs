using System.ComponentModel.DataAnnotations;

namespace CosmeticStore.Interfaces.Base.Entities;

public interface INamedEntity : IEntity
{
    [Required]
    string Name { get; set; }
}
