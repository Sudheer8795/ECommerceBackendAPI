namespace ECommerce.Api.DTOs;

public record OrderItemDto(int ProductId, int Quantity);
public record CreateOrderDto(List<OrderItemDto> Items);
