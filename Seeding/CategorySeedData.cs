using KodaStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KodaStore.Api.Seeding;

public sealed class CategorySeedData
{
    private static IEnumerable<Category> InitialCategory()
    {
        yield return new Category("Elektonik");
        yield return new Category("Properti");
        yield return new Category("Furniture");
    }

    public static void Seed(DbContext context)
    {
        foreach (var item in InitialCategory())
        {
            bool exist = context.Set<Category>().Any(c => c.CategoryName == item.CategoryName);

            if (!exist)
            {
                context.Set<Category>().Add(item);
            }
        }
        context.SaveChanges();
    }

    public static async Task SeedAsync (DbContext context, CancellationToken ct)
    {
        foreach (var item in InitialCategory())
        {
            bool exist = await context.Set<Category>().AnyAsync(c => c.CategoryName == item.CategoryName,ct);

            if (!exist)
            {
                context.Set<Category>().Add(item);
            }
        }
        await context.SaveChangesAsync(ct);
    }
}