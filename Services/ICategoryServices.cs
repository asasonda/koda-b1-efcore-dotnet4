using KodaStore.Api.DTOs;

namespace KodaStore.Api.Services;

public interface ICategoryService
{
    Task<ProductDetailResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
}