using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;
        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetCustomerOrdersAsync(int customerId)
        {
            try
            {
                logger?.LogInformation($"Querying orders for customer {customerId}");
                var orders = await dbContext.Orders.Include(o => o.Items).Where(o => o.CustomerId == customerId).ToListAsync();
                if (orders != null && orders.Any())
                {
                    logger?.LogInformation($"{orders.Count} order(s) found");
                    var result = mapper.Map<IEnumerable<Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.Order Order, string ErrorMessage)> GetOrderAsync(int id)
        {
            try
            {
                logger?.LogInformation($"Querying orders with id: {id}");
                var order = await dbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
                if (order != null)
                {
                    logger?.LogInformation("Order found");
                    var result = mapper.Map<Order, Models.Order>(order);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync()
        {
            try
            {
                logger?.LogInformation("Querying orders");
                var orders = await dbContext.Orders.Include(o => o.Items).ToListAsync();
                if (orders != null && orders.Any())
                {
                    logger?.LogInformation($"{orders.Count} order(s) found");
                    var result = mapper.Map<IEnumerable<Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        private void SeedData()
        {
            if (!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Order
                {
                    Id = 1,
                    CustomerId = 2,
                    OrderDate = DateTime.Now.AddDays(-5),
                    Total = 170,
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Id=100, OrderId=1, ProductId=2, Quantity=4, UnitPrice=5  },
                        new OrderItem { Id=101, OrderId=1, ProductId=3, Quantity=1, UnitPrice=150  },
                    }
                });
                dbContext.Orders.Add(new Order
                {
                    Id = 2,
                    CustomerId = 3,
                    OrderDate = DateTime.Now,
                    Total = 600,
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Id=105, OrderId=1, ProductId=4, Quantity=2, UnitPrice=200  },
                        new OrderItem { Id=106, OrderId=1, ProductId=1, Quantity=10, UnitPrice=20  },
                    }
                });
                dbContext.SaveChanges();
            }
        }
    }
}
