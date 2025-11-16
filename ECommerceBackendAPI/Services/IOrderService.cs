using ECommerce.Api.DTOs;

namespace ECommerce.Api.Services;

public interface IOrderService
{
    Task<int> PlaceOrderAsync(int customerId, CreateOrderDto dto);
    Task CancelOrderAsync(int orderId, int customerId);
}
