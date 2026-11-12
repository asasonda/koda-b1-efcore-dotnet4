namespace KodaStore.Api.Models;

public sealed class Category
{
    private Category() {}
    public int CategoryId {get; private set;}
    public string CategoryName {get; private set;}

    public ICollection<Products> Products = [];


    public Category(int categoryId, string categoryName)
    {
        if (categoryId < 0)
        {
            throw new ArgumentException("Id category tidak valid", nameof(categoryId));
        }

        CategoryId = categoryId;
        CategoryName = categoryName;
    }

    public Category(string categoryName)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
        {
            throw new ArgumentException("Name tidak valid", nameof(Category));
        }

        CategoryName = categoryName;
    }
}