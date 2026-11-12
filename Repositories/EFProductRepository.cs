using KodaStore.Api.Data;
using KodaStore.Api.DTOs;
using KodaStore.Api.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace KodaStore.Api.Repositories;

public sealed class EFProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public EFProductRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> AddProductAsync(Products products, CancellationToken cancellationToken)
    {
        _db.Products.Add(products);
        try
        {
            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException e) when (e.InnerException is SqlException {Number: 2601 or 2627})
        {
            return false;
        }
    }

    public async Task<bool> DeleteProductAsync(string productCode, CancellationToken cancellationToken)
    {
        var code = productCode.Trim().ToUpperInvariant();
        var existing = await _db.Products.FirstOrDefaultAsync(p => p.ProductCode == code, cancellationToken);

        if (existing is null)
        {
            return false;
        }
        _db.Products.Remove(existing);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IReadOnlyList<Products>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.Products.AsNoTracking().OrderBy(p => p.ProductCode).ToListAsync(cancellationToken);
    }

    public async Task<Products?> GetByCodeAsync(string productCode, CancellationToken cancellationToken)
    {
        return await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductCode == productCode, cancellationToken);
    }

    public async Task<bool> UpdateProductAsync(Products products, CancellationToken cancellationToken)
    {
        var existing = await _db.Products.FirstOrDefaultAsync(p => p.ProductCode == products.ProductCode, cancellationToken);
        if(existing is null)
        {
            return false;
        }

        existing.UpdateDetail(
            products.Name,
            products.Price,
            products.Stock,
            products.CategoryId
        );

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}