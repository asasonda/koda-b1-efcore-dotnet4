using KodaStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KodaStore.Api.Seeding;

public sealed class ProductSeedData
{
    private static IEnumerable<Products> InitialProducts()
    {
        yield return new Products("P004", "Kipas", 35000, 20, 1);
        yield return new Products("P005", "Gelas", 25000, 20, 2);
        yield return new Products("P006", "Galon", 5000, 20, 2);
        yield return new Products("P007", "Meja", 75000, 20, 3);
        yield return new Products("P008", "Kursi", 85000, 20, 3);
        yield return new Products("P009", "Kaca", 95000, 20, 3);
        yield return new Products("P0010", "Monitor", 125000, 20, 1);
        yield return new Products("P0011", "Vas", 35000, 20, 1);
        yield return new Products("P0012", "Kabel", 35000, 20, 3);
        yield return new Products("P0013", "Handphone", 35000, 20, 3);
    }

    public static void Seed(DbContext context)
    {
        foreach (var item in InitialProducts())
        {
            bool exist = context.Set<Products>().Any(p => p.ProductCode == item.ProductCode);

            if (!exist)
            {
                context.Set<Products>().Add(item);
            }
        }
        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, CancellationToken ct)
    {
        foreach (var product in InitialProducts())
        {
            bool exists = await context.Set<Products>().AnyAsync(p => p.ProductCode == product.ProductCode, ct);

            if (!exists)
            {
                context.Set<Products>().Add(product);
            }
        }
        await context.SaveChangesAsync(ct);
    }
}