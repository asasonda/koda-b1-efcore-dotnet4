using KodaStore.Api.DTOs;

namespace KodaStore.Api.Repositories;

public interface ICategoryRepository
{
    Task<ProductDetailResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
}