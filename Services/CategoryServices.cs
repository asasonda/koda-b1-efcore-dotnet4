using KodaStore.Api.DTOs;
using KodaStore.Api.Repositories;

namespace KodaStore.Api.Services;

public sealed class CategoryServices : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryServices(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductDetailResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(id, cancellationToken);
    }
}