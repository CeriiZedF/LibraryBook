using ShopLibrary.DAL.Data;
using ShopLibrary.DAL.Repository.IRepository;
using ShopLibrary.Models;

namespace ShopLibrary.DAL.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly DataContext _db;

        public OrderRepository(DataContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Order order)
        {
            _db.Update(order);
        }
    }
}
