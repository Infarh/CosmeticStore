﻿using System.ComponentModel.DataAnnotations;
using CosmeticStore.Interfaces.Base.Entities;

namespace CosmeticStore.Domain.Entities.Base;

public class NamedEntity : Entity, INamedEntity
{
    [Required]
    public string Name { get; set; } = null!;

    public override string ToString() => $"[{Id}]{Name}";
}
