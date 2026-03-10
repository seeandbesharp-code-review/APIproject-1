using DTOs;
using Entities;
using Repositories;

namespace Servers
{
    public interface IOrdersService
    {

        Task<OrderDTO> AddOrder(OrderDTO order);
        Task<OrderDTO> GetOrderById(int id);
    }
}