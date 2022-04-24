using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;
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

    public string? Description { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();

    [JsonIgnore]
    public decimal TotalPrice => Items?.Sum(item => item.TotalPrice) ?? 0;

    public override string ToString()
    {
        FormattableString str = $"id:{Id} от {Customer?.Name} ({Date}) на сумму {TotalPrice:c2}";
        return str.ToString(CultureInfo.GetCultureInfo("ru-RU"));
    }
}