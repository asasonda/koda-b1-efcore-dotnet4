using KodaStore.Api.Data;
using KodaStore.Api.DTOs;
using KodaStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KodaStore.Api.Repositories;

public sealed class EFCategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _db;

    public EFCategoryRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ProductDetailResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var category = await _db.Category.AsNoTracking().Include(p => p.Products).FirstOrDefaultAsync(c => c.CategoryId == id, cancellationToken);

        return category is null ? null : MapDetail(category);
    }

    private static ProductDetailResponse MapDetail(Category category)
    {
        var items = category.Products.OrderBy(i => i.CategoryId).Select(i => new ProductToCategoryResponse(i.ProductCode, i.Name, i.Price, i.Stock)).ToArray();

        return new ProductDetailResponse(category.CategoryId, category.CategoryName, items);
    }
}