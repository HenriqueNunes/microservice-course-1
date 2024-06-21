using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Services;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<ICustomersService, CustomersService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddHttpClient("OrdersService", options =>
{
    options.BaseAddress = new Uri(builder.Configuration["Services:Orders"] ?? "");
}).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(4, _ => TimeSpan.FromMilliseconds(1000)));
builder.Services.AddHttpClient("ProductsService", options =>
{
    options.BaseAddress = new Uri(builder.Configuration["Services:Products"] ?? "");
}).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(4, _ => TimeSpan.FromMilliseconds(1000)));
builder.Services.AddHttpClient("CustomersService", options =>
{
    options.BaseAddress = new Uri(builder.Configuration["Services:Customers"] ?? "");
}).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(4, _ => TimeSpan.FromMilliseconds(1000)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
