namespace CosmeticStore.Domain.DTO;

public record CreateOderDTO(
    string CustomerName, 
    string PhoneNumber, 
    string? Description,
    IEnumerable<OrderItemInfo> Items);