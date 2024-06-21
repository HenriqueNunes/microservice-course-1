using ECommerce.Api.Search.Interfaces;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly ICustomersService customerService;
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;

        public SearchService(ICustomersService customerService, IOrdersService ordersService, IProductsService productsService)
        {
            this.customerService = customerService;
            this.ordersService = ordersService;
            this.productsService = productsService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var customerSearch = await customerService.GetCustomerAsync(customerId);
            if (customerSearch.IsSuccess)
            {
                var ordersResult = await ordersService.GetOrdersAsync(customerId);
                if (ordersResult.IsSuccess)
                {
                    var productsResult = await productsService.GetProductsAsync();
                    foreach (var order in ordersResult.Orders)
                    {
                        foreach (var item in order.Items)
                        {
                            item.ProductName = productsResult.IsSuccess ?
                                productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name ?? "" :
                                "Product information is not available";
                        }
                    }
                    customerSearch.Customer.Orders = ordersResult.Orders.ToList();
                }
                return (true, customerSearch.Customer);
            }
            return (false, null);
        }
    }
}
