namespace KodaStore.Api.Models;

public sealed class OrderItem
{
    private OrderItem() {}

    public long Id {get; private set;}
    
    // fk order
    public long OrderId {get; private set;}
    public Order Order {get; private set;} = null!;

    // fk product
    public string ProductCode {get; private set;} = string.Empty;
    public Products Products {get; private set;} = null!;

    public int Quantity {get; private set;}
    public decimal UnitPrice {get; private set;}
    public decimal SubTotal {get; private set;}

    public OrderItem(Products products, int quantity)
    {
        ArgumentNullException.ThrowIfNull(products);
        if(quantity <= 0)
        {
            throw new ArgumentException("Quantity harus positiv");
        }

        ProductCode = products.ProductCode;
        Products = products;
        Quantity = quantity;
        UnitPrice = products.Price;

    }

}