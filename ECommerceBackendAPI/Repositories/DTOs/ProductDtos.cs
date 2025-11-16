namespace ECommerce.Api.DTOs;

public record ProductCreateDto(string Name, string? Description, decimal Price, int CategoryId, int StockQuantity);
public record ProductUpdateDto(string Name, string? Description, decimal Price, int CategoryId, int StockQuantity, bool IsActive);
public record ProductListDto(int Id, string Name, string? Description, decimal Price, int StockQuantity, bool IsActive);
