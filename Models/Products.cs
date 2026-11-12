namespace KodaStore.Api.Models;

public sealed class Products
{
    public string ProductCode {get; private set;}
    public string Name {get; private set;}
    public decimal Price {get; private set;}
    public int Stock {get; private set;}
    public bool IsActive {get; private set;}
    public ICollection<OrderItem> items {get; private set;} = [];

    public int CategoryId {get; private set;}
    public Category category {get; private set;} = null!;

    public Products(string productCode, string name, decimal price, int stock, int categoryId)
    {
        if (string.IsNullOrWhiteSpace(productCode))
        {
            throw new ArgumentException("Product is required", nameof(productCode));
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("name is required", nameof(name));
        }
        if (price < 0)
        {
            throw new ArgumentOutOfRangeException("Price not negative", nameof(price));
        }
        if (stock < 0)
        {
            throw new ArgumentOutOfRangeException("stock not negative", nameof(stock));
        }
        if (categoryId < 0)
        {
            throw new ArgumentException("category is required", nameof(categoryId));
        }

        ProductCode = productCode;
        Name = name;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
    }


    public void UpdateDetail(string name, decimal price, int stock, int categoryId)
    {
        var validate = new Products(ProductCode, name, price, stock, categoryId);

        Name = validate.Name;
        Price = validate.Price;
        Stock = validate.Stock;
        CategoryId = validate.CategoryId;
    }
}