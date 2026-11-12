using System.Globalization;
using KodaStore.Api.Data;
using KodaStore.Api.Middlewares;
using KodaStore.Api.Models;
using KodaStore.Api.Repositories;
using KodaStore.Api.Seeding;
using KodaStore.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// builder.Services.AddSingleton<InMemoryProductCatalog>();
// builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
// builder.Services.AddSingleton<IProductService, ProductService>();

// builder.Services.AddScoped<IProductRepository, SqlProductRepository>();

builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();
builder.Services.AddScoped<IProductRepository, EFProductRepository>();

builder.Services.AddScoped<ICategoryService, CategoryServices>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ICustomerRepository, SqlCustomerRepository>();
builder.Services.AddScoped<ICustomerServices, CustomerServices>();


var connectionString = builder.Configuration.GetConnectionString("eccomercedb") ?? throw new InvalidOperationException("ConnectionString: belum diconfigurasi");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString)
        .UseSeeding((context, _) =>
        {
            CategorySeedData.Seed(context);
            ProductSeedData.Seed(context);
        })
        .UseAsyncSeeding(async(context, _, ct) =>
        {
            await CategorySeedData.SeedAsync(context, ct);   
            await ProductSeedData.SeedAsync(context, ct);   
        });
});

var app = builder.Build();
app.UseExceptionHandler();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
