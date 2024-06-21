using ECommerce.Api.Orders.Models;

namespace ECommerce.Api.Orders.Interfaces
{
    public interface IOrdersProvider
    {
        Task<(bool IsSuccess, IEnumerable<Order> Orders, string ErrorMessage)> GetOrdersAsync();
        Task<(bool IsSuccess, Order Order, string ErrorMessage)> GetOrderAsync(int id);

        Task<(bool IsSuccess, IEnumerable<Order> Orders, string ErrorMessage)> GetCustomerOrdersAsync(int customerId);

    }
}
