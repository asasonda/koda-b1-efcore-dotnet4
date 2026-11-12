using Microsoft.AspNetCore.Http.Features;

namespace KodaStore.Api.Models;

public sealed class Order
{
    private Order() {}
    public long Id {get; private set;}
    public string OrderNumber {get; private set;} = string.Empty;
    public DateTimeOffset OrderDate {get; private set;}
    public string Status {get; private set;}

    public ICollection<OrderItem> Items {get; private set;} = [];  

    public Order(string orderNumber)
    {
        if(string.IsNullOrWhiteSpace(orderNumber) || orderNumber.Trim().Length > 35)
        {
            throw new ArgumentException("OrderNumber tidak boleh kosong dan karakter tidak boleh lebih 35");
        }

        OrderNumber = orderNumber;
        OrderDate = DateTimeOffset.UtcNow.AddHours(7);
        Status = "Pending";
    }

    public void AddItem(Products products, int quantity)
    {
        ArgumentNullException.ThrowIfNull(products);
        if(quantity <= 0)
        {
            throw new ArgumentException("Quantity harus bilangan positiv");
        }
        if(Status != "Pending")
        {
            throw new ArgumentException("Order tidak dapat diubah");
        }
        if(Items.Any(i => i.ProductCode == products.ProductCode)) throw new ArgumentException("product order tidak boleh berulang dalam satu order");

        Items.Add(new OrderItem(products, quantity));
    }
}