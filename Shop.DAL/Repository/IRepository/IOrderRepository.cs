using ShopLibrary.Models;

namespace ShopLibrary.DAL.Repository.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order order);
    }
}
