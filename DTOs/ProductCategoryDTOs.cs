namespace KodaStore.Api.DTOs;

public record ProductToCategoryResponse(
    string ProductCode, string Name, decimal Price, int Stock
);

public record ProductDetailResponse(
    int Id, string CategoryName, IReadOnlyList<ProductToCategoryResponse> Products
);